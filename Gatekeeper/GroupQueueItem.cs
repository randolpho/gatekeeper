using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatekeeper
{
    public class GroupQueueItem
    {
        public string GroupName { get; set; } = "Queued Group";
        public ObservableCollection<Group> Groups { get; set; } = new ObservableCollection<Group>();
        public int GroupSize
        {
            get
            {
                return Groups.Sum(g => g.GroupSize);
            }
        }
    }
}
