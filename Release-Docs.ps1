git config --global credential.helper store
Add-Content "$env:USERPROFILE\.git-credentials" "https://$($env:github_access_token):x-oauth-basic@github.com`n"
git config --global user.email "$($env:github_email)"
git config --global user.name "Tolik Pylypchuk"

docfx ./PatternMatching.Docs/docfx.json

$SourceDir = $PSScriptRoot
$TempRepoDir = "$PSScriptRoot\..\patternmatching-gh-pages"
$Version = 'v1.2.0'

Write-Output "Removing the temporary doc directory $TempRepoDir"
git clone https://github.com/TolikPylypchuk/PatternMatching.git --branch gh-pages $TempRepoDir

Write-Output "Clearing the docs directory"

Set-Location "$TempRepoDir"

if (-not (Test-Path ".\$Version"))
{
    git rm -rf "$Version/*"
}

Write-Output "Copying documentation into the repo"
Copy-Item -Path "$SourceDir\docs" -Destination ".\$Version" -Recurse

Write-Output "Pushing the new docs to the remote branch"
git add . -A
git commit -m "Update generated documentation for version $VERSION"
git push origin gh-pages
