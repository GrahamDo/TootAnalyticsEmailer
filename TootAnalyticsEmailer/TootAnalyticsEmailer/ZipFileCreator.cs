using System.IO.Compression;

namespace TootAnalyticsEmailer;

internal class ZipFileCreator : IDisposable
{
    private readonly string _fileName;
    private readonly string _fullPath;

    public ZipFileCreator(string fileName)
    {
        _fileName = fileName;
        _fullPath = $"{Path.GetTempPath()}{_fileName}";
    }

    public string Create(string csvData)
    {
        WriteCsv(csvData, _fileName);
        CreateZip(_fileName);
        return $"{_fullPath}.zip";
    }

    public void Dispose()
    {
        var zipFileName = $"{_fullPath}.zip";
        if (File.Exists(zipFileName))
            File.Delete(zipFileName);
    }

    private void CreateZip(string fileName)
    {
        using var fs = new FileStream($"{_fullPath}.zip", FileMode.Create);
        using var zip = new ZipArchive(fs, ZipArchiveMode.Create);
        zip.CreateEntryFromFile($"{_fullPath}.csv", $"{fileName}.csv", CompressionLevel.SmallestSize);
        File.Delete($"{_fullPath}.csv");
    }

    private void WriteCsv(string csvData, string fileName) => File.WriteAllText($"{_fullPath}.csv", csvData);
}