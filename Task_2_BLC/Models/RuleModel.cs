namespace Task_2_BLC.Models
{
    public class RuleModel
    {
        public string FilePattern { get; set; }

        public string DestinationFolder { get; set; }

        public bool IsOrderAppended { get; set; }

        public bool IsDateAppended { get; set; }

        public int MatchesCount { get; set; }
    }
}
