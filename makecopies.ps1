$excludes = @(".vs",".git")

$source = "."

$dests = @(
	"E:\Working\",
	"C:\Users\yyl_2\OneDrive\ÎÄµµ\",
	"\\YILIN-SUPOWER\Working2\Theory\",
	"\\YILIN-SUPOWER\Working-dev\"
)

foreach($dirName in $dests)
{
    Get-ChildItem $source -Directory | 
        Where-Object{$_.Name -notin $excludes} | 
        Copy-Item -Destination $dirName -Recurse -Force
}
