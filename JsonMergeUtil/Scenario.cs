namespace JsonMergeUtil
{
    public class Scenario
    {
        public string? ContextType { get; set; }

        public string? FeatureFolderPath { get; set; }

        public string? FeatureTitle { get; set; }

        public string ScenarioTitle { get; set; }

        public string[] ScenarioArguments { get; set; }

        public string? Status { get; set; }

        public StepResult[] StepResults { get; set; }

        public string? Outputs { get; set; }
    }
}