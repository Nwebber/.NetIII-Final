ECHO off

rem batch file to run a script to create a db for final project

rem sqlcmd -S localhost -E -i movie_db.sql
sqlcmd -S Natasha\sqlexpress -E -i movie_db.sql
rem sqlcmd -S localhost\mssqlserver -E -i movie_db.sql

rem sqlcmd -S NATASHA\SQLEXPRESS01 -E -i movie_db.sql



ECHO .
ECHO if no errors appear DB was created
PAUSE