using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooManagementSystem.cs
{
    internal class Zebra : Mammal
    {
        public Zebra(int id, int age)
            : base(id, age) { }

        public override string GetFeedingSchedule()
        {
            return "twice a day";
        }

        public override int CalculateFoodCost()
        {
            return 50 * 2;
        }
    }
}
