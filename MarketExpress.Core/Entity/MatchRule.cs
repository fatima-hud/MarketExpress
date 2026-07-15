using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
    public class MatchRule
    {
       
        public enType Type { get; set; }
        public enOccasion Occasion { get; set; }

        public MatchRule(enType type, enOccasion occasion)
        {
            Type = type;
            Occasion = occasion;
        }
    }
}
