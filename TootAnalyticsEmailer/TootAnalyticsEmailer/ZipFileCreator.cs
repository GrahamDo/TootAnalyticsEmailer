using System.IO.Compression;

namespace TootAnalyticsEmailer;

internal class ZipFileCreator : IDisposable
{
    private string _fullPath;

    public string Create(string csvData, string fileName)
    {
        _fullPath = $"{Path.GetTempPath()}{fileName}";

        WriteCsv(csvData, fileName);
        CreateZip(fileName);
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