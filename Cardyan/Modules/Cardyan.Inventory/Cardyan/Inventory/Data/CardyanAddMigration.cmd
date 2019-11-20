cd "D:\Documents\Visual Studio 2017\Projects\Cyxor\Solution\Cardyan\Cardyan.Inventory"
d:
cls
dotnet ef migrations add Initial -s "../Cardyan.Server" -c CardyanDbContext -o "Cardyan/Inventory/Data/Migrations/Initial" --json --prefix-output
pause