namespace XmlParser
{
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Вспомогательный класс для работы с файлами.
    /// </summary>
    public static class FileService
    {
        /// <summary>
        /// Получает Xml документ из файла <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">
        /// Путь к файлу xml.
        /// </param>
        /// <returns>Xml документ.</returns>
        public static XmlDocument LoadXmlDocument(string filePath)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);
            return xmlDocument;
        }

        /// <summary>
        /// Сохраняет список названий тестов со статусом 'Failed' в файл <paramref name="resultFilePath"/>.
        /// </summary>
        /// <param name="resultFilePath">
        /// Путь к файлу с названиями тестов.
        /// </param>
        /// <param name="failedTestNames">
        /// Названия проваленных тестов.
        /// </param>
        public static void SaveFailedTestNamesToFile(string resultFilePath, List<string> failedTestNames)
        {
            using var sw = new StreamWriter(resultFilePath, false, Encoding.UTF8);
            foreach (var testName in failedTestNames)
                sw.WriteLine(testName);
        }
    }
}
