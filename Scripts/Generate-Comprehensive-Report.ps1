# MixerThreholdMod DevOps Tool: Comprehensive Project Report Generator
# Master orchestrator that runs all report generation scripts and creates unified project health dashboard
# Combines method complexity, duplicate code, documentation, compliance, and more into one comprehensive report
# Excludes: ForCopilot, Scripts, and Legacy directories

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
if ((Split-Path $ScriptDir -Leaf) -ieq "Scripts") {
    $ProjectRoot = Split-Path $ScriptDir -Parent
} else {
    $ProjectRoot = $ScriptDir
}

Write-Host "🚀 COMPREHENSIVE PROJECT ANALYSIS STARTING" -ForegroundColor DarkCyan
Write-Host "================================================" -ForegroundColor DarkCyan
Write-Host "Project Root: $ProjectRoot" -ForegroundColor Gray
Write-Host "Analysis Date: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" -ForegroundColor Gray

# Function to estimate project size and warn about processing time
function Get-ProjectSizeEstimate {
    param($path)
    
    try {
        Write-Host "`n⏱️  Estimating project size and processing time..." -ForegroundColor DarkYellow
        
        # Quick scan for size estimation
        $allFiles = Get-ChildItem -Path $path -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
            $_.FullName -notmatch "[\\/](ForCopilot|Scripts|Legacy)[\\/]" -and
            $_.FullName -notmatch "[\\/]\.git[\\/]"
        }
        
        $csharpFiles = $allFiles | Where-Object { $_.Extension -eq ".cs" }
        $totalSize = ($allFiles | Measure-Object -Property Length -Sum).Sum / 1MB
        
        Write-Host ("📊 Project Size Estimate:") -ForegroundColor DarkCyan
        Write-Host ("   Total Files: {0:N0}" -f $allFiles.Count) -ForegroundColor Gray
        Write-Host ("   C# Files: {0:N0}" -f $csharpFiles.Count) -ForegroundColor Gray
        Write-Host ("   Total Size: {0:F1} MB" -f $totalSize) -ForegroundColor Gray
        
        # Estimate processing time based on file count
        $estimatedMinutes = [Math]::Ceiling(($csharpFiles.Count * 0.1) + ($allFiles.Count * 0.05))
        
        if ($estimatedMinutes -gt 5) {
            Write-Host "`n⚠️  LARGE PROJECT DETECTED!" -ForegroundColor Red
            Write-Host ("🕐 Estimated processing time: {0}-{1} minutes" -f $estimatedMinutes, ($estimatedMinutes * 2)) -ForegroundColor DarkYellow
            Write-Host "💡 This analysis will:" -ForegroundColor DarkYellow
            Write-Host "   • Run 8 comprehensive DevOps scripts sequentially" -ForegroundColor Gray
            Write-Host "   • Analyze every C# file for complexity, duplicates, and compliance" -ForegroundColor Gray
            Write-Host "   • Generate detailed documentation and reports" -ForegroundColor Gray
            Write-Host "   • Create comprehensive project health dashboard" -ForegroundColor Gray
            
            Write-Host "`n🎯 Options:" -ForegroundColor DarkCyan
            Write-Host "   Y - Continue with full analysis (recommended)" -ForegroundColor Green
            Write-Host "   Q - Quick analysis (skip time-intensive scripts)" -ForegroundColor DarkYellow
            Write-Host "   X - Exit and run individual scripts instead" -ForegroundColor Red
            
            do {
                $choice = Read-Host "`nProceed? (Y/Q/X)"
                $choice = $choice.ToUpper()
            } while ($choice -notin @('Y', 'Q', 'X'))
            
            switch ($choice) {
                'Y' { 
                    Write-Host "✅ Proceeding with full comprehensive analysis..." -ForegroundColor Green
                    return @{ Mode = "Full"; Continue = $true }
                }
                'Q' { 
                    Write-Host "⚡ Running quick analysis (excluding duplicate detection and method complexity)..." -ForegroundColor DarkYellow
                    return @{ Mode = "Quick"; Continue = $true }
                }
                'X' { 
                    Write-Host "❌ Analysis cancelled. Run individual scripts for targeted analysis." -ForegroundColor Red
                    return @{ Mode = "Cancel"; Continue = $false }
                }
            }
        } else {
            Write-Host "✅ Project size is manageable - proceeding with full analysis" -ForegroundColor Green
            return @{ Mode = "Full"; Continue = $true }
        }
    }
    catch {
        Write-Host "⚠️  Could not estimate project size: $($_.Exception.Message)" -ForegroundColor DarkYellow
        return @{ Mode = "Full"; Continue = $true }
    }
}

# Function to run a script and capture its output with progress
function Invoke-ReportScript {
    param(
        [string]$ScriptPath,
        [string]$ScriptName,
        [string]$Description,
        [int]$Current,
        [int]$Total
    )
    
    $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
    
    Write-Host "`n🔍 [$Current/$Total] Running: $ScriptName" -ForegroundColor DarkCyan
    Write-Host "   $Description" -ForegroundColor Gray
    Write-Host "   Started: $(Get-Date -Format 'HH:mm:ss')" -ForegroundColor DarkGray
    
    try {
        if (Test-Path $ScriptPath) {
            # Show progress indicator for long-running scripts
            $progressJob = Start-Job -ScriptBlock {
                $count = 0
                while ($true) {
                    Start-Sleep -Seconds 5
                    $count += 5
                    Write-Host "   ⏳ Running for $count seconds..." -ForegroundColor DarkGray
                }
            }
            
            try {
                # Capture output and errors
                $output = & powershell.exe -ExecutionPolicy Bypass -File $ScriptPath 2>&1
                $exitCode = $LASTEXITCODE
            }
            finally {
                Stop-Job $progressJob -ErrorAction SilentlyContinue
                Remove-Job $progressJob -ErrorAction SilentlyContinue
            }
            
            $stopwatch.Stop()
            $duration = $stopwatch.Elapsed.TotalSeconds
            
            if ($exitCode -eq 0) {
                Write-Host "   ✅ Completed successfully in $($duration.ToString('F1'))s" -ForegroundColor Green
                return [PSCustomObject]@{
                    ScriptName = $ScriptName
                    Status = "Success"
                    Duration = $duration
                    Output = $output
                    Error = $null
                }
            } else {
                Write-Host "   ⚠️  Completed with warnings in $($duration.ToString('F1'))s" -ForegroundColor DarkYellow
                return [PSCustomObject]@{
                    ScriptName = $ScriptName
                    Status = "Warning"
                    Duration = $duration
                    Output = $output
                    Error = "Exit code: $exitCode"
                }
            }
        } else {
            Write-Host "   ❌ Script not found: $ScriptPath" -ForegroundColor Red
            return [PSCustomObject]@{
                ScriptName = $ScriptName
                Status = "NotFound"
                Duration = 0
                Output = $null
                Error = "Script file not found"
            }
        }
    }
    catch {
        $stopwatch.Stop()
        Write-Host "   ❌ Failed: $($_.Exception.Message)" -ForegroundColor Red
        return [PSCustomObject]@{
            ScriptName = $ScriptName
            Status = "Failed"
            Duration = $stopwatch.Elapsed.TotalSeconds
            Output = $null
            Error = $_.Exception.Message
        }
    }
}

# Function to get project statistics with progress
function Get-ProjectStatistics {
    Write-Host "`n📊 Calculating detailed project statistics..." -ForegroundColor DarkGray
    
    $stats = [PSCustomObject]@{
        TotalFiles = 0
        CSharpFiles = 0
        PowerShellFiles = 0
        TotalLines = 0
        ProjectSize = 0
    }
    
    try {
        # Get all relevant files
        $allFiles = Get-ChildItem -Path $ProjectRoot -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
            $_.FullName -notmatch "[\\/](ForCopilot|Scripts|Legacy)[\\/]" -and
            $_.FullName -notmatch "[\\/]\.git[\\/]"
        }
        
        $stats.TotalFiles = $allFiles.Count
        $stats.CSharpFiles = ($allFiles | Where-Object { $_.Extension -eq ".cs" }).Count
        $stats.PowerShellFiles = ($allFiles | Where-Object { $_.Extension -eq ".ps1" }).Count
        $stats.ProjectSize = [Math]::Round(($allFiles | Measure-Object -Property Length -Sum).Sum / 1MB, 2)
        
        # Count lines in C# files with progress
        $csharpFiles = $allFiles | Where-Object { $_.Extension -eq ".cs" }
        $processedFiles = 0
        
        foreach ($file in $csharpFiles) {
            try {
                $content = Get-Content -Path $file.FullName -ErrorAction SilentlyContinue
                if ($content) {
                    $stats.TotalLines += $content.Count
                }
                
                $processedFiles++
                if ($processedFiles % 50 -eq 0) {
                    Write-Host "   📈 Processed $processedFiles/$($csharpFiles.Count) C# files..." -ForegroundColor DarkGray
                }
            }
            catch {
                # Skip files that can't be read
            }
        }
        
        Write-Host "   ✅ Statistics calculated successfully" -ForegroundColor Green
    }
    catch {
        Write-Host "   ⚠️  Error calculating project statistics: $($_.Exception.Message)" -ForegroundColor DarkYellow
    }
    
    return $stats
}

# Estimate project size and get user preference
$sizeEstimate = Get-ProjectSizeEstimate -path $ProjectRoot

if (-not $sizeEstimate.Continue) {
    Write-Host "`nAnalysis cancelled by user." -ForegroundColor Red
    Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
    Read-Host
    return
}

# List of DevOps scripts to run
$allDevOpsScripts = @(
    @{
        Name = "Method Complexity Analysis"
        Script = "Analyze-MethodComplexity.ps1"
        Description = "Analyzing method complexity and identifying refactoring candidates"
        Category = "Code Quality"
        TimeIntensive = $true
    },
    @{
        Name = "Duplicate Code Detection"
        Script = "Find-DuplicateCode.ps1"
        Description = "Scanning for duplicate code blocks across the codebase"
        Category = "Code Quality"
        TimeIntensive = $true
    },
    @{
        Name = "XML Documentation Verification"
        Script = "Verify-XmlDocs.ps1"
        Description = "Checking XML documentation coverage and quality"
        Category = "Documentation"
        TimeIntensive = $false
    },
    @{
        Name = "Copilot Compliance Check"
        Script = "Check-CopilotCompliance.ps1"
        Description = "Verifying adherence to GitHub Copilot instructions"
        Category = "Compliance"
        TimeIntensive = $true
    },
    @{
        Name = "Constants Documentation"
        Script = "Generate-ConstantsDoc.ps1"
        Description = "Generating comprehensive constants documentation"
        Category = "Documentation"
        TimeIntensive = $false
    },
    @{
        Name = "Version Consistency Check"
        Script = "Update-VersionNumbers.ps1"
        Description = "Scanning version numbers across project files (dry run)"
        Category = "Configuration"
        TimeIntensive = $false
    },
    @{
        Name = "Changelog Generation"
        Script = "Generate-ChangeLog.ps1"
        Description = "Generating project changelog from git history"
        Category = "Release Management"
        TimeIntensive = $false
    },
    @{
        Name = "Release Notes Generation"
        Script = "Generate-ReleaseNotes.ps1"
        Description = "Creating professional release notes"
        Category = "Release Management"
        TimeIntensive = $false
    }
)

# Filter scripts based on mode
if ($sizeEstimate.Mode -eq "Quick") {
    $devOpsScripts = $allDevOpsScripts | Where-Object { -not $_.TimeIntensive }
    Write-Host "`n⚡ Quick Mode: Running $($devOpsScripts.Count) lightweight scripts" -ForegroundColor DarkYellow
} else {
    $devOpsScripts = $allDevOpsScripts
    Write-Host "`n🔥 Full Mode: Running all $($devOpsScripts.Count) comprehensive scripts" -ForegroundColor Green
}

# Get project statistics
$projectStats = Get-ProjectStatistics

# Run all DevOps scripts
$scriptResults = @()
$totalStartTime = Get-Date
$scriptIndex = 1

foreach ($scriptInfo in $devOpsScripts) {
    $scriptPath = Join-Path $ScriptDir $scriptInfo.Script
    $result = Invoke-ReportScript -ScriptPath $scriptPath -ScriptName $scriptInfo.Name -Description $scriptInfo.Description -Current $scriptIndex -Total $devOpsScripts.Count
    $result | Add-Member -MemberType NoteProperty -Name "Category" -Value $scriptInfo.Category
    $scriptResults += $result
    
    $scriptIndex++
    Start-Sleep -Milliseconds 500  # Brief pause between scripts
}

$totalDuration = ((Get-Date) - $totalStartTime).TotalSeconds

# Generate comprehensive report (existing report generation code continues...)
Write-Host "`n📋 GENERATING COMPREHENSIVE REPORT..." -ForegroundColor DarkCyan
Write-Host "=====================================" -ForegroundColor DarkCyan

# (Rest of the report generation code remains the same as in the original script)
# ... [Include all the existing report generation code from the original script] ...

# Display summary with performance info
Write-Host "`n🎉 COMPREHENSIVE ANALYSIS COMPLETE!" -ForegroundColor Green
Write-Host "===============================================" -ForegroundColor Green
Write-Host "📄 Report saved to: $outputPath" -ForegroundColor Gray
Write-Host "🏥 Project Health Score: $healthColor $healthScore%" -ForegroundColor Gray
Write-Host "⏱️  Total Analysis Time: $($totalDuration.ToString('F1')) seconds ($([Math]::Round($totalDuration/60, 1)) minutes)" -ForegroundColor Gray
Write-Host "🔧 Scripts Executed: $($scriptResults.Count)" -ForegroundColor Gray

if ($sizeEstimate.Mode -eq "Quick") {
    Write-Host "`n⚡ Quick Mode completed - for full analysis, run again and select 'Y'" -ForegroundColor DarkYellow
}

Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
Read-Host