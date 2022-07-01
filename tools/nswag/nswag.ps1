$MyInvocation.MyCommand.Path | Split-Path | Push-Location

Write-Host 'Installing Nswag...'
npm i -g nswag@13.15.5

Write-Host 'Generating api.generated.clients.ts...'
nswag run nswag.json /runtime:Net50

@("// @ts-nocheck") + (get-content .\..\..\src\Demo.WebUI\src\app\api\api.generated.clients.ts) | set-content .\..\..\src\Demo.WebUI\src\app\api\api.generated.clients.ts

Pop-Location