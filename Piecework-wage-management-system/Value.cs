using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piecework_wage_management_system
{
    public class Value
    {
        public string Name { set; get; }
        public string Unit { set; get; }
        public int Unit_Price { set; get; }
        public int Procedure_Id { set; get; }
               // Name CHAR(20) PRIMARY KEY,
               // Unit CHAR(20),
               // Unit_Price INT,
               // Procedure_Id INT,
    }
}
