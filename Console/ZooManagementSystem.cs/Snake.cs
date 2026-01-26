using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooManagementSystem.cs
{
    internal class Snake : Reptile
    {
        public Snake(int id, int age)
            : base(id, age) { }

        public override string GetFeedingSchedule()
        {
            return "twice a week";
        }

        public override int CalculateFoodCost()
        {
            return 20;
        }
    }
}
