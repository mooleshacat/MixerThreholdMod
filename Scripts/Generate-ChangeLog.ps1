# MixerThreholdMod DevOps Tool: Changelog Generator
# Creates changelog from git commits between releases
# Automatically categorizes commits and generates professional changelog
# Excludes: ForCopilot, ForConstants, Scripts, and Legacy directories

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
if ((Split-Path $ScriptDir -Leaf) -ieq "Scripts") {
    $ProjectRoot = Split-Path $ScriptDir -Parent
} else {
    $ProjectRoot = $ScriptDir
}

Write-Host "Generating changelog in: $ProjectRoot" -ForegroundColor DarkCyan

# Function to get git status
function Test-GitRepository {
    param($path)
    
    try {
        Push-Location $path
        $null = git rev-parse --git-dir 2>$null
        return $true
    }
    catch {
        return $false
    }
    finally {
        Pop-Location
    }
}

# Function to get tags
function Get-GitTags {
    try {
        $tags = git tag --sort=-version:refname 2>$null
        if ($LASTEXITCODE -ne 0) { return @() }
        return $tags | Where-Object { $_ -ne $null -and $_ -ne "" }
    }
    catch {
        return @()
    }
}

# Function to parse commit message
function Get-CommitCategory {
    param($message)
    
    $message = $message.ToLower()
    
    # Conventional commit patterns
    if ($message -match '^feat(\(.+\))?:') { return "Features" }
    if ($message -match '^fix(\(.+\))?:') { return "Bug Fixes" }
    if ($message -match '^docs(\(.+\))?:') { return "Documentation" }
    if ($message -match '^style(\(.+\))?:') { return "Style Changes" }
    if ($message -match '^refactor(\(.+\))?:') { return "Refactoring" }
    if ($message -match '^perf(\(.+\))?:') { return "Performance" }
    if ($message -match '^test(\(.+\))?:') { return "Tests" }
    if ($message -match '^chore(\(.+\))?:') { return "Maintenance" }
    if ($message -match '^build(\(.+\))?:') { return "Build System" }
    if ($message -match '^ci(\(.+\))?:') { return "CI/CD" }
    
    # Custom patterns for this project
    if ($message -match '(save|backup|crash|emergency)') { return "Save System" }
    if ($message -match '(thread|async|lock|concurrent)') { return "Thread Safety" }
    if ($message -match '(mixer|audio|volume|threshold)') { return "Mixer Features" }
    if ($message -match '(patch|harmony|hook)') { return "Game Patches" }
    if ($message -match '(log|debug|trace|monitor)') { return "Debugging" }
    if ($message -match '(config|setting|constant)') { return "Configuration" }
    if ($message -match '(devops|script|tool|automation)') { return "DevOps Tools" }
    if ($message -match '(performance|optimize|memory|cpu)') { return "Performance" }
    if ($message -match '(error|exception|handle|recover)') { return "Error Handling" }
    if ($message -match '(ui|interface|display)') { return "User Interface" }
    
    # Breaking change indicators
    if ($message -match '!:' -or $message -match 'breaking') { return "BREAKING CHANGES" }
    
    # Default category
    return "Other Changes"
}

# Function to clean commit message
function Clean-CommitMessage {
    param($message)
    
    # Remove conventional commit prefix
    $message = $message -replace '^(feat|fix|docs|style|refactor|perf|test|chore|build|ci)(\(.+\))?:\s*', ''
    
    # Remove merge commit messages
    if ($message -match '^Merge ') { return $null }
    
    # Remove revert commit messages (but keep the content)
    $message = $message -replace '^Revert\s*"([^"]+)".*', 'Reverted: $1'
    
    # Capitalize first letter
    if ($message.Length -gt 0) {
        $message = $message.Substring(0, 1).ToUpper() + $message.Substring(1)
    }
    
    return $message
}

# Check if we're in a git repository
if (-not (Test-GitRepository -path $ProjectRoot)) {
    Write-Host "❌ Not a git repository or git not available" -ForegroundColor Red
    Write-Host "This script requires git and must be run in a git repository" -ForegroundColor DarkYellow
    Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
    Read-Host
    return
}

Push-Location $ProjectRoot

Write-Host "Analyzing git history..." -ForegroundColor DarkGray

# Get tags
$tags = Get-GitTags
Write-Host ("Found {0} tags" -f $tags.Count) -ForegroundColor Gray

# Determine version range
$fromTag = $null
$toRef = "HEAD"

if ($tags.Count -eq 0) {
    Write-Host "No tags found, generating changelog from first commit" -ForegroundColor DarkYellow
    $fromTag = $null
} elseif ($tags.Count -eq 1) {
    Write-Host "One tag found, generating changelog since tag: $($tags[0])" -ForegroundColor DarkYellow
    $fromTag = $tags[0]
} else {
    Write-Host "Multiple tags found:" -ForegroundColor DarkCyan
    for ($i = 0; $i -lt [Math]::Min(10, $tags.Count); $i++) {
        Write-Host ("  {0}. {1}" -f ($i + 1), $tags[$i]) -ForegroundColor Gray
    }
    
    Write-Host "`nOptions:" -ForegroundColor DarkCyan
    Write-Host "  1. Latest release ($($tags[0])) to HEAD" -ForegroundColor Gray
    Write-Host "  2. Between two specific tags" -ForegroundColor Gray
    Write-Host "  3. All history (from beginning)" -ForegroundColor Gray
    Write-Host "  4. Custom range" -ForegroundColor Gray
    
    do {
        $choice = Read-Host "`nEnter choice (1-4)"
        $validChoice = $choice -in @('1', '2', '3', '4')
        if (-not $validChoice) {
            Write-Host "Invalid choice. Please enter 1, 2, 3, or 4." -ForegroundColor Red
        }
    } while (-not $validChoice)
    
    switch ($choice) {
        '1' { 
            $fromTag = $tags[0]
            Write-Host "Generating changelog from $fromTag to HEAD" -ForegroundColor Green
        }
        '2' {
            Write-Host "`nAvailable tags:" -ForegroundColor DarkCyan
            for ($i = 0; $i -lt $tags.Count; $i++) {
                Write-Host ("  {0}. {1}" -f ($i + 1), $tags[$i]) -ForegroundColor Gray
            }
            
            do {
                $fromChoice = Read-Host "Enter FROM tag number"
                $fromIndex = [int]$fromChoice - 1
                $validFrom = $fromIndex -ge 0 -and $fromIndex -lt $tags.Count
                if (-not $validFrom) {
                    Write-Host "Invalid tag number." -ForegroundColor Red
                }
            } while (-not $validFrom)
            
            do {
                $toChoice = Read-Host "Enter TO tag number (or press ENTER for HEAD)"
                if ($toChoice -eq "") {
                    $toRef = "HEAD"
                    $validTo = $true
                } else {
                    $toIndex = [int]$toChoice - 1
                    $validTo = $toIndex -ge 0 -and $toIndex -lt $tags.Count
                    if ($validTo) {
                        $toRef = $tags[$toIndex]
                    } else {
                        Write-Host "Invalid tag number." -ForegroundColor Red
                    }
                }
            } while (-not $validTo)
            
            $fromTag = $tags[$fromIndex]
            Write-Host "Generating changelog from $fromTag to $toRef" -ForegroundColor Green
        }
        '3' {
            $fromTag = $null
            Write-Host "Generating changelog from beginning to HEAD" -ForegroundColor Green
        }
        '4' {
            $fromTag = Read-Host "Enter FROM reference (tag, commit hash, etc.)"
            $toRef = Read-Host "Enter TO reference (or press ENTER for HEAD)"
            if ($toRef -eq "") { $toRef = "HEAD" }
            Write-Host "Generating changelog from $fromTag to $toRef" -ForegroundColor Green
        }
    }
}

# Build git log command
if ($fromTag) {
    $gitRange = "$fromTag..$toRef"
} else {
    $gitRange = $toRef
}

Write-Host "`nFetching commits..." -ForegroundColor DarkGray

# Get commits
try {
    $gitLogFormat = '--pretty=format:%H|%s|%an|%ad|%b'
    $commits = git log $gitRange $gitLogFormat --date=short --no-merges 2>$null
    
    if ($LASTEXITCODE -ne 0) {
        throw "Git log failed"
    }
} catch {
    Write-Host "❌ Failed to fetch git commits" -ForegroundColor Red
    Write-Host "Error: $_" -ForegroundColor Red
    Pop-Location
    Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
    Read-Host
    return
}

if (-not $commits) {
    Write-Host "❌ No commits found in range $gitRange" -ForegroundColor Red
    Pop-Location
    Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
    Read-Host
    return
}

Write-Host ("Processing {0} commits..." -f $commits.Count) -ForegroundColor DarkGray

# Parse commits
$commitData = @()
foreach ($commitLine in $commits) {
    if (-not $commitLine) { continue }
    
    $parts = $commitLine -split '\|', 5
    if ($parts.Count -lt 4) { continue }
    
    $hash = $parts[0]
    $subject = $parts[1]
    $author = $parts[2]
    $date = $parts[3]
    $body = if ($parts.Count -gt 4) { $parts[4] } else { "" }
    
    $cleanMessage = Clean-CommitMessage -message $subject
    if (-not $cleanMessage) { continue }  # Skip merge commits
    
    $category = Get-CommitCategory -message $subject
    
    $commitData += [PSCustomObject]@{
        Hash = $hash.Substring(0, 7)  # Short hash
        Subject = $subject
        CleanMessage = $cleanMessage
        Author = $author
        Date = $date
        Body = $body
        Category = $category
    }
}

Write-Host ("Categorized {0} commits" -f $commitData.Count) -ForegroundColor Gray

# Group commits by category
$groupedCommits = $commitData | Group-Object Category | Sort-Object @{
    Expression = {
        switch ($_.Name) {
            "BREAKING CHANGES" { 1 }
            "Features" { 2 }
            "Bug Fixes" { 3 }
            "Save System" { 4 }
            "Thread Safety" { 5 }
            "Performance" { 6 }
            "Mixer Features" { 7 }
            "Game Patches" { 8 }
            "Error Handling" { 9 }
            "Configuration" { 10 }
            "Debugging" { 11 }
            "DevOps Tools" { 12 }
            "Refactoring" { 13 }
            "Documentation" { 14 }
            "Tests" { 15 }
            "Build System" { 16 }
            "CI/CD" { 17 }
            "Style Changes" { 18 }
            "Maintenance" { 19 }
            "User Interface" { 20 }
            "Other Changes" { 99 }
            default { 50 }
        }
    }
}

# Generate changelog
$changelogContent = @()

# Header
$versionHeader = if ($fromTag -and $toRef -ne "HEAD") {
    "# Changelog: $fromTag → $toRef"
} elseif ($fromTag) {
    "# Changelog: $fromTag → Latest"
} else {
    "# Changelog: Complete History"
}

$changelogContent += $versionHeader
$changelogContent += ""
$changelogContent += "**Generated:** $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
$changelogContent += "**Commits:** $($commitData.Count)"
$changelogContent += "**Date Range:** $(($commitData | Sort-Object Date | Select-Object -First 1).Date) → $(($commitData | Sort-Object Date | Select-Object -Last 1).Date)"
$changelogContent += ""

# Table of Contents
if ($groupedCommits.Count -gt 1) {
    $changelogContent += "## Table of Contents"
    $changelogContent += ""
    foreach ($group in $groupedCommits) {
        $anchor = $group.Name.ToLower() -replace '\s+', '-' -replace '[^\w\-]', ''
        $changelogContent += "- [$($group.Name)](#$anchor) ($($group.Count) commits)"
    }
    $changelogContent += ""
    $changelogContent += "---"
    $changelogContent += ""
}

# Generate sections
foreach ($group in $groupedCommits) {
    $icon = switch ($group.Name) {
        "BREAKING CHANGES" { "🚨" }
        "Features" { "✨" }
        "Bug Fixes" { "🐛" }
        "Save System" { "💾" }
        "Thread Safety" { "🔒" }
        "Performance" { "⚡" }
        "Mixer Features" { "🎵" }
        "Game Patches" { "🔧" }
        "Error Handling" { "🛡️" }
        "Configuration" { "⚙️" }
        "Debugging" { "🔍" }
        "DevOps Tools" { "🔨" }
        "Refactoring" { "♻️" }
        "Documentation" { "📝" }
        "Tests" { "✅" }
        "Build System" { "📦" }
        "CI/CD" { "🚀" }
        "Style Changes" { "💄" }
        "Maintenance" { "🧹" }
        "User Interface" { "🎨" }
        default { "📌" }
    }
    
    $changelogContent += "## $icon $($group.Name)"
    $changelogContent += ""
    
    # Sort commits by date (newest first)
    $sortedCommits = $group.Group | Sort-Object Date -Descending
    
    foreach ($commit in $sortedCommits) {
        $changelogContent += "- **$($commit.CleanMessage)** ([``$($commit.Hash)``](../../commit/$($commit.Hash)))"
        
        # Add body if significant
        if ($commit.Body -and $commit.Body.Trim().Length -gt 20 -and $commit.Body -notmatch '^\s*$') {
            $bodyLines = $commit.Body -split "`n" | Where-Object { $_.Trim() } | Select-Object -First 3
            foreach ($line in $bodyLines) {
                $changelogContent += "  - $($line.Trim())"
            }
        }
        
        # Add metadata for important commits
        if ($group.Name -in @("BREAKING CHANGES", "Features", "Bug Fixes")) {
            $changelogContent += "  - *Author: $($commit.Author), Date: $($commit.Date)*"
        }
        
        $changelogContent += ""
    }
}

# Statistics section
$changelogContent += "---"
$changelogContent += ""
$changelogContent += "## 📊 Statistics"
$changelogContent += ""
$changelogContent += "| Category | Commits | Percentage |"
$changelogContent += "|----------|---------|------------|"

foreach ($group in $groupedCommits) {
    $percentage = [Math]::Round(($group.Count / $commitData.Count) * 100, 1)
    $changelogContent += "| $($group.Name) | $($group.Count) | $percentage% |"
}

$changelogContent += ""
$changelogContent += "### Top Contributors"
$changelogContent += ""

$authorStats = $commitData | Group-Object Author | Sort-Object Count -Descending | Select-Object -First 10
foreach ($author in $authorStats) {
    $percentage = [Math]::Round(($author.Count / $commitData.Count) * 100, 1)
    $changelogContent += "- **$($author.Name)**: $($author.Count) commits ($percentage%)"
}

$changelogContent += ""
$changelogContent += "### Commit Activity"
$changelogContent += ""

$dateStats = $commitData | Group-Object Date | Sort-Object Name -Descending | Select-Object -First 5
$changelogContent += "Most active days:"
foreach ($dateStat in $dateStats) {
    $changelogContent += "- **$($dateStat.Name)**: $($dateStat.Count) commits"
}

# Footer
$changelogContent += ""
$changelogContent += "---"
$changelogContent += ""
$changelogContent += "*This changelog was automatically generated by Generate-ChangeLog.ps1*"

# Save changelog
$outputFile = Join-Path $ProjectRoot "CHANGELOG.md"
$changelogContent | Out-File -FilePath $outputFile -Encoding UTF8

Pop-Location

Write-Host "`n=== Changelog Generation Complete ===" -ForegroundColor DarkCyan
Write-Host ("✅ Generated changelog: {0}" -f $outputFile) -ForegroundColor Green
Write-Host ("📊 Processed {0} commits in {1} categories" -f $commitData.Count, $groupedCommits.Count) -ForegroundColor Gray

# Summary by category
Write-Host "`n📋 Commit Summary:" -ForegroundColor DarkCyan
foreach ($group in $groupedCommits | Sort-Object Count -Descending) {
    $color = switch ($group.Name) {
        "BREAKING CHANGES" { "Red" }
        "Features" { "Green" }
        "Bug Fixes" { "DarkYellow" }
        default { "Gray" }
    }
    Write-Host ("  {0,-20}: {1,3} commits" -f $group.Name, $group.Count) -ForegroundColor $color
}

Write-Host "`n💡 Next Steps:" -ForegroundColor DarkCyan
Write-Host "  📝 Review the generated changelog for accuracy" -ForegroundColor Gray
Write-Host "  ✏️  Edit manually if needed for clarity" -ForegroundColor Gray
Write-Host "  📋 Copy sections to release notes" -ForegroundColor Gray
Write-Host "  🏷️  Tag the release when ready" -ForegroundColor Gray

Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
Read-Host