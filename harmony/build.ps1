cd ./OpenHarmony.Avalonia
dotnet publish  ./Src/Entry/Entry.csproj -c Release -r linux-musl-arm64 -p:PublishAot=true  -o OHOS_Project/entry/libs/arm64-v8a
cd ../