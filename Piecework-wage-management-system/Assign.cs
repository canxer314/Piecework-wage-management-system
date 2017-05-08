using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piecework_wage_management_system
{
    public class Assign
    {
        //Id INT AUTO_INCREMENT PRIMARY KEY,
        //EmployeeId INT,
        //CONSTRAINT fk_Reckon_EmployeeId FOREIGN KEY (EmployeeId)
        //    REFERENCES tbl_Employee (Id) ON DELETE CASCADE ON UPDATE CASCADE,
        //Value_Id INT,
        //CONSTRAINT fk_Reckon_Value_Id FOREIGN KEY (Value_Id)
        //	REFERENCES tbl_Value (Id) ON DELETE CASCADE ON UPDATE CASCADE,
        //Procedure_Id INT,
        //CONSTRAINT fk_Reckon_Procedure_Id FOREIGN KEY (Procedure_Id)
        //    REFERENCES tbl_Procedure (Id) ON DELETE CASCADE ON UPDATE CASCADE
        public int Id { set; get; }
        public int EmployeeId { set; get; }
        public int Value_Id { set; get; }
        public int Procedure_Id { set; get; }
    }
}
