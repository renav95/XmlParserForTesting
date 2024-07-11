namespace XmlDataImporter
{
    using System.Xml;

    /// <summary>
    /// Вспомогательная служба для работы с файлами.
    /// </summary>
    public class FileService
    {
        /// <summary>
        /// Получает xml документ из файла.
        /// </summary>
        /// <param name="filePath">
        /// Путь к файлу.
        /// </param>
        /// <returns>
        /// Xml документ.
        /// </returns>
        public static XmlDocument GetXmlDocument(string filePath)
        {
            var xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.Load(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки файла с расширением xml. Вызвано исключение - '{ex.Message}'");
                throw;
            }

            return xmlDocument;
        }

        /// <summary>
        /// Получает результаты тестов.
        /// </summary>
        /// <param name="xmlDocument">
        /// Xml документ.
        /// </param>
        /// <returns>
        /// Результаты тестов.
        /// </returns>
        public static List<TestDataModel> GetTestResultsData(XmlDocument xmlDocument)
        {
            var xmlAttributeCollection = GetAttributeCollectionsWithTagName(xmlDocument, tagName: "test-case");

            var testDataList = new List<TestDataModel>();

            foreach (var xmlAttribute in xmlAttributeCollection)
            {
                var testModel = new TestDataModel();

                foreach (XmlAttribute attr in xmlAttribute)
                {
                    switch (attr.LocalName)
                    {
                        case "name":
                            testModel.TestName = attr.Value;
                            break;
                        case "duration":
                        {
                            try
                            {
                                testModel.Duration = Math.Round(double.Parse(attr.Value.Replace('.', ',')) / 60, 2);
                            }
                            catch (Exception)
                            {
                                testModel.Duration = 0.0;
                            }
                        }
                            break;
                        case "result":
                            testModel.Result = attr.Value;
                            break;
                    }

                    if (!testDataList.Exists(i => i.TestName == testModel.TestName))
                        testDataList.Add(testModel);
                }
            }

            return testDataList;
        }

        /// <summary>
        /// Получает коллекцию атрибутов по тегу xml документа.
        /// </summary>
        /// <param name="xmlDocument">
        /// Xml документ.
        /// </param>
        /// <param name="tagName">
        /// Имя тега.
        /// </param>
        /// <returns>
        /// Список с коллекцией атрибутов.
        /// </returns>
        private static List<XmlAttributeCollection> GetAttributeCollectionsWithTagName(XmlDocument xmlDocument, string tagName)
        {
            var xmlNodeList = xmlDocument.GetElementsByTagName(tagName);
            var xmlAttributeCollection = new List<XmlAttributeCollection>();

            foreach (XmlNode node in xmlNodeList)
            {
                var nodeAttributes = node.Attributes;

                if (nodeAttributes != null)
                {
                    foreach (XmlAttribute attr in nodeAttributes)
                        if (attr.LocalName == "duration")
                            xmlAttributeCollection.Add(nodeAttributes);
                }
            }

            return xmlAttributeCollection;
        }
    }
}
