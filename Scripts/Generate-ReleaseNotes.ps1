# MixerThreholdMod DevOps Tool: Release Notes Generator  
# Creates professional release notes from git commits and closed issues
# Integrates with changelog and generates formatted release documentation
# Excludes: ForCopilot, ForConstants, Scripts, and Legacy directories

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
if ((Split-Path $ScriptDir -Leaf) -ieq "Scripts") {
    $ProjectRoot = Split-Path $ScriptDir -Parent
} else {
    $ProjectRoot = $ScriptDir
}

Write-Host "Generating release notes in: $ProjectRoot" -ForegroundColor DarkCyan

# Function to get current version
function Get-CurrentVersion {
    $constantsFile = Join-Path $ProjectRoot "Constants\SystemConstants.cs"
    
    if (Test-Path $constantsFile) {
        try {
            $content = Get-Content -Path $constantsFile -Raw
            if ($content -match 'MOD_VERSION\s*=\s*"([^"]+)"') {
                return $matches[1]
            }
        }
        catch {
            Write-Host "⚠️  Error reading version from SystemConstants.cs" -ForegroundColor DarkYellow
        }
    }
    
    return "1.0.0"  # Default version
}

# Function to check git repository
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

# Function to get git tags
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

# Function to get commits for release
function Get-ReleaseCommits {
    param($fromTag, $toRef)
    
    try {
        if ($fromTag) {
            $gitRange = "$fromTag..$toRef"
        } else {
            $gitRange = $toRef
        }
        
        $gitLogFormat = '--pretty=format:%H|%s|%an|%ad|%b'
        $commits = git log $gitRange $gitLogFormat --date=short --no-merges 2>$null
        
        if ($LASTEXITCODE -ne 0) {
            return @()
        }
        
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
            
            # Skip merge commits
            if ($subject -match '^Merge ') { continue }
            
            $commitData += [PSCustomObject]@{
                Hash = $hash.Substring(0, 7)
                Subject = $subject
                Author = $author
                Date = $date
                Body = $body
                Category = Get-CommitCategory -message $subject
            }
        }
        
        return $commitData
    }
    catch {
        return @()
    }
}

# Function to categorize commits
function Get-CommitCategory {
    param($message)
    
    $message = $message.ToLower()
    
    # High priority categories first
    if ($message -match '!:' -or $message -match 'breaking') { return "BREAKING" }
    if ($message -match '^feat(\(.+\))?:') { return "FEATURE" }
    if ($message -match '^fix(\(.+\))?:') { return "BUGFIX" }
    if ($message -match '(security|vulnerability|cve)') { return "SECURITY" }
    
    # Project-specific categories
    if ($message -match '(save|backup|crash|emergency)') { return "SAVE_SYSTEM" }
    if ($message -match '(thread|async|lock|concurrent)') { return "THREAD_SAFETY" }
    if ($message -match '(mixer|audio|volume|threshold)') { return "MIXER_FEATURES" }
    if ($message -match '(performance|optimize|memory|cpu)') { return "PERFORMANCE" }
    if ($message -match '(patch|harmony|hook)') { return "GAME_PATCHES" }
    
    # Standard categories
    if ($message -match '^docs(\(.+\))?:') { return "DOCUMENTATION" }
    if ($message -match '^perf(\(.+\))?:') { return "PERFORMANCE" }
    if ($message -match '^refactor(\(.+\))?:') { return "REFACTORING" }
    if ($message -match '^test(\(.+\))?:') { return "TESTING" }
    if ($message -match '^chore(\(.+\))?:') { return "MAINTENANCE" }
    if ($message -match '^build(\(.+\))?:') { return "BUILD" }
    if ($message -match '^ci(\(.+\))?:') { return "CI_CD" }
    
    return "OTHER"
}

# Function to format commit for release notes
function Format-CommitForRelease {
    param($commit)
    
    $message = $commit.Subject
    
    # Remove conventional commit prefixes
    $message = $message -replace '^(feat|fix|docs|style|refactor|perf|test|chore|build|ci)(\(.+\))?:\s*', ''
    
    # Capitalize first letter
    if ($message.Length -gt 0) {
        $message = $message.Substring(0, 1).ToUpper() + $message.Substring(1)
    }
    
    return $message
}

# Function to get release type based on commits
function Get-ReleaseType {
    param($commits)
    
    $hasBreaking = $commits | Where-Object { $_.Category -eq "BREAKING" }
    $hasFeatures = $commits | Where-Object { $_.Category -eq "FEATURE" }
    $hasBugfixes = $commits | Where-Object { $_.Category -eq "BUGFIX" }
    $hasSecurity = $commits | Where-Object { $_.Category -eq "SECURITY" }
    
    if ($hasBreaking) { return "Major Release" }
    if ($hasFeatures -or $hasSecurity) { return "Minor Release" }
    if ($hasBugfixes) { return "Patch Release" }
    return "Maintenance Release"
}

# Function to generate download links
function Get-DownloadSection {
    param($version)
    
    return @"
## 📥 Downloads

| Platform | Download | Checksum |
|----------|----------|----------|
| **Windows (x64)** | [MixerThreholdMod-$version-win-x64.dll](../../releases/download/v$version/MixerThreholdMod-$version-win-x64.dll) | [SHA256](../../releases/download/v$version/MixerThreholdMod-$version-win-x64.dll.sha256) |
| **Linux (x64)** | [MixerThreholdMod-$version-linux-x64.dll](../../releases/download/v$version/MixerThreholdMod-$version-linux-x64.dll) | [SHA256](../../releases/download/v$version/MixerThreholdMod-$version-linux-x64.dll.sha256) |
| **Source Code** | [Source (zip)](../../archive/v$version.zip) | [Source (tar.gz)](../../archive/v$version.tar.gz) |

### Installation
1. Install [MelonLoader](https://melonwiki.xyz/#/) for Schedule 1
2. Download the appropriate DLL for your platform
3. Place the DLL in your `Mods` folder
4. Launch Schedule 1

"@
}

# Function to generate compatibility matrix
function Get-CompatibilitySection {
    return @"
## ⚙️ Compatibility

| Component | Version | Status |
|-----------|---------|--------|
| **Schedule 1** | Latest | ✅ Supported |
| **MelonLoader** | 0.5.7+ | ✅ Required |
| **.NET Framework** | 4.8.1 | ✅ Compatible |
| **Unity Runtime** | MONO/IL2CPP | ✅ Both Supported |
| **Platform** | Windows/Linux | ✅ Cross-platform |

"@
}

# Main script execution
if (-not (Test-GitRepository -path $ProjectRoot)) {
    Write-Host "❌ Not a git repository or git not available" -ForegroundColor Red
    Write-Host "This script requires git and must be run in a git repository" -ForegroundColor DarkYellow
    Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
    Read-Host
    return
}

Push-Location $ProjectRoot

Write-Host "=== Release Notes Generator ===" -ForegroundColor DarkCyan

# Get current version and tags
$currentVersion = Get-CurrentVersion
$tags = Get-GitTags

Write-Host ("Current version: {0}" -f $currentVersion) -ForegroundColor Gray
Write-Host ("Available tags: {0}" -f $tags.Count) -ForegroundColor Gray

# Determine release range
$releaseVersion = $currentVersion
$fromTag = $null
$toRef = "HEAD"

Write-Host "`nRelease Notes Options:" -ForegroundColor DarkCyan
Write-Host "  1. Generate for current version ($currentVersion)" -ForegroundColor Gray
Write-Host "  2. Generate for custom version" -ForegroundColor Gray
Write-Host "  3. Generate from specific tag range" -ForegroundColor Gray
Write-Host "  4. Generate from latest tag to HEAD" -ForegroundColor Gray

do {
    $choice = Read-Host "`nEnter choice (1-4)"
    $validChoice = $choice -in @('1', '2', '3', '4')
    if (-not $validChoice) {
        Write-Host "Invalid choice. Please enter 1-4." -ForegroundColor Red
    }
} while (-not $validChoice)

switch ($choice) {
    '1' {
        # Use current version
        if ($tags.Count -gt 0) {
            $fromTag = $tags[0]  # Latest tag
        }
        Write-Host ("Generating release notes for version {0}" -f $releaseVersion) -ForegroundColor Green
    }
    '2' {
        $releaseVersion = Read-Host "Enter release version (e.g., 1.2.0)"
        if ($tags.Count -gt 0) {
            $fromTag = $tags[0]  # Latest tag
        }
        Write-Host ("Generating release notes for version {0}" -f $releaseVersion) -ForegroundColor Green
    }
    '3' {
        if ($tags.Count -eq 0) {
            Write-Host "❌ No tags available for range selection" -ForegroundColor Red
            Pop-Location
            return
        }
        
        Write-Host "`nAvailable tags:" -ForegroundColor DarkCyan
        for ($i = 0; $i -lt [Math]::Min(10, $tags.Count); $i++) {
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
        
        $fromTag = $tags[$fromIndex]
        $releaseVersion = Read-Host "Enter release version for these changes"
        Write-Host ("Generating release notes from {0} to HEAD for version {1}" -f $fromTag, $releaseVersion) -ForegroundColor Green
    }
    '4' {
        if ($tags.Count -eq 0) {
            Write-Host "❌ No tags available" -ForegroundColor Red
            Pop-Location
            return
        }
        
        $fromTag = $tags[0]
        $releaseVersion = Read-Host "Enter release version"
        Write-Host ("Generating release notes from {0} to HEAD for version {1}" -f $fromTag, $releaseVersion) -ForegroundColor Green
    }
}

# Get commits for this release
Write-Host "`nFetching commits..." -ForegroundColor DarkGray
$commits = Get-ReleaseCommits -fromTag $fromTag -toRef $toRef

if ($commits.Count -eq 0) {
    Write-Host "❌ No commits found for release range" -ForegroundColor Red
    Pop-Location
    Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
    Read-Host
    return
}

Write-Host ("Found {0} commits for release" -f $commits.Count) -ForegroundColor Gray

# Categorize commits
$categorizedCommits = $commits | Group-Object Category | Sort-Object @{
    Expression = {
        switch ($_.Name) {
            "BREAKING" { 1 }
            "SECURITY" { 2 }
            "FEATURE" { 3 }
            "BUGFIX" { 4 }
            "SAVE_SYSTEM" { 5 }
            "THREAD_SAFETY" { 6 }
            "MIXER_FEATURES" { 7 }
            "PERFORMANCE" { 8 }
            "GAME_PATCHES" { 9 }
            "DOCUMENTATION" { 10 }
            "REFACTORING" { 11 }
            "TESTING" { 12 }
            "BUILD" { 13 }
            "CI_CD" { 14 }
            "MAINTENANCE" { 15 }
            "OTHER" { 99 }
            default { 50 }
        }
    }
}

# Determine release type
$releaseType = Get-ReleaseType -commits $commits

# Generate release notes content
$releaseNotes = @()

# Header
$releaseNotes += "# MixerThreholdMod v$releaseVersion"
$releaseNotes += ""
$releaseNotes += "**Release Type**: $releaseType"
$releaseNotes += "**Release Date**: $(Get-Date -Format 'yyyy-MM-dd')"
$releaseNotes += "**Commits**: $($commits.Count)"

# Add author info if available
$topAuthor = $commits | Group-Object Author | Sort-Object Count -Descending | Select-Object -First 1
if ($topAuthor) {
    $releaseNotes += "**Primary Contributor**: $($topAuthor.Name)"
}

$releaseNotes += ""

# Release highlights
$releaseNotes += "## 🎯 Release Highlights"
$releaseNotes += ""

$highlights = @()
$featureCount = ($categorizedCommits | Where-Object { $_.Name -eq "FEATURE" }).Count
$bugfixCount = ($categorizedCommits | Where-Object { $_.Name -eq "BUGFIX" }).Count
$performanceCount = ($categorizedCommits | Where-Object { $_.Name -eq "PERFORMANCE" }).Count

if ($featureCount -gt 0) { $highlights += "✨ **$featureCount new features** added" }
if ($bugfixCount -gt 0) { $highlights += "🐛 **$bugfixCount bug fixes** resolved" }
if ($performanceCount -gt 0) { $highlights += "⚡ **$performanceCount performance improvements**" }

$securityCommits = $categorizedCommits | Where-Object { $_.Name -eq "SECURITY" }
if ($securityCommits) { $highlights += "🔒 **Security updates** included" }

$breakingCommits = $categorizedCommits | Where-Object { $_.Name -eq "BREAKING" }
if ($breakingCommits) { $highlights += "🚨 **Breaking changes** - see migration guide below" }

foreach ($highlight in $highlights) {
    $releaseNotes += "- $highlight"
}

if ($highlights.Count -eq 0) {
    $releaseNotes += "- 🔧 Maintenance release with code quality improvements"
}

$releaseNotes += ""

# Downloads section
$releaseNotes += Get-DownloadSection -version $releaseVersion
$releaseNotes += ""

# What's Changed section
$releaseNotes += "## 📋 What's Changed"
$releaseNotes += ""

foreach ($categoryGroup in $categorizedCommits) {
    $categoryName = $categoryGroup.Name
    $categoryCommits = $categoryGroup.Group
    
    # Map category to display info
    $categoryInfo = switch ($categoryName) {
        "BREAKING" { @{ Icon = "🚨"; Title = "Breaking Changes"; Color = "Red" } }
        "SECURITY" { @{ Icon = "🔒"; Title = "Security Updates"; Color = "Red" } }
        "FEATURE" { @{ Icon = "✨"; Title = "New Features"; Color = "Green" } }
        "BUGFIX" { @{ Icon = "🐛"; Title = "Bug Fixes"; Color = "DarkYellow" } }
        "SAVE_SYSTEM" { @{ Icon = "💾"; Title = "Save System"; Color = "Green" } }
        "THREAD_SAFETY" { @{ Icon = "🔒"; Title = "Thread Safety"; Color = "Green" } }
        "MIXER_FEATURES" { @{ Icon = "🎵"; Title = "Mixer Features"; Color = "Green" } }
        "PERFORMANCE" { @{ Icon = "⚡"; Title = "Performance"; Color = "Green" } }
        "GAME_PATCHES" { @{ Icon = "🔧"; Title = "Game Patches"; Color = "DarkYellow" } }
        "DOCUMENTATION" { @{ Icon = "📝"; Title = "Documentation"; Color = "Gray" } }
        "REFACTORING" { @{ Icon = "♻️"; Title = "Code Refactoring"; Color = "Gray" } }
        "TESTING" { @{ Icon = "✅"; Title = "Testing"; Color = "Gray" } }
        "BUILD" { @{ Icon = "📦"; Title = "Build System"; Color = "Gray" } }
        "CI_CD" { @{ Icon = "🚀"; Title = "CI/CD"; Color = "Gray" } }
        "MAINTENANCE" { @{ Icon = "🧹"; Title = "Maintenance"; Color = "Gray" } }
        default { @{ Icon = "📌"; Title = "Other Changes"; Color = "Gray" } }
    }
    
    $releaseNotes += "### $($categoryInfo.Icon) $($categoryInfo.Title)"
    $releaseNotes += ""
    
    foreach ($commit in $categoryCommits | Sort-Object Date -Descending) {
        $formattedMessage = Format-CommitForRelease -commit $commit
        $releaseNotes += "- $formattedMessage ([``$($commit.Hash)``](../../commit/$($commit.Hash)))"
        
        # Add body for important categories
        if ($categoryName -in @("BREAKING", "SECURITY", "FEATURE")) {
            if ($commit.Body -and $commit.Body.Trim().Length -gt 20) {
                $bodyLines = $commit.Body -split "`n" | Where-Object { $_.Trim() } | Select-Object -First 2
                foreach ($line in $bodyLines) {
                    $releaseNotes += "  - $($line.Trim())"
                }
            }
        }
    }
    $releaseNotes += ""
}

# Breaking changes migration guide
$breakingCommits = $categorizedCommits | Where-Object { $_.Name -eq "BREAKING" }
if ($breakingCommits) {
    $releaseNotes += "## 🚨 Breaking Changes & Migration Guide"
    $releaseNotes += ""
    $releaseNotes += "This release contains breaking changes. Please review the following:"
    $releaseNotes += ""
    
    foreach ($commit in $breakingCommits.Group) {
        $releaseNotes += "### $($commit.Subject -replace '^[^:]*:\s*', '')"
        $releaseNotes += ""
        if ($commit.Body) {
            $bodyLines = $commit.Body -split "`n" | Where-Object { $_.Trim() }
            foreach ($line in $bodyLines) {
                $releaseNotes += "- $($line.Trim())"
            }
        } else {
            $releaseNotes += "- Review your integration with this change"
            $releaseNotes += "- Test thoroughly before deploying"
        }
        $releaseNotes += ""
    }
}

# Compatibility section
$releaseNotes += Get-CompatibilitySection

# Installation/Upgrade instructions
$releaseNotes += "## 🔄 Upgrade Instructions"
$releaseNotes += ""
if ($breakingCommits) {
    $releaseNotes += "⚠️ **Important**: This release contains breaking changes. Please:"
    $releaseNotes += ""
    $releaseNotes += "1. **Backup your saves** before upgrading"
    $releaseNotes += "2. **Read the migration guide** above"
    $releaseNotes += "3. **Test in a non-production environment** first"
    $releaseNotes += "4. Replace the old DLL with the new version"
    $releaseNotes += "5. **Verify functionality** after upgrade"
} else {
    $releaseNotes += "1. Download the new version from the links above"
    $releaseNotes += "2. Replace the old DLL in your `Mods` folder"
    $releaseNotes += "3. Restart Schedule 1"
    $releaseNotes += "4. Verify the mod loads correctly in the console"
}
$releaseNotes += ""

# Technical details
$releaseNotes += "## 🔧 Technical Details"
$releaseNotes += ""
$releaseNotes += "- **Build Target**: .NET Framework 4.8.1"
$releaseNotes += "- **Unity Compatibility**: MONO and IL2CPP"
$releaseNotes += "- **Thread Safety**: All operations thread-safe"
$releaseNotes += "- **Save System**: Atomic operations with backup recovery"
$releaseNotes += "- **Performance**: Optimized for extended gameplay sessions"
$releaseNotes += ""

# Footer
$releaseNotes += "---"
$releaseNotes += ""
$releaseNotes += "**Full Changelog**: https://github.com/YourRepo/MixerThreholdMod/compare/$fromTag...v$releaseVersion"
if ($fromTag) {
    $releaseNotes += "**Previous Release**: [$fromTag](../../releases/tag/$fromTag)"
}
$releaseNotes += ""
$releaseNotes += "*Generated automatically by Generate-ReleaseNotes.ps1*"

# Save release notes
$outputPath = Join-Path $ProjectRoot "RELEASE-NOTES-v$releaseVersion.md"
$releaseNotes | Out-File -FilePath $outputPath -Encoding UTF8

Pop-Location

# Summary
Write-Host "`n=== Release Notes Generation Complete ===" -ForegroundColor DarkCyan
Write-Host ("✅ Generated release notes: {0}" -f $outputPath) -ForegroundColor Green
Write-Host ("📋 Release type: {0}" -f $releaseType) -ForegroundColor Gray
Write-Host ("📊 Total commits: {0}" -f $commits.Count) -ForegroundColor Gray
Write-Host ("🏷️  Version: {0}" -f $releaseVersion) -ForegroundColor Gray

# Category breakdown
Write-Host "`n📋 Changes by Category:" -ForegroundColor DarkCyan
foreach ($category in $categorizedCommits | Sort-Object Count -Descending) {
    $color = switch ($category.Name) {
        "BREAKING" { "Red" }
        "SECURITY" { "Red" }
        "FEATURE" { "Green" }
        "BUGFIX" { "DarkYellow" }
        default { "Gray" }
    }
    $displayName = switch ($category.Name) {
        "BREAKING" { "Breaking Changes" }
        "SECURITY" { "Security Updates" }
        "FEATURE" { "New Features" }
        "BUGFIX" { "Bug Fixes" }
        "SAVE_SYSTEM" { "Save System" }
        "THREAD_SAFETY" { "Thread Safety" }
        "MIXER_FEATURES" { "Mixer Features" }
        "PERFORMANCE" { "Performance" }
        "GAME_PATCHES" { "Game Patches" }
        "DOCUMENTATION" { "Documentation" }
        "REFACTORING" { "Refactoring" }
        "TESTING" { "Testing" }
        "BUILD" { "Build System" }
        "CI_CD" { "CI/CD" }
        "MAINTENANCE" { "Maintenance" }
        default { "Other Changes" }
    }
    Write-Host ("  {0,-20}: {1,3} changes" -f $displayName, $category.Count) -ForegroundColor $color
}

Write-Host "`n💡 Next Steps:" -ForegroundColor DarkCyan
Write-Host "  📝 Review the generated release notes" -ForegroundColor Gray
Write-Host "  ✏️  Edit manually for clarity if needed" -ForegroundColor Gray
Write-Host "  📋 Copy content for GitHub release" -ForegroundColor Gray
Write-Host "  🏷️  Create git tag: git tag v$releaseVersion" -ForegroundColor Gray
Write-Host "  🚀 Publish the release" -ForegroundColor Gray

if ($breakingCommits) {
    Write-Host "`n⚠️  Important Reminders:" -ForegroundColor DarkYellow
    Write-Host "  🚨 This release has breaking changes" -ForegroundColor Red
    Write-Host "  📖 Ensure migration guide is complete" -ForegroundColor Red
    Write-Host "  🧪 Recommend thorough testing to users" -ForegroundColor Red
}

Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
Read-Host