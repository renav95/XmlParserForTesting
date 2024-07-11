namespace XmlParser
{
    using System.Configuration;

    /// <summary>
    /// Аргументы по умолчанию.
    /// </summary>
    public static class DefaultArguments
    {
        /// <summary>
        /// Название директории с результатами тестирования.
        /// </summary>
        public static string TestResultDirectoryPath = ConfigurationManager.AppSettings["TestResultDirectoryPath"] ?? throw new ArgumentNullException(nameof(TestResultDirectoryPath));
        /// <summary>
        /// Название xml файла с результатом тестирования.
        /// </summary>
        public static string TestResultXmlFileName = ConfigurationManager.AppSettings["TestResultXmlFileName"] ?? throw new ArgumentNullException(nameof(TestResultXmlFileName));

        /// <summary>
        /// Название файла со списком названий проваленных тестов.
        /// </summary>
        public static string FailedTestsTxtFileName = ConfigurationManager.AppSettings["FailedTestsTxtFileName"] ?? throw new ArgumentNullException(nameof(FailedTestsTxtFileName));
    }
}
