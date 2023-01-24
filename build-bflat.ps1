$AllProjectFiles = Get-ChildItem consoleApp -Filter *.cs -Exclude *Assembly* -Recurse | % { $_.FullName }

echo "File list:"
echo $AllProjectFiles


echo "Creating output dir..."
New-Item -Path .\output -ItemType Directory -Force

echo "Compiling..."
.\bflat/bflat.exe build $AllProjectFiles --target Exe --out "output/ConvertisseurSTAR.exe" --arch:x64 --os:windows --optimize-time --no-reflection --no-globalization --no-debug-info --deterministic

echo "Finished!"