cd "D:\Documents\Visual Studio 2017\Projects\Cyxor\Solution\Alimatic\Alimatic.Server"
d:
cls
dotnet ef migrations add Initial -c DataDin2DbContext -o "Alimatic/Modules/DataDin2/Data/Migrations" --json --prefix-output --msbuildprojectextensionspath "D:\Documents\Visual Studio 2017\Projects\Cyxor\_out\_obj\Alimatic.Server"
pause