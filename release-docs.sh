#!/bin/sh
set -e

docfx ./PatternMatching.Docs/docfx.json

SOURCE_DIR=$PWD
TEMP_REPO_DIR=$PWD/../PatternMatching-gh-pages
VERSION=v1.2.0

echo "Removing the temporary doc directory $TEMP_REPO_DIR"
rm -rf $TEMP_REPO_DIR
mkdir $TEMP_REPO_DIR

echo "Cloning the repo with the gh-pages branch"
git clone https://github.com/TolikPylypchuk/PatternMatching.git --branch gh-pages $TEMP_REPO_DIR

echo "Clearing the docs directory"

cd $TEMP_REPO_DIR

if [ -d "$VERSION" ]; then
    git rm -rf "$VERSION/*" || true
fi

mkdir "$VERSION"

cd "$VERSION"

echo "Copying documentation into the repo"
cp -r $SOURCE_DIR/docs/* .

echo "Pushing the new docs to the remote branch"
git add . -A
git commit -m "Update generated documentation for version $VERSION"
git push origin gh-pages
