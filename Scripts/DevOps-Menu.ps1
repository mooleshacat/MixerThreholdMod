# MixerThreholdMod DevOps Menu Launcher
# A PowerShell-based categorized menu for launching DevOps scripts with prefix-based organization
# Groups scripts by their filename patterns for better organization and user experience

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
if ((Split-Path $ScriptDir -Leaf) -ieq "Scripts") {
    $ProjectRoot = Split-Path $ScriptDir -Parent
} else {
    $ProjectRoot = $ScriptDir
}

# Function to categorize scripts by prefix and patterns
function Get-CategorizedScripts {
    param($ScriptDirectory)
    
    # Get all .ps1 scripts excluding this menu script
    $allScripts = Get-ChildItem -Path $ScriptDirectory -Filter "*.ps1" | 
        Where-Object { $_.Name -ne "DevOps-Menu.ps1" } | 
        Sort-Object Name
    
    # Define script categories with their patterns and metadata
    $categories = @{
        "High-Level Reports" = @{
            Patterns = @("*-Report.ps1")  # Scripts with -Report in filename
            Icon = "🛡️"
            Color = "Red"
            Description = "Comprehensive orchestrators that run multiple analysis tools and provide project-wide insights"
        }
        "Analysis & Quality Assurance" = @{
            Patterns = @("Analyze-*", "Check-*", "Find-*", "Verify-*")
            Icon = "🔍"
            Color = "DarkCyan"
            Description = "Code analysis, quality checks, and verification tools for maintaining code standards"
        }
        "Report Generation" = @{
            Patterns = @("Generate-*")  # Scripts without -Report suffix
            Icon = "📋"
            Color = "DarkGreen"
            Description = "Detailed report generators for specific aspects of the codebase"
        }
        "Version & Release Management" = @{
            Patterns = @("Update-*")
            Icon = "🚀"
            Color = "DarkMagenta"
            Description = "Version management and release preparation tools"
        }
        "Git & Source Control" = @{
            Patterns = @("Git-*")
            Icon = "🌿"
            Color = "DarkYellow"
            Description = "Git operations and source control management utilities"
        }
        "Development Tools" = @{
            Patterns = @()  # Catch-all for uncategorized scripts
            Icon = "🔧"
            Color = "Gray"
            Description = "Miscellaneous development utilities and tools"
        }
    }
    
    # Categorize scripts
    $categorizedScripts = @{}
    
    foreach ($categoryName in $categories.Keys) {
        $categorizedScripts[$categoryName] = @{
            Scripts = @()
            Metadata = $categories[$categoryName]
        }
    }
    
    foreach ($script in $allScripts) {
        $assigned = $false
        
        # Special handling for high-level reports (must contain -Report in filename)
        if ($script.Name -match ".*-Report\.ps1$") {
            $categorizedScripts["High-Level Reports"].Scripts += $script
            $assigned = $true
        }
        # Handle other categories (excluding -Report scripts from Generate- category)
        elseif (-not ($script.Name -match ".*-Report\.ps1$")) {
            foreach ($categoryName in $categories.Keys) {
                if ($categoryName -eq "High-Level Reports") { continue }  # Skip high-level reports category
                
                $patterns = $categories[$categoryName].Patterns
                
                foreach ($pattern in $patterns) {
                    if ($script.Name -like $pattern) {
                        $categorizedScripts[$categoryName].Scripts += $script
                        $assigned = $true
                        break
                    }
                }
                
                if ($assigned) { break }
            }
        }
        
        # If not assigned to any specific category, put in "Development Tools"
        if (-not $assigned) {
            $categorizedScripts["Development Tools"].Scripts += $script
        }
    }
    
    # Remove empty categories
    $result = @{}
    foreach ($categoryName in $categorizedScripts.Keys) {
        if ($categorizedScripts[$categoryName].Scripts.Count -gt 0) {
            $result[$categoryName] = $categorizedScripts[$categoryName]
        }
    }
    
    return $result
}

# Function to display the categorized menu
function Show-CategorizedMenu {
    param($CategorizedScripts)
    
    Clear-Host
    
    Write-Host ""
    Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor DarkCyan
    Write-Host " 🚀 MixerThreholdMod DevOps Suite v2.0" -ForegroundColor White
    Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor DarkCyan
    Write-Host ""
    
    $totalScripts = 0
    foreach ($category in $CategorizedScripts.Values) {
        $totalScripts += $category.Scripts.Count
    }
    
    Write-Host "📊 Professional DevOps Tools: $totalScripts scripts across $($CategorizedScripts.Count) categories" -ForegroundColor Gray
    Write-Host ""
    
    $optionNumber = 1
    $scriptMap = @{}
    
    # Define category display order for better UX
    $categoryOrder = @(
        "High-Level Reports",
        "Analysis & Quality Assurance", 
        "Report Generation",
        "Version & Release Management",
        "Git & Source Control",
        "Development Tools"
    )
    
    # Display each category in the defined order
    foreach ($categoryName in $categoryOrder) {
        if (-not $CategorizedScripts.ContainsKey($categoryName)) { continue }
        
        $category = $CategorizedScripts[$categoryName]
        $metadata = $category.Metadata
        
        # Category header with enhanced styling
        Write-Host "$($metadata.Icon) $categoryName" -ForegroundColor $metadata.Color
        Write-Host "   $($metadata.Description)" -ForegroundColor DarkGray
        Write-Host ""
        
        # Scripts in this category
        foreach ($script in $category.Scripts | Sort-Object Name) {
            $scriptMap[$optionNumber] = $script
            
            # Extract description from script if available
            $description = Get-ScriptDescription -ScriptPath $script.FullName -ScriptName $script.Name
            
            Write-Host "   $optionNumber. " -NoNewline -ForegroundColor Gray
            Write-Host $script.Name -NoNewline -ForegroundColor White
            
            if ($description) {
                Write-Host " - $description" -ForegroundColor DarkGray
            } else {
                Write-Host ""
            }
            
            $optionNumber++
        }
        Write-Host ""
    }
    
    # Special exit option
    Write-Host "🎯 Navigation:" -ForegroundColor DarkCyan
    Write-Host ""
    Write-Host "   Q. " -NoNewline -ForegroundColor Gray
    Write-Host "Quit DevOps Suite" -ForegroundColor Red
    Write-Host ""
    
    return $scriptMap
}

# Function to extract description from script header with enhanced patterns
function Get-ScriptDescription {
    param($ScriptPath, $ScriptName)
    
    try {
        # Special descriptions for high-level orchestrators
        if ($ScriptName -eq "Generate-Comprehensive-Report.ps1") {
            return "Comprehensive project analysis - runs ALL DevOps tools and provides unified dashboard"
        }
        if ($ScriptName -eq "Generate-Corruption-Report.ps1") {
            return "Ultimate corruption detection - identifies merge conflicts, encoding issues, and structural problems"
        }
        
        $firstFewLines = Get-Content -Path $ScriptPath -TotalCount 15 -ErrorAction SilentlyContinue
        
        foreach ($line in $firstFewLines) {
            # Look for description patterns in comments
            if ($line -match '^#\s*(.+)$' -and 
                $line -notmatch '^#\s*MixerThreholdMod' -and 
                $line -notmatch '^#\s*Excludes:' -and
                $line -notmatch '^#\s*\$' -and
                $line -notmatch '^#\s*(Master|Ultimate|Orchestrator)') {
                
                $description = $matches[1].Trim()
                
                # Enhanced description filtering
                if ($description.Length -gt 10 -and $description.Length -lt 100 -and
                    $description -notmatch '(DevOps Tool:|COMPATIBLE|VERSION)') {
                    return $description
                }
            }
        }
    }
    catch {
        # Ignore errors reading script files
    }
    
    return $null
}

# Function to handle user selection with enhanced messaging
function Process-UserSelection {
    param($Choice, $ScriptMap, $CategorizedScripts)
    
    # Handle quit
    if ($Choice -ieq "Q") {
        Write-Host "👋 Exiting MixerThreholdMod DevOps Suite..." -ForegroundColor Gray
        return $false
    }
    
    # Handle numeric selection with enhanced error handling
    try {
        $choiceNum = [int]$Choice
        
        if ($choiceNum -ge 1 -and $choiceNum -le $ScriptMap.Count -and $ScriptMap.ContainsKey($choiceNum)) {
            $selectedScript = $ScriptMap[$choiceNum]
            
            # Enhanced messaging for different script types
            if ($selectedScript.Name -match ".*-Report\.ps1$") {
                Write-Host ""
                Write-Host "🛡️  LAUNCHING HIGH-LEVEL ORCHESTRATOR: " -NoNewline -ForegroundColor Red
                Write-Host $selectedScript.Name -ForegroundColor Yellow
                Write-Host ""
                Write-Host "📊 This script will:" -ForegroundColor DarkCyan
                Write-Host "   • Run multiple analysis tools automatically" -ForegroundColor Gray
                Write-Host "   • Generate comprehensive reports in Reports/ directory" -ForegroundColor Gray
                Write-Host "   • Display summary results in console" -ForegroundColor Gray
                Write-Host "   • Identify critical issues requiring attention" -ForegroundColor Gray
                Write-Host ""
                Write-Host "💡 For detailed analysis, check Reports/* or run specific tools from this menu" -ForegroundColor DarkYellow
                Write-Host ""
            } else {
                Write-Host ""
                Write-Host "🔍 Running DevOps Tool: " -NoNewline -ForegroundColor Gray
                Write-Host $selectedScript.Name -ForegroundColor Green
                Write-Host ""
            }
            
            try {
                # Execute the script with proper error handling
                & $selectedScript.FullName
                
                if ($selectedScript.Name -match ".*-Report\.ps1$") {
                    Write-Host ""
                    Write-Host "✅ High-level analysis completed successfully!" -ForegroundColor Green
                    Write-Host "📄 Detailed reports saved to Reports/ directory" -ForegroundColor Cyan
                } else {
                    Write-Host ""
                    Write-Host "✅ DevOps tool completed successfully!" -ForegroundColor Green
                }
            } catch {
                Write-Host "❌ Script execution error: $($_.Exception.Message)" -ForegroundColor Red
                Write-Host "📋 Stack trace: $($_.ScriptStackTrace)" -ForegroundColor DarkYellow
            }
        } else {
            Write-Host "❌ Invalid selection: $Choice (valid range: 1-$($ScriptMap.Count), Q)" -ForegroundColor Red
        }
    } catch {
        Write-Host "❌ Invalid input format: $Choice (please enter a number or Q)" -ForegroundColor Red
    }
    
    return $true
}

# Main menu loop with enhanced error handling
function Start-DevOpsMenu {
    try {
        do {
            # Get categorized scripts
            $categorizedScripts = Get-CategorizedScripts -ScriptDirectory $ScriptDir
            
            if ($categorizedScripts.Count -eq 0) {
                Write-Host "❌ No PowerShell scripts found in Scripts folder!" -ForegroundColor Red
                Write-Host "Press ENTER to exit..." -ForegroundColor Gray -NoNewline
                Read-Host
                return
            }
            
            # Show menu and get user choice
            $scriptMap = Show-CategorizedMenu -CategorizedScripts $categorizedScripts
            
            Write-Host "Select a DevOps tool (1-$($scriptMap.Count)) or Q to quit: " -NoNewline -ForegroundColor Green
            $choice = Read-Host
            
            # Skip processing if user just pressed ENTER
            if ([string]::IsNullOrWhiteSpace($choice)) {
                Write-Host "⚠️  No selection made. Please choose a DevOps tool." -ForegroundColor DarkYellow
                Start-Sleep -Seconds 1
                continue
            }
            
            # Process the selection
            $continue = Process-UserSelection -Choice $choice.Trim() -ScriptMap $scriptMap -CategorizedScripts $categorizedScripts
            
            if ($continue) {
                Write-Host ""
                Write-Host "Press ENTER to return to DevOps Suite menu..." -ForegroundColor Gray -NoNewline
                Read-Host
            }
            
        } while ($continue)
        
    } catch {
        Write-Host "💥 Unexpected error in DevOps Suite: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host "📋 Stack trace: $($_.ScriptStackTrace)" -ForegroundColor DarkYellow
        Write-Host "Press ENTER to exit..." -ForegroundColor Gray -NoNewline
        Read-Host
    }
}

# Display startup banner
Write-Host ""
Write-Host "🚀 Initializing MixerThreholdMod DevOps Suite..." -ForegroundColor DarkCyan
Start-Sleep -Milliseconds 500

# Start the enhanced menu system
Start-DevOpsMenu