using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auxilium.Core.LogAnalytics
{
    public interface ILogAnalyticsService
    {
        Task<T> LogAnalyticsSearch<T>(LogAnalyticsQuery search, string workspaceId);
    }
}
