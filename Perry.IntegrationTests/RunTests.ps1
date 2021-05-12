[CmdletBinding()]
param(
	[Parameter(Mandatory = $true)]
	[String] $Path
)

Import-Module .\pester.5.1.1\Pester.psd1 -Force
Import-Module .\Perry.psd1 -Force

$testResult = Invoke-Pester -Path $Path -PassThru

if (-not $testResult) {
	throw "Error running tests."
}

if ($testResult.FailedCount) {
	throw "Failed tests: " + $testResult.FailedCount
}
