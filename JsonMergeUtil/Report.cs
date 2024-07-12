namespace JsonMergeUtil
{
    public class Report
    {
        public string[] Nodes { get; set; }

        public string? ExecutionTime { get; set; }

        public string? GenerationTime { get; set; }

        public string? PluginUserSpecFlowId { get; set; }

        public string? CLIUserSpecFlowId { get; set; }

        public Scenario[] ExecutionResults { get; set; }

        public string? StepReports { get; set; }
    }
}