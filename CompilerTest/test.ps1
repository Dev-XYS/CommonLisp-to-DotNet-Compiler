Set-ItemProperty "HKCU:\Software\Microsoft\Windows\Windows Error Reporting" -Name DontShowUI -Value 1

$(If ($args.count -eq 0) { Get-ChildItem ".\programs" } Else { Get-Item (".\programs\" + $args[0]) }) |
ForEach-Object {
	Write-Host -NoNewline $_.Name.PadRight(32, " ")

	sbcl.exe --script $_.FullName > .\answer.txt 2> $null
	if ($LASTEXITCODE -ne 0) {
		Write-Output "sbcl failed"
		return
	}

	..\Compiler\bin\Debug\netcoreapp3.1\Compiler.exe $_.FullName > $null 2> $null
	if ($LASTEXITCODE -ne 0) {
		Write-Output "Compile error"
		return
	}

	dotnet.exe .\temp.dll > .\output.txt 2> $null
	if ($LASTEXITCODE -ne 0) {
		Write-Output "Runtime error"
		return
	}

	if (Compare-Object (Get-Content answer.txt) (Get-Content output.txt)) {
		Write-Output "Diff"
	}
	else {
		Write-Output "OK"
	}
}

Set-ItemProperty "HKCU:\Software\Microsoft\Windows\Windows Error Reporting" -Name DontShowUI -Value 0
