# MixerThreholdMod DevOps Menu Launcher
# A PowerShell-based categorized menu for launching DevOps scripts with prefix-based organization
# Groups scripts by their filename prefixes for better organization and user experience

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
if ((Split-Path $ScriptDir -Leaf) -ieq "Scripts") {
    $ProjectRoot = Split-Path $ScriptDir -Parent
} else {
    $ProjectRoot = $ScriptDir
}

# Function to categorize scripts by prefix
function Get-CategorizedScripts {
    param($ScriptDirectory)
    
    # Get all .ps1 scripts excluding this menu script
    $allScripts = Get-ChildItem -Path $ScriptDirectory -Filter "*.ps1" | 
        Where-Object { $_.Name -ne "DevOps-Menu.ps1" } | 
        Sort-Object Name
    
    # Define script categories with their prefixes and metadata
    $categories = @{
        "Analysis & Quality" = @{
            Prefixes = @("Analyze-", "Check-", "Find-", "Verify-")
            Icon = "🔍"
            Color = "DarkCyan"
            Description = "Code analysis, quality checks, and verification tools"
        }
        "Documentation & Reports" = @{
            Prefixes = @("Generate-")
            Icon = "📋"
            Color = "DarkGreen"
            Description = "Documentation generation and comprehensive reporting"
        }
        "Version & Release" = @{
            Prefixes = @("Update-")
            Icon = "🚀"
            Color = "DarkMagenta"
            Description = "Version management and release preparation tools"
        }
        "Git & Source Control" = @{
            Prefixes = @("Git-")
            Icon = "🌿"
            Color = "DarkYellow"
            Description = "Git operations and source control management"
        }
        "Other Tools" = @{
            Prefixes = @()  # Catch-all for uncategorized scripts
            Icon = "🔧"
            Color = "Gray"
            Description = "Miscellaneous DevOps utilities and tools"
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
        
        foreach ($categoryName in $categories.Keys) {
            $prefixes = $categories[$categoryName].Prefixes
            
            foreach ($prefix in $prefixes) {
                if ($script.Name.StartsWith($prefix)) {
                    $categorizedScripts[$categoryName].Scripts += $script
                    $assigned = $true
                    break
                }
            }
            
            if ($assigned) { break }
        }
        
        # If not assigned to any specific category, put in "Other Tools"
        if (-not $assigned) {
            $categorizedScripts["Other Tools"].Scripts += $script
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
    Write-Host "════════════════════════════════════════════════" -ForegroundColor DarkCyan
    Write-Host " 🚀 MixerThreholdMod DevOps Suite" -ForegroundColor White
    Write-Host "════════════════════════════════════════════════" -ForegroundColor DarkCyan
    Write-Host ""
    
    $totalScripts = 0
    foreach ($category in $CategorizedScripts.Values) {
        $totalScripts += $category.Scripts.Count
    }
    
    Write-Host "📊 Available Tools: $totalScripts scripts across $($CategorizedScripts.Count) categories" -ForegroundColor Gray
    Write-Host ""
    
    $optionNumber = 1
    $scriptMap = @{}
    
    # Display each category
    foreach ($categoryName in $CategorizedScripts.Keys | Sort-Object) {
        $category = $CategorizedScripts[$categoryName]
        $metadata = $category.Metadata
        
        # Category header
        Write-Host "$($metadata.Icon) $categoryName" -ForegroundColor $metadata.Color
        Write-Host "   $($metadata.Description)" -ForegroundColor DarkGray
        Write-Host ""
        
        # Scripts in this category
        foreach ($script in $category.Scripts | Sort-Object Name) {
            $scriptMap[$optionNumber] = $script
            
            # Extract description from script if available
            $description = Get-ScriptDescription -ScriptPath $script.FullName
            
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
    
    # Special options
    Write-Host "🎯 Special Actions:" -ForegroundColor DarkCyan
    Write-Host ""
    Write-Host "   C. " -NoNewline -ForegroundColor Gray
    Write-Host "Generate-Comprehensive-Report.ps1" -NoNewline -ForegroundColor Yellow
    Write-Host " - Run ALL analysis tools" -ForegroundColor DarkGray
    Write-Host ""
    Write-Host "   Q. " -NoNewline -ForegroundColor Gray
    Write-Host "Quit" -ForegroundColor Red
    Write-Host ""
    
    return $scriptMap
}

# Function to extract description from script header
function Get-ScriptDescription {
    param($ScriptPath)
    
    try {
        $firstFewLines = Get-Content -Path $ScriptPath -TotalCount 10 -ErrorAction SilentlyContinue
        
        foreach ($line in $firstFewLines) {
            # Look for description patterns in comments
            if ($line -match '^#\s*(.+)$' -and 
                $line -notmatch '^#\s*MixerThreholdMod' -and 
                $line -notmatch '^#\s*Excludes:' -and
                $line -notmatch '^#\s*\$') {
                
                $description = $matches[1].Trim()
                if ($description.Length -gt 5 -and $description.Length -lt 80) {
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

# Function to handle user selection
function Process-UserSelection {
    param($Choice, $ScriptMap, $CategorizedScripts)
    
    if ($Choice -ieq "Q") {
        Write-Host "👋 Exiting DevOps Suite..." -ForegroundColor Gray
        return $false
    }
    
    if ($Choice -ieq "C") {
        # Run comprehensive report
        $comprehensiveScript = Join-Path $ScriptDir "Generate-Comprehensive-Report.ps1"
        if (Test-Path $comprehensiveScript) {
            Write-Host ""
            Write-Host "🚀 Running Comprehensive Analysis..." -ForegroundColor Green
            Write-Host "This will execute ALL analysis tools sequentially." -ForegroundColor DarkYellow
            Write-Host ""
            
            & $comprehensiveScript
        } else {
            Write-Host "❌ Comprehensive report script not found!" -ForegroundColor Red
        }
        return $true
    }
    
    # Handle numeric selection
    try {
        $choiceNum = [int]$Choice
        if ($ScriptMap.ContainsKey($choiceNum)) {
            $selectedScript = $ScriptMap[$choiceNum]
            
            Write-Host ""
            Write-Host "🔍 Running: " -NoNewline -ForegroundColor Gray
            Write-Host $selectedScript.Name -ForegroundColor Green
            Write-Host ""
            
            # Execute the script
            & $selectedScript.FullName
        } else {
            Write-Host "❌ Invalid selection: $Choice" -ForegroundColor Red
        }
    } catch {
        Write-Host "❌ Invalid selection: $Choice" -ForegroundColor Red
    }
    
    return $true
}

# Main menu loop
function Start-DevOpsMenu {
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
        
        Write-Host "Select an option (1-$($scriptMap.Count), C for comprehensive, Q to quit): " -NoNewline -ForegroundColor Green
        $choice = Read-Host
        
        # Process the selection
        $continue = Process-UserSelection -Choice $choice -ScriptMap $scriptMap -CategorizedScripts $categorizedScripts
        
        if ($continue) {
            Write-Host ""
            Write-Host "Press ENTER to return to menu..." -ForegroundColor Gray -NoNewline
            Read-Host
        }
        
    } while ($continue)
}

# Start the menu system
Start-DevOpsMenu