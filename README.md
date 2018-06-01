About
=====
The South Carolina Law Enforcement Division publishes a log of all flights undertaken using one of their helicopters. This utility scrapes all those pages and publishes an Atom feed to Azure Blob Storage, which is then piped into the [@SLEDHelicopter](https://twitter.com/SLEDHelicopter) account in a roughly anthropomorphised way.

Info
----
According to [FAA records](http://registry.faa.gov/aircraftinquiry/Name_Results.aspx?Nametxt=SOUTH+CAROLINA+LAW+ENFORCEMENT+DIVISION&sort_option=1&PageNo=1) SLED currently has three aircraft registered:

* 500SC - A McDonnell Douglas 369E registered in 1990
* 502SL - A Hughes 369A registered in 2004
* 504SL - A 2014 MD Helicopters 369E registered in 2015

Usage
-----
Be sure to replace the `StorageConnectionString` value in SLEDHelicopter.Exporter/App.config with your Azure Storage connection string.
