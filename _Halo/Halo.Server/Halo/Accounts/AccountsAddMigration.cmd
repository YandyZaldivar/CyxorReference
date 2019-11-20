cd "D:\Documents\Visual Studio 2017\Projects\Cyxor\Solution\_Halo\Halo.Server"
d:
cls
dotnet ef migrations add Initial -c AccountsDbContext -o "Halo/Accounts/Data/Migrations" --json --prefix-output
pause