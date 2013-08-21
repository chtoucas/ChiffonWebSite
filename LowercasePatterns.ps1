# Cf. http://stackoverflow.com/questions/3822745/rename-files-to-lowercase-in-powershell
dir "../_patterns" -r | % { if ($_.Name -cne $_.Name.ToLower()) { ren $_.FullName $_.Name.ToLower() } }