$customization_project_dir = "..\cust\project\*"

$out_dir = "output"
$out_bin_dir = "$out_dir\Bin"

# remove existing output folder
Remove-Item $out_dir -Force -Recurse -ErrorAction SilentlyContinue

# copy all items are needed for customization project
New-Item -ItemType "directory" -Path "$out_bin_dir"
Copy-Item "$customization_project_dir" -Destination "$out_dir" -Recurse -Force