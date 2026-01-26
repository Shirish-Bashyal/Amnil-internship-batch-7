using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooManagementSystem.cs
{
    internal class Reptile : Animal
    {
        public Reptile(int id, int age)
            : base(id, age) { }

        public override string GetFeedingSchedule()
        {
            return "Once in two day";
        }

        public override int CalculateFoodCost()
        {
            return 500;
        }
    }
}
