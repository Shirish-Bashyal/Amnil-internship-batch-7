using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooManagementSystem.cs
{
    internal class Turtle : Reptile
    {
        public Turtle(int id, int age)
            : base(id, age) { }

        public override string GetFeedingSchedule()
        {
            return "once a week";
        }

        public override int CalculateFoodCost()
        {
            return 10;
        }
    }
}
