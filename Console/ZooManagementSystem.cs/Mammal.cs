using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooManagementSystem.cs
{
    internal class Mammal : Animal
    {
        public Mammal(int id, int age)
            : base(id, age) { }

        public override string GetFeedingSchedule()
        {
            return "Thrice a day";
        }

        public override int CalculateFoodCost()
        {
            return 1000;
        }
    }
}
