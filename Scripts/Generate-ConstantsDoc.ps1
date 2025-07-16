# MixerThreholdMod DevOps Tool: Constants Documentation Generator
# Auto-generates markdown documentation for all constants in the project
# Creates a comprehensive reference guide with categorization and usage examples
# Excludes: ForCopilot, ForConstants, and Legacy directories

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
if ((Split-Path $ScriptDir -Leaf) -ieq "Scripts") {
    $ProjectRoot = Split-Path $ScriptDir -Parent
} else {
    $ProjectRoot = $ScriptDir
}

Write-Host "Generating constants documentation for: $ProjectRoot" -ForegroundColor DarkCyan
Write-Host "Excluding: ForCopilot directory" -ForegroundColor DarkGray

# Function to get constant declarations with documentation
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
        $content = Get-Content -Path $file -Raw
        $lines = Get-Content -Path $file
        
        # Find constants with preceding comments
        for ($i = 0; $i -lt $lines.Count; $i++) {
            if ($lines[$i] -match 'public\s+const\s+(\w+)\s+(\w+)\s*=\s*(.+);') {
                $type = $matches[1]
                $name = $matches[2]
                $value = $matches[3].Trim()
                
                # Look for XML documentation or comments above
                $documentation = ""
                $j = $i - 1
                $xmlDoc = @()
                
                # Check for XML documentation
                while ($j -ge 0 -and $lines[$j] -match '^\s*///') {
                    $xmlDoc = @($lines[$j]) + $xmlDoc
                    $j--
                }
                
                # If no XML doc, check for regular comments
                if ($xmlDoc.Count -eq 0) {
                    $j = $i - 1
                    while ($j -ge 0 -and ($lines[$j] -match '^\s*//' -or $lines[$j] -match '^\s*$')) {
                        if ($lines[$j] -match '^\s*//\s*(.*)') {
                            $documentation = $matches[1].Trim() + " " + $documentation
                        }
                        $j--
                    }
                } else {
                    # Parse XML documentation
                    $xmlDocString = $xmlDoc -join "`n"
                    if ($xmlDocString -match '<summary>\s*(.*?)\s*</summary>') {
                        $documentation = $matches[1] -replace '^\s*///\s*', '' -replace '\s+', ' '
                    }
                }
                
                $documentation = $documentation.Trim()
                
                # Categorize by file name
                $category = "General"
                $fileName = [System.IO.Path]::GetFileNameWithoutExtension($file)
                if ($fileName -match "(\w+)Constants") {
                    $category = $matches[1]
                }
                
                $declarations += [PSCustomObject]@{
                    File = $file
                    FileName = [System.IO.Path]::GetFileName($file)
                    Category = $category
                    Type = $type
                    Name = $name
                    Value = $value
                    Documentation = $documentation
                    LineNumber = $i + 1
                }
            }
        }
    }
    
    return $declarations
}

# Function to find constant usages
function Get-ConstantUsages {
    param([string]$constantName, [string]$Path)
    
    $usages = @()
    $files = Get-ChildItem -Path $Path -Recurse -Include *.cs | Where-Object {
        $_.PSIsContainer -eq $false -and
        $_.FullName -notmatch "[\\/](ForCopilot)[\\/]" -and
        $_.FullName -notmatch "[\\/](Scripts)[\\/]" -and
        $_.FullName -notmatch "[\\/](Constants)[\\/]" -and
        $_.Name -ne "Constants.cs"
    }
    
    foreach ($file in $files) {
        $content = Get-Content -Path $file.FullName -Raw
        if ($content -match "\b$constantName\b") {
            $lines = $content -split "`n"
            for ($i = 0; $i -lt $lines.Count; $i++) {
                if ($lines[$i] -match "\b$constantName\b" -and $lines[$i] -notmatch "public\s+const") {
                    $usages += [PSCustomObject]@{
                        File = [System.IO.Path]::GetFileName($file.FullName)
                        LineNumber = $i + 1
                        Context = $lines[$i].Trim()
                    }
                }
            }
        }
    }
    
    return $usages | Select-Object -First 3  # Limit to 3 examples
}

Write-Host "Scanning for constants..." -ForegroundColor DarkGray
$constants = Get-ConstantDeclarations -Path $ProjectRoot

Write-Host ("Found {0} constants in {1} files" -f $constants.Count, ($constants | Select-Object -ExpandProperty File -Unique).Count) -ForegroundColor Gray

# Generate markdown documentation
$markdown = @"
# MixerThreholdMod Constants Reference

This document provides a comprehensive reference for all constants defined in the MixerThreholdMod project.

**Generated**: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")  
**Total Constants**: $($constants.Count)  
**Files Scanned**: $(($constants | Select-Object -ExpandProperty File -Unique).Count)

## Table of Contents

"@

# Get unique categories
$categories = $constants | Select-Object -ExpandProperty Category -Unique | Sort-Object

foreach ($category in $categories) {
    $categoryConstants = $constants | Where-Object { $_.Category -eq $category }
    $markdown += "- [$category Constants](#$($category.ToLower())-constants) ($($categoryConstants.Count) constants)`n"
}

$markdown += "`n---`n"

# Generate sections for each category
foreach ($category in $categories) {
    $categoryConstants = $constants | Where-Object { $_.Category -eq $category } | Sort-Object Name
    
    $markdown += @"

## $category Constants

"@
    
    # Group by file within category
    $fileGroups = $categoryConstants | Group-Object FileName
    
    foreach ($fileGroup in $fileGroups) {
        $markdown += @"
### $($fileGroup.Name)

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
"@
        
        foreach ($const in $fileGroup.Group) {
            $value = $const.Value
            # Escape markdown special characters
            $value = $value -replace '\|', '\|' -replace '<', '&lt;' -replace '>', '&gt;'
            
            # Truncate long values
            if ($value.Length -gt 50) {
                $value = $value.Substring(0, 47) + "..."
            }
            
            $description = if ($const.Documentation) { $const.Documentation } else { "_No description_" }
            
            $markdown += "| ``$($const.Name)`` | $($const.Type) | ``$value`` | $description |`n"
        }
        
        $markdown += "`n"
    }
}

# Add usage statistics section
Write-Host "Analyzing constant usage..." -ForegroundColor DarkGray

$markdown += @"

## Usage Statistics

### Most Duplicated Constants

| Constant | Occurrences | Files |
|----------|-------------|-------|
"@

$duplicates = $constants | Group-Object Name | Where-Object { $_.Count -gt 1 } | Sort-Object Count -Descending | Select-Object -First 10

foreach ($dup in $duplicates) {
    $files = ($dup.Group | Select-Object -ExpandProperty FileName -Unique) -join ", "
    $markdown += "| ``$($dup.Name)`` | $($dup.Count) | $files |`n"
}

# Add constants by type
$markdown += @"

### Constants by Type

| Type | Count | Percentage |
|------|-------|------------|
"@

$typeStats = $constants | Group-Object Type | Sort-Object Count -Descending

foreach ($type in $typeStats) {
    $percentage = [Math]::Round(($type.Count / $constants.Count) * 100, 1)
    $markdown += "| $($type.Name) | $($type.Count) | $percentage% |`n"
}

# Add usage examples for some constants
$markdown += @"

## Usage Examples

Below are examples of how some constants are used in the codebase:

"@
# Select a few important constants to show usage
$importantConstants = $constants | Where-Object { 
    $_.Name -match "MOD_NAME|MOD_VERSION|LOG_LEVEL|SAVE_FILENAME|TIMEOUT" 
} | Select-Object -First 5

foreach ($const in $importantConstants) {
    Write-Host ("Finding usages for {0}..." -f $const.Name) -ForegroundColor DarkGray
    $usages = Get-ConstantUsages -constantName $const.Name -Path $ProjectRoot
    
    if ($usages.Count -gt 0) {
        $markdown += @"
### $($const.Name)

**Definition**: ``$($const.Type) $($const.Name) = $($const.Value)``

**Usage Examples**:
"@ + '```' + "`n"
        foreach ($usage in $usages) {
            $markdown += "// $($usage.File):$($usage.LineNumber)`n"
            $markdown += "$($usage.Context)`n`n"
        }
        $markdown += '```' + "`n`n"
    }
}

# Add appendix with all constants alphabetically
$markdown += @"

## Appendix: All Constants (Alphabetical)

<details>
<summary>Click to expand full alphabetical list</summary>

| Constant | File | Type | Value |
|----------|------|------|-------|
"@

$allConstantsSorted = $constants | Sort-Object Name

foreach ($const in $allConstantsSorted) {
    $value = $const.Value
    $value = $value -replace '\|', '\|' -replace '<', '&lt;' -replace '>', '&gt;'
    if ($value.Length -gt 40) {
        $value = $value.Substring(0, 37) + "..."
    }
    
    $markdown += "| ``$($const.Name)`` | $($const.FileName) | $($const.Type) | ``$value`` |`n"
}

$markdown += @"

</details>

---

_This documentation was auto-generated by Generate-ConstantsDoc.ps1_
"@

# Save documentation
$outputPath = Join-Path $ProjectRoot "Constants-Reference.md"
$markdown | Out-File -FilePath $outputPath -Encoding UTF8

Write-Host "`n✅ Documentation generated successfully!" -ForegroundColor Green
Write-Host "📄 Output saved to: $outputPath" -ForegroundColor Gray
Write-Host ("📊 Documented {0} constants across {1} categories" -f $constants.Count, $categories.Count) -ForegroundColor Gray

# Summary
Write-Host "`n📈 Summary:" -ForegroundColor DarkCyan
Write-Host ("  Total constants: {0}" -f $constants.Count) -ForegroundColor Gray
Write-Host ("  Categories: {0}" -f ($categories -join ", ")) -ForegroundColor Gray
Write-Host ("  Files processed: {0}" -f ($constants | Select-Object -ExpandProperty File -Unique).Count) -ForegroundColor Gray
Write-Host ("  Constants with documentation: {0}" -f ($constants | Where-Object { $_.Documentation }).Count) -ForegroundColor Gray

if (($constants | Where-Object { -not $_.Documentation }).Count -gt 0) {
    Write-Host "`n⚠️  Warning: $( ($constants | Where-Object { -not $_.Documentation }).Count) constants lack documentation" -ForegroundColor DarkYellow
}

Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
Read-Host