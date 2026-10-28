using System.Runtime.Serialization;

namespace ExporterStrategies;
public sealed class TextFileReportExporter : IReportExporter
{
    private readonly string _path;
    public TextFileReportExporter(string path)
    {
        // TODO 4: Проверьте путь; пустая строка не должна считаться допустимым именем файла.
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(nameof(path), "название файла не может быть пустым");
        }
        _path = path;
    }
    public void Export(ReportDocument document)
    {
        // TODO 5: Проверьте документ.
        if (document is null)
            throw new ArgumentNullException(nameof(document), "Документ не может быть null");
        if (string.IsNullOrWhiteSpace(document.Title))
            throw new ArgumentException("Заголовок отчёта не может быть пустым", nameof(document.Title));
        if (string.IsNullOrWhiteSpace(document.Body))
            throw new ArgumentException("Тело отчёта не может быть пустым", nameof(document.Body));
        // TODO 6: Запишите отчёт в согласованном формате.
        // Ошибки записи не проглатывать.
        string content = 
        $"=== {document.Title} ===" +
        $"{Environment.NewLine}" +
        $"{document.Body}" +
        $"{Environment.NewLine}" +
        $"=== END OF REPORT ===";

        File.WriteAllText(_path, content);
    }
}
