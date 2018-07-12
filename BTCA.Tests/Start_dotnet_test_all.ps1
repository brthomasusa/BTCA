Write-Host "Running WebApi unit tests ..."
$scriptblock = 'dotnet test --filter Category=UnitTest.WebApiControllers'
Invoke-Expression $scriptblock