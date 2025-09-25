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
    * **sprintno** Add a subdirectory when generating. Use it as you want
    * **onefile** Create one file per script
    * **comment** Add a description to your migration
* **list** Display list of all migrations
	* **input** Directory where to find migration files. If not set look in current working directory.
	* **ip** _localhost_
	* **port** _8182_
* **upgrade**
	* **input** Can be a file or a directory containing migrations to be applied. If not set look in current working directory.
	* **ip** _localhost_
	* **port** _8182_

Docker image
=======
Docker image is available there:
- https://hub.docker.com/r/dlecoconnier/ivet
- https://github.com/etrange02/Ivet/pkgs/container/ivet

To use Docker image, run the following command replacing `MyDir` with your local dierctory

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


What's next?
=======
You can now run a `generate` command in order to create migration files.

> [!NOTE]
> Many files are created by the `generate` command. One contains vertices and edges and properties. Indices are in different files to avoid timeout at migration time.

You can can have some details of migrations with `list`.

> [!TIP]
> You can open and manually edit your migration files. In particular there is a description field whose content can be seen with `list` command

```
> Ivet list --input ".\Migrations"

Directory: D:\Migrations

Migrations:
 -----------------------------------------------------------------------------------------------------------
 | Name                      | Relative path                  | Description | Date                | Multi? |
 -----------------------------------------------------------------------------------------------------------
 | Migration_202403032330_#0 | 12\Migration_202403032330.json | Part #0     | 10/03/2024 14:21:54 | True   |
 -----------------------------------------------------------------------------------------------------------
 | Migration_202403032330_#1 | 12\Migration_202403032330.json | Part #1     | 10/03/2024 14:22:04 | True   |
 -----------------------------------------------------------------------------------------------------------
 | Migration_202403032330_#2 | 12\Migration_202403032330.json | Part #2     | 10/03/2024 14:22:14 | True   |
 -----------------------------------------------------------------------------------------------------------
 | Migration_202403032330_#3 | 12\Migration_202403032330.json | Part #3     | 10/03/2024 14:22:24 | True   |
 -----------------------------------------------------------------------------------------------------------
 | Migration_202403032330_#4 | 12\Migration_202403032330.json | Part #4     | 10/03/2024 14:22:34 | True   |
 -----------------------------------------------------------------------------------------------------------

 Count:5
```