using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piecework_wage_management_system
{
    public class ValuePrice
    {
        public int Id { set; get; }
        public string Unit { set; get; }
        public double Unit_Price { set; get; }
        public int Sequence { set; get; }
        public int Value_Id { set; get; }
        public int Procedure_Id { set; get; }
    }
}
