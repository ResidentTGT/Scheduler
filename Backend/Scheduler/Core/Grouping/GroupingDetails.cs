using Scheduler.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.Grouping
{
    public class GroupingDetails
    {
        private DbManager _dbManager;

        public GroupingDetails(DbManager dbManager)
        {
            _dbManager = dbManager;
        }
    }
}
