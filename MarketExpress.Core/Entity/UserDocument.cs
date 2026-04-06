using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MarketExpress.Core.Entity
{
   public  class UserDocument
    {
     public int Id {  get; set; }

public string UserId {  get; set; }
   
public string FilePath {  get; set; }
public enDocumentType Type {  get; set; }
        public User User { get; set; }
    }
}
