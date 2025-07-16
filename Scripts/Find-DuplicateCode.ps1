# MixerThreholdMod DevOps Tool: Duplicate Code Detector
# Detects copy-pasted code blocks across C# files
# Helps identify code that should be refactored into shared methods
# Excludes: ForCopilot, ForConstants, and Legacy directories

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
if ((Split-Path $ScriptDir -Leaf) -ieq "Scripts") {
    $ProjectRoot = Split-Path $ScriptDir -Parent
} else {
    $ProjectRoot = $ScriptDir
}

Write-Host "Scanning for duplicate code in: $ProjectRoot" -ForegroundColor DarkCyan
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

# Function to normalize code for comparison
function Get-NormalizedCode {
    param($code)
    
    # Remove comments
    $code = $code -replace '//.*$', '' -replace '/\*[\s\S]*?\*/', ''
    
    # Normalize whitespace
    $code = $code -replace '\s+', ' '
    $code = $code.Trim()
    
    # Remove empty lines
    $code = ($code -split "`n" | Where-Object { $_.Trim() -ne "" }) -join "`n"
    
    return $code
}

# Function to extract code blocks (methods, loops, conditions)
function Get-CodeBlocks {
    param($filePath)
    
    $content = Get-Content -Path $filePath -Raw
    $blocks = @()
    
    # Extract method bodies
    $methodPattern = '(?ms)(public|private|protected|internal|static|async|override|virtual|sealed)\s+(?:(?:public|private|protected|internal|static|async|override|virtual|sealed)\s+)*(?:Task<[^>]+>|Task|void|[A-Za-z0-9_<>\[\]]+)\s+([A-Za-z0-9_]+)\s*\([^)]*\)\s*\{([^{}]*(?:\{[^{}]*\}[^{}]*)*)\}'
    $methodMatches = [regex]::Matches($content, $methodPattern)
    
    foreach ($match in $methodMatches) {
        $methodName = $match.Groups[2].Value
        $methodBody = $match.Groups[3].Value
        
        # Skip very small methods
        if ($methodBody.Length -lt 100) { continue }
        
        $normalizedCode = Get-NormalizedCode -code $methodBody
        $lineNumber = ($content.Substring(0, $match.Index) -split "`n").Count
        
        $blocks += [PSCustomObject]@{
            File = $filePath
            Type = "Method"
            Name = $methodName
            Code = $normalizedCode
            OriginalCode = $methodBody
            LineNumber = $lineNumber
            Length = $methodBody.Length
            Hash = [System.Security.Cryptography.SHA256]::Create().ComputeHash([System.Text.Encoding]::UTF8.GetBytes($normalizedCode)) | ForEach-Object { $_.ToString("x2") } | Join-String
        }
    }
    
    # Extract significant code blocks (if/else, loops, try/catch)
    $blockPatterns = @(
        @{ Pattern = 'if\s*\([^)]+\)\s*\{([^{}]*(?:\{[^{}]*\}[^{}]*)*)\}'; Type = "If Block" },
        @{ Pattern = 'for\s*\([^)]+\)\s*\{([^{}]*(?:\{[^{}]*\}[^{}]*)*)\}'; Type = "For Loop" },
        @{ Pattern = 'foreach\s*\([^)]+\)\s*\{([^{}]*(?:\{[^{}]*\}[^{}]*)*)\}'; Type = "Foreach Loop" },
        @{ Pattern = 'while\s*\([^)]+\)\s*\{([^{}]*(?:\{[^{}]*\}[^{}]*)*)\}'; Type = "While Loop" },
        @{ Pattern = 'try\s*\{([^{}]*(?:\{[^{}]*\}[^{}]*)*)\}'; Type = "Try Block" }
    )
    
    foreach ($pattern in $blockPatterns) {
        $matches = [regex]::Matches($content, $pattern.Pattern)
        foreach ($match in $matches) {
            $blockContent = $match.Groups[1].Value
            
            # Skip small blocks
            if ($blockContent.Length -lt 150) { continue }
            
            $normalizedCode = Get-NormalizedCode -code $blockContent
            $lineNumber = ($content.Substring(0, $match.Index) -split "`n").Count
            
            $blocks += [PSCustomObject]@{
                File = $filePath
                Type = $pattern.Type
                Name = "$($pattern.Type) at line $lineNumber"
                Code = $normalizedCode
                OriginalCode = $blockContent
                LineNumber = $lineNumber
                Length = $blockContent.Length
                Hash = [System.Security.Cryptography.SHA256]::Create().ComputeHash([System.Text.Encoding]::UTF8.GetBytes($normalizedCode)) | ForEach-Object { $_.ToString("x2") } | Join-String
            }
        }
    }
    
    return $blocks
}

# Analyze all files
$allBlocks = @()
$i = 0

foreach ($file in $files) {
    $i++
    if ($i % 10 -eq 0) {
        Write-Host "Progress: Analyzed $i of $($files.Count) files..." -ForegroundColor DarkGray
    }
    
    $blocks = Get-CodeBlocks -filePath $file.FullName
    $allBlocks += $blocks
}

Write-Host "`n=== Duplicate Code Analysis Report ===" -ForegroundColor DarkCyan
Write-Host ("Total code blocks analyzed: {0}" -f $allBlocks.Count) -ForegroundColor Gray

# Find duplicates by hash
$duplicateGroups = $allBlocks | Group-Object -Property Hash | Where-Object { $_.Count -gt 1 }

Write-Host ("Duplicate code groups found: {0}" -f $duplicateGroups.Count) -ForegroundColor $(if ($duplicateGroups.Count -gt 0) { "DarkYellow" } else { "Green" })

if ($duplicateGroups.Count -eq 0) {
    Write-Host "`n✅ No significant duplicate code blocks detected!" -ForegroundColor Green
} else {
    # Calculate statistics
    $totalDuplicateLines = 0
    $duplicateGroups | ForEach-Object {
        $totalDuplicateLines += ($_.Group[0].OriginalCode -split "`n").Count * ($_.Count - 1)
    }
    
    Write-Host ("Estimated duplicate lines: {0}" -f $totalDuplicateLines) -ForegroundColor Red
    
    # Sort by impact (size * occurrences)
    $sortedGroups = $duplicateGroups | ForEach-Object {
        $group = $_
        $impact = $group.Group[0].Length * ($group.Count - 1)
        [PSCustomObject]@{
            Group = $group
            Impact = $impact
            Occurrences = $group.Count
            CodeLength = $group.Group[0].Length
        }
    } | Sort-Object -Property Impact -Descending
    
    # Display top duplicates
    Write-Host "`n🔄 Top 10 Duplicate Code Blocks (by impact):" -ForegroundColor DarkCyan
    
    $topDuplicates = $sortedGroups | Select-Object -First 10
    $duplicateNumber = 1
    
    foreach ($duplicate in $topDuplicates) {
        $group = $duplicate.Group
        $firstBlock = $group.Group[0]
        
        Write-Host "`n━━━ Duplicate #$duplicateNumber ━━━" -ForegroundColor DarkYellow
        Write-Host ("Type: {0}, Occurrences: {1}, Size: {2} chars" -f $firstBlock.Type, $duplicate.Occurrences, $duplicate.CodeLength) -ForegroundColor Gray
        Write-Host "Found in:" -ForegroundColor Gray
        
        foreach ($block in $group.Group) {
            $fileName = [System.IO.Path]::GetFileName($block.File)
            Write-Host ("  • {0} - {1} (line {2})" -f $fileName, $block.Name, $block.LineNumber) -ForegroundColor DarkYellow
        }
        
        # Show code preview (first 3 lines)
        $codeLines = $firstBlock.OriginalCode -split "`n" | Select-Object -First 3
        Write-Host "`nCode preview:" -ForegroundColor DarkGray
        $codeLines | ForEach-Object { Write-Host ("  {0}" -f $_.Trim()) -ForegroundColor DarkGray }
        if (($firstBlock.OriginalCode -split "`n").Count -gt 3) {
            Write-Host "  ..." -ForegroundColor DarkGray
        }
        
        $duplicateNumber++
    }
    
    if ($sortedGroups.Count -gt 10) {
        $remaining = $sortedGroups.Count - 10
        Write-Host ("`n... ({0} more duplicate groups not shown)" -f $remaining) -ForegroundColor DarkGray
    }
    
    # File-level summary
    Write-Host "`n📁 Files with Most Duplicates:" -ForegroundColor DarkCyan
    $fileStats = $allBlocks | Where-Object { 
        $hash = $_.Hash
        ($allBlocks | Where-Object { $_.Hash -eq $hash }).Count -gt 1 
    } | Group-Object File | ForEach-Object {
        [PSCustomObject]@{
            File = $_.Name
            DuplicateBlocks = $_.Count
        }
    } | Sort-Object -Property DuplicateBlocks -Descending | Select-Object -First 10
    
    $fileStats | ForEach-Object {
        $fileName = [System.IO.Path]::GetFileName($_.File)
        Write-Host ("  {0} - {1} duplicate blocks" -f $fileName, $_.DuplicateBlocks) -ForegroundColor DarkYellow
    }
    
    # Type summary
    Write-Host "`n📊 Duplicates by Type:" -ForegroundColor DarkCyan
    $typeStats = $duplicateGroups | ForEach-Object {
        $_.Group[0].Type
    } | Group-Object | Sort-Object Count -Descending
    
    $typeStats | ForEach-Object {
        Write-Host ("  {0}: {1} duplicate groups" -f $_.Name, $_.Count) -ForegroundColor Gray
    }
    
    # Recommendations
    Write-Host "`n💡 Recommendations:" -ForegroundColor DarkCyan
    Write-Host "  1. Extract duplicate methods into a shared utility class" -ForegroundColor Gray
    Write-Host "  2. Create base classes for common functionality" -ForegroundColor Gray
    Write-Host "  3. Use inheritance or composition to share code" -ForegroundColor Gray
    Write-Host "  4. Consider using generic methods for similar logic" -ForegroundColor Gray
    
    if ($totalDuplicateLines -gt 500) {
        Write-Host "`n🚨 HIGH DUPLICATION: Over $totalDuplicateLines lines of duplicate code!" -ForegroundColor Red
        Write-Host "   This significantly impacts maintainability and increases bug risk" -ForegroundColor Red
    }
}

Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
Read-Host