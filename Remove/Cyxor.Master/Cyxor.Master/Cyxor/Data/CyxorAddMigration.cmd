cd "D:\Documents\Visual Studio 2017\Projects\Cyxor\Solution\Cyxor.Master\Cyxor.Master"
d:
cls
dotnet ef migrations add Initial -c MasterDbContext -o "Cyxor/Data/Migrations" --json --prefix-output
pause