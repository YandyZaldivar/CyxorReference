@echo off

cls

set /p "module=Enter module name: "

set "project=Cardyan.%module%"

set /p "migration=Enter migration name: "

set "master=Cardyan.Master"

echo We're creating %migration% migration in %project%...

dotnet ef migrations add %migration% -s "%master%.csproj" -p "../%project%/%project%.csproj" -c CardyanDbContext -o "../%project%/Cardyan/%module%/Data/Migrations" --msbuildprojectextensionspath "%CD%\..\..\..\_out\_obj\%master%" --json --prefix-output

pause