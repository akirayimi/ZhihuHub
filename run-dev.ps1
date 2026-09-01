# ZhihuHub 开发环境启动脚本
# 使用方法: .\run-dev.ps1

Write-Host "=== ZhihuHub 开发环境 ===" -ForegroundColor Cyan

# 设置 .NET 路径
$dotnetPath = "$env:USERPROFILE\scoop\apps\dotnet8-sdk\current"
$env:Path = "$dotnetPath;$env:Path"

# 验证 .NET 可用性
Write-Host "`n✓ .NET SDK: " -NoNewline -ForegroundColor Green
& "$dotnetPath\dotnet.exe" --version

# 显示选项
Write-Host "`n请选择操作:" -ForegroundColor Yellow
Write-Host "  1. 恢复依赖 (dotnet restore)"
Write-Host "  2. 运行 Avalonia 项目 (热重载)"
Write-Host "  3. 编译 Release 版本"
Write-Host "  4. 发布单文件 exe"
Write-Host "  5. 在 VS Code 中打开项目"
Write-Host "  0. 退出"

$choice = Read-Host "`n输入选项"

switch ($choice) {
    "1" {
        Write-Host "`n恢复依赖..." -ForegroundColor Cyan
        & "$dotnetPath\dotnet.exe" restore
    }
    "2" {
        Write-Host "`n运行 Avalonia 项目（按 Ctrl+C 退出）..." -ForegroundColor Cyan
        & "$dotnetPath\dotnet.exe" watch run --project ZhihuHub.Avalonia/ZhihuHub.Avalonia.csproj
    }
    "3" {
        Write-Host "`n编译 Release 版本..." -ForegroundColor Cyan
        & "$dotnetPath\dotnet.exe" build --configuration Release
    }
    "4" {
        Write-Host "`n发布单文件 exe..." -ForegroundColor Cyan
        & "$dotnetPath\dotnet.exe" publish ZhihuHub.Avalonia/ZhihuHub.Avalonia.csproj `
          --configuration Release `
          --runtime win-x64 `
          --self-contained true `
          --output ./publish `
          -p:PublishSingleFile=true `
          -p:IncludeNativeLibrariesForSelfExtract=true `
          -p:EnableCompressionInSingleFile=true

        if (Test-Path ./publish/ZhihuHub.exe) {
            Write-Host "`n✓ 发布成功: ./publish/ZhihuHub.exe" -ForegroundColor Green
            $size = (Get-Item ./publish/ZhihuHub.exe).Length / 1MB
            Write-Host "文件大小: $($size.ToString('F2')) MB" -ForegroundColor Gray
        }
    }
    "5" {
        Write-Host "`n在 VS Code 中打开..." -ForegroundColor Cyan
        code .
    }
    "0" {
        Write-Host "`n再见！" -ForegroundColor Gray
    }
    default {
        Write-Host "`n无效选项" -ForegroundColor Red
    }
}
