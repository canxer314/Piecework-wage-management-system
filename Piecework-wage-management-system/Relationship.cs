using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piecework_wage_management_system
{
    public class Relationship
    {
        //Id INT AUTO_INCREMENT PRIMARY KEY,
        //Sequence_Number INT,
        //Procedure_Name char(20) UNIQUE KEY,
        //CONSTRAINT fk_Procedure_Name FOREIGN KEY (Procedure_Name)
        //	REFERENCES tbl_Procedure(Name),
        //Input_Output_Ratio INT,
        //Product_Id INT,
        //CONSTRAINT fk_Product_Id FOREIGN KEY (Product_Id)
        //	REFERENCES tbl_Procedure (Product_Id)
        public int Id { set; get; }
        public string InputProcedure { set; get; }
        public string OutputProcedure { set; get; }
        public int Input_Output_Ratio { set; get; }
        public int Product_Id { set; get; }
    }
}
