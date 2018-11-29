Remove-Item -Path */bin/Release -Recurse
dotnet restore
dotnet clean
dotnet build -c Release
Remove-Item -Path .\PackedPackages\*
Get-ChildItem -Path */bin/Release/*.nupkg | Copy-Item -Destination PackedPackages\
