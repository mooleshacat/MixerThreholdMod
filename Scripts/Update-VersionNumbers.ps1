# MixerThreholdMod DevOps Tool: Version Numbers Synchronizer
# Synchronizes version numbers across all project files
# Updates Constants, AssemblyInfo, manifests, and documentation
# Excludes: ForCopilot, ForConstants, Scripts, and Legacy directories

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
if ((Split-Path $ScriptDir -Leaf) -ieq "Scripts") {
    $ProjectRoot = Split-Path $ScriptDir -Parent
} else {
    $ProjectRoot = $ScriptDir
}

Write-Host "Updating version numbers in: $ProjectRoot" -ForegroundColor DarkCyan

# Function to validate semantic version
function Test-SemanticVersion {
    param($version)
    
    if (-not $version) { return $false }
    
    # Semantic version pattern: MAJOR.MINOR.PATCH[-PRERELEASE][+BUILD]
    $pattern = '^(\d+)\.(\d+)\.(\d+)(?:-([0-9A-Za-z\-\.]+))?(?:\+([0-9A-Za-z\-\.]+))?$'
    
    return $version -match $pattern
}

# Function to parse semantic version
function Get-SemanticVersionParts {
    param($version)
    
    $pattern = '^(\d+)\.(\d+)\.(\d+)(?:-([0-9A-Za-z\-\.]+))?(?:\+([0-9A-Za-z\-\.]+))?$'
    
    if ($version -match $pattern) {
        return [PSCustomObject]@{
            Major = [int]$matches[1]
            Minor = [int]$matches[2]
            Patch = [int]$matches[3]
            PreRelease = $matches[4]
            Build = $matches[5]
            Full = $version
            Simple = "$($matches[1]).$($matches[2]).$($matches[3])"
        }
    }
    
    return $null
}

# Function to get current version from SystemConstants.cs
function Get-CurrentVersion {
    $constantsFile = Join-Path $ProjectRoot "Constants\SystemConstants.cs"
    
    if (-not (Test-Path $constantsFile)) {
        Write-Host "⚠️  SystemConstants.cs not found, trying fallback locations..." -ForegroundColor DarkYellow
        
        # Try alternative locations
        $alternatives = @(
            "Constants\Constants.cs",
            "Main.cs"
        )
        
        foreach ($alt in $alternatives) {
            $altPath = Join-Path $ProjectRoot $alt
            if (Test-Path $altPath) {
                $constantsFile = $altPath
                break
            }
        }
    }
    
    if (-not (Test-Path $constantsFile)) {
        return $null
    }
    
    try {
        $content = Get-Content -Path $constantsFile -Raw
        if ($content -match 'MOD_VERSION\s*=\s*"([^"]+)"') {
            return $matches[1]
        }
    }
    catch {
        Write-Host "❌ Error reading constants file: $_" -ForegroundColor Red
    }
    
    return $null
}

# Function to find version-containing files
function Find-VersionFiles {
    $versionFiles = @()
    
    # Find all potential files
    $files = Get-ChildItem -Path $ProjectRoot -Recurse -Include *.cs,*.json,*.xml,*.md,*.txt,*.info,*.config | Where-Object {
        $_.PSIsContainer -eq $false -and
        $_.FullName -notmatch "[\\/](ForCopilot)[\\/]" -and
        $_.FullName -notmatch "[\\/](Scripts)[\\/]" -and
        $_.FullName -notmatch "[\\/](Legacy)[\\/]" -and
        $_.FullName -notmatch "[\\/]\.git[\\/]"
    }
    
    foreach ($file in $files) {
        try {
            $content = Get-Content -Path $file.FullName -Raw -ErrorAction SilentlyContinue
            if (-not $content) { continue }
            
            $hasVersion = $false
            $versionPatterns = @()
            
            # Check for various version patterns
            if ($content -match 'MOD_VERSION\s*=\s*"([^"]+)"') {
                $hasVersion = $true
                $versionPatterns += "MOD_VERSION constant"
            }
            
            if ($content -match 'AssemblyVersion\s*\(\s*"([^"]+)"\s*\)') {
                $hasVersion = $true
                $versionPatterns += "AssemblyVersion attribute"
            }
            
            if ($content -match 'AssemblyFileVersion\s*\(\s*"([^"]+)"\s*\)') {
                $hasVersion = $true
                $versionPatterns += "AssemblyFileVersion attribute"
            }
            
            if ($content -match '"version"\s*:\s*"([^"]+)"') {
                $hasVersion = $true
                $versionPatterns += "JSON version field"
            }
            
            if ($content -match '<version>([^<]+)</version>') {
                $hasVersion = $true
                $versionPatterns += "XML version element"
            }
            
            if ($content -match 'Version:\s*([0-9]+\.[0-9]+\.[0-9]+)') {
                $hasVersion = $true
                $versionPatterns += "Documentation version"
            }
            
            # MelonLoader mod patterns
            if ($content -match 'MelonModInfo\s*\([^)]*"([^"]*)"[^)]*"([^"]*)"[^)]*"([^"]*)"') {
                $hasVersion = $true
                $versionPatterns += "MelonModInfo attribute"
            }
            
            if ($hasVersion) {
                $versionFiles += [PSCustomObject]@{
                    File = $file.FullName
                    RelativePath = $file.FullName.Replace($ProjectRoot, "").TrimStart('\', '/')
                    Name = $file.Name
                    Patterns = $versionPatterns
                    Content = $content
                }
            }
        }
        catch {
            # Skip files that can't be read
            continue
        }
    }
    
    return $versionFiles
}

# Function to update version in file
function Update-VersionInFile {
    param($filePath, $newVersion, $dryRun = $false)
    
    try {
        $content = Get-Content -Path $filePath -Raw
        $originalContent = $content
        $changesCount = 0
        
        # Update MOD_VERSION constant
        if ($content -match 'MOD_VERSION\s*=\s*"([^"]+)"') {
            $oldVersion = $matches[1]
            $content = $content -replace 'MOD_VERSION\s*=\s*"[^"]+"', "MOD_VERSION = `"$newVersion`""
            $changesCount++
            Write-Host ("    MOD_VERSION: {0} → {1}" -f $oldVersion, $newVersion) -ForegroundColor Green
        }
        
        # Update AssemblyVersion
        if ($content -match 'AssemblyVersion\s*\(\s*"([^"]+)"\s*\)') {
            $oldVersion = $matches[1]
            $content = $content -replace 'AssemblyVersion\s*\(\s*"[^"]+"\s*\)', "AssemblyVersion(`"$newVersion`")"
            $changesCount++
            Write-Host ("    AssemblyVersion: {0} → {1}" -f $oldVersion, $newVersion) -ForegroundColor Green
        }
        
        # Update AssemblyFileVersion
        if ($content -match 'AssemblyFileVersion\s*\(\s*"([^"]+)"\s*\)') {
            $oldVersion = $matches[1]
            $content = $content -replace 'AssemblyFileVersion\s*\(\s*"[^"]+"\s*\)', "AssemblyFileVersion(`"$newVersion`")"
            $changesCount++
            Write-Host ("    AssemblyFileVersion: {0} → {1}" -f $oldVersion, $newVersion) -ForegroundColor Green
        }
        
        # Update JSON version
        if ($content -match '"version"\s*:\s*"([^"]+)"') {
            $oldVersion = $matches[1]
            $content = $content -replace '"version"\s*:\s*"[^"]+"', "`"version`": `"$newVersion`""
            $changesCount++
            Write-Host ("    JSON version: {0} → {1}" -f $oldVersion, $newVersion) -ForegroundColor Green
        }
        
        # Update XML version
        if ($content -match '<version>([^<]+)</version>') {
            $oldVersion = $matches[1]
            $content = $content -replace '<version>[^<]+</version>', "<version>$newVersion</version>"
            $changesCount++
            Write-Host ("    XML version: {0} → {1}" -f $oldVersion, $newVersion) -ForegroundColor Green
        }
        
        # Update documentation version
        if ($content -match 'Version:\s*([0-9]+\.[0-9]+\.[0-9]+)') {
            $oldVersion = $matches[1]
            $versionParts = Get-SemanticVersionParts -version $newVersion
            if ($versionParts) {
                $content = $content -replace 'Version:\s*[0-9]+\.[0-9]+\.[0-9]+', "Version: $($versionParts.Simple)"
                $changesCount++
                Write-Host ("    Documentation version: {0} → {1}" -f $oldVersion, $versionParts.Simple) -ForegroundColor Green
            }
        }
        
        # Update MelonModInfo (more complex pattern)
        if ($content -match 'MelonModInfo\s*\(\s*typeof\([^)]+\)\s*,\s*"([^"]*)",\s*"([^"]*)",\s*"([^"]*)"') {
            $oldVersion = $matches[3]
            $modName = $matches[1]
            $modAuthor = $matches[2]
            $content = $content -replace 'MelonModInfo\s*\(\s*typeof\([^)]+\)\s*,\s*"([^"]*)",\s*"([^"]*)",\s*"([^"]*)"', "MelonModInfo(typeof(`$1), `"$modName`", `"$newVersion`", `"$modAuthor`")"
            $changesCount++
            Write-Host ("    MelonModInfo version: {0} → {1}" -f $oldVersion, $newVersion) -ForegroundColor Green
        }
        
        # Write changes if not dry run
        if ($changesCount -gt 0 -and -not $dryRun) {
            $content | Out-File -FilePath $filePath -Encoding UTF8 -NoNewline
        }
        
        return $changesCount
    }
    catch {
        Write-Host ("    ❌ Error updating file: {0}" -f $_.Message) -ForegroundColor Red
        return 0
    }
}

# Main script execution
Write-Host "`n=== Version Number Synchronization Tool ===" -ForegroundColor DarkCyan

# Get current version
$currentVersion = Get-CurrentVersion
if ($currentVersion) {
    Write-Host ("Current version detected: {0}" -f $currentVersion) -ForegroundColor Gray
    
    if (-not (Test-SemanticVersion -version $currentVersion)) {
        Write-Host "⚠️  Current version is not a valid semantic version" -ForegroundColor DarkYellow
    }
} else {
    Write-Host "⚠️  No current version detected" -ForegroundColor DarkYellow
    $currentVersion = "1.0.0"  # Default version
}

# Options
Write-Host "`nVersion Update Options:" -ForegroundColor DarkCyan
Write-Host "  1. Set specific version" -ForegroundColor Gray
Write-Host "  2. Increment patch (X.Y.Z+1)" -ForegroundColor Gray
Write-Host "  3. Increment minor (X.Y+1.0)" -ForegroundColor Gray
Write-Host "  4. Increment major (X+1.0.0)" -ForegroundColor Gray
Write-Host "  5. Add pre-release suffix" -ForegroundColor Gray
Write-Host "  6. Scan files only (no changes)" -ForegroundColor Gray

do {
    $choice = Read-Host "`nEnter choice (1-6)"
    $validChoice = $choice -in @('1', '2', '3', '4', '5', '6')
    if (-not $validChoice) {
        Write-Host "Invalid choice. Please enter 1-6." -ForegroundColor Red
    }
} while (-not $validChoice)

$newVersion = $currentVersion
$dryRun = $false

switch ($choice) {
    '1' {
        do {
            $inputVersion = Read-Host "Enter new version (e.g., 1.2.3, 2.0.0-beta, 1.0.0+build123)"
            $isValid = Test-SemanticVersion -version $inputVersion
            if (-not $isValid) {
                Write-Host "Invalid semantic version format. Use MAJOR.MINOR.PATCH[-PRERELEASE][+BUILD]" -ForegroundColor Red
            }
        } while (-not $isValid)
        $newVersion = $inputVersion
    }
    '2' {
        $versionParts = Get-SemanticVersionParts -version $currentVersion
        if ($versionParts) {
            $newVersion = "$($versionParts.Major).$($versionParts.Minor).$($versionParts.Patch + 1)"
        } else {
            Write-Host "❌ Cannot parse current version for increment" -ForegroundColor Red
            return
        }
    }
    '3' {
        $versionParts = Get-SemanticVersionParts -version $currentVersion
        if ($versionParts) {
            $newVersion = "$($versionParts.Major).$($versionParts.Minor + 1).0"
        } else {
            Write-Host "❌ Cannot parse current version for increment" -ForegroundColor Red
            return
        }
    }
    '4' {
        $versionParts = Get-SemanticVersionParts -version $currentVersion
        if ($versionParts) {
            $newVersion = "$($versionParts.Major + 1).0.0"
        } else {
            Write-Host "❌ Cannot parse current version for increment" -ForegroundColor Red
            return
        }
    }
    '5' {
        $versionParts = Get-SemanticVersionParts -version $currentVersion
        if ($versionParts) {
            $suffix = Read-Host "Enter pre-release suffix (e.g., alpha, beta, rc1)"
            $newVersion = "$($versionParts.Simple)-$suffix"
        } else {
            Write-Host "❌ Cannot parse current version for suffix" -ForegroundColor Red
            return
        }
    }
    '6' {
        $dryRun = $true
        Write-Host "📋 Dry run mode - scanning files only" -ForegroundColor DarkCyan
    }
}

if (-not $dryRun) {
    Write-Host ("New version will be: {0}" -f $newVersion) -ForegroundColor Green
    
    $confirm = Read-Host "`nProceed with version update? (y/N)"
    if ($confirm -ne 'y' -and $confirm -ne 'Y') {
        Write-Host "❌ Operation cancelled" -ForegroundColor Red
        return
    }
}

# Find all version-containing files
Write-Host "`nScanning for version-containing files..." -ForegroundColor DarkGray
$versionFiles = Find-VersionFiles

Write-Host ("Found {0} files containing version information:" -f $versionFiles.Count) -ForegroundColor Gray

# Display files and update versions
$totalChanges = 0
foreach ($versionFile in $versionFiles) {
    Write-Host ("`n📄 {0}" -f $versionFile.RelativePath) -ForegroundColor DarkCyan
    Write-Host ("   Patterns: {0}" -f ($versionFile.Patterns -join ", ")) -ForegroundColor DarkGray
    
    if (-not $dryRun) {
        $changes = Update-VersionInFile -filePath $versionFile.File -newVersion $newVersion -dryRun $dryRun
        $totalChanges += $changes
        
        if ($changes -eq 0) {
            Write-Host "   No changes made" -ForegroundColor DarkGray
        }
    } else {
        # In dry run, just show what would be updated
        $content = $versionFile.Content
        
        if ($content -match 'MOD_VERSION\s*=\s*"([^"]+)"') {
            Write-Host ("   Would update MOD_VERSION: {0} → {1}" -f $matches[1], $newVersion) -ForegroundColor DarkYellow
        }
        if ($content -match 'AssemblyVersion\s*\(\s*"([^"]+)"\s*\)') {
            Write-Host ("   Would update AssemblyVersion: {0} → {1}" -f $matches[1], $newVersion) -ForegroundColor DarkYellow
        }
        if ($content -match 'AssemblyFileVersion\s*\(\s*"([^"]+)"\s*\)') {
            Write-Host ("   Would update AssemblyFileVersion: {0} → {1}" -f $matches[1], $newVersion) -ForegroundColor DarkYellow
        }
        if ($content -match '"version"\s*:\s*"([^"]+)"') {
            Write-Host ("   Would update JSON version: {0} → {1}" -f $matches[1], $newVersion) -ForegroundColor DarkYellow
        }
    }
}

# Summary
Write-Host "`n=== Version Update Summary ===" -ForegroundColor DarkCyan

if ($dryRun) {
    Write-Host ("📋 Dry run completed") -ForegroundColor DarkCyan
    Write-Host ("Files scanned: {0}" -f $versionFiles.Count) -ForegroundColor Gray
    Write-Host ("Files with version info: {0}" -f $versionFiles.Count) -ForegroundColor Gray
} else {
    Write-Host ("✅ Version update completed") -ForegroundColor Green
    Write-Host ("Files processed: {0}" -f $versionFiles.Count) -ForegroundColor Gray
    Write-Host ("Total changes made: {0}" -f $totalChanges) -ForegroundColor Gray
    Write-Host ("New version: {0}" -f $newVersion) -ForegroundColor Green
}

# Recommendations
Write-Host "`n💡 Next Steps:" -ForegroundColor DarkCyan

if (-not $dryRun) {
    Write-Host "  🔍 Review changed files for accuracy" -ForegroundColor Gray
    Write-Host "  🧪 Test the build with new version" -ForegroundColor Gray
    Write-Host "  📝 Update CHANGELOG.md if needed" -ForegroundColor Gray
    Write-Host "  📋 Create release notes" -ForegroundColor Gray
    Write-Host "  🏷️  Tag the release: git tag v$newVersion" -ForegroundColor Gray
    Write-Host "  🚀 Build and publish release" -ForegroundColor Gray
    
    # Git integration
    if (Test-Path (Join-Path $ProjectRoot ".git")) {
        Write-Host "`n🔧 Git Commands:" -ForegroundColor DarkCyan
        Write-Host "  git add -A" -ForegroundColor Gray
        Write-Host "  git commit -m `"chore: bump version to $newVersion`"" -ForegroundColor Gray
        Write-Host "  git tag v$newVersion" -ForegroundColor Gray
        Write-Host "  git push origin main --tags" -ForegroundColor Gray
    }
} else {
    Write-Host "  ✏️  Run again without dry run to apply changes" -ForegroundColor Gray
    Write-Host "  📋 Review the file list above" -ForegroundColor Gray
}

# Version validation
if (-not $dryRun) {
    Write-Host "`n🔍 Verifying version consistency..." -ForegroundColor DarkGray
    Start-Sleep -Seconds 1
    
    $verificationVersion = Get-CurrentVersion
    if ($verificationVersion -eq $newVersion) {
        Write-Host "✅ Version verification successful" -ForegroundColor Green
    } else {
        Write-Host ("⚠️  Version verification warning: Expected {0}, found {1}" -f $newVersion, $verificationVersion) -ForegroundColor DarkYellow
    }
}

Write-Host "`nPress ENTER to continue..." -ForegroundColor Gray -NoNewline
Read-Host