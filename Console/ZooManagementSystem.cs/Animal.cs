using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooManagementSystem.cs
{
    internal class Animal : IFeedable
    {
        private readonly int _id; //id field
        private int _age; //age field
        public int Id => _id; //Id property

        public int Age //Age property with validation
        {
            get => _age;
            set
            {
                if (value < 0 || value > 100)
                    throw new Exception("Age must be within 0 and 100");
                _age = value;
            }
        }

        public Animal(int id, int age)
        {
            _id = id;
            _age = age;
        }

        public virtual string GetFeedingSchedule()
        {
            return "once a day";
        }

        public virtual int CalculateFoodCost()
        {
            return 100;
        }
    }
}
