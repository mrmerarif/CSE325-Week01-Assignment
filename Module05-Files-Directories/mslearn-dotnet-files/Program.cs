
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(
    Path.Combine(salesTotalDir, "totals.txt"),
    $"{salesTotal}{Environment.NewLine}"
);

var reportPath = Path.Combine(salesTotalDir, "sales-summary.txt");
WriteSalesSummary(salesFiles, reportPath);

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(
        folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);

        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    foreach (var file in salesFiles)
    {
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Convert the JSON into a SalesData record
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the sales amount to the total
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}


void WriteSalesSummary(IEnumerable<string> salesFiles, string reportPath)
{
    decimal grandTotal = 0;
    List<string> details = new List<string>();

    foreach (var file in salesFiles)
    {
        // Only include the original sales.json files
        if (Path.GetFileName(file) != "sales.json")
        {
            continue;
        }

        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        decimal fileTotal = data == null ? 0 : (decimal)data.Total;
        grandTotal += fileTotal;

        string fileName = Path.GetRelativePath(
            Directory.GetCurrentDirectory(), file);

        details.Add($"{fileName}: {fileTotal:C2}");
    }

    string report =
        "Sales Summary" + Environment.NewLine +
        "-------------" + Environment.NewLine +
        $"Total Sales: {grandTotal:C2}" + Environment.NewLine +
        Environment.NewLine +
        "Details:" + Environment.NewLine +
        string.Join(Environment.NewLine, details);

    File.WriteAllText(reportPath, report);
}


record SalesData(double Total);
