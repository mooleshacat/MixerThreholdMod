<#
.SYNOPSIS
    Enhanced Git Commit Signing Tool with Individual/Bulk Options and Auto-Push
.DESCRIPTION
    Offers choice between signing all commits at once or individual commit confirmation
    Fixes issues with unsigned commits being missed and handles push automatically
#>

param(
    [Parameter(Mandatory=$false)]
    [string]$RepositoryPath = "C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0",
    
    [Parameter(Mandatory=$false)]
    [string]$Branch = "",
    
    [Parameter(Mandatory=$false)]
    [string]$SigningKey = "",
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipPause = $false,
    
    [Parameter(Mandatory=$false)]
    [ValidateSet("Individual", "Bulk", "Ask")]
    [string]$SigningMode = "Ask"
)

$ErrorActionPreference = "Continue"

function Write-ColorText {
    param([string]$Text, [string]$Color = "White")
    try {
        Write-Host $Text -ForegroundColor $Color
    }
    catch {
        Write-Host $Text
    }
}

function Write-Header {
    param([string]$Text)
    Write-Host "`n" -NoNewline
    Write-ColorText ("=" * 80) -Color "Cyan"
    Write-ColorText " $Text" -Color "Yellow"
    Write-ColorText ("=" * 80) -Color "Cyan"
}

function Test-GPGSetup {
    Write-ColorText "Testing GPG setup..." -Color "Blue"
    
    try {
        $gpgVersion = gpg --version 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-ColorText "SUCCESS: GPG found" -Color "Green"
        } else {
            Write-ColorText "ERROR: GPG not found in PATH" -Color "Red"
            return $false
        }
    }
    catch {
        Write-ColorText "ERROR: GPG not available: $($_.Exception.Message)" -Color "Red"
        return $false
    }
    
    $signingKey = git config --get user.signingkey 2>$null
    if (-not $signingKey) {
        Write-ColorText "ERROR: No signing key configured" -Color "Red"
        Write-ColorText "Run: git config --global user.signingkey YOUR_KEY_ID" -Color "Cyan"
        return $false
    }
    
    Write-ColorText "Signing key: $signingKey" -Color "Green"
    
    try {
        $testMessage = "test signing"
        $testSig = echo $testMessage | gpg --sign --armor --local-user $signingKey 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-ColorText "SUCCESS: GPG signing test passed" -Color "Green"
            return $true
        } else {
            Write-ColorText "ERROR: GPG signing test failed: $testSig" -Color "Red"
            return $false
        }
    }
    catch {
        Write-ColorText "ERROR: GPG signing test exception: $($_.Exception.Message)" -Color "Red"
        return $false
    }
}

function Get-AllCommits {
    param([string]$Branch)
    
    Write-ColorText "Getting complete commit list..." -Color "Blue"
    
    # Get ALL commits in chronological order (oldest first)
    $commits = git rev-list --reverse $Branch 2>$null
    if ($LASTEXITCODE -ne 0) {
        Write-ColorText "ERROR: Could not get commit list" -Color "Red"
        return $null
    }
    
    $commitHashes = $commits -split "`n" | Where-Object { $_ -ne "" -and $_.Trim() -ne "" }
    Write-ColorText "Found $($commitHashes.Count) total commits" -Color "Green"
    
    return $commitHashes
}

function Get-CommitDetails {
    param([string]$CommitHash)
    
    $commitInfo = git show --format="format:%H%n%an%n%ae%n%ad%n%s" -s $CommitHash 2>$null
    if ($LASTEXITCODE -ne 0) {
        return $null
    }
    
    $lines = $commitInfo -split "`n"
    return @{
        Hash = $lines[0]
        ShortHash = $lines[0].Substring(0, 8)
        Author = $lines[1]
        Email = $lines[2]
        Date = $lines[3]
        Subject = $lines[4]
    }
}

function Test-CommitSigned {
    param([string]$CommitHash)
    
    $signature = git show --format="%GS" -s $CommitHash 2>$null
    return ($LASTEXITCODE -eq 0 -and $signature.Trim() -ne "")
}

function Get-UnsignedCommits {
    param([string[]]$AllCommits)
    
    Write-ColorText "Checking signature status of all commits..." -Color "Blue"
    
    $unsigned = @()
    $signed = @()
    
    foreach ($commit in $AllCommits) {
        if (Test-CommitSigned $commit) {
            $signed += $commit
        } else {
            $unsigned += $commit
        }
    }
    
    Write-ColorText "Analysis: $($signed.Count) signed, $($unsigned.Count) unsigned" -Color "Cyan"
    return $unsigned
}

function Sign-SingleCommit {
    param([string]$CommitHash)
    
    Write-ColorText "Signing commit $($CommitHash.Substring(0,8))..." -Color "Blue"
    
    # Checkout the specific commit
    git checkout $CommitHash 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-ColorText "ERROR: Could not checkout commit $CommitHash" -Color "Red"
        return $false
    }
    
    # Amend with signature
    git commit --amend --no-edit --gpg-sign 2>&1 | Out-Null
    if ($LASTEXITCODE -eq 0) {
        Write-ColorText "SUCCESS: Signed commit $($CommitHash.Substring(0,8))" -Color "Green"
        return $true
    } else {
        Write-ColorText "ERROR: Failed to sign commit $($CommitHash.Substring(0,8))" -Color "Red"
        return $false
    }
}

function Sign-AllCommitsBulk {
    param([string[]]$UnsignedCommits, [string]$Branch)
    
    if ($UnsignedCommits.Count -eq 0) {
        Write-ColorText "No unsigned commits to process!" -Color "Green"
        return $true
    }
    
    Write-ColorText "Bulk signing $($UnsignedCommits.Count) unsigned commits..." -Color "Blue"
    Write-ColorText "Using simplified rebase approach..." -Color "Blue"
    
    try {
        # Clean up any existing rebase state
        git rebase --abort 2>&1 | Out-Null
        
        # Method 1: Simple rebase from root (most reliable)
        Write-ColorText "Executing: git rebase --root --exec 'git commit --amend --no-edit --gpg-sign'" -Color "Gray"
        
        $result = git rebase --root --exec "git commit --amend --no-edit --gpg-sign" 2>&1
        $exitCode = $LASTEXITCODE
        
        Write-ColorText "Rebase output:" -Color "Gray"
        Write-ColorText $result -Color "Gray"
        Write-ColorText "Rebase exit code: $exitCode" -Color "Gray"
        
        if ($exitCode -eq 0) {
            Write-ColorText "SUCCESS: Bulk signing completed!" -Color "Green"
            return $true
        } 
        else {
            Write-ColorText "Method 1 failed, trying method 2..." -Color "Yellow"
            
            # Clean up failed rebase
            git rebase --abort 2>&1 | Out-Null
            
            # Method 2: Use git filter-repo if available, otherwise manual approach
            Write-ColorText "Trying git filter-repo approach..." -Color "Blue"
            
            # Check if git filter-repo is available
            $filterRepoCheck = git filter-repo --version 2>&1
            if ($LASTEXITCODE -eq 0) {
                Write-ColorText "Using git filter-repo for signing..." -Color "Blue"
                $filterResult = git filter-repo --commit-callback 'commit.committer_date = commit.author_date' --sign-commits 2>&1
                
                if ($LASTEXITCODE -eq 0) {
                    Write-ColorText "SUCCESS: Filter-repo signing completed!" -Color "Green"
                    return $true
                } else {
                    Write-ColorText "Filter-repo failed, falling back to manual method..." -Color "Yellow"
                }
            }
            
            # Method 3: Last resort - force manual signing
            Write-ColorText "Using last resort method..." -Color "Yellow"
            return Sign-AllCommitsManually $UnsignedCommits $Branch
        }
    }
    catch {
        Write-ColorText "ERROR: Exception during bulk signing: $($_.Exception.Message)" -Color "Red"
        
        # Clean up any rebase in progress
        git rebase --abort 2>&1 | Out-Null
        
        return $false
    }
}

function Sign-AllCommitsManually {
    param([string[]]$UnsignedCommits, [string]$Branch)
    
    Write-ColorText "Manual signing method - creating new signed history..." -Color "Blue"
    
    try {
        # Create backup
        $backupBranch = "$Branch-backup-$(Get-Date -Format 'yyyyMMddHHmmss')"
        git branch $backupBranch 2>&1 | Out-Null
        Write-ColorText "Created backup: $backupBranch" -Color "Cyan"
        
        # Get all commits in order (oldest first)
        $allCommits = git rev-list --reverse $Branch 2>$null
        $commitList = $allCommits -split "`n" | Where-Object { $_ -ne "" -and $_.Trim() -ne "" }
        
        Write-ColorText "Processing $($commitList.Count) commits sequentially..." -Color "Blue"
        
        # Start with empty repository state
        $tempBranch = "temp-manual-$(Get-Date -Format 'yyyyMMddHHmmss')"
        
        # Get the first commit and start from there
        $firstCommit = $commitList[0]
        git checkout $firstCommit 2>&1 | Out-Null
        git checkout -b $tempBranch 2>&1 | Out-Null
        
        # Sign the first commit
        git commit --amend --no-edit --gpg-sign 2>&1 | Out-Null
        Write-ColorText "Signed root commit" -Color "Green"
        
        $successCount = 1
        $errorCount = 0
        
        # Process remaining commits one by one
        for ($i = 1; $i -lt $commitList.Count; $i++) {
            $commit = $commitList[$i]
            $commitShort = $commit.Substring(0,8)
            
            Write-ColorText "Processing commit $($i+1)/$($commitList.Count): $commitShort" -Color "Blue"
            
            # Get the commit's changes
            $commitMessage = git show --format="%s" -s $commit 2>$null
            $commitAuthor = git show --format="%an <%ae>" -s $commit 2>$null
            $commitDate = git show --format="%ad" -s $commit 2>$null
            
            # Apply the changes from this commit
            $cherryResult = git cherry-pick $commit 2>&1
            
            if ($LASTEXITCODE -eq 0) {
                # Sign the commit
                git commit --amend --no-edit --gpg-sign 2>&1 | Out-Null
                if ($LASTEXITCODE -eq 0) {
                    Write-ColorText "SUCCESS: $commitShort" -Color "Green"
                    $successCount++
                } else {
                    Write-ColorText "ERROR: Failed to sign $commitShort" -Color "Red"
                    $errorCount++
                }
            } else {
                Write-ColorText "ERROR: Cherry-pick failed for $commitShort" -Color "Red"
                Write-ColorText "Cherry-pick output: $cherryResult" -Color "Gray"
                
                # Try to resolve conflicts automatically
                git add . 2>&1 | Out-Null
                git cherry-pick --continue 2>&1 | Out-Null
                
                if ($LASTEXITCODE -eq 0) {
                    git commit --amend --no-edit --gpg-sign 2>&1 | Out-Null
                    Write-ColorText "RECOVERED: $commitShort (after conflict resolution)" -Color "Yellow"
                    $successCount++
                } else {
                    git cherry-pick --abort 2>&1 | Out-Null
                    Write-ColorText "FAILED: $commitShort (could not resolve)" -Color "Red"
                    $errorCount++
                }
            }
        }
        
        if ($errorCount -eq 0 -or $successCount -gt ($commitList.Count * 0.8)) {
            # Replace original branch with signed version
            Write-ColorText "Replacing original branch with signed commits..." -Color "Blue"
            git checkout $Branch 2>&1 | Out-Null
            git reset --hard $tempBranch 2>&1 | Out-Null
            
            if ($LASTEXITCODE -eq 0) {
                Write-ColorText "SUCCESS: Branch replaced with signed history!" -Color "Green"
                git branch -D $tempBranch 2>&1 | Out-Null
                return $true
            } else {
                Write-ColorText "ERROR: Failed to replace branch" -Color "Red"
                return $false
            }
        } else {
            Write-ColorText "Too many errors ($errorCount) - keeping original branch" -Color "Red"
            git checkout $Branch 2>&1 | Out-Null
            git branch -D $tempBranch 2>&1 | Out-Null
            return $false
        }
    }
    catch {
        Write-ColorText "ERROR: Critical exception in manual signing: $($_.Exception.Message)" -Color "Red"
        git checkout $Branch 2>&1 | Out-Null
        return $false
    }
}

function Sign-CommitsSequentially {
    param([string[]]$UnsignedCommits, [string]$Branch)
    
    Write-ColorText "Sequential signing fallback method with branch rebuilding..." -Color "Blue"
    
    # Store current branch
    $originalBranch = $Branch
    $successCount = 0
    $errorCount = 0
    
    try {
        # Create a temporary branch to work on
        $tempBranch = "temp-signing-$(Get-Date -Format 'yyyyMMddHHmmss')"
        Write-ColorText "Creating temporary branch: $tempBranch" -Color "Cyan"
        
        # Start from the first commit and rebuild the branch with signatures
        $firstCommit = $UnsignedCommits[0]
        
        # Checkout the first unsigned commit
        git checkout $firstCommit 2>&1 | Out-Null
        if ($LASTEXITCODE -ne 0) {
            Write-ColorText "ERROR: Could not checkout first commit $($firstCommit.Substring(0,8))" -Color "Red"
            return $false
        }
        
        # Sign the first commit and create new branch
        git commit --amend --no-edit --gpg-sign 2>&1 | Out-Null
        if ($LASTEXITCODE -ne 0) {
            Write-ColorText "ERROR: Could not sign first commit $($firstCommit.Substring(0,8))" -Color "Red"
            return $false
        }
        
        # Create the temporary branch from this signed commit
        git checkout -b $tempBranch 2>&1 | Out-Null
        if ($LASTEXITCODE -ne 0) {
            Write-ColorText "ERROR: Could not create temporary branch" -Color "Red"
            return $false
        }
        
        Write-ColorText "SUCCESS: Signed first commit $($firstCommit.Substring(0,8))" -Color "Green"
        $successCount++
        
        # Now cherry-pick and sign the remaining commits
        for ($i = 1; $i -lt $UnsignedCommits.Count; $i++) {
            $commit = $UnsignedCommits[$i]
            
            try {
                Write-ColorText "Cherry-picking and signing commit $($commit.Substring(0,8))..." -Color "Blue"
                
                # Cherry-pick the commit
                git cherry-pick $commit 2>&1 | Out-Null
                if ($LASTEXITCODE -ne 0) {
                    Write-ColorText "ERROR: Could not cherry-pick $($commit.Substring(0,8))" -Color "Red"
                    $errorCount++
                    
                    # Try to abort the cherry-pick
                    git cherry-pick --abort 2>&1 | Out-Null
                    continue
                }
                
                # Sign the cherry-picked commit
                git commit --amend --no-edit --gpg-sign 2>&1 | Out-Null
                if ($LASTEXITCODE -eq 0) {
                    Write-ColorText "SUCCESS: Signed $($commit.Substring(0,8))" -Color "Green"
                    $successCount++
                } else {
                    Write-ColorText "ERROR: Failed to sign $($commit.Substring(0,8))" -Color "Red"
                    $errorCount++
                }
            }
            catch {
                Write-ColorText "ERROR: Exception signing $($commit.Substring(0,8)): $($_.Exception.Message)" -Color "Red"
                $errorCount++
            }
        }
        
        if ($errorCount -eq 0) {
            # Replace the original branch with our signed version
            Write-ColorText "Replacing original branch with signed version..." -Color "Blue"
            
            # Switch back to original branch
            git checkout $originalBranch 2>&1 | Out-Null
            
            # Reset the original branch to match our signed branch
            git reset --hard $tempBranch 2>&1 | Out-Null
            if ($LASTEXITCODE -eq 0) {
                Write-ColorText "SUCCESS: Branch updated with signed commits!" -Color "Green"
            } else {
                Write-ColorText "ERROR: Failed to update original branch" -Color "Red"
                $errorCount++
            }
        }
        
        # Clean up temporary branch
        git branch -D $tempBranch 2>&1 | Out-Null
        
    }
    catch {
        Write-ColorText "ERROR: Critical exception in sequential signing: $($_.Exception.Message)" -Color "Red"
        $errorCount++
        
        # Clean up - try to get back to original branch
        git checkout $originalBranch 2>&1 | Out-Null
        git branch -D $tempBranch 2>&1 | Out-Null
    }
    
    Write-ColorText "Sequential signing complete: $successCount signed, $errorCount errors" -Color "Cyan"
    return ($errorCount -eq 0)
}

function Sign-IndividualCommits {
    param([string[]]$UnsignedCommits, [string]$Branch)
    
    if ($UnsignedCommits.Count -eq 0) {
        Write-ColorText "No unsigned commits to process!" -Color "Green"
        return $true
    }
    
    Write-ColorText "Individual commit signing mode..." -Color "Blue"
    
    $signedCount = 0
    $skippedCount = 0
    $errorCount = 0
    
    # Store original branch
    $originalBranch = $Branch
    
    foreach ($commit in $UnsignedCommits) {
        $details = Get-CommitDetails $commit
        if (-not $details) {
            Write-ColorText "ERROR: Could not get details for commit $commit" -Color "Red"
            $errorCount++
            continue
        }
        
        Write-Header "Commit: $($details.ShortHash)"
        Write-ColorText "Author: $($details.Author) [$($details.Email)]" -Color "Blue"
        Write-ColorText "Date: $($details.Date)" -Color "Gray"
        Write-ColorText "Subject: $($details.Subject)" -Color "Yellow"
        Write-ColorText "Status: NOT SIGNED" -Color "Red"
        
        $response = Read-Host "`nSign this commit? (Y/n/q to quit)"
        
        if ($response -match "^[Qq]") {
            Write-ColorText "User requested to quit" -Color "Yellow"
            break
        }
        
        $shouldSign = ($response -eq "" -or $response -match "^[Yy]")
        
        if ($shouldSign) {
            if (Sign-SingleCommit $commit) {
                $signedCount++
            } else {
                $errorCount++
            }
        } else {
            Write-ColorText "Skipped commit $($details.ShortHash)" -Color "Gray"
            $skippedCount++
        }
    }
    
    # Return to original branch
    git checkout $originalBranch 2>&1 | Out-Null
    
    Write-Header "Individual Signing Summary"
    Write-ColorText "Signed: $signedCount" -Color "Green"
    Write-ColorText "Skipped: $skippedCount" -Color "Yellow"
    Write-ColorText "Errors: $errorCount" -Color "Red"
    
    return ($errorCount -eq 0)
}

function Handle-PushOptions {
    param([string]$Branch)
    
    Write-Header "Push Options"
    Write-ColorText "All commits are now signed! Choose how to push:" -Color "Green"
    Write-ColorText "" -Color "White"
    Write-ColorText "1. Auto Push (force-with-lease)" -Color "Green"
    Write-ColorText "2. Auto Push (force - more aggressive)" -Color "Yellow" 
    Write-ColorText "3. Show commands only (manual)" -Color "Blue"
    Write-ColorText "4. Skip push" -Color "Gray"
    Write-ColorText "" -Color "White"
    
    $pushChoice = Read-Host "Select option (1/2/3/4)"
    
    switch ($pushChoice) {
        "1" {
            Write-ColorText "Attempting force-with-lease push..." -Color "Blue"
            $result = git push --force-with-lease origin $Branch 2>&1
            $exitCode = $LASTEXITCODE
            
            Write-ColorText "Push output:" -Color "Gray"
            Write-ColorText $result -Color "Gray"
            
            if ($exitCode -eq 0) {
                Write-ColorText "SUCCESS: Push completed!" -Color "Green"
            } else {
                Write-ColorText "ERROR: Force-with-lease failed (stale info)" -Color "Red"
                Write-ColorText "Trying regular force push..." -Color "Yellow"
                
                $forceResult = git push --force origin $Branch 2>&1
                $forceExitCode = $LASTEXITCODE
                
                Write-ColorText "Force push output:" -Color "Gray"  
                Write-ColorText $forceResult -Color "Gray"
                
                if ($forceExitCode -eq 0) {
                    Write-ColorText "SUCCESS: Force push completed!" -Color "Green"
                } else {
                    Write-ColorText "ERROR: Force push also failed" -Color "Red"
                    Write-ColorText "Manual intervention required" -Color "Yellow"
                }
            }
        }
        
        "2" {
            Write-ColorText "Attempting force push..." -Color "Yellow"
            $result = git push --force origin $Branch 2>&1
            $exitCode = $LASTEXITCODE
            
            Write-ColorText "Push output:" -Color "Gray"
            Write-ColorText $result -Color "Gray"
            
            if ($exitCode -eq 0) {
                Write-ColorText "SUCCESS: Force push completed!" -Color "Green"
            } else {
                Write-ColorText "ERROR: Force push failed" -Color "Red"
                Write-ColorText "Manual intervention required" -Color "Yellow"
            }
        }
        
        "3" {
            Write-ColorText "Manual push commands:" -Color "Cyan"
            Write-ColorText "git push --force-with-lease origin $Branch" -Color "White"
            Write-ColorText "OR if that fails:" -Color "Yellow"
            Write-ColorText "git push --force origin $Branch" -Color "White"
        }
        
        "4" {
            Write-ColorText "Skipping push - you can push manually later" -Color "Gray"
        }
        
        default {
            Write-ColorText "Invalid choice - showing manual commands:" -Color "Yellow"
            Write-ColorText "git push --force-with-lease origin $Branch" -Color "White"
            Write-ColorText "OR: git push --force origin $Branch" -Color "White"
        }
    }
}

# Main execution
try {
    Write-Header "Enhanced Git Commit Signing Tool"
    
    if (-not (Test-Path $RepositoryPath)) {
        Write-ColorText "ERROR: Repository path not found: $RepositoryPath" -Color "Red"
        exit 1
    }
    
    Set-Location $RepositoryPath
    Write-ColorText "Repository: $(Get-Location)" -Color "Green"
    
    if ($Branch -eq "") {
        $Branch = git branch --show-current 2>$null
        if ($LASTEXITCODE -ne 0) {
            Write-ColorText "ERROR: Could not determine current branch" -Color "Red"
            exit 1
        }
    }
    
    Write-ColorText "Branch: $Branch" -Color "Green"
    
    if (-not (Test-GPGSetup)) {
        Write-ColorText "CRITICAL: GPG setup failed. Cannot proceed with signing." -Color "Red"
        exit 1
    }
    
    # Get all commits and find unsigned ones
    $allCommits = Get-AllCommits $Branch
    if (-not $allCommits) {
        Write-ColorText "ERROR: Could not get commit list" -Color "Red"
        exit 1
    }
    
    $unsignedCommits = Get-UnsignedCommits $allCommits
    
    if ($unsignedCommits.Count -eq 0) {
        Write-ColorText "SUCCESS: All commits are already signed!" -Color "Green"
        
        # Still offer push options even if already signed
        $response = Read-Host "All commits signed. Do you want push options? (Y/n)"
        if ($response -eq "" -or $response -match "^[Yy]") {
            Handle-PushOptions $Branch
        }
        exit 0
    }
    
    # Choose signing mode
    if ($SigningMode -eq "Ask") {
        Write-Header "Signing Mode Selection"
        Write-ColorText "Found $($unsignedCommits.Count) unsigned commits out of $($allCommits.Count) total" -Color "Yellow"
        Write-ColorText "" -Color "White"
        Write-ColorText "Choose signing mode:" -Color "Cyan"
        Write-ColorText "1. Bulk - Sign all unsigned commits at once (faster)" -Color "Green"
        Write-ColorText "2. Individual - Review and confirm each commit (safer)" -Color "Blue"
        Write-ColorText "" -Color "White"
        
        $modeChoice = Read-Host "Select mode (1/2)"
        
        if ($modeChoice -eq "2") {
            $SigningMode = "Individual"
        } else {
            $SigningMode = "Bulk"
        }
    }
    
    Write-ColorText "Selected mode: $SigningMode" -Color "Cyan"
    
    # Execute signing based on chosen mode
    $success = $false
    
    if ($SigningMode -eq "Individual") {
        $success = Sign-IndividualCommits $unsignedCommits $Branch
    } else {
        # Bulk mode
        Write-ColorText "`nWARNING: Bulk mode will rewrite ALL commit history!" -Color "Yellow"
        Write-ColorText "This will change all commit hashes!" -Color "Yellow"
        $response = Read-Host "`nProceed with bulk signing? (y/N)"
        
        if ($response -match "^[Yy]") {
            $success = Sign-AllCommitsBulk $unsignedCommits $Branch
        } else {
            Write-ColorText "Operation cancelled" -Color "Yellow"
            exit 0
        }
    }
    
    if ($success) {
        # Verify the results
        Write-Header "Final Verification"
        $finalUnsigned = Get-UnsignedCommits (Get-AllCommits $Branch)
        
        if ($finalUnsigned.Count -eq 0) {
            Write-ColorText "SUCCESS: ALL COMMITS ARE NOW SIGNED!" -Color "Green"
            
            # Automatically offer push options
            Handle-PushOptions $Branch
            
        } else {
            Write-ColorText "WARNING: $($finalUnsigned.Count) commits are still unsigned" -Color "Yellow"
            Write-ColorText "You may need to run the script again or check for conflicts" -Color "Yellow"
        }
    } else {
        Write-ColorText "ERROR: Signing process failed" -Color "Red"
    }
    
}
catch {
    Write-ColorText "CRITICAL ERROR: $($_.Exception.Message)" -Color "Red"
}
finally {
    if (-not $SkipPause) {
        Write-Host "`nPress any key to exit..." -ForegroundColor Yellow -NoNewline
        try {
            $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
        }
        catch {
            Read-Host
        }
    }
}