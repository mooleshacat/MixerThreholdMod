# MixerThreholdMod DevOps Tool: Conflict Marker Scanner
# Scans the project for merge conflict markers (<<<<<<<, =======, >>>>>>>)
# Distinguishes between REAL merge conflicts and false positives
# Excludes: ForCopilot, ForConstants, and Legacy directories

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
if ((Split-Path $ScriptDir -Leaf) -ieq "Scripts") {
    $ProjectRoot = Split-Path $ScriptDir -Parent
} else {
    $ProjectRoot = $ScriptDir
}

Write-Host "Scanning project root for conflict markers: $ProjectRoot" -ForegroundColor DarkCyan
Write-Host "Excluding: ForCopilot, ForConstants, and Legacy directories" -ForegroundColor DarkGray

# Find all text/code files
$files = Get-ChildItem -Path $ProjectRoot -Recurse -File | Where-Object {
    $_.Extension -match "^\.(cs|txt|md|json|xml|config|yaml|yml|ini|cpp|h|js|ts|py|java|csproj|sln|ps1|psm1|psd1|bat|cmd|sh)$" -and
    $_.FullName -notmatch "[\\/](ForCopilot)[\\/]" -and
    $_.FullName -notmatch "[\\/](ForConstants)[\\/]" -and
    $_.FullName -notmatch "[\\/](Legacy)[\\/]"
}

Write-Host "Files to scan: $($files.Count)" -ForegroundColor DarkGray

# Function to check if a file has a complete merge conflict pattern
function Test-MergeConflictPattern {
    param($lines, $startIndex)
    
    # Look for complete conflict pattern within reasonable distance
    $foundEquals = $false
    $foundEnd = $false
    
    for ($i = $startIndex + 1; $i -lt $lines.Count -and $i -lt ($startIndex + 50); $i++) {
        if ($lines[$i] -match "^=======$") {
            $foundEquals = $true
        }
        elseif ($foundEquals -and $lines[$i] -match "^>>>>>>>\s+") {
            $foundEnd = $true
            return $true
        }
    }
    return $false
}

$realConflicts = @()
$falsePositives = @()
$i = 0

foreach ($file in $files) {
    $i++
    if ($i % 50 -eq 0) {
        Write-Host "Progress: Scanned $i of $($files.Count) files..." -ForegroundColor DarkGray
    }
    
    $lines = Get-Content -Path $file.FullName -ErrorAction SilentlyContinue
    if (!$lines) { continue }
    
    for ($lineNum = 0; $lineNum -lt $lines.Count; $lineNum++) {
        $line = $lines[$lineNum]
        
        # Check for conflict start marker
        if ($line -match "^<<<<<<<\s+") {
            if (Test-MergeConflictPattern -lines $lines -startIndex $lineNum) {
                $realConflicts += [PSCustomObject]@{
                    File = $file.FullName
                    LineNumber = $lineNum + 1
                    Marker = "<<<<<<< (START)"
                    Line = $line.Substring(0, [Math]::Min($line.Length, 100))
                }
            }
        }
        # Check for standalone markers (potential false positives)
        elseif ($line -match "(=======|<<<<<<<|>>>>>>>)") {
            # Check if it's part of a logging statement or comment
            $isInString = $line -match '".*======.*"' -or $line -match "'.*======.*'"
            $isInComment = $line -match "//.*======" -or $line -match "/\*.*======"
            
            if ($isInString -or $isInComment) {
                $falsePositives += [PSCustomObject]@{
                    File = $file.FullName
                    LineNumber = $lineNum + 1
                    Context = if ($isInString) { "STRING" } else { "COMMENT" }
                    Line = $line.Substring(0, [Math]::Min($line.Length, 100))
                }
            }
            else {
                # Standalone marker not in string/comment - suspicious but not confirmed conflict
                $falsePositives += [PSCustomObject]@{
                    File = $file.FullName
                    LineNumber = $lineNum + 1
                    Context = "STANDALONE"
                    Line = $line.Substring(0, [Math]::Min($line.Length, 100))
                }
            }
        }
    }
}

Write-Host "`n=== Merge Conflict Marker Report ===" -ForegroundColor DarkCyan

if ($realConflicts.Count -eq 0 -and $falsePositives.Count -eq 0) {
    Write-Host "No conflict markers found in project." -ForegroundColor Green
} else {
    if ($realConflicts.Count -gt 0) {
        Write-Host "`n🚨 REAL MERGE CONFLICTS DETECTED:" -ForegroundColor Red
        Write-Host "These require immediate attention!" -ForegroundColor Red
        $realConflicts | Format-Table File, LineNumber, Marker, Line -AutoSize
        Write-Host ("Total real merge conflicts: {0}" -f $realConflicts.Count) -ForegroundColor Red
        
        # Show affected files
        $conflictFiles = $realConflicts | Select-Object -ExpandProperty File -Unique
        Write-Host "`nFiles with real conflicts:" -ForegroundColor Red
        $conflictFiles | ForEach-Object { Write-Host "  $_" -ForegroundColor Red }
    }
    
    if ($falsePositives.Count -gt 0) {
        Write-Host "`n⚠️  POTENTIAL FALSE POSITIVES:" -ForegroundColor DarkYellow
        Write-Host "These appear to be separators or content, not merge conflicts:" -ForegroundColor DarkGray
        $falsePositives | Format-Table File, LineNumber, Context, Line -AutoSize
        Write-Host ("Total potential false positives: {0}" -f $falsePositives.Count) -ForegroundColor DarkYellow
    }
}

# Summary
if ($realConflicts.Count -gt 0) {
    Write-Host "`n❌ ACTION REQUIRED: $($realConflicts.Count) real merge conflict(s) found!" -ForegroundColor Red
    Write-Host "Run 'git status' to see conflicted files and resolve them." -ForegroundColor Yellow
} else {
    Write-Host "`n✅ No real merge conflicts detected." -ForegroundColor Green
    if ($falsePositives.Count -gt 0) {
        Write-Host "Found $($falsePositives.Count) false positive(s) that can be ignored." -ForegroundColor Gray
    }
}

Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
Read-Host