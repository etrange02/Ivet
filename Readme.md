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
https://hub.docker.com/r/dlecoconnier/ivet
