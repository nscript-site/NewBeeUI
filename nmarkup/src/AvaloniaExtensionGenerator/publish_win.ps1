dotnet publish -r win-x64 -c Release --self-contained -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -o ../../dist/tools

function DeletePdb {
    param(
        # 参数列表（可选）
        [string]$dllName
    )

    $pdbPath = "../../dist/tools/$dllName.pdb"

    if (Test-Path $pdbPath) {
        Remove-Item $pdbPath
    }
}

DeletePdb("AvaloniaExtensionGenerator")