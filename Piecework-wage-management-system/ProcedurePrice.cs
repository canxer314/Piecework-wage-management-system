using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piecework_wage_management_system
{
    public class ProcedurePrice
    {
        public int Id { set; get; }
        public int Sequence { set; get; }
        public string ProcedureName { set; get; }
        public string Unit { set; get; }
        public double Price { set; get; }
    }
}
