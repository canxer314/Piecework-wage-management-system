using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piecework_wage_management_system
{
    public class wage
    {
        //<GridViewColumn Header="TaskDate"/>
        //<GridViewColumn Header="Product"/>
        //<GridViewColumn Header="Procedure"/>
        //<GridViewColumn Header="Unit"/>
        //<GridViewColumn Header="Price"/>
        //<GridViewColumn Header="Count"/>
        //<GridViewColumn Header="Wage"/>
        public DateTime Date { set; get; }
        public string Product { set; get; }
        public string Value { set; get; }
        public string Procedure { set; get; }
        public string Unit { set; get; }
        public double Price { set; get; }
        public int Count { set; get; }
        public double Wage { set; get; }
    }
}
