namespace XmlDataImporter
{
    using static AuthorizationData;
    using System.Data.SqlClient;

    /// <summary>
    /// Вспомогательный класс для работы с базой данных MS Sql Server.
    /// </summary>
    public class DbService
    {
        /// <summary>
        /// Записывает данные результатов тестирования в базу.
        /// </summary>
        /// <param name="results">
        /// Результаты тестирования.
        /// </param>
        public static void WriteData(List<TestDataModel> results)
        {
            try
            {
                WriteData(connectionString: GetConnectionString(), results);
            }
            catch (Exception ex)
            {
                throw new Exception($"При записи данных в базу возникло исключение -  \"{ex.Message}\"");
            }
        }

        /// <summary>
        /// Выполняет запись данных в БД.
        /// </summary>
        /// <param name="connectionString">
        /// Строка подключения.
        /// </param>
        /// <param name="testsData">
        /// Данные для записи.
        /// </param>
        private static void WriteData(string connectionString, List<TestDataModel> testsData)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var testData in testsData)
                {
                    var checkTestQuery = "SELECT Id FROM Tests WHERE TestName = @TestName";
                    using (SqlCommand checkCommand = new SqlCommand(checkTestQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@TestName", testData.TestName);
                        var existingTestId = checkCommand.ExecuteScalar();

                        if (existingTestId == null)
                        {
                            WriteTest(connection, testData);
                            WriteTestResults(connection, testData);
                        }
                        else
                        {
                            WriteTestResults(connection, testData);
                        }
                    }
                }
                
                SqlConnection.ClearAllPools();
                connection.Close();
            }
        }

        /// <summary>
        /// Записывает данные теста.
        /// </summary>
        /// <param name="connection">
        /// Соединение с БД.
        /// </param>
        /// <param name="testData">
        /// Тестовые данные для записи.
        /// </param>
        private static void WriteTest(SqlConnection connection, TestDataModel testData)
        {
            var insertTestQuery1 = "INSERT INTO Tests (TestName) VALUES (@TestName)";

            using (var insertCommand = new SqlCommand(insertTestQuery1, connection))
            {
                insertCommand.Parameters.AddWithValue("@TestName", testData.TestName);
                insertCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Записывает результаты теста.
        /// </summary>
        /// <param name="connection">
        /// Соединение с БД.
        /// </param>
        /// <param name="testData">
        /// Данные для записи.
        /// </param>
        private static void WriteTestResults(SqlConnection connection, TestDataModel testData)
        {
            var selectTestIdQuery = $"Select Id From Tests Where TestName= '{testData.TestName}'";

            using (var command = new SqlCommand(selectTestIdQuery, connection))
            {
                var insertTestResultQuery = "INSERT INTO TestResult (TestId, Duration, Result, RecorderTime) VALUES (@TestId, @Duration, @Result, @RecorderTime)";
                
                var testId = (Guid)command.ExecuteScalar();
                
                using (var testResultCommand = new SqlCommand(insertTestResultQuery, connection))
                {
                    testResultCommand.Parameters.AddWithValue("@TestId", testId);
                    testResultCommand.Parameters.AddWithValue("@Duration", testData.Duration);
                    testResultCommand.Parameters.AddWithValue("@Result", testData.Result);
                    testResultCommand.Parameters.AddWithValue("@RecorderTime", DateTime.Now);

                    testResultCommand.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Получает строку подключения из файла конфигурации.
        /// </summary>
        /// <returns>
        /// Строка подключения.
        /// </returns>
        private static string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = ServerName,
                InitialCatalog = DbName,
                UserID = UserName,
                Password = UserPassword,
                PersistSecurityInfo = false,
                Encrypt = false
            };

            return builder.ConnectionString;
        }
    }
}
