@echo off

cls

set /p "ppath=Enter project path: "

set /p "project=Enter project name: "

set /p "context=Enter DbContext name: "

set /p "mpath=Enter migration path: "

set /p "migration=Enter migration name: "

set "startup=Cyxor.Tools"

if exist "../../../../_out/_obj" (set output=--msbuildprojectextensionspath "../../../../_out/_obj/%startup%")

echo We're creating %migration% migration in %project%...

dotnet ef migrations add %migration% -s "%startup%.csproj" -p "%ppath%/%project%.csproj" -c %context% -o "%mpath%" %output% --json --prefix-output

pause