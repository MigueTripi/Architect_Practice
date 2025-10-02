#!/bin/bash
set -e

RESULTS_DIR=./TestResults

echo "Cleaning old coverage reports..."
rm -rf $RESULTS_DIR

echo "Running tests with coverage..."
dotnet test --settings coverlet.runsettings

REPORT=$(find ./TestResults -name "coverage.cobertura.xml" | head -n 1)

echo "Generating HTML report..."
reportgenerator \
  -reports:$REPORT \
  -targetdir:$RESULTS_DIR/coverage-report \
  -reporttypes:Html
