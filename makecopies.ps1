$source = "."

$excludes = @(".vs",".git")

$name = $env:COMPUTERNAME

if ($name -eq "YILIN-WIN10")
{
    $dests = @(
	    "E:\Working\CalcPi",
	    "F:\Working\CalcPi",
	    "G:\Working\CalcPi",
        "C:\Users\yyl_2\iCloudDrive\CalcPi",
	    "C:\Users\yyl_2\OneDrive\ÎÄµµ\CalcPi",
	    "\\YILIN-SUPOWER\Working2\Theory\CalcPi",
        "\\YILIN-SUPOWER\Working-dev\CalcPi"
    )
}
elseif ($name -eq "YILIN-SUPOWER")
{
    $dests = @(
	    "K:\Users\Yilin\OneDrive\ÎÄµµ",
	    "\\YILIN-SUPOWER\Working2\Theory\CalcPi",
        "\\YILIN-SUPOWER\Working-dev\CalcPi"
    )
}
else
{
    $dests = ""
}

if ($source -ne "")
{
    $dests | ForEach-Object {  Get-ChildItem $source | Copy-Item -Destination (New-ITem $_ -ItemType "Directory" -Force) -Force -Recurse }
}