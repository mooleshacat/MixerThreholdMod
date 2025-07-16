# MixerThreholdMod Constants Verification Script (Concise Reporting)
# Scans for constant declarations and usage in:
# - Any file named Constants.cs (any location)
# - Any .cs file within the Constants directory or its subdirectories
# - Any .cs file in the project, EXCLUDING ForCopilot and Scripts directories

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
if ((Split-Path $ScriptDir -Leaf) -ieq "Scripts") {
    $ProjectRoot = Split-Path $ScriptDir -Parent
} else {
    $ProjectRoot = $ScriptDir
}

Write-Host "Scanning project root: ${ProjectRoot}" -ForegroundColor DarkCyan

function Get-ConstantDeclarations {
    param([string]$Path)
    $constantFiles = @()
    $constantFiles += Get-ChildItem -Path $Path -Recurse -Include Constants.cs | Where-Object { $_.PSIsContainer -eq $false }
    $constantsDir = Join-Path $Path "Constants"
    if (Test-Path $constantsDir) {
        $constantFiles += Get-ChildItem -Path $constantsDir -Recurse -Include *.cs | Where-Object { $_.PSIsContainer -eq $false }
    }
    # Exclude ForCopilot directory
    $constantFiles = $constantFiles | Where-Object { $_.FullName -notmatch "[\\/](ForCopilot)[\\/]" }
    $constantFiles = $constantFiles | Sort-Object -Unique

    $declarations = @()
    foreach ($fileObj in $constantFiles) {
        $file = $fileObj.FullName
        $matches = Select-String -Path $file -Pattern 'public\s+const\s+(\w+)\s+(\w+)\s*='
        foreach ($match in $matches) {
            $type = $match.Matches[0].Groups[1].Value
            $name = $match.Matches[0].Groups[2].Value
            $declarations += [PSCustomObject]@{ File = $file; Type = $type; Name = $name }
        }
    }
    return $declarations
}

function Get-ConstantUsages {
    param([string[]]$ConstantNames, [string]$Path)
    $usages = @()
    # Scan all .cs files in project, excluding ForCopilot, Scripts, and Constants directories
    $files = Get-ChildItem -Path $Path -Recurse -Include *.cs | Where-Object {
        $_.PSIsContainer -eq $false -and
        $_.FullName -notmatch "[\\/](ForCopilot)[\\/]" -and
        $_.FullName -notmatch "[\\/](Scripts)[\\/]" -and
        $_.FullName -notmatch "[\\/](Constants)[\\/]" -and
        $_.Name -ne "Constants.cs"
    }
    $files = $files | Sort-Object -Unique

    # Debug output
    Write-Host "Total files to scan for usage: $($files.Count)" -ForegroundColor DarkGray
    Write-Host "Sample files being scanned (non-constant files):" -ForegroundColor DarkGray
    $files | Select-Object -First 5 | ForEach-Object { Write-Host "  $($_.FullName)" -ForegroundColor DarkGray }

    # Return both usages and files count
    $result = @{
        Usages = @()
        FilesCount = $files.Count
    }

    $pattern = ($ConstantNames | ForEach-Object { [regex]::Escape($_) }) -join "|"
    $i = 0
    foreach ($fileObj in $files) {
        $i++
        if ($i % 10 -eq 0) {
            Write-Host "Progress: Scanned $i of $($files.Count) files..." -ForegroundColor DarkGray
        }
        
        $file = $fileObj.FullName
        $matches = Select-String -Path $file -Pattern $pattern
        foreach ($match in $matches) {
            # Skip lines that are constant declarations
            if ($match.Line -notmatch 'public\s+const\s+') {
                foreach ($name in $ConstantNames) {
                    if ($match.Line -match "\b$name\b") {
                        $result.Usages += [PSCustomObject]@{ Name = $name; File = $file; Count = 1 }
                    }
                }
            }
        }
    }
    return $result
}

# --- Main Analysis ---
$constants = Get-ConstantDeclarations -Path $ProjectRoot
$constantNames = $constants | Select-Object -ExpandProperty Name
$duplicateNames = $constantNames | Group-Object | Where-Object { $_.Count -gt 1 } | Select-Object -ExpandProperty Name
$usageResult = Get-ConstantUsages -ConstantNames $constantNames -Path $ProjectRoot
$usages = $usageResult.Usages
$scannedFilesCount = $usageResult.FilesCount
$usedNames = $usages | Select-Object -ExpandProperty Name | Sort-Object -Unique
$unusedNames = $constantNames | Where-Object { $usedNames -notcontains $_ }

# Calculate usage statistics - now counts usage in NON-CONSTANT files
$fileStats = $constants | Group-Object File | ForEach-Object {
    $file = $_.Name
    $count = $_.Count
    # Get unique constants from this file that are used anywhere
    $usedFromThisFile = $constants | Where-Object { $_.File -eq $file } | Where-Object { $usedNames -contains $_.Name } | Select-Object -ExpandProperty Name -Unique
    $used = $usedFromThisFile.Count
    $percentUsed = if ($count -gt 0) { [math]::Round(100 * $used / $count, 1) } else { 0 }
    [PSCustomObject]@{ File = $file; ConstantCount = $count; UsedCount = $used; PercentUsed = $percentUsed }
}

# --- Concise Report ---
Write-Host "`n=== MixerThreholdMod Constants Verification Summary ===" -ForegroundColor DarkCyan
Write-Host ("Total constants: {0}" -f $constants.Count) -ForegroundColor Gray
Write-Host ("Total unique constant names: {0}" -f ($constantNames | Sort-Object -Unique).Count) -ForegroundColor Gray
Write-Host ("Total constant files scanned: {0}" -f ($fileStats | Measure-Object).Count) -ForegroundColor Gray
Write-Host ("Total code files scanned for usage: {0}" -f $scannedFilesCount) -ForegroundColor Gray
Write-Host ("Total duplicate constant names: {0}" -f $duplicateNames.Count) -ForegroundColor Red
Write-Host ("Total unused constants: {0}" -f $unusedNames.Count) -ForegroundColor Red

# Show some example used constants for verification
if ($usedNames.Count -gt 0) {
    Write-Host "`nExample constants that ARE being used:" -ForegroundColor DarkCyan
    $usedNames | Select-Object -First 5 | ForEach-Object { 
        $usage = $usages | Where-Object { $_.Name -eq $_ } | Select-Object -First 1
        Write-Host ("  {0} - used in {1}" -f $_, [System.IO.Path]::GetFileName($usage.File)) -ForegroundColor Green 
    }
}

Write-Host "`nPer-file stats (top 10 by constant count):" -ForegroundColor DarkCyan
$fileStats | Sort-Object -Property ConstantCount -Descending | Select-Object -First 10 | ForEach-Object {
    $color = if ($_.PercentUsed -eq 0) { "Red" } elseif ($_.PercentUsed -lt 10) { "DarkYellow" } else { "Green" }
    Write-Host ("{0,-80} {1,5} {2,5} {3,8}%" -f $_.File, $_.ConstantCount, $_.UsedCount, $_.PercentUsed) -ForegroundColor $color
}

# Top N most duplicated constants
$N = 10
$dupGroups = $constants | Group-Object Name | Where-Object { $_.Count -gt 1 }
if ($dupGroups.Count -gt 0) {
    Write-Host ("`nTop {0} most duplicated constants:" -f $N) -ForegroundColor DarkCyan
    $dupGroups | Sort-Object -Property Count -Descending | Select-Object -First $N | ForEach-Object {
        $files = ($_.Group | Select-Object -ExpandProperty File | Sort-Object -Unique) -join ", "
        Write-Host ("  {0} ({1} times) in: {2}" -f $_.Name, $_.Count, $files) -ForegroundColor DarkYellow
    }
}

# Top N unused constants
if ($unusedNames.Count -gt 0) {
    Write-Host ("`nTop {0} unused constants:" -f $N) -ForegroundColor DarkCyan
    $unusedNames | Select-Object -First $N | ForEach-Object { Write-Host ("  {0}" -f $_) -ForegroundColor Red }
    if ($unusedNames.Count -gt $N) { 
        $remaining = $unusedNames.Count - $N
        Write-Host ("  ... ({0} more unused constants not shown)" -f $remaining) -ForegroundColor DarkGray 
    }
}

# Files with 0 used constants
$zeroUsedFiles = $fileStats | Where-Object { $_.UsedCount -eq 0 }
if ($zeroUsedFiles.Count -gt 0) {
    Write-Host "`nFiles with 0 used constants:" -ForegroundColor DarkCyan
    $displayCount = [Math]::Min($zeroUsedFiles.Count, 10)
    $zeroUsedFiles | Select-Object -First $displayCount | ForEach-Object { Write-Host ("  {0}" -f $_.File) -ForegroundColor Red }
    if ($zeroUsedFiles.Count -gt $displayCount) {
        $remaining = $zeroUsedFiles.Count - $displayCount
        Write-Host ("  ... ({0} more files with 0 used constants)" -f $remaining) -ForegroundColor DarkGray
    }
}

# Separation of concerns issues (top 10)
$overlap = $constants | Group-Object Name | Where-Object { $_.Count -gt 1 }
if ($overlap.Count -gt 0) {
    Write-Host "`nConstants with overlapping names (top 10):" -ForegroundColor DarkCyan
    $overlap | Sort-Object -Property Count -Descending | Select-Object -First $N | ForEach-Object {
        $files = ($_.Group | Select-Object -ExpandProperty File | Sort-Object -Unique) -join ", "
        Write-Host ("  {0} in: {1}" -f $_.Name, $files) -ForegroundColor DarkYellow
    }
    if ($overlap.Count -gt $N) { 
        $remaining = $overlap.Count - $N
        Write-Host ("  ... ({0} more overlapping constants)" -f $remaining) -ForegroundColor DarkGray 
    }
}

# AllConstants.cs analysis
$allConstantsFile = $fileStats | Where-Object { $_.File -match "AllConstants.cs" }
if ($allConstantsFile) {
    if ($allConstantsFile.UsedCount -eq 0) {
        Write-Host ("`nAllConstants.cs: {0} constants, {1} used" -f $allConstantsFile.ConstantCount, $allConstantsFile.UsedCount) -ForegroundColor Red
        Write-Host "  Note: AllConstants.cs may be a holding file or contains unused/duplicated constants." -ForegroundColor Red
    } else {
        Write-Host ("`nAllConstants.cs: {0} constants, {1} used" -f $allConstantsFile.ConstantCount, $allConstantsFile.UsedCount) -ForegroundColor DarkYellow
    }
}

Write-Host "`n=== End of comprehensive report ===" -ForegroundColor DarkCyan
Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
Read-Host