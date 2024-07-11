namespace XmlDataImporter;

using static XmlFileService;
using static FilePaths;
using static DbService;

/// <summary>
/// Определяет главную точку входа приложения.
/// </summary>
public class Program
{
    /// <summary>
    /// Тестовые данные для записи в базу данных.
    /// </summary>
    private static readonly List<TestDataModel> OverallTestData = new();

    /// <summary>
    /// Главная точка входа приложения.
    /// </summary>
    public static int Main()
    {
        try
        {
            var firstIterationTestResultsData = GetTestResultsData(GetXmlDocument(TestResultsXmlFilePath));
            var secondIterationTestResultsData = GetTestResultsData(GetXmlDocument(RetryTestResultsXmlFilePath));

            PrepareData(firstIterationTestResultsData, secondIterationTestResultsData);
            WriteData(OverallTestData);

            Console.WriteLine("Данные успешно записаны в БД!");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка записи данных в базу. Вызвано исключение - '{ex.Message}'");
            return -1;
        }
    }

    public static void PrepareData(List<TestDataModel> firstTestIterationData, List<TestDataModel> secondTestIterationData)
    {
        foreach (var obj in firstTestIterationData)
        {
            if (obj.Result == "Passed")
            {
                obj.TryNumber = 1;
                OverallTestData.Add(obj);
            }
        }

        foreach (var obj in secondTestIterationData)
        {
            if (obj.Result == "Passed")
            {
                obj.TryNumber = 2;
                OverallTestData.Add(obj);
            }
            else if (obj.Result == "Failed")
            {
                OverallTestData.Add(obj);
            }
        }
    }
}