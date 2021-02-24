param(
	[Parameter(Mandatory = $true)]
	[String] $Version,

	[Parameter(Mandatory = $false)]
	[String] $ModuleFilePath = "$PSScriptRoot\..\Perry\Perry.psd1"
)

if ($Version[0] -ne "v") {
	throw "Expected version tag, for example v1.0.0"
}

$moduleVersionRegex = "(?<=ModuleVersion\s*=\s*')[\d.]+(?=')"

$psdContent = Get-Content -Path $ModuleFilePath
$currentModuleVersion = [Regex]::Match($fileContent, $moduleVersionRegex).Value
if (-not $currentModuleVersion) {
	Write-Warning "Invalid file: $psdContent"
	throw "Current module version not found"
}

$versionNumber = $Version.Substring(1)

if ($versionNumber -eq $currentModuleVersion) {
	Write-Output "Version is already bumped."
	return
}

$psdContent -replace $moduleVersionRegex, $versionNumber | Out-File $ModuleFilePath
