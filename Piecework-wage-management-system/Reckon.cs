using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piecework_wage_management_system
{
    public class Reckon
    {
        //Assign_Id INT PRIMAEY KEY,
        //CONSTRAINT fk_Assign_Id FOREIGN KEY (Assign_Id)
        //    REFERENCES tbl_Assign (Id) ON DELETE CASCADE ON UPDATE CASCADE,
        //Price_Id INT,
        //CONSTRAINT fk_Price_Id FOREIGN KEY (Price_Id)
        //    REFERENCES tbl_Value_Price (Id) ON DELETE CASCADE ON UPDATE CASCADE,
        //Count INT 
        public int Assign_Id { set; get; }
        public int Count { set; get; }
    }
}
