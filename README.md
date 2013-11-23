AzureLogSpelunker
=================

This tool aids in spelunking for answers in a Windows Azure Diagnostic Logs Table (WADLogsTable).

Use it to request the logs for a specific time period, then cache them locally in
a temporary SQLite database where you may filter them repeatedly with SQL queries
until you find what you're looking for.

Save a list of the SQL query filters you use most, and enable and disable them
at will.

See only the columns you're interested in, in the order you want.  Hide the rest.

Export the filtered data to XML and share your results.

## Requirements

Developed for Microsoft Visual Studio 2012 & .NET 4.5.  YMMV

## Installation

#Source
Clone this repository with a git client.
Run msbuild in the root of the project.  NuGet should fetch all dependencies.
AzureLogSpelunker.exe is the executable.

#Binary
If you don't want to build this yourself, there are binary installers in
the Releases directory.
* You'll still need to procure a compatible version of the .NET framework yourself.
* It's a per-user installation, so administrative privileges are unnecessary.
* The uninstaller does not remove your configuration settings.

## Configuration

All settings are stored in a SQLite database created in your Windows roaming profile.
e.g. C:\Users\you\AppData\Roaming\AzureLogSpelunker\AzureLogSpelunker.db