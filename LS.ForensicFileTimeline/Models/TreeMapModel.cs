using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.ForensicFileTimeline.Models
{
    public class TreeMapModel
    {
        public TreeMapModel(string name, int value, List<TreeMapModel> items)
        {
            Name = name;
            Value = value;
            Items = items;
        }
        public string Name { get; set; }
        public int Value { get; set; }

        public List<TreeMapModel> Items { get; set; }
    }
}
