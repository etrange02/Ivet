Ivet
=======

Ivet is command line tool to generate a schema for a janusgraph database and apply this schema. It has been inspired from Doctrine behaviour.

Available commands:
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
