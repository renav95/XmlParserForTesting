namespace XmlDataImporter
{
    using System.Configuration;

    /// <summary>
    /// Возвращает пути к файлам.
    /// </summary>
    public static class FilePaths
    {
        /// <summary>
        /// Получает имя файла xml с результатами тестов первого запуска.
        /// </summary>
        public static string TestResultsXmlFilePath => ConfigurationManager.AppSettings["TestResultsXmlFilePath"] ?? throw new ArgumentNullException(nameof(TestResultsXmlFilePath));

        /// <summary>
        /// Получает имя файла xml с результатами тестов второго запуска.
        /// </summary>
        public static string RetryTestResultsXmlFilePath => ConfigurationManager.AppSettings["RetryTestResultsXmlFilePath"] ?? throw new ArgumentNullException(nameof(RetryTestResultsXmlFilePath));
    }
}