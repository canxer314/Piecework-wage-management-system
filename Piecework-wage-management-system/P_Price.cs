using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piecework_wage_management_system 
{
    public class P_Price : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int PriceId { set; get; }
        public int AssignId { set; get; }
        public string ProcedureName { set; get; }
        public string Unit { set; get; }
        public double Price { set; get; }
        private bool isSubmited;
        public bool IsSubmited
        {
            get
            {
                return isSubmited;
            }
            set
            {
                isSubmited = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsSubmited"));
                }
            }
        }
    }
}
