# build.ps1

Write-Host "🔨 Restoring packages..."
dotnet restore WebAPI1.sln

Write-Host "🔧 Building solution..."
dotnet build WebAPI1.sln --no-restore --configuration Release

Write-Host "🧪 Running unit tests..."
dotnet test WebAPI.UnitTests/WebAPI.UnitTests.csproj --no-build --configuration Release --logger trx --results-directory ./TestResults

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build and tests completed successfully." -ForegroundColor Green
} else {
    Write-Host "❌ Build or tests failed." -ForegroundColor Red
    exit 1
}
