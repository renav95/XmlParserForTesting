namespace XmlDataImporter
{
    /// <summary>
    /// Данные для записи в БД.
    /// </summary>
    public class TestDataModel
    {
        /// <summary>
        /// Имя теста.
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Длительность теста.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Результат выполнения теста.
        /// </summary>
        public string Result { get; set;}

        /// <summary>
        /// Количество попыток выполнения теста в рамках одного тестового запуска.
        /// </summary>
        public int TryNumber { get; set; }
    }
}
