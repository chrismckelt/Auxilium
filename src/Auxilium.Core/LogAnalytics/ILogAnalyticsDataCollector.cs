using System.Threading.Tasks;

namespace Auxilium.Core.LogAnalytics
{
    public interface ILogAnalyticsDataCollector
    {
        Task Collect(string json, string logType = "ApplicationLog", string apiVersion = "2016-04-01", string timeGeneratedPropertyName = null);
        Task Collect(object objectToSerialize, string logType = "ApplicationLog", string apiVersion = "2016-04-01", string timeGeneratedPropertyName = null);
    }
}
