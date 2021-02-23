# Perry

Perry is a PowerShell module intended to help with debugging, to get some error-information when working with code that doesn't give much information on its own.

## Installing from [PowerShell Gallery](https://www.powershellgallery.com/packages/Perry)

```powershell
Install-Module -Name Perry
```

## Example

```powershell
# Load Perry with Interactive-flag to output errors to console (alternatively there is Get-Perry or Add-Perry -LogPath)

Add-Perry -Interactive

# Run script with bug in it

function DoStuff {
	param ($InputStuff)

	return (10 / ($InputStuff - 7))
}

try {
	for ($i = 10; $i -gt 0; $i--) {
		$result = DoStuff -InputStuff $i
		"$i stuff: $result"
	}
}
catch {
	throw "Stuff not done."
}

```

```powershell
# What Perry does when running this example is to capture and output the error when it happens to easily find where it originates from.

PS> RunScript.ps1

10 result: 3.33333333333333
9 result: 5
8 result: 10
WARNING: Perry (DivideByZeroException): Attempted to divide by zero.
At line:3 char:12
+     return (10 / ($InputStuff - 7))
+            ~~~~~~~~~~~~~~~~~~~~~~~~
at DoStuff, <No file>: line 3
at <ScriptBlock>, <No file>: line 8
WARNING: Perry (RuntimeException): Stuff not done.
At line:13 char:5
+     throw "Stuff not done."
+     ~~~~~~~~~~~~~~~~~~~~~~~
at <ScriptBlock>, <No file>: line 13
Stuff not done.
At line:13 char:5
+     throw "Stuff not done."
+     ~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : OperationStopped: (Stuff not done.:String) [], RuntimeException
    + FullyQualifiedErrorId : Stuff not done.
```
