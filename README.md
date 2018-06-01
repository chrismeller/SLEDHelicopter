About
=====
The South Carolina Law Enforcement Division publishes a log of all flights undertaken using one of their helicopters. This utility scrapes all those pages and publishes an Atom feed to Azure Blob Storage, which is then piped into the [@SLEDHelicopter](https://twitter.com/SLEDHelicopter) account in a roughly anthropomorphised way.

Usage
-----
Be sure to replace the `StorageConnectionString` value in SLEDHelicopter.Exporter/App.config with your Azure Storage connection string.
