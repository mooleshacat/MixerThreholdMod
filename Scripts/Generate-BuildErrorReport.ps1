# MixerThreholdMod DevOps Tool: Build Error Report Generator (NON-INTERACTIVE)
# Performs dotnet build, captures errors, and triages them by criticality
# Consolidates duplicate errors and lists all occurrence lines
# Excludes: ForCopilot, Scripts, and Legacy directories

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
if ((Split-Path $ScriptDir -Leaf) -ieq "Scripts") {
    $ProjectRoot = Split-Path $ScriptDir -Parent
} else {
    $ProjectRoot = $ScriptDir
}

# Check if running interactively or from another script
$IsInteractive = [Environment]::UserInteractive -and $Host.Name -ne "ConsoleHost"
$RunningFromScript = $MyInvocation.InvocationName -notmatch "\.ps1$"

Write-Host "🕐 Build error analysis started: $(Get-Date -Format 'HH:mm:ss')" -ForegroundColor Gray
Write-Host "Analyzing build errors in: $ProjectRoot" -ForegroundColor DarkCyan
Write-Host "🚀 NON-INTERACTIVE VERSION - Compatible with automation" -ForegroundColor Green

# Function to find project files
function Find-ProjectFiles {
    try {
        # Look for .csproj, .sln files
        $projectFiles = @()
        
        # Find solution files first (preferred)
        $solutionFiles = Get-ChildItem -Path $ProjectRoot -Filter "*.sln" -ErrorAction SilentlyContinue
        if ($solutionFiles.Count -gt 0) {
            $projectFiles += $solutionFiles | Select-Object -First 1  # Use first solution file
        }
        
        # If no solution, look for .csproj files
        if ($projectFiles.Count -eq 0) {
            $csprojFiles = Get-ChildItem -Path $ProjectRoot -Recurse -Filter "*.csproj" -ErrorAction SilentlyContinue | Where-Object {
                $_.FullName -notmatch "[\\/](ForCopilot|Scripts|Legacy)[\\/]"
            }
            $projectFiles += $csprojFiles
        }
        
        return $projectFiles
    }
    catch {
        Write-Host "⚠️  Error finding project files: $_" -ForegroundColor DarkYellow
        return @()
    }
}

# Function to execute dotnet build
function Invoke-DotNetBuild {
    param($projectFile)
    
    try {
        Write-Host "   🔨 Building project: $($projectFile.Name)" -ForegroundColor Gray
        
        # Ensure we're in the right directory
        Push-Location $ProjectRoot
        
        # Execute dotnet build and capture output
        $buildOutput = & dotnet build $projectFile.FullName --verbosity normal 2>&1
        $buildExitCode = $LASTEXITCODE
        
        Pop-Location
        
        return [PSCustomObject]@{
            ExitCode = $buildExitCode
            Output = $buildOutput
            Success = $buildExitCode -eq 0
            ProjectFile = $projectFile.FullName
            ProjectName = $projectFile.BaseName
        }
    }
    catch {
        Write-Host "   ⚠️  Error executing build: $_" -ForegroundColor DarkYellow
        Pop-Location
        return [PSCustomObject]@{
            ExitCode = -1
            Output = @("Build execution failed: $_")
            Success = $false
            ProjectFile = $projectFile.FullName
            ProjectName = $projectFile.BaseName
        }
    }
}

# Function to parse build errors
function Parse-BuildErrors {
    param($buildOutput)
    
    try {
        $errors = @()
        $warnings = @()
        $messages = @()
        
        foreach ($line in $buildOutput) {
            if (-not $line) { continue }
            
            # Parse MSBuild/dotnet error format: file(line,column): error/warning CODE: message
            if ($line -match '^(.+?)\((\d+),(\d+)\):\s*(error|warning)\s+([A-Z0-9]+):\s*(.+)$') {
                $file = $matches[1]
                $lineNum = [int]$matches[2]
                $column = [int]$matches[3]
                $severity = $matches[4]
                $code = $matches[5]
                $message = $matches[6].Trim()
                
                $parsedError = [PSCustomObject]@{
                    File = $file
                    FileName = [System.IO.Path]::GetFileName($file)
                    Line = $lineNum
                    Column = $column
                    Severity = $severity
                    Code = $code
                    Message = $message
                    FullLine = $line
                    Criticality = Get-ErrorCriticality -severity $severity -code $code -message $message
                }
                
                if ($severity -eq "error") {
                    $errors += $parsedError
                } else {
                    $warnings += $parsedError
                }
            }
            # Parse general error messages without file location
            elseif ($line -match '(error|warning)\s+([A-Z0-9]+):\s*(.+)') {
                $severity = $matches[1]
                $code = $matches[2]
                $message = $matches[3].Trim()
                
                $parsedError = [PSCustomObject]@{
                    File = ""
                    FileName = "General"
                    Line = 0
                    Column = 0
                    Severity = $severity
                    Code = $code
                    Message = $message
                    FullLine = $line
                    Criticality = Get-ErrorCriticality -severity $severity -code $code -message $message
                }
                
                if ($severity -eq "error") {
                    $errors += $parsedError
                } else {
                    $warnings += $parsedError
                }
            }
            # Capture other important build messages
            elseif ($line -match '(Build FAILED|Build succeeded|Build FAILED with errors)') {
                $messages += $line
            }
        }
        
        return [PSCustomObject]@{
            Errors = $errors
            Warnings = $warnings
            Messages = $messages
            TotalErrors = $errors.Count
            TotalWarnings = $warnings.Count
        }
    }
    catch {
        Write-Host "   ⚠️  Error parsing build output: $_" -ForegroundColor DarkYellow
        return [PSCustomObject]@{
            Errors = @()
            Warnings = @()
            Messages = @()
            TotalErrors = 0
            TotalWarnings = 0
        }
    }
}

# Function to determine error criticality
function Get-ErrorCriticality {
    param($severity, $code, $message)
    
    # Critical errors that prevent compilation
    $criticalCodes = @(
        'CS0103',  # Name does not exist in current context
        'CS0246',  # Type or namespace not found
        'CS0117',  # Does not contain a definition
        'CS1002',  # Syntax error
        'CS1513',  # } expected
        'CS0029',  # Cannot implicitly convert type
        'CS0118',  # Variable is used like a method
        'CS0019'   # Operator cannot be applied
    )
    
    # High severity errors
    $highSeverityCodes = @(
        'CS0161',  # Not all code paths return a value
        'CS0162',  # Unreachable code detected
        'CS0168',  # Variable declared but never used
        'CS0219',  # Variable assigned but never used
        'CS0414'   # Field assigned but never used
    )
    
    # Project-specific critical patterns
    $criticalPatterns = @(
        'MelonLoader',
        'Thread',
        'async',
        'Task',
        'ConfigureAwait',
        'CancellationToken',
        'IL2CPP'
    )
    
    if ($severity -eq "error") {
        # Critical compilation errors
        if ($criticalCodes -contains $code) {
            return "CRITICAL"
        }
        
        # Project-specific critical errors
        foreach ($pattern in $criticalPatterns) {
            if ($message -match $pattern) {
                return "CRITICAL"
            }
        }
        
        # High severity errors
        if ($highSeverityCodes -contains $code) {
            return "HIGH"
        }
        
        # Default error severity
        return "MEDIUM"
    } else {
        # Warning severity
        if ($highSeverityCodes -contains $code) {
            return "MEDIUM"
        }
        return "LOW"
    }
}

# Function to consolidate duplicate errors
function Consolidate-Errors {
    param($errors)
    
    try {
        # Group errors by Code + Message (ignoring file location)
        $groupedErrors = $errors | Group-Object { "$($_.Code):$($_.Message)" }
        
        $consolidatedErrors = @()
        
        foreach ($group in $groupedErrors) {
            $firstError = $group.Group[0]
            $allLocations = $group.Group | Sort-Object File, Line
            
            $consolidatedError = [PSCustomObject]@{
                Code = $firstError.Code
                Message = $firstError.Message
                Severity = $firstError.Severity
                Criticality = $firstError.Criticality
                Occurrences = $group.Count
                Locations = $allLocations | ForEach-Object {
                    [PSCustomObject]@{
                        File = $_.File
                        FileName = $_.FileName
                        Line = $_.Line
                        Column = $_.Column
                    }
                }
                Files = ($allLocations | Select-Object -ExpandProperty FileName -Unique | Sort-Object) -join ", "
                FirstOccurrence = $firstError
            }
            
            $consolidatedErrors += $consolidatedError
        }
        
        return $consolidatedErrors | Sort-Object @{
            Expression = {
                switch ($_.Criticality) {
                    "CRITICAL" { 1 }
                    "HIGH" { 2 }
                    "MEDIUM" { 3 }
                    "LOW" { 4 }
                    default { 5 }
                }
            }
        }, Occurrences -Descending
    }
    catch {
        Write-Host "   ⚠️  Error consolidating errors: $_" -ForegroundColor DarkYellow
        return $errors
    }
}

# Main script execution
Write-Host "``n📂 Scanning for project files..." -ForegroundColor DarkGray
$projectFiles = Find-ProjectFiles

if ($projectFiles.Count -eq 0) {
    Write-Host "❌ No project files (.sln or .csproj) found!" -ForegroundColor Red
    Write-Host "Ensure you're running this script from a .NET project directory" -ForegroundColor DarkYellow
    if ($IsInteractive -and -not $RunningFromScript) {
        Write-Host "``nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
        Read-Host
    }
    return
}

Write-Host "📊 Found $($projectFiles.Count) project file(s):" -ForegroundColor Gray
foreach ($project in $projectFiles) {
    Write-Host "   📄 $($project.Name)" -ForegroundColor Gray
}

# Check if dotnet CLI is available
try {
    $dotnetVersion = & dotnet --version 2>$null
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ .NET CLI not found! Please install .NET SDK" -ForegroundColor Red
        if ($IsInteractive -and -not $RunningFromScript) {
            Write-Host "``nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
            Read-Host
        }
        return
    }
    Write-Host "✅ .NET CLI version: $dotnetVersion" -ForegroundColor Green
}
catch {
    Write-Host "❌ .NET CLI not available: $_" -ForegroundColor Red
    if ($IsInteractive -and -not $RunningFromScript) {
        Write-Host "``nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
        Read-Host
    }
    return
}

# Execute builds for each project
Write-Host "``n🔨 Executing dotnet build..." -ForegroundColor DarkCyan
$buildResults = @()

foreach ($project in $projectFiles) {
    $buildResult = Invoke-DotNetBuild -projectFile $project
    $buildResults += $buildResult
    
    if ($buildResult.Success) {
        Write-Host "   ✅ $($project.Name): Build succeeded" -ForegroundColor Green
    } else {
        Write-Host "   ❌ $($project.Name): Build failed (exit code: $($buildResult.ExitCode))" -ForegroundColor Red
    }
}

# Parse errors from all build results
Write-Host "``n📊 Analyzing build output..." -ForegroundColor DarkGray
$allErrors = @()
$allWarnings = @()
$buildMessages = @()

foreach ($buildResult in $buildResults) {
    $parsed = Parse-BuildErrors -buildOutput $buildResult.Output
    $allErrors += $parsed.Errors
    $allWarnings += $parsed.Warnings
    $buildMessages += $parsed.Messages
}

Write-Host "✅ Found $($allErrors.Count) errors and $($allWarnings.Count) warnings" -ForegroundColor Gray

# Consolidate duplicate errors
Write-Host "``n🔄 Consolidating duplicate errors..." -ForegroundColor DarkGray
$consolidatedErrors = Consolidate-Errors -errors $allErrors
$consolidatedWarnings = Consolidate-Errors -errors $allWarnings

Write-Host "✅ Consolidated to $($consolidatedErrors.Count) unique errors and $($consolidatedWarnings.Count) unique warnings" -ForegroundColor Gray

Write-Host "``n=== BUILD ERROR ANALYSIS REPORT ===" -ForegroundColor DarkCyan
Write-Host "🕐 Analysis completed: $(Get-Date -Format 'HH:mm:ss')" -ForegroundColor Gray

# Overall build status
$overallSuccess = $buildResults | Where-Object { $_.Success }
$overallFailed = $buildResults | Where-Object { -not $_.Success }

Write-Host "📊 Build Summary:" -ForegroundColor DarkCyan
Write-Host "   Projects built: $($buildResults.Count)" -ForegroundColor Gray
Write-Host "   Successful: $($overallSuccess.Count)" -ForegroundColor $(if ($overallSuccess.Count -eq $buildResults.Count) { "Green" } else { "DarkYellow" })
Write-Host "   Failed: $($overallFailed.Count)" -ForegroundColor $(if ($overallFailed.Count -eq 0) { "Green" } else { "Red" })
Write-Host "   Total errors: $($allErrors.Count) (consolidated: $($consolidatedErrors.Count))" -ForegroundColor $(if ($allErrors.Count -eq 0) { "Green" } else { "Red" })
Write-Host "   Total warnings: $($allWarnings.Count) (consolidated: $($consolidatedWarnings.Count))" -ForegroundColor $(if ($allWarnings.Count -eq 0) { "Green" } else { "DarkYellow" })

# Display critical errors
if ($consolidatedErrors.Count -gt 0) {
    Write-Host "``n🚨 Build Errors by Criticality:" -ForegroundColor DarkCyan
    
    $criticalErrors = $consolidatedErrors | Where-Object { $_.Criticality -eq "CRITICAL" }
    $highErrors = $consolidatedErrors | Where-Object { $_.Criticality -eq "HIGH" }
    $mediumErrors = $consolidatedErrors | Where-Object { $_.Criticality -eq "MEDIUM" }
    
    if ($criticalErrors.Count -gt 0) {
        Write-Host "``n   🚨 CRITICAL ERRORS (Fix First):" -ForegroundColor Red
        foreach ($error in $criticalErrors | Select-Object -First 5) {
            Write-Host "      $($error.Code): $($error.Message)" -ForegroundColor Red
            Write-Host "         Occurrences: $($error.Occurrences) | Files: $($error.Files)" -ForegroundColor DarkGray
        }
        if ($criticalErrors.Count -gt 5) {
            Write-Host "      ... and $($criticalErrors.Count - 5) more critical errors" -ForegroundColor DarkGray
        }
    }
    
    if ($highErrors.Count -gt 0) {
        Write-Host "``n   ⚠️  HIGH PRIORITY ERRORS:" -ForegroundColor DarkYellow
        foreach ($error in $highErrors | Select-Object -First 3) {
            Write-Host "      $($error.Code): $($error.Message)" -ForegroundColor DarkYellow
            Write-Host "         Occurrences: $($error.Occurrences) | Files: $($error.Files)" -ForegroundColor DarkGray
        }
        if ($highErrors.Count -gt 3) {
            Write-Host "      ... and $($highErrors.Count - 3) more high priority errors" -ForegroundColor DarkGray
        }
    }
    
    if ($mediumErrors.Count -gt 0) {
        Write-Host "``n   📝 MEDIUM PRIORITY ERRORS:" -ForegroundColor Gray
        Write-Host "      $($mediumErrors.Count) errors - see detailed report for full list" -ForegroundColor Gray
    }
} else {
    Write-Host "``n✅ NO BUILD ERRORS! Clean compilation achieved." -ForegroundColor Green
}

# Display warning summary
if ($consolidatedWarnings.Count -gt 0) {
    Write-Host "``n⚠️  Build Warnings Summary:" -ForegroundColor DarkYellow
    $topWarnings = $consolidatedWarnings | Sort-Object Occurrences -Descending | Select-Object -First 3
    foreach ($warning in $topWarnings) {
        Write-Host "   • $($warning.Code): $($warning.Occurrences) occurrences" -ForegroundColor DarkYellow
        Write-Host "     $($warning.Message)" -ForegroundColor DarkGray
    }
    if ($consolidatedWarnings.Count -gt 3) {
        Write-Host "   ... and $($consolidatedWarnings.Count - 3) more warning types" -ForegroundColor DarkGray
    }
}

# Quick recommendations
Write-Host "``n💡 Recommendations:" -ForegroundColor DarkCyan

if ($overallFailed.Count -gt 0) {
    Write-Host "   🚨 IMMEDIATE: Fix build failures before proceeding" -ForegroundColor Red
    
    if ($criticalErrors.Count -gt 0) {
        Write-Host "   • Start with $($criticalErrors.Count) CRITICAL errors" -ForegroundColor Red
        Write-Host "   • Focus on syntax and missing references first" -ForegroundColor Red
    }
    
    if ($highErrors.Count -gt 0) {
        Write-Host "   • Address $($highErrors.Count) HIGH priority errors next" -ForegroundColor DarkYellow
    }
} else {
    Write-Host "   ✅ All builds successful!" -ForegroundColor Green
    
    if ($consolidatedWarnings.Count -gt 0) {
        Write-Host "   • Consider addressing $($consolidatedWarnings.Count) warning types" -ForegroundColor DarkYellow
        Write-Host "   • Warnings may indicate potential runtime issues" -ForegroundColor Gray
    }
}

Write-Host "   • Review detailed report for complete error analysis" -ForegroundColor Gray
Write-Host "   • Run this analysis before committing code changes" -ForegroundColor Gray

# Create Reports directory if it doesn't exist
$reportsDir = Join-Path $ProjectRoot "Reports"
if (-not (Test-Path $reportsDir)) {
    try {
        New-Item -Path $reportsDir -ItemType Directory -Force | Out-Null
        Write-Host "``n📁 Created Reports directory: $reportsDir" -ForegroundColor Green
    }
    catch {
        Write-Host "``n⚠️ Could not create Reports directory, using project root" -ForegroundColor DarkYellow
        $reportsDir = $ProjectRoot
    }
}

# Generate detailed build error report
Write-Host "``n📝 Generating detailed build error report..." -ForegroundColor DarkGray

$timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
$reportPath = Join-Path $reportsDir "BUILD-ERROR-REPORT_$timestamp.md"

$reportContent = @()
$reportContent += "# Build Error Analysis Report"
$reportContent += ""
$reportContent += "**Generated**: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
$reportContent += "**Projects Built**: $($buildResults.Count)"
$reportContent += "**Build Status**: $(if ($overallFailed.Count -eq 0) { "✅ Success" } else { "❌ Failed ($($overallFailed.Count)/$($buildResults.Count))" })"
$reportContent += "**Total Errors**: $($allErrors.Count) (consolidated: $($consolidatedErrors.Count))"
$reportContent += "**Total Warnings**: $($allWarnings.Count) (consolidated: $($consolidatedWarnings.Count))"
$reportContent += ""

# Executive Summary
$reportContent += "## Executive Summary"
$reportContent += ""

if ($overallFailed.Count -eq 0) {
    $reportContent += "🎉 **BUILD SUCCESSFUL!** All projects compiled without errors."
    $reportContent += ""
    if ($consolidatedWarnings.Count -eq 0) {
        $reportContent += "Perfect compilation - no errors or warnings detected."
    } else {
        $reportContent += "Consider addressing $($consolidatedWarnings.Count) warning types for optimal code quality."
    }
} else {
    $reportContent += "❌ **BUILD FAILURES DETECTED** - Immediate action required."
    $reportContent += ""
    $reportContent += "$($overallFailed.Count) of $($buildResults.Count) projects failed to compile with $($consolidatedErrors.Count) unique error types."
}

$reportContent += ""
$reportContent += "| Metric | Value | Status |"
$reportContent += "|--------|-------|--------|"
$reportContent += "| **Projects Built** | $($buildResults.Count) | - |"
$reportContent += "| **Successful Builds** | $($overallSuccess.Count) | $(if ($overallSuccess.Count -eq $buildResults.Count) { "✅ All Passed" } else { "⚠️ Some Failed" }) |"
$reportContent += "| **Failed Builds** | $($overallFailed.Count) | $(if ($overallFailed.Count -eq 0) { "✅ None" } else { "❌ Action Required" }) |"
$reportContent += "| **Unique Errors** | $($consolidatedErrors.Count) | $(if ($consolidatedErrors.Count -eq 0) { "✅ None" } else { "🚨 Fix Required" }) |"
$reportContent += "| **Unique Warnings** | $($consolidatedWarnings.Count) | $(if ($consolidatedWarnings.Count -eq 0) { "✅ None" } elseif ($consolidatedWarnings.Count -le 5) { "📝 Few" } else { "⚠️ Many" }) |"

# Add criticality breakdown
if ($consolidatedErrors.Count -gt 0) {
    $criticalCount = ($consolidatedErrors | Where-Object { $_.Criticality -eq "CRITICAL" }).Count
    $highCount = ($consolidatedErrors | Where-Object { $_.Criticality -eq "HIGH" }).Count
    $mediumCount = ($consolidatedErrors | Where-Object { $_.Criticality -eq "MEDIUM" }).Count
    
    $reportContent += "| **Critical Errors** | $criticalCount | $(if ($criticalCount -eq 0) { "✅ None" } else { "🚨 Immediate Fix" }) |"
    $reportContent += "| **High Priority** | $highCount | $(if ($highCount -eq 0) { "✅ None" } else { "⚠️ Fix Soon" }) |"
    $reportContent += "| **Medium Priority** | $mediumCount | $(if ($mediumCount -eq 0) { "✅ None" } else { "📝 Review" }) |"
}

$reportContent += ""

# Project Build Results
$reportContent += "## Project Build Results"
$reportContent += ""
$reportContent += "| Project | Status | Errors | Warnings | Exit Code |"
$reportContent += "|---------|--------|--------|----------|-----------|"

foreach ($buildResult in $buildResults) {
    $projectErrors = $allErrors | Where-Object { $_.File -like "*$($buildResult.ProjectName)*" }
    $projectWarnings = $allWarnings | Where-Object { $_.File -like "*$($buildResult.ProjectName)*" }
    
    $status = if ($buildResult.Success) { "✅ Success" } else { "❌ Failed" }
    $reportContent += "| ````$($buildResult.ProjectName)```` | $status | $($projectErrors.Count) | $($projectWarnings.Count) | $($buildResult.ExitCode) |"
}

$reportContent += ""

# Detailed Error Analysis
if ($consolidatedErrors.Count -gt 0) {
    $reportContent += "## Detailed Error Analysis"
    $reportContent += ""
    
    # Group by criticality
    $errorsByCriticality = $consolidatedErrors | Group-Object Criticality | Sort-Object @{
        Expression = {
            switch ($_.Name) {
                "CRITICAL" { 1 }
                "HIGH" { 2 }
                "MEDIUM" { 3 }
                "LOW" { 4 }
                default { 5 }
            }
        }
    }
    
    foreach ($criticalityGroup in $errorsByCriticality) {
        $criticality = $criticalityGroup.Name
        $criticalityErrors = $criticalityGroup.Group
        
        $sectionIcon = switch ($criticality) {
            "CRITICAL" { "🚨" }
            "HIGH" { "⚠️" }
            "MEDIUM" { "📝" }
            "LOW" { "💭" }
            default { "📌" }
        }
        
        $reportContent += "### $sectionIcon $criticality Priority Errors ($($criticalityErrors.Count) types)"
        $reportContent += ""
        
        foreach ($error in $criticalityErrors | Sort-Object Occurrences -Descending) {
            $reportContent += "#### $($error.Code): $(if ($error.Message.Length -gt 80) { $error.Message.Substring(0, 77) + "..." } else { $error.Message })"
            $reportContent += ""
            $reportContent += "**Occurrences**: $($error.Occurrences) | **Files**: $($error.Files)"
            $reportContent += ""
            $reportContent += "**Error Details**: $($error.Message)"
            $reportContent += ""
            
            if ($error.Occurrences -le 10) {
                # Show all locations for small number of occurrences
                $reportContent += "**All Locations**:"
                $reportContent += ""
                foreach ($location in $error.Locations) {
                    if ($location.File) {
                        $reportContent += "- ````$($location.FileName)```` (Line $($location.Line), Column $($location.Column))"
                    } else {
                        $reportContent += "- General error (no specific location)"
                    }
                }
            } else {
                # Show sample locations for large number of occurrences
                $reportContent += "**Sample Locations** (showing first 5 of $($error.Occurrences)):"
                $reportContent += ""
                foreach ($location in $error.Locations | Select-Object -First 5) {
                    if ($location.File) {
                        $reportContent += "- ````$($location.FileName)```` (Line $($location.Line), Column $($location.Column))"
                    } else {
                        $reportContent += "- General error (no specific location)"
                    }
                }
                $reportContent += ""
                $reportContent += "*... and $($error.Occurrences - 5) more locations*"
            }
            $reportContent += ""
        }
    }
}

# Warning Analysis
if ($consolidatedWarnings.Count -gt 0) {
    $reportContent += "## Warning Analysis"
    $reportContent += ""
    $reportContent += "| Warning Code | Occurrences | Message | Files |"
    $reportContent += "|--------------|-------------|---------|-------|"
    
    foreach ($warning in $consolidatedWarnings | Sort-Object Occurrences -Descending | Select-Object -First 15) {
        $message = if ($warning.Message.Length -gt 50) { $warning.Message.Substring(0, 47) + "..." } else { $warning.Message }
        $reportContent += "| ````$($warning.Code)```` | $($warning.Occurrences) | $message | $($warning.Files) |"
    }
    
    if ($consolidatedWarnings.Count -gt 15) {
        $reportContent += ""
        $reportContent += "*... and $($consolidatedWarnings.Count - 15) more warning types*"
    }
    $reportContent += ""
}

# Build Output Summary
$reportContent += "## Build Environment"
$reportContent += ""
$reportContent += "### System Information"
$reportContent += ""
$reportContent += "- **.NET CLI Version**: $dotnetVersion"
$reportContent += "- **Build Date**: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
$reportContent += "- **Project Root**: $ProjectRoot"
$reportContent += "- **Target Framework**: .NET Framework 4.8.1"
$reportContent += ""
$reportContent += "### Project Files Analyzed"
$reportContent += ""
foreach ($project in $projectFiles) {
    $reportContent += "- ````$($project.Name)```` - $($project.Extension.ToUpper()) file"
}
$reportContent += ""

# Fix Priority Guide
if ($consolidatedErrors.Count -gt 0) {
    $reportContent += "## 🎯 Fix Priority Guide"
    $reportContent += ""
    
    $criticalErrors = $consolidatedErrors | Where-Object { $_.Criticality -eq "CRITICAL" }
    $highErrors = $consolidatedErrors | Where-Object { $_.Criticality -eq "HIGH" }
    
    if ($criticalErrors.Count -gt 0) {
        $reportContent += "### 🚨 IMMEDIATE (Critical Errors - $($criticalErrors.Count) types)"
        $reportContent += ""
        $reportContent += "These errors prevent compilation and must be fixed first:"
        $reportContent += ""
        
        foreach ($error in $criticalErrors | Select-Object -First 5) {
            $reportContent += "1. **$($error.Code)**: $($error.Message)"
            $reportContent += "   - **Fix**: $(Get-ErrorFixSuggestion -code $error.Code)"
            $reportContent += "   - **Locations**: $($error.Occurrences) occurrences in $($error.Files)"
            $reportContent += ""
        }
        
        if ($criticalErrors.Count -gt 5) {
            $reportContent += "*... and $($criticalErrors.Count - 5) more critical errors requiring immediate attention*"
            $reportContent += ""
        }
    }
    
    if ($highErrors.Count -gt 0) {
        $reportContent += "### ⚠️ HIGH PRIORITY (High Errors - $($highErrors.Count) types)"
        $reportContent += ""
        $reportContent += "Address these after fixing critical errors:"
        $reportContent += ""
        
        foreach ($error in $highErrors | Select-Object -First 3) {
            $reportContent += "- **$($error.Code)**: $($error.Message) ($($error.Occurrences) occurrences)"
        }
        
        if ($highErrors.Count -gt 3) {
            $reportContent += "- *... and $($highErrors.Count - 3) more high priority errors*"
        }
        $reportContent += ""
    }
    
    $reportContent += "### 📋 General Fix Strategy"
    $reportContent += ""
    $reportContent += "1. **Fix Critical Errors First**: Start with syntax errors and missing references"
    $reportContent += "2. **Build Frequently**: Test compilation after each fix"
    $reportContent += "3. **One Error Type at a Time**: Fix all instances of the same error together"
    $reportContent += "4. **Check Dependencies**: Ensure all NuGet packages and references are correct"
    $reportContent += "5. **Verify .NET 4.8.1 Compatibility**: Check for unsupported language features"
} else {
    $reportContent += "## ✅ Maintenance Recommendations"
    $reportContent += ""
    $reportContent += "Build is successful! Consider these improvements:"
    $reportContent += ""
    
    if ($consolidatedWarnings.Count -gt 0) {
        $reportContent += "1. **Address Warnings**: $($consolidatedWarnings.Count) warning types detected"
        $reportContent += "2. **Code Quality**: Review warnings for potential runtime issues"
        $reportContent += "3. **Best Practices**: Consider enabling more strict compiler warnings"
    } else {
        $reportContent += "1. **Perfect Build**: No errors or warnings detected"
        $reportContent += "2. **Maintain Quality**: Keep running build analysis before commits"
        $reportContent += "3. **Consider Additional Analysis**: Run other DevOps reports for complete quality check"
    }
}

# Helper function for error fix suggestions
function Get-ErrorFixSuggestion($code) {
    switch ($code) {
        'CS0103' { return "Check variable/method names and using statements" }
        'CS0246' { return "Verify namespace imports and assembly references" }
        'CS0117' { return "Check method/property exists in the specified type" }
        'CS1002' { return "Review syntax - likely missing semicolon or brace" }
        'CS1513' { return "Add missing closing brace '}'" }
        'CS0029' { return "Add explicit type conversion or cast" }
        'CS0118' { return "Ensure correct usage syntax (method vs property)" }
        'CS0019' { return "Check operator compatibility with operand types" }
        'CS0161' { return "Ensure all code paths return a value" }
        'CS0162' { return "Remove or fix unreachable code" }
        default { return "Review error message and fix according to compiler guidance" }
    }
}

# Technical Details
$reportContent += ""
$reportContent += "## Technical Analysis Details"
$reportContent += ""
$reportContent += "### Build Process"
$reportContent += ""
# FIXED: Split problematic markdown string to avoid parsing issues
$buildCommandText = "- **Build Command**: ````dotnet build --verbosity normal````"
$reportContent += $buildCommandText
$errorDetectionText = "- **Error Detection**: MSBuild error format parsing"
$reportContent += $errorDetectionText
$reportContent += "- **Consolidation Logic**: Group by error code and message"
$reportContent += "- **Criticality Assessment**: Based on error impact and project patterns"
$reportContent += ""
$reportContent += "### Error Categorization"
$reportContent += ""
$reportContent += "- **CRITICAL**: Prevents compilation, syntax errors, missing references"
$reportContent += "- **HIGH**: Logic errors, unused variables, unreachable code"
$reportContent += "- **MEDIUM**: Standard compiler errors requiring attention"
$reportContent += "- **LOW**: Minor issues and certain warning types"
$reportContent += ""
$reportContent += "### Project-Specific Patterns"
$reportContent += ""
$reportContent += "Special attention given to:"
$reportContent += "- MelonLoader compatibility issues"
$reportContent += "- Thread safety and async/await patterns"
$reportContent += "- IL2CPP and .NET 4.8.1 compatibility"
$reportContent += "- Task and CancellationToken usage"

# Footer
$reportContent += ""
$reportContent += "---"
$reportContent += ""
$reportContent += "**Build Analysis**: Automated error detection and prioritization"
$reportContent += ""
$reportContent += "*Generated by MixerThreholdMod DevOps Suite - Build Error Report Generator*"

try {
    $reportContent | Out-File -FilePath $reportPath -Encoding UTF8
    $saveSuccess = $true
}
catch {
    Write-Host "⚠️ Error saving detailed report: $_" -ForegroundColor DarkYellow
    $saveSuccess = $false
}

Write-Host "``n🚀 Build error analysis complete!" -ForegroundColor Green

# OUTPUT PATH AT THE END for easy finding
if ($saveSuccess) {
    Write-Host "``n📄 DETAILED REPORT SAVED:" -ForegroundColor Green
    Write-Host "   Location: $reportPath" -ForegroundColor Cyan
    Write-Host "   Size: $([Math]::Round((Get-Item $reportPath).Length / 1KB, 1)) KB" -ForegroundColor Gray
} else {
    Write-Host "``n⚠️ No detailed report generated" -ForegroundColor DarkYellow
}

# INTERACTIVE WORKFLOW LOOP (only when running standalone)
if ($IsInteractive -and -not $RunningFromScript) {
    do {
        Write-Host "``n🎯 What would you like to do next?" -ForegroundColor DarkCyan
        Write-Host "   D - Display report in console" -ForegroundColor Green
        Write-Host "   R - Re-run build error analysis" -ForegroundColor DarkYellow
        Write-Host "   X - Exit to DevOps menu" -ForegroundColor Gray
        
        $choice = Read-Host "``nEnter choice (D/R/X)"
        $choice = $choice.ToUpper()
        
        switch ($choice) {
            'D' {
                if ($saveSuccess) {
                    Write-Host "``n📋 DISPLAYING BUILD ERROR REPORT:" -ForegroundColor DarkCyan
                    Write-Host "==================================" -ForegroundColor DarkCyan
                    try {
                        $reportDisplay = Get-Content -Path $reportPath -Raw
                        Write-Host $reportDisplay -ForegroundColor White
                        Write-Host "``n==================================" -ForegroundColor DarkCyan
                        Write-Host "📋 END OF REPORT" -ForegroundColor DarkCyan
                    }
                    catch {
                        Write-Host "❌ Could not display report: $_" -ForegroundColor Red
                    }
                } else {
                    Write-Host "❌ No report available to display" -ForegroundColor Red
                }
            }
            'R' {
                Write-Host "``n🔄 RE-RUNNING BUILD ERROR ANALYSIS..." -ForegroundColor DarkYellow
                Write-Host "====================================" -ForegroundColor DarkYellow
                & $MyInvocation.MyCommand.Path
                return
            }
            'X' {
                Write-Host "``n👋 Returning to DevOps menu..." -ForegroundColor Gray
                return
            }
            default {
                Write-Host "❌ Invalid choice. Please enter D, R, or X." -ForegroundColor Red
            }
        }
    } while ($choice -notin @('X'))
} else {
    # FIXED: Proper string termination for PowerShell 5.1 compatibility
    Write-Host "📄 Script completed - returning to caller" -ForegroundColor DarkGray
}