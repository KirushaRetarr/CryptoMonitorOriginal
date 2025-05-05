// Models/DataUpdateLog.cs
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoMonitor.Models
{
    public class DataUpdateLog
    {
        public int LogId { get; set; }

        // Стало:
        [ForeignKey(nameof(ApiSource))]
        public int ApiSourceId { get; set; }

        public ApiSource ApiSource { get; set; }

        public DateTime RunTime { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
