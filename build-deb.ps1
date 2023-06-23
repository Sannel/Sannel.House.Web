#!/usr/bin/env pwsh
param(
	[Parameter(mandatory=$true)]
	[string]$version,
	[Parameter(mandatory=$true)]
	[int]$buildNumber
)

$rootPath = Join-Path -Path "build" -ChildPath "sannel.house.web_${version}-${buildNumber}_all"
New-Item -ItemType Directory $rootPath


$path = [System.IO.Path]::Combine($rootPath,"etc","Sannel","House")
New-Item -ItemType Directory -Path $path

$path = [System.IO.Path]::Combine($rootPath,"var","lib","Sannel","House","Web")
New-Item -ItemType Directory -Path $path

dotnet publish -c Release --sc -o $path src/Sannel.House.Web/Sannel.House.Web.csproj

Remove-Item -Force ([System.IO.Path]::Combine($path, "wwwroot", "appsettings.json.br"))
Remove-Item -Force ([System.IO.Path]::Combine($path, "wwwroot", "appsettings.json.gz"))

$content = "Package: sannel.house.web
Version: $version-$buildNumber
Maintainer: Adam Holt <holtsoftware@outlook.com>
Architecture: all
Homepage: https://github.com/Sannel/Sannel.House.Web
Description: Client application for Sannel House
"

$path = [System.IO.Path]::Combine($rootPath,"DEBIAN")
New-Item -ItemType Directory -Path $path
$debianDir = $path
$path = Join-Path -Path $path -ChildPath "control"
Set-Content -Path $path -Value $content

$content = "/var/lib/Sannel/House/Web/wwwroot/appsettings.json"
$path = Join-Path -Path $debianDir -ChildPath "conffiles"
Set-Content -Path $path -Value $content

$content = "#!/bin/bash
" -replace "`r",""
$path = Join-Path -Path $debianDir -ChildPath "postinst"
Set-Content -Path $path -Value $content
chmod 0775 $path


dpkg-deb --build --root-owner-group -Zgzip $rootPath

aptly repo add sannel-repository "$rootPath.deb"
