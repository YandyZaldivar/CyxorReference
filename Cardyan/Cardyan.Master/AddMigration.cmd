@echo off

cls

set /p "module=Enter module name: "

set "project=Cardyan.%module%"

set /p "migration=Enter migration name: "

set "master=Cardyan.Master"

if exist "../../../_out/_obj" (set output=--msbuildprojectextensionspath "../../../_out/_obj/%master%")

echo We're creating %migration% migration in %project%...

dotnet ef migrations add %migration% -s "%master%.csproj" -p "../Modules/%project%/%project%.csproj" -c CardyanDbContext -o "Cardyan/%module%/Data/Migrations" %output% --json --prefix-output

pause