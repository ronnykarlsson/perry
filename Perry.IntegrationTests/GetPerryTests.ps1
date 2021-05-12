BeforeAll {
	function DivideFunction {
		param ([int] $InputValue)

		return (10 / ($InputValue - 7))
	}

	function BuggyCode {
		[CmdletBinding()]
		param()

		try {
			for ($i = 10; $i -gt 0; $i--) {
				$result = DivideFunction -InputValue $i
				"$i stuff: $result"
			}
		}
		catch {
			Write-Output "Unable to run code: $_"
		}
	}
}

Describe "Get-Perry" {
	BeforeEach {
		Remove-Perry
	}

	It "Get script error from buggy code" {
		Add-Perry
		BuggyCode

		$perryErrorMessage = (Get-Perry)[0].ErrorMessage
		$perryErrorMessage | Should -BeLike "*Attempted to divide by zero.*"
		$perryErrorMessage | Should -BeLike "*return (10 / (`$InputValue - 7))*"
		$perryErrorMessage | Should -BeLike "*at DivideFunction, *GetPerryTests.ps1: line *"
		$perryErrorMessage | Should -BeLike "*at BuggyCode, *GetPerryTests.ps1: line *"
	}
}
