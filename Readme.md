Summary
=======

Ivet is a command line tool created to generate a schema for a janusgraph database and apply this schema. It has been inspired from Doctrine behaviour.
The **generate** command creates a json file containing commands to execute. The file can be manually modified if you find migration is not complete or does not cover all you deserves.
To keep trace of all applied migrations, migration names are saved into janusgraph server when **upgrade** is called.
You can check at any moment the **list** of migrations both applied and not.

Available commands
=======
* **generate**
	* **dir** Dlls to analyze. If not set look in current working directory.
	* **output** Directory where to put migration file. If not set files are created in current directory.
	* **ip** _localhost_
	* **port** _8182_
	* **ssl** Use SSL/TLS for JanusGraph connection (default: false)
    * **sprintno** Add a subdirectory when generating. Use it as you want
    * **onefile** Create one file per script
    * **description** Add a description to your migration
* **list** Display list of all migrations
	* **input** Directory where to find migration files. If not set look in current working directory.
	* **ip** _localhost_
	* **port** _8182_
	* **ssl** Use SSL/TLS for JanusGraph connection (default: false)
* **upgrade**
	* **input** Can be a file or a directory containing migrations to be applied. If not set look in current working directory.
	* **ip** _localhost_
	* **port** _8182_
	* **ssl** Use SSL/TLS for JanusGraph connection (default: false)
	* **timeout** Default evaluation timeout in milliseconds for Gremlin scripts. Can be overridden per script or per migration file in JSON

Docker image
=======
Docker image is available there:
- https://hub.docker.com/r/dlecoconnier/ivet
- https://github.com/etrange02/Ivet/pkgs/container/ivet

To use Docker image, run the following command replacing `MyDir` with your local directory

```
docker run -v MyDir:/app/Migrations -e ip=localhost ivet
```
See upgrade command to add your own parameters.

Dotnet Tool
=======
You can install Ivet as a dotnet tool
```
dotnet tool install --global Ivet
```
All versions are available on https://www.nuget.org/packages/Ivet

After the package is installed you can use it like
```
Ivet list --input "C:\MigrationFiles"
```

How to tag code?
=======
Add `Ivet.Model` (https://www.nuget.org/packages/Ivet.Model) to your project. You are now ready to tag your code.
All names and possibilities are listed in Janusgraph documentation at https://docs.janusgraph.org/schema/.

Tag a Vertex:
```
[Vertex]
public class MyVertex
{
    [PropertyKey]
    [PrimaryKey()]
    public string Id { get; set; }

    [PropertyKey(Name="NewName", Cardinality=Cardinality.SINGLE)]
    public string ANamedProperty { get; set; }

    [EdgeProperty]
    public List<Vertex3> AListProperty { get; private set; } = new List<Vertex3>();

    [EdgeProperty]
    public Vertex3[] AnArrayProperty { get; private set; } = Array.Empty<Vertex3>(); 
}
```


You can also specify a different name or a cardinality (details at https://docs.janusgraph.org/schema/#property-key-cardinality):
```
[PropertyKey(Name="NewName", Cardinality=Cardinality.SINGLE)]
public string ANamedProperty { get; set; }
```


Tag an Edge:
```
[Edge(typeof(MyVertex1), typeof(MyVertex2))]
[Edge(typeof(MyVertex1), typeof(MyVertex3))]
public class EdgeWithProperties
{
    [PropertyKey]
    public int AProperty { get; set; }
}
```


You can also tag a property to be used with an index (https://docs.janusgraph.org/schema/index-management/index-performance/):
```
[Vertex]
public class MyVertex
{
    [PropertyKey]
    [CompositeIndex("unicity_index", IsUnique = true)] // Set unicity of the data
    public string ACompositeIndexProperty { get; set; }

    [PropertyKey]
    [MixedIndex("vertex2_mixed", Backend = "search", Mapping = MappingType.TEXTSTRING)]
    public string SearchProperty { get; set; }
}
```

Mixed index with shared properties (inheritance)
-------
When multiple vertex types inherit the same `[MixedIndex]` property from an abstract base class, Ivet detects that the index spans several vertex labels and generates a **global** mixed index (without `indexOnly`). The search backend will index the property on all vertex types that carry it, and the `hasLabel()` step in queries still filters efficiently.

If only one vertex type uses a given index name, the generated script uses `indexOnly(vertex)` as before.

```
public abstract class AbstractItem
{
    [PropertyKey]
    [MixedIndex("search", Backend = "search", Mapping = MappingType.TEXT)]
    public string Name { get; set; }

    [PropertyKey]
    [MixedIndex("search", Backend = "search", Mapping = MappingType.STRING)]
    public string Code { get; set; }
}

[Vertex]
public class ConcreteItemA : AbstractItem { }

[Vertex]
public class ConcreteItemB : AbstractItem { }
// → Ivet generates one global "search" index with Name (TEXT) + Code (STRING)
```

Mapping types:
| MappingType | Behavior |
|-------------|----------|
| `TEXT` | Tokenized — splits on spaces, hyphens, etc. Good for natural text |
| `STRING` | Not tokenized — the entire value is compared as-is. Good for codes and identifiers |
| `TEXTSTRING` | Both tokenized and exact match |


Timeout & Resilience
=======
Index operations (`awaitGraphIndexStatus`, `REINDEX`) can take a long time. By default, generated index scripts include a 5-minute evaluation timeout (300000 ms).

You can control the timeout at three levels (highest priority first):
1. **Per script** in JSON: `"evaluationTimeout": 600000` inside a script entry
2. **Per file** in JSON: `"evaluationTimeout": 600000` at the migration file level
3. **CLI flag**: `--timeout 600000` on the `upgrade` command

```
Ivet upgrade --input ".\Migrations" --timeout 600000
```

> [!NOTE]
> If an index script times out, the migration is still marked as applied. JanusGraph registers the index operation and will complete it on restart. Re-running the same script would fail with "index already exists". Scripts without an explicit timeout (e.g., entity creation scripts) are NOT marked as applied on timeout — the error propagates normally.

What's next?
=======
You can now run a `generate` command in order to create migration files.

> [!NOTE]
> Many files are created by the `generate` command. One contains vertices and edges and properties. Indices are in different files to avoid timeout at migration time.

You can have some details of migrations with `list`.

> [!TIP]
> You can open and manually edit your migration files. In particular there is a description field whose content can be seen with `list` command

```
> Ivet list --input ".\Migrations"

Directory: D:\Migrations

Migrations:
 --------------------------------------------------------------------------------------------------------------------------
 | Name                      | Relative path                  | Description | Date                | Multi? | Timeout  |
 --------------------------------------------------------------------------------------------------------------------------
 | Migration_202403032330_#0 | 12\Migration_202403032330.json | [0]         | 10/03/2024 14:21:54 | True   |          |
 --------------------------------------------------------------------------------------------------------------------------
 | Migration_202403032330_#1 | 12\Migration_202403032330.json | [1]         | 10/03/2024 14:22:04 | True   | 300000ms |
 --------------------------------------------------------------------------------------------------------------------------
 | Migration_202403032330_#2 | 12\Migration_202403032330.json | [2]         | 10/03/2024 14:22:14 | True   | 300000ms |
 --------------------------------------------------------------------------------------------------------------------------

 Count:3
```