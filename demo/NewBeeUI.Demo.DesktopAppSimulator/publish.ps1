dotnet publish -r win-x64 -c Release -o ../../dist/appdemo

function DeletePdb {
    param(
        # 参数列表（可选）
        [string]$dllName
    )

    $pdbPath = "../../dist/appdemo/$dllName.pdb"

    if (Test-Path $pdbPath) {
        Remove-Item $pdbPath
    }
}

DeletePdb("NewBeeUI")
DeletePdb("NewBeeUI.Demo")
DeletePdb("NewBeeUIAppDemo")