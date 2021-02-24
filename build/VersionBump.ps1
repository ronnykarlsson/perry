param(
	[Parameter(Mandatory = $true)]
	[String] $Version,

	[Parameter(Mandatory = $false)]
	[String] $ModuleFilePath = "$PSScriptRoot/../Perry/Perry.psd1"
)

if ($Version[0] -ne "v") {
	throw "Expected version tag, for example v1.0.0"
}

$moduleVersionRegex = "(?<=ModuleVersion *= *')[\d.]+(?=')"

$psdContent = $content = [IO.File]::ReadAllText($ModuleFilePath)
$currentModuleVersion = [Regex]::Match($psdContent, $moduleVersionRegex).Value
if (-not $currentModuleVersion) {
	throw "Current module version not found"
}

$versionNumber = $Version.Substring(1)

if ($versionNumber -eq $currentModuleVersion) {
	Write-Output "Version is already bumped."
	return
}

$psdContent = [Regex]::Replace($psdContent, $moduleVersionRegex, $versionNumber)
[IO.File]::WriteAllText($ModuleFilePath, $psdContent)
