using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Enums
{
    public enum enComplaintStatus
    {
        Pending=0,      // الشكوى انفتحت ولسا ما حدا اشتغل عليها
        InProgress,   // صاحب المتجر شافها وعم يعالجها
        Resolved,     // انحلت
        Closed        // انغلقت نهائياً
    }
}
