$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Runtime.WindowsRuntime
[void][Windows.Storage.StorageFile, Windows.Storage, ContentType=WindowsRuntime]
[void][Windows.Data.Pdf.PdfDocument, Windows.Data.Pdf, ContentType=WindowsRuntime]
[void][Windows.Storage.Streams.InMemoryRandomAccessStream, Windows.Storage.Streams, ContentType=WindowsRuntime]
$methods = [System.WindowsRuntimeSystemExtensions].GetMethods()
$asTask = $methods | Where-Object {
    $_.Name -eq 'AsTask' -and $_.IsGenericMethodDefinition -and
    $_.GetGenericArguments().Length -eq 1 -and $_.GetParameters().Length -eq 1 -and
    $_.GetParameters()[0].ParameterType.Name -eq 'IAsyncOperation`1'
} | Select-Object -First 1
$asAction = $methods | Where-Object {
    $_.Name -eq 'AsTask' -and !$_.IsGenericMethod -and
    $_.GetParameters().Length -eq 1 -and $_.GetParameters()[0].ParameterType.Name -eq 'IAsyncAction'
} | Select-Object -First 1
function Wait-WinRt($operation, $type) {
    $task = $asTask.MakeGenericMethod($type).Invoke($null, @($operation))
    $task.Wait()
    $task.Result
}
$file = Wait-WinRt ([Windows.Storage.StorageFile]::GetFileFromPathAsync([IO.Path]::GetFullPath($pdf))) ([Windows.Storage.StorageFile])
$document = Wait-WinRt ([Windows.Data.Pdf.PdfDocument]::LoadFromFileAsync($file)) ([Windows.Data.Pdf.PdfDocument])
if ($index -lt 0 -or $index -ge $document.PageCount) { exit 1 }
$page = $document.GetPage([uint32]$index)
try {
    $options = New-Object Windows.Data.Pdf.PdfPageRenderOptions
    $edge = [Math]::Max($page.Size.Width, $page.Size.Height)
    $options.DestinationWidth = [uint32][Math]::Max(1, [Math]::Round(1600 * $page.Size.Width / $edge))
    $options.DestinationHeight = [uint32][Math]::Max(1, [Math]::Round(1600 * $page.Size.Height / $edge))
    $stream = New-Object Windows.Storage.Streams.InMemoryRandomAccessStream
    $asAction.Invoke($null, @($page.RenderToStreamAsync($stream, $options))).Wait()
    $stream.Seek(0)
    $inputStream = [System.IO.WindowsRuntimeStreamExtensions]::AsStreamForRead($stream)
    $outputStream = [IO.File]::Create($png)
    try { $inputStream.CopyTo($outputStream) }
    finally { $outputStream.Dispose(); $inputStream.Dispose() }
    $document.PageCount
}
finally { $page.Dispose() }
