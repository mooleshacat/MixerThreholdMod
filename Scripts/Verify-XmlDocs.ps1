# MixerThreholdMod DevOps Tool: XML Documentation Verifier
# Ensures all public methods, classes, and properties have proper XML documentation
# Validates XML documentation quality and completeness
# Excludes: ForCopilot, ForConstants, Scripts, and Legacy directories

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
if ((Split-Path $ScriptDir -Leaf) -ieq "Scripts") {
    $ProjectRoot = Split-Path $ScriptDir -Parent
} else {
    $ProjectRoot = $ScriptDir
}

Write-Host "Verifying XML documentation in: $ProjectRoot" -ForegroundColor DarkCyan
Write-Host "Excluding: ForCopilot, ForConstants, Scripts, and Legacy directories" -ForegroundColor DarkGray

# Find all C# files
$files = Get-ChildItem -Path $ProjectRoot -Recurse -Include *.cs | Where-Object {
    $_.PSIsContainer -eq $false -and
    $_.FullName -notmatch "[\\/](ForCopilot)[\\/]" -and
    $_.FullName -notmatch "[\\/](ForConstants)[\\/]" -and
    $_.FullName -notmatch "[\\/](Scripts)[\\/]" -and
    $_.FullName -notmatch "[\\/](Legacy)[\\/]"
}

Write-Host "Files to analyze: $($files.Count)" -ForegroundColor DarkGray

# Function to extract public members and their documentation
function Get-PublicMembers {
    param($filePath)
    
    $content = Get-Content -Path $filePath -Raw
    $lines = Get-Content -Path $filePath
    $members = @()
    
    # Patterns for different member types
    $patterns = @{
        'Class' = '(?m)^\s*public\s+(?:static\s+|abstract\s+|sealed\s+)*class\s+([A-Za-z0-9_<>]+)'
        'Method' = '(?m)^\s*public\s+(?:static\s+|virtual\s+|override\s+|async\s+)*(?:Task<[^>]+>|Task|void|[A-Za-z0-9_<>\[\]]+)\s+([A-Za-z0-9_]+)\s*\([^)]*\)'
        'Property' = '(?m)^\s*public\s+(?:static\s+)*([A-Za-z0-9_<>\[\]]+)\s+([A-Za-z0-9_]+)\s*\{\s*(?:get|set)'
        'Field' = '(?m)^\s*public\s+(?:static\s+|readonly\s+)*([A-Za-z0-9_<>\[\]]+)\s+([A-Za-z0-9_]+)\s*(?:=|;)'
        'Event' = '(?m)^\s*public\s+event\s+([A-Za-z0-9_<>\[\]]+)\s+([A-Za-z0-9_]+)'
    }
    
    foreach ($type in $patterns.Keys) {
        $matches = [regex]::Matches($content, $patterns[$type])
        
        foreach ($match in $matches) {
            $memberName = if ($type -eq 'Class') { $match.Groups[1].Value } else { $match.Groups[2].Value }
            $lineNumber = ($content.Substring(0, $match.Index) -split "`n").Count
            
            # Skip certain members
            if ($memberName -match '^(get_|set_|add_|remove_)' -or 
                $memberName -eq 'ToString' -or 
                $memberName -eq 'GetHashCode' -or 
                $memberName -eq 'Equals' -or
                $memberName -eq 'GetType') {
                continue
            }
            
            # Check for XML documentation above the member
            $hasXmlDoc = $false
            $xmlDocLines = @()
            $docQuality = "None"
            
            # Look for XML documentation in preceding lines
            $startLine = [Math]::Max(0, $lineNumber - 10)
            for ($i = $lineNumber - 2; $i -ge $startLine; $i--) {
                if ($i -ge 0 -and $i -lt $lines.Count) {
                    if ($lines[$i] -match '^\s*///') {
                        $xmlDocLines = @($lines[$i]) + $xmlDocLines
                        $hasXmlDoc = $true
                    } elseif ($lines[$i] -match '^\s*$' -or $lines[$i] -match '^\s*\[') {
                        # Empty line or attribute, continue
                        continue
                    } else {
                        # Non-XML doc line, stop looking
                        break
                    }
                }
            }
            
            # Analyze documentation quality
            if ($hasXmlDoc) {
                $xmlDocText = $xmlDocLines -join "`n"
                
                # Check for summary
                $hasSummary = $xmlDocText -match '<summary>'
                
                # Check for param docs (if it's a method with parameters)
                $hasParamDocs = $true
                if ($type -eq 'Method' -and $match.Groups[0].Value -match '\([^)]+\)') {
                    $paramMatches = [regex]::Matches($match.Groups[0].Value, '\b([A-Za-z0-9_]+)\s*(?=,|\))')
                    foreach ($paramMatch in $paramMatches) {
                        $paramName = $paramMatch.Groups[1].Value
                        if ($paramName -ne 'void' -and $xmlDocText -notmatch "<param name=`"$paramName`">") {
                            $hasParamDocs = $false
                            break
                        }
                    }
                }
                
                # Check for return docs (if method returns something)
                $hasReturnDocs = $true
                if ($type -eq 'Method' -and $match.Groups[0].Value -notmatch '\bvoid\s+' -and 
                    $match.Groups[0].Value -notmatch '\b(constructor|~)' -and
                    $xmlDocText -notmatch '<returns>') {
                    $hasReturnDocs = $false
                }
                
                # Determine quality
                if ($hasSummary -and $hasParamDocs -and $hasReturnDocs) {
                    $docQuality = "Complete"
                } elseif ($hasSummary) {
                    $docQuality = "Basic"
                } else {
                    $docQuality = "Minimal"
                }
            }
            
            # Extract brief description from summary
            $description = ""
            if ($hasXmlDoc -and $xmlDocLines) {
                $summaryMatch = ($xmlDocLines -join "`n") -match '<summary>\s*(.*?)\s*</summary>'
                if ($summaryMatch) {
                    $description = $matches[1] -replace '^\s*///\s*', '' -replace '\s+', ' '
                    $description = $description.Trim()
                }
            }
            
            $members += [PSCustomObject]@{
                File = $filePath
                Type = $type
                Name = $memberName
                LineNumber = $lineNumber
                HasXmlDoc = $hasXmlDoc
                DocQuality = $docQuality
                Description = $description
                XmlDocLines = $xmlDocLines
            }
        }
    }
    
    return $members
}

# Analyze all files
$allMembers = @()
$i = 0

foreach ($file in $files) {
    $i++
    if ($i % 10 -eq 0) {
        Write-Host "Progress: Analyzed $i of $($files.Count) files..." -ForegroundColor DarkGray
    }
    
    $members = Get-PublicMembers -filePath $file.FullName
    $allMembers += $members
}

Write-Host "`n=== XML Documentation Verification Report ===" -ForegroundColor DarkCyan
Write-Host ("Total public members analyzed: {0}" -f $allMembers.Count) -ForegroundColor Gray

# Calculate statistics
$documented = $allMembers | Where-Object { $_.HasXmlDoc }
$undocumented = $allMembers | Where-Object { -not $_.HasXmlDoc }
$complete = $allMembers | Where-Object { $_.DocQuality -eq "Complete" }
$basic = $allMembers | Where-Object { $_.DocQuality -eq "Basic" }
$minimal = $allMembers | Where-Object { $_.DocQuality -eq "Minimal" }

# Overall statistics
Write-Host "`n📊 Documentation Coverage:" -ForegroundColor DarkCyan
$coveragePercent = if ($allMembers.Count -gt 0) { [Math]::Round(($documented.Count / $allMembers.Count) * 100, 1) } else { 0 }
Write-Host ("  Overall coverage: {0}% ({1}/{2})" -f $coveragePercent, $documented.Count, $allMembers.Count) -ForegroundColor $(if ($coveragePercent -gt 80) { "Green" } elseif ($coveragePercent -gt 60) { "DarkYellow" } else { "Red" })

Write-Host ("  Complete documentation: {0}" -f $complete.Count) -ForegroundColor $(if ($complete.Count -gt ($allMembers.Count * 0.5)) { "Green" } else { "DarkYellow" })
Write-Host ("  Basic documentation: {0}" -f $basic.Count) -ForegroundColor Gray
Write-Host ("  Minimal documentation: {0}" -f $minimal.Count) -ForegroundColor DarkYellow
Write-Host ("  No documentation: {0}" -f $undocumented.Count) -ForegroundColor $(if ($undocumented.Count -eq 0) { "Green" } else { "Red" })

# Documentation by member type
Write-Host "`n📝 Documentation by Member Type:" -ForegroundColor DarkCyan
$typeStats = $allMembers | Group-Object Type | Sort-Object Name

foreach ($typeStat in $typeStats) {
    $typeDocumented = $typeStat.Group | Where-Object { $_.HasXmlDoc }
    $typePercent = if ($typeStat.Count -gt 0) { [Math]::Round(($typeDocumented.Count / $typeStat.Count) * 100, 1) } else { 0 }
    $color = if ($typePercent -gt 80) { "Green" } elseif ($typePercent -gt 60) { "DarkYellow" } else { "Red" }
    Write-Host ("  {0,-10}: {1,3}% ({2}/{3})" -f $typeStat.Name, $typePercent, $typeDocumented.Count, $typeStat.Count) -ForegroundColor $color
}

# Files with poor documentation
Write-Host "`n⚠️  Files Needing Documentation Attention:" -ForegroundColor DarkCyan
$fileStats = $allMembers | Group-Object File | ForEach-Object {
    $fileDocumented = $_.Group | Where-Object { $_.HasXmlDoc }
    $filePercent = if ($_.Count -gt 0) { [Math]::Round(($fileDocumented.Count / $_.Count) * 100, 1) } else { 100 }
    [PSCustomObject]@{
        File = $_.Name
        Total = $_.Count
        Documented = $fileDocumented.Count
        Undocumented = $_.Count - $fileDocumented.Count
        Percentage = $filePercent
    }
} | Sort-Object Percentage | Select-Object -First 10

foreach ($fileStat in $fileStats) {
    if ($fileStat.Undocumented -gt 0) {
        $fileName = [System.IO.Path]::GetFileName($fileStat.File)
        $color = if ($fileStat.Percentage -lt 50) { "Red" } else { "DarkYellow" }
        Write-Host ("  {0,-30} {1,3}% ({2}/{3} undocumented)" -f $fileName, $fileStat.Percentage, $fileStat.Undocumented, $fileStat.Total) -ForegroundColor $color
    }
}

# Show undocumented members by priority
if ($undocumented.Count -gt 0) {
    Write-Host "`n🚨 Undocumented Members (Priority Order):" -ForegroundColor DarkCyan
    
    # Prioritize by type: Classes first, then Methods, then Properties, etc.
    $priorityOrder = @('Class', 'Method', 'Property', 'Field', 'Event')
    $undocumentedSorted = @()
    
    foreach ($priority in $priorityOrder) {
        $undocumentedSorted += $undocumented | Where-Object { $_.Type -eq $priority } | Sort-Object File, Name
    }
    
    $showCount = [Math]::Min(20, $undocumentedSorted.Count)
    for ($i = 0; $i -lt $showCount; $i++) {
        $member = $undocumentedSorted[$i]
        $fileName = [System.IO.Path]::GetFileName($member.File)
        $color = switch ($member.Type) {
            'Class' { "Red" }
            'Method' { "DarkYellow" }
            default { "Gray" }
        }
        Write-Host ("  {0,-8} {1,-25} in {2} (line {3})" -f $member.Type, $member.Name, $fileName, $member.LineNumber) -ForegroundColor $color
    }
    
    if ($undocumentedSorted.Count -gt 20) {
        Write-Host ("  ... and {0} more undocumented members" -f ($undocumentedSorted.Count - 20)) -ForegroundColor DarkGray
    }
}

# Documentation quality issues
Write-Host "`n📋 Documentation Quality Issues:" -ForegroundColor DarkCyan

$qualityIssues = $allMembers | Where-Object { $_.HasXmlDoc -and $_.DocQuality -ne "Complete" }
if ($qualityIssues.Count -eq 0) {
    Write-Host "  ✅ All documented members have complete documentation!" -ForegroundColor Green
} else {
    $showQualityCount = [Math]::Min(15, $qualityIssues.Count)
    for ($i = 0; $i -lt $showQualityCount; $i++) {
        $issue = $qualityIssues[$i]
        $fileName = [System.IO.Path]::GetFileName($issue.File)
        $color = if ($issue.DocQuality -eq "Minimal") { "Red" } else { "DarkYellow" }
        Write-Host ("  {0,-8} {1,-25} in {2} - {3} docs" -f $issue.Type, $issue.Name, $fileName, $issue.DocQuality) -ForegroundColor $color
    }
    
    if ($qualityIssues.Count -gt 15) {
        Write-Host ("  ... and {0} more quality issues" -f ($qualityIssues.Count - 15)) -ForegroundColor DarkGray
    }
}

# Examples of good documentation
$goodExamples = $allMembers | Where-Object { $_.DocQuality -eq "Complete" -and $_.Description.Length -gt 10 } | Select-Object -First 3
if ($goodExamples.Count -gt 0) {
    Write-Host "`n✅ Examples of Good Documentation:" -ForegroundColor DarkCyan
    foreach ($example in $goodExamples) {
        $fileName = [System.IO.Path]::GetFileName($example.File)
        Write-Host ("  {0}.{1}()" -f $fileName, $example.Name) -ForegroundColor Green
        Write-Host ("    {0}" -f $example.Description) -ForegroundColor DarkGray
    }
}

# Compliance with .github/copilot-instructions.md
Write-Host "`n🎯 Copilot Instructions Compliance:" -ForegroundColor DarkCyan

# Check for thread safety warnings
$threadSafetyDocs = $documented | Where-Object { 
    ($_.XmlDocLines -join " ") -match "(thread.safe|Thread.Safe|THREAD.SAFE)" 
}
Write-Host ("  Thread safety documented: {0} members" -f $threadSafetyDocs.Count) -ForegroundColor $(if ($threadSafetyDocs.Count -gt 10) { "Green" } else { "DarkYellow" })

# Check for .NET 4.8.1 compatibility notes
$compatibilityDocs = $documented | Where-Object { 
    ($_.XmlDocLines -join " ") -match "(\.NET 4\.8\.1|NET 4\.8\.1|4\.8\.1)" 
}
Write-Host ("  .NET 4.8.1 compatibility noted: {0} members" -f $compatibilityDocs.Count) -ForegroundColor Gray

# Check for main thread warnings
$mainThreadDocs = $documented | Where-Object { 
    ($_.XmlDocLines -join " ") -match "(main thread|Unity thread|UI thread)" 
}
Write-Host ("  Main thread warnings: {0} members" -f $mainThreadDocs.Count) -ForegroundColor Gray

# Generate recommendations
Write-Host "`n💡 Recommendations:" -ForegroundColor DarkCyan

if ($coveragePercent -lt 80) {
    Write-Host "  🚨 PRIORITY: Documentation coverage is below 80%" -ForegroundColor Red
    Write-Host "     Focus on documenting Classes and Methods first" -ForegroundColor Red
}

if ($undocumented.Count -gt 0) {
    $classesNeedingDocs = $undocumented | Where-Object { $_.Type -eq "Class" }
    $methodsNeedingDocs = $undocumented | Where-Object { $_.Type -eq "Method" }
    
    if ($classesNeedingDocs.Count -gt 0) {
        Write-Host ("  📚 Document {0} public classes first" -f $classesNeedingDocs.Count) -ForegroundColor DarkYellow
    }
    if ($methodsNeedingDocs.Count -gt 0) {
        Write-Host ("  🔧 Document {0} public methods" -f $methodsNeedingDocs.Count) -ForegroundColor DarkYellow
    }
}

if ($qualityIssues.Count -gt 0) {
    Write-Host ("  📝 Improve documentation quality for {0} members" -f $qualityIssues.Count) -ForegroundColor DarkYellow
    Write-Host "     Add <param> and <returns> tags where appropriate" -ForegroundColor DarkYellow
}

Write-Host "  🛡️  Add thread safety warnings to concurrent code" -ForegroundColor Gray
Write-Host "  ⚙️  Note .NET 4.8.1 compatibility requirements" -ForegroundColor Gray
Write-Host "  🧵 Document main thread usage restrictions" -ForegroundColor Gray

Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
Read-Host