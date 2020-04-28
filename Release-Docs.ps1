git config --global credential.helper store
Add-Content "$env:USERPROFILE/.git-credentials" "https://$($env:github_access_token):x-oauth-basic@github.com`n"
git config --global user.email "$($env:github_email)"
git config --global user.name "Tolik Pylypchuk"

docfx ./docs/docfx.json

$SourceDir = $PSScriptRoot
$TempRepoDir = "$PSScriptRoot/../matchmaker-gh-pages"

if (Test-Path "$TempRepoDir")
{
    Write-Output "Removing the temporary doc directory $TempRepoDir"
    Remove-Item $TempRepoDir -Recurse -ErrorAction Ignore
}

New-Item -Path "$TempRepoDir" -ItemType Directory

Write-Output "Cloning the repo on the gh-pages branch"
git clone https://github.com/TolikPylypchuk/Matchmaker.git --branch gh-pages $TempRepoDir

Set-Location "$TempRepoDir"

if (Test-Path "v$($env:version)")
{
    Write-Output "Clearing the docs directory"
    git rm -rf "v$($env:version)/*"
} else
{
    New-Item -Name "v$($env:version)" -ItemType Directory
}

Write-Output "Copying documentation into the repo"
Copy-Item -Path "$SourceDir/docs-dest/*" -Destination "v$($env:version)" -Recurse

Write-Output "Pushing the new docs to the remote branch"
git add . -A
git commit -m "Update generated documentation for version $env:version"
git push origin gh-pages
