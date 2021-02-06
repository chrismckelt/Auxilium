namespace Auxilium.Core.LogAnalytics
{
    public class LogMessage
    {
        public string Message { get; set; }

        public static LogMessage Log(string note)
        {
            return new LogMessage() {Message = note};
        }
    }
}