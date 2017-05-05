using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piecework_wage_management_system
{
    public class Relationship
    {
        //InputProcedure INT PRIMARY KEY,
        //OutputProceduce INT,
        //Product_Id INT,
        //Scale INT
        public string InputProcedure { set; get; }
        public string OutputProcedure { set; get; }
        public int Product_Id { set; get; }
        public int Scale { set; get; }
    }
}
