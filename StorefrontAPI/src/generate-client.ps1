# ============================================================================
# Generate TypeScript Client from Storefront API
# ============================================================================
# Make sure the API is running before executing this script!
# ============================================================================

$ApiUrl = "https://localhost:7164"
$OutputPath = "frontend/src/api/storefront-client.ts"

Write-Host ""
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "  Generating TypeScript Client from Storefront API" -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

# Check if API is running
Write-Host "Checking if API is running at $ApiUrl..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$ApiUrl" -Method Get -TimeoutSec 5 -SkipCertificateCheck
    Write-Host "✓ API is running!" -ForegroundColor Green
} catch {
    Write-Host ""
    Write-Host "✗ API is NOT running!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please start the API first:" -ForegroundColor Yellow
    Write-Host "  cd src/Storefront.API" -ForegroundColor White
    Write-Host "  dotnet run" -ForegroundColor White
    Write-Host ""
    exit 1
}

# Check if NSwag CLI is installed
Write-Host "Checking for NSwag CLI..." -ForegroundColor Yellow
if (!(Get-Command nswag -ErrorAction SilentlyContinue)) {
    Write-Host "NSwag CLI not found. Installing..." -ForegroundColor Yellow
    dotnet tool install -g NSwag.ConsoleCore
} else {
    Write-Host "✓ NSwag CLI is installed" -ForegroundColor Green
}

# Create output directory
$OutputDir = Split-Path $OutputPath -Parent
if (!(Test-Path $OutputDir)) {
    Write-Host "Creating output directory: $OutputDir" -ForegroundColor Yellow
    New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null
}

Write-Host ""
Write-Host "Generating TypeScript client..." -ForegroundColor Yellow
Write-Host "  Input:  $ApiUrl/swagger/v1/swagger.json" -ForegroundColor Gray
Write-Host "  Output: $OutputPath" -ForegroundColor Gray
Write-Host ""

# Generate TypeScript client
nswag openapi2tsclient `
    /input:"$ApiUrl/swagger/v1/swagger.json" `
    /output:"$OutputPath" `
    /template:Fetch `
    /generateClientClasses:true `
    /generateClientInterfaces:true `
    /generateDtoTypes:true `
    /typeScriptVersion:5.0 `
    /dateTimeType:Date `
    /nullValue:Null `
    /useAbortSignal:true `
    /clientBaseClass:ClientBase

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "============================================================================" -ForegroundColor Green
    Write-Host "  ✅ TypeScript Client Generated Successfully!" -ForegroundColor Green
    Write-Host "============================================================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "File location: $OutputPath" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Usage Example:" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  import { ProductsClient, CartClient } from './api/storefront-client';" -ForegroundColor White
    Write-Host "" 
    Write-Host "  const productsClient = new ProductsClient('https://localhost:7164');" -ForegroundColor White
    Write-Host "  const products = await productsClient.getAllProducts();" -ForegroundColor White
    Write-Host ""
    Write-Host "  const cartClient = new CartClient('https://localhost:7164');" -ForegroundColor White
    Write-Host "  await cartClient.addToCart({ sessionId: 'abc', productId: 1, quantity: 2 });" -ForegroundColor White
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "✗ Failed to generate TypeScript client" -ForegroundColor Red
    Write-Host ""
}