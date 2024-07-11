namespace XmlParser;

using System.Xml;
using static FileService;
using static DefaultArguments;

/// <summary>
/// Определяет главную точку входа приложения.
/// </summary>
public class Program
{
    /// <summary>
    /// Название директории с результатами тестирования.
    /// </summary>
    private static string _testResultDirectoryPath = TestResultDirectoryPath;

    /// <summary>
    /// Название xml файла с результатом тестирования.
    /// </summary>
    private static string _testResultXmlFileName = TestResultXmlFileName;

    /// <summary>
    /// Название файла со списком названий проваленных тестов.
    /// </summary>
    private static string _failedTestsTxtFileName = FailedTestsTxtFileName;

    /// <summary>
    /// Точка входа приложения.
    /// </summary>
    /// <param name="args">
    /// Аргументы командной строки.
    /// </param>
    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            _testResultDirectoryPath = args[0];
            _testResultXmlFileName = args[1];
            _failedTestsTxtFileName = args[2];
        }

        var filePath = Path.Combine(_testResultDirectoryPath, _testResultXmlFileName);
            
        var xmlDocument = LoadXmlDocument(filePath);
        var failedTestCases = GetFailedTestCases(xmlDocument);

        var resFilePath = Path.Combine(_testResultDirectoryPath, _failedTestsTxtFileName);
        var failedTestNames = GetFailedTestNames(failedTestCases);
        SaveFailedTestNamesToFile(resFilePath, failedTestNames);
    }

    /// <summary>
    /// Получает список тестов с атрибутом result="Failed" тега test-case из результирующего Xml документа.
    /// </summary>
    /// <param name="xmlDocument">
    /// Xml документ.
    /// </param>
    /// <returns>
    /// Список тестов со статусом Failed.
    /// </returns>
    private static List<XmlAttributeCollection> GetFailedTestCases(XmlDocument xmlDocument)
    {
        var xmlNodeList = xmlDocument.GetElementsByTagName("test-case");
        var failedTestCases = new List<XmlAttributeCollection>();

        foreach (XmlNode node in xmlNodeList)
        {
            var nodeAttributes = node.Attributes;
            if (nodeAttributes != null)
            {
                bool hasFailedResult = false;
                foreach (XmlAttribute attr in nodeAttributes)
                {
                    if (attr.LocalName == "result" && attr.Value == "Failed")
                    {
                        hasFailedResult = true;
                        break;
                    }
                }

                if (hasFailedResult)
                    failedTestCases.Add(nodeAttributes);
            }
        }

        return failedTestCases;
    }

    /// <summary>
    /// Получает список названий тестов со статусом 'Failed' из атрибута 'fullname' тега 'test-case'.
    /// </summary>
    /// <param name="failedTestCases">
    /// Проваленные тесты.
    /// </param>
    /// <returns>
    /// Список названий тестов со статусом 'Failed' из атрибута 'fullname' тега 'test-case'.
    /// </returns>
    private static List<string> GetFailedTestNames(List<XmlAttributeCollection> failedTestCases)
    {
        var failedTestNames = new List<string>();

        foreach (var attributes in failedTestCases)
            foreach (XmlAttribute attr in attributes)
                if (attr.LocalName == "fullname")
                    failedTestNames.Add(attr.Value);

        return failedTestNames;
    }
}