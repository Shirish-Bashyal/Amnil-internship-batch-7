using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQAssingment.Model;

public class Student
{
    private static int Counter = 0;
    public int Id { get; private set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public double Marks { get; set; }
    public string City { get; set; }

    public Student()
    {
        Id = Counter++;
    }
}
