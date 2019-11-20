cd "D:\Documents\Visual Studio 2017\Projects\Cyxor\Solution\_Halo\Halo.Server"
d:
cls
dotnet ef migrations add Initial -c HaloDbContext -o "Halo/Data/Migrations" --json --prefix-output
pause