---
external help file: Perry.dll-Help.xml
Module Name: Perry
online version:
schema: 2.0.0
---

# Add-Perry

## SYNOPSIS
Start error logging.

## SYNTAX

```
Add-Perry [-Interactive] [-LogPath <String>] [-IncludeException] [-IncludeVariable] [<CommonParameters>]
```

## DESCRIPTION
Starts logging of errors with Perry. Use Get-Perry to see recent errors.

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-Perry
```

Log errors in the background.

### Example 2
```powershell
PS C:\> Add-Perry -Interactive
```

Log errors and output them as they happen to the console.

## PARAMETERS

### -IncludeException
Output exceptions related to the errors, otherwise only script stack is shown.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IncludeVariable
Parse and output variables from the error message.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Interactive
Errors will be output as warnings when they happen.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -LogPath
Path for logging errors to file.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
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
