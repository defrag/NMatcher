
echo "build: Version is $version"


Push-Location $PSScriptRoot

if(Test-Path .\artifacts) {
	echo "build: Cleaning .\artifacts"
	Remove-Item .\artifacts -Force -Recurse
}
# run restore on all *.csproj files in the src folder including 2>1 to redirect stderr to stdout for badly behaved tools
Get-ChildItem -Path .\src -Filter *.csproj -Recurse | ForEach-Object { 
	& dotnet restore $_.FullName --no-cache
	if($LASTEXITCODE -ne 0) { exit 1 }
}

# run tests
Get-ChildItem -Path .\tests -Filter *.csproj -Recurse | ForEach-Object {
	 & dotnet test -c Release $_.FullName
	 if($LASTEXITCODE -ne 0) { exit 1 }
}

# run pack on all *.csproj files in the src folder including 2>1 to redirect stderr to stdout for badly behaved tools
Get-ChildItem -Path .\src -Filter *.csproj -Recurse | ForEach-Object {

	& dotnet pack $_.FullName -c Release -o .\artifacts
	
	if($LASTEXITCODE -ne 0) { exit 1 }
}
