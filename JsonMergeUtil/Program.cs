namespace JsonMergeUtil
{
    using static JsonFileUtil;

    internal class Program
    {
        static void Main()
        {
            var firstJson = GetJsonObject(FirstJsonFile) ?? throw new InvalidOperationException();
            var secondJson = GetJsonObject(SecondJsonFile) ?? throw new InvalidOperationException();

            var testErrorsDict = GetTestErrors(firstJson.ExecutionResults);
            var matchingScenarios = GetMatchingScenarios(testErrorsDict, secondJson.ExecutionResults);

            testErrorsDict = matchingScenarios;

            UpdateFirstJsonResults(firstJson, testErrorsDict);
            AddSkippedScenarios(firstJson, secondJson);

            SaveJsonToFile(firstJson);
        }
    }
}
