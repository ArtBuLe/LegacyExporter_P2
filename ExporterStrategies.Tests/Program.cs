using ExporterStrategies;
var failed = new List<string>();
void Check(string name, bool condition)
{
    Console.WriteLine($"{(condition ? "PASS" : "FAIL")} — {name}");
    if (!condition) failed.Add(name);
}
var path = Path.Combine(Path.GetTempPath(), $"report-test-{Guid.NewGuid():N}.txt");
try
{
    IReportExporter fileExporter = new TextFileReportExporter(path);
    var service = new ReportService(fileExporter);
    service.PublishDailySummary(10, 1);
    var contents = File.ReadAllText(path);
    Check("файловый экспорт создаёт файл", File.Exists(path));
    Check("файл содержит заголовок", contents.Contains("Сводка за день"));
    Check("файл содержит значения отчёта", contents.Contains("Всего задач: 10") && contents.Contains("Ошибок: 1"));
}
finally { if (File.Exists(path)) File.Delete(path); }


// TODO 7: Добавьте проверку LegacyReportAdapter через Console.Out.
// TODO 8: Докажите, что ReportService не меняется при переключении экспортёра.
var output = new StringWriter();
var originalOut = Console.Out;
try
{
    Console.SetOut(output);
    IReportExporter legacyExporter = new LegacyReportAdapter(new LegacyReportPrinter());
    var service2 = new ReportService(legacyExporter);
    service2.PublishDailySummary(5, 0);

    var text = output.ToString();
    Check("LegacyAdapter: заголовок передан принтеру", text.Contains("=== Сводка за день ==="));
    Check("LegacyAdapter: тело передано принтеру", text.Contains("Всего задач: 5"));
}
finally
{
    Console.SetOut(originalOut);
}
// TODO 9: Проверьте контракт пустого документа.
try
{
    var exporter = new TextFileReportExporter(path);
    exporter.Export(new ReportDocument("", "Тело"));
    Check("пустой заголовок вызывает ArgumentException", false);
}
catch (ArgumentException)
{
    Check("пустой заголовок вызывает ArgumentException", true);
}
// TODO 10: Проверьте ошибочный путь и отсутствие подавления ошибки записи.
try
{
    var badExporter = new TextFileReportExporter("");
    Check("пустой путь в конструкторе вызывает ArgumentException", false);
}
catch (ArgumentException)
{
    Check("пустой путь в конструкторе вызывает ArgumentException", true);
}
Console.WriteLine($"Провалено базовых проверок: {failed.Count} из 5.");
