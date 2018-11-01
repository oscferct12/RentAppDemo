using System;
using System.Collections.Generic;
using System.Text;

namespace RentApp.Entities.BackEndEntities.Request
{
    public class MovementInsertRequest
    {
        public string userId { get; set; }
        public string year { get; set; }
        public string monthRevenue { get; set; }
        public string otherRevenue { get; set; }
        public string expenses { get; set; }
        public string otherExpenses { get; set; }
        public string partitionKey { get; set; }
        public string rowKey { get; set; }
        public DateTime timestamp { get; set; }
        public string eTag { get; set; }
    }
}
