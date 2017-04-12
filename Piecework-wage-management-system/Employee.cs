using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piecework_wage_management_system
{
    class Employee
    {
        public string Name { get; set; }
        public short EmployeeId { get; set; }
        public string Password { get; set; }
        public string Department { get; set; }
        public string Workshop { get; set; }
        public string Job { get; set; }
        public byte Authority { get; set; }
        public string Telephone { get; set; }
    }
}
