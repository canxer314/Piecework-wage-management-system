using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piecework_wage_management_system
{
    public class Value
    {
        public int Id { set; get; }
        public int TaskNum { set; get; }
        public DateTime TaskDate { set; get; }
        public string Name { set; get; }
        public string Product_Name { set; get; }
        public int Product_Id { set; get; }
    }
}
