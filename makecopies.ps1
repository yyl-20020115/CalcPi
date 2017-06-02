$source = "."

$excludes = @(".vs",".git")

$dests = @(
	"E:\Working\CalcPi",
	"F:\Working\CalcPi",
	"G:\Working\CalcPi",
	"C:\Users\yyl_2\OneDrive\ÎÄµµ\CalcPi",
	"\\YILIN-SUPOWER\Working2\Theory\CalcPi",
	"\\YILIN-SUPOWER\Working-dev\CalcPi"
)

$dests | ForEach-Object {  Get-ChildItem $source | Copy-Item -Destination (New-ITem $_ -ItemType "Directory" -Force) -Force -Recurse }
