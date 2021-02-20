namespace CBF_API.Models
{
    public class CustomInt
    {
        public int WeekNumber { get; set; }
    }
    public class CustomCount
    {
        public int Count { get; set; }
    }

    public class PickCustomCount
    {
         
        public int PickMadeCount { get; set; }
        public int NoPickCount { get; set; }
        public int EliminatedCount { get; set; }
        public int AllEntriesCount { get; set; }
        public int TotalAliveCount { get; set; }
        public int TotalMemberCount { get; set; }
    }
}