Summary
=======

Ivet is a command line tool created to generate a schema for a janusgraph database and apply this schema. It has been inspired from Doctrine behaviour.
The **generate** command creates a json file containing commands to execute. The file can be manually modified if you find migration is not complete or does not cover all you deserves.
To keep trace of all applied migrations, migration names are saved into janusgraph server when **upgrade** is called.
You can check at any moment the **list** of migrations both applied and not.

A **test** command is used to populate database either for personnal tests or unit tests. Do not use it in your production environment.

Available commands
=======
* **generate**
	* **dir*** Dlls to analyze
	* **output*** Directory where to put migration file
	* **ip** _localhost_
	* **port** _8182_
* **list** Display list of all migrations
	* **input*** Directory where to find migration files
	* **ip** _localhost_
	* **port** _8182_
* **upgrade**
	* **input*** Can be a file or a directory containing migrations to be applied
	* **ip** _localhost_
	* **port** _8182_
* test* _Must not be used in Prod. This command is only used to apply a schema to a Janusgraph server_
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
Add `Ivet.Model` to your project. You are now ready to tag your code.
All names and possibilities are listed in Janusgraph documentation at https://docs.janusgraph.org/schema/.

Tag a Vertex:
```
[Vertex]
public class MyVertex
{
    [PropertyKey]
    [PrimaryKey()]
    public string Id { get; set; }

    [PropertyKey(Name="NewName", Cardinality=Cardinality = Cardinality.SINGLE)]
    public string ANamedProperty { get; set; }

    [EdgeProperty]
    public List<Vertex3> AListProperty { get; private set; } = new List<Vertex3>();

    [EdgeProperty]
    public Vertex3[] AnArrayProperty { get; private set; } = Array.Empty<Vertex3>(); 
}
```


You can also specify a different name or a cardinality (details at https://docs.janusgraph.org/schema/#property-key-cardinality):
```
[PropertyKey(Name="NewName", Cardinality=Cardinality = Cardinality.SINGLE)]
public string ANamedProperty { get; set; }
```


Tag an Edge:
```
[Edge(typeof(MyVertex1), typeof(MyVertex2))]
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