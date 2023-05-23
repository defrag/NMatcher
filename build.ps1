$version=$args[0]

if (!$version)
{
    Write-Output "Please provide version as first argument"
    exit 1
}

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

Write-Output "Building projects..."
Get-ChildItem -Path .\src -Filter *.csproj -Recurse | ForEach-Object { 
	& dotnet build $_.FullName --no-cache --no-restore -c Release /p:Version=$version
	if($LASTEXITCODE -ne 0) { exit 1 }
}

# run tests
Write-Output "Running tests..."
Get-ChildItem -Path .\tests -Filter *.csproj -Recurse | ForEach-Object {
	 & dotnet test --no-build -c Release $_.FullName
	 if($LASTEXITCODE -ne 0) { exit 1 }
}

Write-Output "Packaging files..."
# run pack on all *.csproj files in the src folder including 2>1 to redirect stderr to stdout for badly behaved tools
Get-ChildItem -Path .\src -Filter *.csproj -Recurse | ForEach-Object {

	& dotnet pack --no-build $_.FullName -c Release -o .\artifacts /p:PackageVersion=$version
	
	if($LASTEXITCODE -ne 0) { exit 1 }
}
