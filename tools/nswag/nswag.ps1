Write-Host 'Installing Nswag...'
npm i -g nswag@13.11.1

Write-Host 'Generating api.service.ts...'
nswag run nswag.json /runtime:Net50