#!/bin/bash
# Dave's Arcade - Code Coverage Report Generator
# Generates HTML coverage report with integration tests

echo "?? Dave's Arcade - Generating Code Coverage Report..."
echo ""

# Clean previous coverage
echo "Cleaning previous coverage data..."
rm -rf tests/DavesArcade.Tests/TestResults
rm -rf coverage-report
rm -rf coverage

# Run tests with coverage
echo ""
echo "Running all tests with coverage collection..."
dotnet test tests/DavesArcade.Tests/DavesArcade.Tests.csproj \
    --configuration Release \
    --collect:"XPlat Code Coverage" \
    --results-directory:"./coverage"

# Find coverage file
COVERAGE_FILE=$(find coverage -name "coverage.cobertura.xml" | head -n 1)

if [ -z "$COVERAGE_FILE" ]; then
    echo ""
    echo "? Coverage file not found!"
    exit 1
fi

echo ""
echo "? Coverage data collected: $COVERAGE_FILE"

# Check if reportgenerator is installed
echo ""
echo "Checking for ReportGenerator..."

if ! dotnet tool list -g | grep -q "dotnet-reportgenerator-globaltool"; then
    echo "ReportGenerator not found. Installing..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
    echo "? ReportGenerator installed"
else
    echo "? ReportGenerator already installed"
fi

# Generate HTML report
echo ""
echo "Generating HTML report..."

reportgenerator \
    -reports:"$COVERAGE_FILE" \
    -targetdir:"coverage-report" \
    -reporttypes:"Html;TextSummary"

if [ $? -eq 0 ]; then
    echo ""
    echo "? Coverage report generated successfully!"
    echo ""
    
    # Display summary
    echo "?? Coverage Summary:"
    cat coverage-report/Summary.txt
    
    echo ""
    echo "?? Report Location: coverage-report/index.html"
    echo ""
    
    # Open report (OS-specific)
    read -p "Open coverage report in browser? (Y/n) " -n 1 -r
    echo ""
    if [[ $REPLY =~ ^[Yy]$ ]] || [[ -z $REPLY ]]; then
        if [[ "$OSTYPE" == "darwin"* ]]; then
            # macOS
            open coverage-report/index.html
        elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
            # Linux
            xdg-open coverage-report/index.html 2>/dev/null || echo "Please open coverage-report/index.html manually"
        fi
    fi
    
    echo ""
    echo "? Done!"
else
    echo ""
    echo "? Failed to generate report"
    exit 1
fi
