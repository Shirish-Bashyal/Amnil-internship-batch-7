using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooManagementSystem.cs
{
    internal class Lion : Mammal
    {
        public Lion(int id, int age)
            : base(id, age) { }

        public override string GetFeedingSchedule()
        {
            return "once a day";
        }

        public override int CalculateFoodCost()
        {
            return 50 * 1;
        }
    }
}
