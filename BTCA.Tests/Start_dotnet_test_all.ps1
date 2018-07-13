
Write-Host ">>>> Running Repository unit tests ...  <<<<"
$scriptblock = 'dotnet test --filter Category=UnitTest.Repository'
Invoke-Expression $scriptblock

Write-Host ">>>> Running CompanyManager unit tests ....  <<<<"
$scriptblock = 'dotnet test --no-restore --no-build --filter Category=UnitTest.CompanyManager' 
Invoke-Expression $scriptblock

Write-Host ">>>> Running CompanyAddressManager unit tests ....  <<<<"
$scriptblock = 'dotnet test --no-restore --no-build --filter Category=UnitTest.CompanyAddressManager'
Invoke-Expression $scriptblock

Write-Host ">>>> Running WebApi Controller unit tests ....  <<<<"
$scriptblock = 'dotnet test --no-restore --no-build --filter Category=UnitTest.WebApiControllers'
Invoke-Expression $scriptblock

Write-Host ">>>> Running DailyLog integration tests ....  <<<<"
$scriptblock = 'dotnet test --no-restore --no-build --filter Category=Integration.DailyLogTests'
Invoke-Expression $scriptblock

Write-Host ">>>> Running CompanyManager integration tests ....  <<<<"
$scriptblock = 'dotnet test --no-restore --no-build --filter Category=Integration.CompanyManager'
Invoke-Expression $scriptblock

Write-Host ">>>> Running StateProvinceCodeManager integration tests ....  <<<<"
$scriptblock = 'dotnet test --no-restore --no-build --filter Category=Integration.StateProvinceCodeManager'
Invoke-Expression $scriptblock

Write-Host ">>>> Running CompanyAddressManager integration tests ....  <<<<"
$scriptblock = 'dotnet test --no-restore --no-build --filter Category=Integration.CompanyAddressManager'
Invoke-Expression $scriptblock