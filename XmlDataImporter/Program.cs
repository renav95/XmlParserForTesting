namespace XmlDataImporter;

using static FileService;
using static DbService;
using static FilePaths;

/// <summary>
/// Определяет главную точку входа приложения.
/// </summary>
public class Program
{
    /// <summary>
    /// Главная точка входа приложения.
    /// </summary>
    public static int Main()
    {
        try
        {
            var firstTestResultsData = GetTestResultsData(GetXmlDocument(TestResultsXmlFilePath));
            var secondTestResultsData = GetTestResultsData(GetXmlDocument(TestResultsXmlFilePath));

            WriteData(firstTestResultsData);

            Console.WriteLine("Данные успешно записаны в БД!");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка записи данных в базу. Вызвано исключение - '{ex.Message}'");
            return -1;
        }
    }
}