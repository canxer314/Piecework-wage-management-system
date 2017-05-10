using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piecework_wage_management_system
{
    public class P_Price
    {
        public int PriceId { set; get; }
        public int AssignId { set; get; }
        public string ProcedureName { set; get; }
        public string Unit { set; get; }
        public double Price { set; get; }
    }
}
