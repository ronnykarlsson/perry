---
external help file: Perry.dll-Help.xml
Module Name: Perry
online version:
schema: 2.0.0
---

# Add-PerryStackTraceFilter

## SYNOPSIS
Add filter to the stack trace.

## SYNTAX

```
Add-PerryStackTraceFilter -Pattern <String> -Replace <String> [<CommonParameters>]
```

## DESCRIPTION
Add filter for updating the strack trace. This can be useful to keep clutter away in some instances. By default Pester-errors are filtered out.

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-PerryStackTraceFilter -Pattern "\nat[^$]+Modules\\Pester[^$]+" -Replace "`n"
```

Filter out errors from Pester.

## PARAMETERS

### -Pattern
Regex pattern to match.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Replace
Replace matches with this string.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
