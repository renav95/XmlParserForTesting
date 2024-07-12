namespace JsonMergeUtil
{
    using System.Configuration;
    using System.Text;
    using Newtonsoft.Json;

    public static class JsonFileUtil
    {
        public static string FirstJsonFile => ConfigurationManager.AppSettings["FirstReportJsonFile"] ?? throw new ArgumentNullException(nameof(FirstJsonFile));

        public static string SecondJsonFile => ConfigurationManager.AppSettings["SecondReportJsonFile"] ?? throw new ArgumentNullException(nameof(SecondJsonFile));

        public static string TestResultFilePath => ConfigurationManager.AppSettings["TestResultFilePath"] ?? throw new ArgumentNullException(nameof(TestResultFilePath));

        public static Report GetJsonObject(string jsonFilePath)
        {
            using var streamReader = new StreamReader(jsonFilePath, Encoding.UTF8);
            var json = streamReader.ReadToEnd();

            return JsonConvert.DeserializeObject<Report>(json);
        }

        public static void SaveJsonToFile(Report json)
        {
            File.WriteAllText(TestResultFilePath, JsonConvert.SerializeObject(json));
        }

        /// <summary>
        /// Получает названия сценариев из отчета.
        /// </summary>
        /// <param name="scenarios">
        /// Сценарии.
        /// </param>
        /// <returns>
        /// Названия сценариев отчета.
        /// </returns>
        public static List<string> GetScenariosTitle(Scenario[] scenarios)
        {
            return scenarios.Select(scenario => scenario.ScenarioTitle).ToList();
        }

        /// <summary>
        /// Получает сценарии, которые были выполнены при втором тестировании.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="scenarios"></param>
        /// <returns></returns>
        public static Dictionary<int, Scenario> GetMatchingScenarios(Dictionary<int, Scenario> dict, Scenario[] scenarios)
        {
            var newDict = new Dictionary<int, Scenario>();

            foreach (var item in dict)
            {
                var title = item.Value.ScenarioTitle;
                var tempScenario = scenarios.FirstOrDefault(i => i.ScenarioTitle == title && i.Status == "OK");

                if (tempScenario != null)
                {
                    newDict.Add(item.Key, tempScenario);
                }
            }

            return newDict;
        }

        /// <summary>
        /// Получает коллекцию тестовых сценариев со статусом "TestError" из отчета.
        /// </summary>
        /// <param name="scenarios">
        /// Сценарии отчета.
        /// </param>
        /// <returns>
        /// Коллекция проваленных тестовых сценариев.
        /// </returns>
        public static Dictionary<int, Scenario> GetTestErrors(Scenario[] scenarios)
        {
            var testErrorsDict = new Dictionary<int, Scenario>();

            for (int i = 0; i < scenarios.Length; i++)
                if (scenarios[i].Status == "TestError")
                    testErrorsDict.Add(i, scenarios[i]);

            return testErrorsDict;
        }

        /// <summary>
        /// Обновляет сценарии в отчете.
        /// </summary>
        /// <param name="firstJson">
        /// Отчет.
        /// </param>
        /// <param name="matchingScenarios">
        /// Сценарии для замены.
        /// </param>
        public static void UpdateFirstJsonResults(Report firstJson, Dictionary<int, Scenario> matchingScenarios)
        {
            foreach (var item in matchingScenarios)
            {
                if (item.Value != null)
                    firstJson.ExecutionResults[item.Key] = item.Value;
            }
        }

        /// <summary>
        /// Добавляет в отчет пропущенные сценарии тестирования в первом json.
        /// </summary>
        /// <param name="firstJson">
        /// Отчет первого тестирования.
        /// </param>
        /// <param name="secondJson">
        /// Отчет второго тестирования.
        /// </param>
        public static void AddSkippedScenarios(Report firstJson, Report secondJson)
        {
            var firstScenarios = firstJson.ExecutionResults;
            var secondScenarios = secondJson.ExecutionResults;

            var firstScenariosTitle = GetScenariosTitle(firstScenarios);
            var secondScenariosTitle = GetScenariosTitle(secondScenarios);

            var s = new List<Scenario>();

            foreach (var secondScenarioTitle in secondScenariosTitle)
            {
                if (!firstScenariosTitle.Contains(secondScenarioTitle))
                {
                    var q = secondScenarios.FirstOrDefault(i => i.ScenarioTitle == secondScenarioTitle);
                    if (q != null)
                        s.Add(q);
                }
            }

            foreach (var item in s)
            {
                var newArr = firstJson.ExecutionResults.ToList();
                newArr.Add(item);
                firstJson.ExecutionResults = newArr.ToArray();
            }
        }
    }
}
