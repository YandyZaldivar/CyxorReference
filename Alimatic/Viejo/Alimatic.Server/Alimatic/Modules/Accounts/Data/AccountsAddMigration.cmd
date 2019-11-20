cd "D:\Documents\Visual Studio 2017\Projects\Cyxor\Solution\Alimatic\Alimatic.Server"
d:
cls
dotnet ef migrations add Initial -c AccountsDbContext -o "Alimatic/Modules/Accounts/Data/Migrations" --json --prefix-output
pause