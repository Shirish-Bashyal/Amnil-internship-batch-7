using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCrudConsole
{
    /// <summary>
    ///represents a student entity
    /// </summary>
    public class StudentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    /// <summary>
    /// performs CRUD operations on student entity
    /// </summary>
    public class StudentCrud
    {
        private Dictionary<int, StudentModel> StudentCache = new Dictionary<int, StudentModel>();

        /// <summary>
        /// Adds a new student object to the memory
        /// </summary>
        /// <param name="model"> represents student object </param>
        /// <returns> message if the operation is success or not</returns>
        public string AddStudent(StudentModel model)
        {
            if (StudentCache.TryAdd(model.Id, model))
                return "Student Added";

            return "Id already exists";
        }

        /// <summary>
        /// Gets all the students from the memory
        /// </summary>
        /// <returns> list of student entity if any exists</returns>
        public ICollection<StudentModel> GetStudents()
        {
            return StudentCache.Values;
        }

        /// <summary>
        /// updates the existing student entity
        /// </summary>
        /// <param name="model"> represent student object</param>
        /// <returns> message if the operation is success or not </returns>
        public string UpdateStudent(StudentModel model)
        {
            if (StudentCache.ContainsKey(model.Id))
            {
                StudentCache[model.Id] = model;
                return "Student updated";
            }
            else
                return "Student doesnot exists";
        }

        /// <summary>
        /// deletes the student entity on the basis of id
        /// </summary>
        /// <param name="id"> represents student id</param>
        /// <returns>message if the operation is success or not</returns>
        public string DeleteStudent(int id)
        {
            if (StudentCache.ContainsKey(id))
            {
                StudentCache.Remove(id);
                return "Student Deleted";
            }
            else
                return "Student not found";
        }
    }
}
