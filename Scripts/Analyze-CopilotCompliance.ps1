# MixerThreholdMod DevOps Tool: Copilot Instructions Compliance Checker
# Verifies code follows .github/copilot-instructions.md rules and standards
# Checks for .NET 4.8.1 compatibility, thread safety, documentation, and more
# Excludes: ForCopilot, ForConstants, Scripts, and Legacy directories

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
if ((Split-Path $ScriptDir -Leaf) -ieq "Scripts") {
    $ProjectRoot = Split-Path $ScriptDir -Parent
} else {
    $ProjectRoot = $ScriptDir
}

Write-Host "Checking Copilot Instructions compliance in: $ProjectRoot" -ForegroundColor DarkCyan
Write-Host "Excluding: ForCopilot, ForConstants, Scripts, and Legacy directories" -ForegroundColor DarkGray

# Load copilot instructions if available
$copilotInstructionsPath = Join-Path $ProjectRoot ".github\copilot-instructions.md"
$copilotRules = @()
if (Test-Path $copilotInstructionsPath) {
    $copilotContent = Get-Content -Path $copilotInstructionsPath -Raw
    Write-Host "Found copilot instructions file" -ForegroundColor Gray
} else {
    Write-Host "Warning: .github/copilot-instructions.md not found" -ForegroundColor DarkYellow
    $copilotContent = ""
}

# Find all C# files
$files = Get-ChildItem -Path $ProjectRoot -Recurse -Include *.cs | Where-Object {
    $_.PSIsContainer -eq $false -and
    $_.FullName -notmatch "[\\/](ForCopilot)[\\/]" -and
    $_.FullName -notmatch "[\\/](ForConstants)[\\/]" -and
    $_.FullName -notmatch "[\\/](Scripts)[\\/]" -and
    $_.FullName -notmatch "[\\/](Legacy)[\\/]"
}

Write-Host "Files to analyze: $($files.Count)" -ForegroundColor DarkGray

# Compliance check functions
function Test-DotNet481Compatibility {
    param($content, $filePath)
    
    $issues = @()
    
    # Check for string interpolation (should use string.Format)
    if ($content -match '\$"[^"]*\{[^}]+\}[^"]*"') {
        $issues += [PSCustomObject]@{
            Rule = ".NET 4.8.1 Compatibility"
            Issue = "String interpolation detected (use string.Format instead)"
            Severity = "High"
            Line = ($content.Substring(0, $content.IndexOf('$"')) -split "`n").Count
            Context = "String interpolation not compatible with .NET 4.8.1"
        }
    }
    
    # Check for var usage (should use explicit types)
    $varMatches = [regex]::Matches($content, '(?m)^\s*var\s+\w+\s*=')
    foreach ($match in $varMatches) {
        $lineNum = ($content.Substring(0, $match.Index) -split "`n").Count
        $issues += [PSCustomObject]@{
            Rule = ".NET 4.8.1 Compatibility"
            Issue = "Implicit 'var' usage detected (use explicit types)"
            Severity = "Medium"
            Line = $lineNum
            Context = "Explicit types required for IL2CPP compatibility"
        }
    }
    
    # Check for yield return in try/catch
    if ($content -match 'try\s*\{[^}]*yield\s+return' -or $content -match 'catch[^}]*yield\s+return') {
        $issues += [PSCustomObject]@{
            Rule = ".NET 4.8.1 Compatibility"
            Issue = "yield return in try/catch block detected"
            Severity = "Critical"
            Line = -1
            Context = "Not allowed in .NET 4.8.1"
        }
    }
    
    # Check for default() usage without explicit type
    if ($content -match 'default\s*\(\s*\)') {
        $issues += [PSCustomObject]@{
            Rule = ".NET 4.8.1 Compatibility"
            Issue = "default() without explicit type (use default(Type))"
            Severity = "High"
            Line = -1
            Context = "IL2CPP requires explicit types"
        }
    }
    
    return $issues
}

function Test-ThreadSafety {
    param($content, $filePath)
    
    $issues = @()
    
    # Check for Thread.Sleep usage (should use async patterns)
    if ($content -match 'Thread\.Sleep\s*\(') {
        $issues += [PSCustomObject]@{
            Rule = "Thread Safety & Performance"
            Issue = "Thread.Sleep detected (use async patterns instead)"
            Severity = "High"
            Line = -1
            Context = "Never block Unity main thread"
        }
    }
    
    # Check for missing ConfigureAwait(false)
    $awaitMatches = [regex]::Matches($content, 'await\s+[^;]+(?<!ConfigureAwait\(false\))\s*;')
    foreach ($match in $awaitMatches) {
        # Skip if already has ConfigureAwait
        if ($match.Value -notmatch 'ConfigureAwait\(false\)') {
            $lineNum = ($content.Substring(0, $match.Index) -split "`n").Count
            $issues += [PSCustomObject]@{
                Rule = "Thread Safety & Performance"
                Issue = "await without ConfigureAwait(false)"
                Severity = "Medium"
                Line = $lineNum
                Context = "Use ConfigureAwait(false) for background operations"
            }
        }
    }
    
    # Check for missing cancellation token parameters
    $asyncMethods = [regex]::Matches($content, 'public\s+async\s+Task[^(]*\([^)]*\)')
    foreach ($method in $asyncMethods) {
        if ($method.Value -notmatch 'CancellationToken') {
            $lineNum = ($content.Substring(0, $method.Index) -split "`n").Count
            $issues += [PSCustomObject]@{
                Rule = "Thread Safety & Performance"
                Issue = "Async method missing CancellationToken parameter"
                Severity = "Medium"
                Line = $lineNum
                Context = "All async methods should support cancellation"
            }
        }
    }
    
    # Check for thread-unsafe collection operations
    $unsafeCollections = @('List<', 'Dictionary<', 'HashSet<', 'Queue<', 'Stack<')
    foreach ($collection in $unsafeCollections) {
        if ($content -match "(?<!ThreadSafe)$collection") {
            $className = [System.IO.Path]::GetFileNameWithoutExtension($filePath)
            # Skip if it's a ThreadSafe* class itself or internal usage
            if ($className -notmatch '^ThreadSafe' -and $content -match 'public.*' + $collection) {
                $issues += [PSCustomObject]@{
                    Rule = "Thread Safety & Performance"
                    Issue = "Non-thread-safe collection in public interface: $collection"
                    Severity = "High"
                    Line = -1
                    Context = "Use ThreadSafe* collections for concurrent access"
                }
            }
        }
    }
    
    return $issues
}

function Test-DocumentationCompliance {
    param($content, $filePath)
    
    $issues = @()
    
    # Check for public methods without XML documentation
    $publicMethods = [regex]::Matches($content, '(?m)^\s*public\s+(?:static\s+|virtual\s+|override\s+|async\s+)*(?:Task<[^>]+>|Task|void|[A-Za-z0-9_<>\[\]]+)\s+([A-Za-z0-9_]+)\s*\([^)]*\)')
    
    foreach ($method in $publicMethods) {
        $methodStart = $method.Index
        $linesBefore = $content.Substring(0, $methodStart) -split "`n"
        $lineNum = $linesBefore.Count
        
        # Check for XML documentation in preceding lines
        $hasXmlDoc = $false
        for ($i = [Math]::Max(0, $linesBefore.Count - 5); $i -lt $linesBefore.Count; $i++) {
            if ($linesBefore[$i] -match '^\s*///') {
                $hasXmlDoc = $true
                break
            }
        }
        
        if (-not $hasXmlDoc) {
            $methodName = $method.Groups[1].Value
            $issues += [PSCustomObject]@{
                Rule = "Documentation & Workflow"
                Issue = "Public method '$methodName' missing XML documentation"
                Severity = "Medium"
                Line = $lineNum
                Context = "All public methods require XML documentation"
            }
        }
    }
    
    return $issues
}

function Test-SaveCrashPrevention {
    param($content, $filePath)
    
    $issues = @()
    
    # Check for direct file operations without atomic patterns
    $directFileOps = @('File\.WriteAllText', 'File\.WriteAllBytes', 'FileStream.*Write')
    foreach ($op in $directFileOps) {
        if ($content -match $op -and $content -notmatch 'atomic|temp|backup|\.tmp') {
            $issues += [PSCustomObject]@{
                Rule = "Save Crash Prevention"
                Issue = "Direct file operation without atomic pattern: $op"
                Severity = "High"
                Line = -1
                Context = "Use atomic file operations with temp files"
            }
        }
    }
    
    # Check for save operations without try-catch
    if ($content -match '(Save|Write|Persist)' -and $content -notmatch 'try\s*\{.*catch') {
        $saveMatches = [regex]::Matches($content, 'public[^{]*(?:Save|Write|Persist)[^{]*\{')
        foreach ($match in $saveMatches) {
            # Check if this method has try-catch
            $methodBody = ""
            $braceCount = 0
            $startIndex = $match.Index + $match.Length
            
            for ($i = $startIndex; $i -lt $content.Length; $i++) {
                $char = $content[$i]
                $methodBody += $char
                if ($char -eq '{') { $braceCount++ }
                elseif ($char -eq '}') { 
                    $braceCount--
                    if ($braceCount -eq 0) { break }
                }
            }
            
            if ($methodBody -notmatch 'try\s*\{') {
                $lineNum = ($content.Substring(0, $match.Index) -split "`n").Count
                $issues += [PSCustomObject]@{
                    Rule = "Save Crash Prevention"
                    Issue = "Save method without comprehensive try-catch"
                    Severity = "High"
                    Line = $lineNum
                    Context = "All save operations must have error handling"
                }
            }
        }
    }
    
    return $issues
}

function Test-ErrorHandling {
    param($content, $filePath)
    
    $issues = @()
    
    # Check for empty catch blocks
    if ($content -match 'catch[^{]*\{\s*\}') {
        $issues += [PSCustomObject]@{
            Rule = "Extreme Verbose Debugging"
            Issue = "Empty catch block detected"
            Severity = "High"
            Line = -1
            Context = "All exceptions must be logged with context"
        }
    }
    
    # Check for catch blocks without logging
    $catchBlocks = [regex]::Matches($content, 'catch\s*\([^)]*\)\s*\{([^}]+)\}')
    foreach ($catch in $catchBlocks) {
        $catchBody = $catch.Groups[1].Value
        if ($catchBody -notmatch '(Log|logger\.|Main\.logger)') {
            $lineNum = ($content.Substring(0, $catch.Index) -split "`n").Count
            $issues += [PSCustomObject]@{
                Rule = "Extreme Verbose Debugging"
                Issue = "Catch block without logging"
                Severity = "Medium"
                Line = $lineNum
                Context = "Log all exceptions with stack traces and context"
            }
        }
    }
    
    return $issues
}

function Test-SpecificPatterns {
    param($content, $filePath)
    
    $issues = @()
    
    # Check for Unity main thread blocking operations
    $blockingOps = @('\.Result', '\.Wait\(\)', 'Task\.Run\(.*\)\.Result')
    foreach ($op in $blockingOps) {
        if ($content -match $op) {
            $issues += [PSCustomObject]@{
                Rule = "Thread Safety & Performance"
                Issue = "Potentially blocking operation: $op"
                Severity = "High"
                Line = -1
                Context = "Never block Unity main thread"
            }
        }
    }
    
    # Check for missing null checks on public parameters
    $publicMethods = [regex]::Matches($content, 'public[^{]*\([^)]+\)[^{]*\{([^}]+)\}')
    foreach ($method in $publicMethods) {
        $methodBody = $method.Groups[1].Value
        if ($method.Groups[0].Value -match '\(([^)]+)\)' -and $methodBody -notmatch 'null.*check|ArgumentNull|if.*null') {
            $lineNum = ($content.Substring(0, $method.Index) -split "`n").Count
            $issues += [PSCustomObject]@{
                Rule = "Extreme Verbose Debugging"
                Issue = "Public method may be missing null parameter validation"
                Severity = "Low"
                Line = $lineNum
                Context = "Validate public method parameters"
            }
        }
    }
    
    return $issues
}

# Analyze all files
Write-Host "`nAnalyzing compliance..." -ForegroundColor DarkGray

$allIssues = @()
$fileAnalysis = @()
$i = 0

foreach ($file in $files) {
    $i++
    if ($i % 20 -eq 0) {
        Write-Host "Progress: Analyzed $i of $($files.Count) files..." -ForegroundColor DarkGray
    }
    
    $content = Get-Content -Path $file.FullName -Raw
    $fileName = [System.IO.Path]::GetFileName($file.FullName)
    
    # Run all compliance checks
    $fileIssues = @()
    $fileIssues += Test-DotNet481Compatibility -content $content -filePath $file.FullName
    $fileIssues += Test-ThreadSafety -content $content -filePath $file.FullName
    $fileIssues += Test-DocumentationCompliance -content $content -filePath $file.FullName
    $fileIssues += Test-SaveCrashPrevention -content $content -filePath $file.FullName
    $fileIssues += Test-ErrorHandling -content $content -filePath $file.FullName
    $fileIssues += Test-SpecificPatterns -content $content -filePath $file.FullName
    
    # Add file information to each issue
    $fileIssues | ForEach-Object { 
        $_.File = $fileName
        $_.FilePath = $file.FullName
    }
    
    $allIssues += $fileIssues
    
    $fileAnalysis += [PSCustomObject]@{
        File = $fileName
        FilePath = $file.FullName
        TotalIssues = $fileIssues.Count
        CriticalIssues = ($fileIssues | Where-Object { $_.Severity -eq "Critical" }).Count
        HighIssues = ($fileIssues | Where-Object { $_.Severity -eq "High" }).Count
        MediumIssues = ($fileIssues | Where-Object { $_.Severity -eq "Medium" }).Count
        LowIssues = ($fileIssues | Where-Object { $_.Severity -eq "Low" }).Count
    }
}

Write-Host "`n=== Copilot Instructions Compliance Report ===" -ForegroundColor DarkCyan
Write-Host ("Total files analyzed: {0}" -f $files.Count) -ForegroundColor Gray
Write-Host ("Total compliance issues found: {0}" -f $allIssues.Count) -ForegroundColor $(if ($allIssues.Count -eq 0) { "Green" } elseif ($allIssues.Count -lt 20) { "DarkYellow" } else { "Red" })

# Severity breakdown
$criticalIssues = $allIssues | Where-Object { $_.Severity -eq "Critical" }
$highIssues = $allIssues | Where-Object { $_.Severity -eq "High" }
$mediumIssues = $allIssues | Where-Object { $_.Severity -eq "Medium" }
$lowIssues = $allIssues | Where-Object { $_.Severity -eq "Low" }

Write-Host "`n🚨 Issues by Severity:" -ForegroundColor DarkCyan
Write-Host ("  Critical: {0}" -f $criticalIssues.Count) -ForegroundColor $(if ($criticalIssues.Count -eq 0) { "Green" } else { "Red" })
Write-Host ("  High: {0}" -f $highIssues.Count) -ForegroundColor $(if ($highIssues.Count -eq 0) { "Green" } elseif ($highIssues.Count -lt 10) { "DarkYellow" } else { "Red" })
Write-Host ("  Medium: {0}" -f $mediumIssues.Count) -ForegroundColor $(if ($mediumIssues.Count -lt 20) { "DarkYellow" } else { "Red" })
Write-Host ("  Low: {0}" -f $lowIssues.Count) -ForegroundColor Gray

# Issues by rule
Write-Host "`n📋 Issues by Copilot Rule:" -ForegroundColor DarkCyan
$ruleStats = $allIssues | Group-Object Rule | Sort-Object Count -Descending

foreach ($ruleStat in $ruleStats) {
    $color = switch ($ruleStat.Count) {
        { $_ -eq 0 } { "Green" }
        { $_ -lt 5 } { "DarkYellow" }
        default { "Red" }
    }
    Write-Host ("  {0}: {1} issues" -f $ruleStat.Name, $ruleStat.Count) -ForegroundColor $color
}

# Critical and High issues details
if ($criticalIssues.Count -gt 0) {
    Write-Host "`n🚨 CRITICAL Issues (Must Fix Immediately):" -ForegroundColor Red
    foreach ($issue in $criticalIssues) {
        Write-Host ("  {0} in {1}" -f $issue.Issue, $issue.File) -ForegroundColor Red
        Write-Host ("    Rule: {0}" -f $issue.Rule) -ForegroundColor DarkGray
        Write-Host ("    Context: {0}" -f $issue.Context) -ForegroundColor DarkGray
    }
}

if ($highIssues.Count -gt 0) {
    Write-Host "`n⚠️  HIGH Priority Issues:" -ForegroundColor DarkYellow
    $showHighCount = [Math]::Min(15, $highIssues.Count)
    for ($i = 0; $i -lt $showHighCount; $i++) {
        $issue = $highIssues[$i]
        Write-Host ("  {0} in {1}" -f $issue.Issue, $issue.File) -ForegroundColor DarkYellow
        if ($issue.Line -gt 0) {
            Write-Host ("    Line: {0}" -f $issue.Line) -ForegroundColor DarkGray
        }
        Write-Host ("    Rule: {0}" -f $issue.Rule) -ForegroundColor DarkGray
    }
    
    if ($highIssues.Count -gt 15) {
        Write-Host ("  ... and {0} more high priority issues" -f ($highIssues.Count - 15)) -ForegroundColor DarkGray
    }
}

# Files with most issues
Write-Host "`n📁 Files with Most Compliance Issues:" -ForegroundColor DarkCyan
$topProblematicFiles = $fileAnalysis | Where-Object { $_.TotalIssues -gt 0 } | Sort-Object TotalIssues -Descending | Select-Object -First 10

foreach ($fileInfo in $topProblematicFiles) {
    $color = if ($fileInfo.CriticalIssues -gt 0) { "Red" } elseif ($fileInfo.HighIssues -gt 0) { "DarkYellow" } else { "Gray" }
    Write-Host ("  {0,-30} Total: {1,2} (C:{2} H:{3} M:{4} L:{5})" -f $fileInfo.File, $fileInfo.TotalIssues, $fileInfo.CriticalIssues, $fileInfo.HighIssues, $fileInfo.MediumIssues, $fileInfo.LowIssues) -ForegroundColor $color
}

# Compliance score
$totalPossibleIssues = $files.Count * 10  # Rough estimate
$complianceScore = if ($totalPossibleIssues -gt 0) { [Math]::Max(0, [Math]::Round((($totalPossibleIssues - $allIssues.Count) / $totalPossibleIssues) * 100, 1)) } else { 100 }

Write-Host "`n📊 Overall Compliance Score: $complianceScore%" -ForegroundColor $(if ($complianceScore -gt 90) { "Green" } elseif ($complianceScore -gt 75) { "DarkYellow" } else { "Red" })

# Recommendations
Write-Host "`n💡 Priority Recommendations:" -ForegroundColor DarkCyan

if ($criticalIssues.Count -gt 0) {
    Write-Host "  🚨 IMMEDIATE: Fix $($criticalIssues.Count) critical issues first" -ForegroundColor Red
}

if ($highIssues.Count -gt 10) {
    Write-Host "  ⚠️  Address $($highIssues.Count) high priority issues" -ForegroundColor DarkYellow
}

# Specific recommendations based on issue types
$dotNetIssues = $allIssues | Where-Object { $_.Rule -like "*4.8.1*" }
if ($dotNetIssues.Count -gt 0) {
    Write-Host "  🎯 Replace string interpolation with string.Format()" -ForegroundColor DarkYellow
    Write-Host "  🎯 Use explicit types instead of 'var'" -ForegroundColor DarkYellow
}

$threadIssues = $allIssues | Where-Object { $_.Rule -like "*Thread*" }
if ($threadIssues.Count -gt 0) {
    Write-Host "  🧵 Add ConfigureAwait(false) to await statements" -ForegroundColor DarkYellow
    Write-Host "  🧵 Replace Thread.Sleep with async patterns" -ForegroundColor DarkYellow
}

$docIssues = $allIssues | Where-Object { $_.Rule -like "*Documentation*" }
if ($docIssues.Count -gt 0) {
    Write-Host "  📝 Add XML documentation to public methods" -ForegroundColor DarkYellow
}

if ($allIssues.Count -eq 0) {
    Write-Host "`n🎉 Excellent! No compliance issues found!" -ForegroundColor Green
    Write-Host "   Your code follows all Copilot instructions perfectly!" -ForegroundColor Green
}

Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
Read-Host