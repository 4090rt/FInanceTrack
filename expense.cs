using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp4
{
    //класс коснтруктор данных категория/сумма/дата
    internal class expense
    {
        public string category { get; set; }
        public decimal count { get; set; }
        public DateTime date { get; set; }
    }
}
