dotnet publish -r win-x64 -c Release -o ../../dist/publish-aot-demo


function DeletePdb {
    param(
        # 参数列表（可选）
        [string]$dllName
    )

    $pdbPath = "../../dist/publish-aot-demo/$dllName.pdb"

    if (Test-Path $pdbPath) {
        Remove-Item $pdbPath
    }
}

DeletePdb("CherylCrossTest.Desktop")
DeletePdb("CherylCrossTest")
DeletePdb("CherylUI")