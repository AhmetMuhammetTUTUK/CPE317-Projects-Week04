using System;

namespace StudentRegisterApp
{
    /// <summary>
    /// CourseCode struct holds the course code and its credits.
    /// </summary>
    public struct CourseCode
    {
        /// <summary>Course code (e.g., BIL101)</summary>
        public string Code { get; set; }

        /// <summary>Credit value of the course</summary>
        public int Credits { get; set; }

        /// <summary>Constructor for CourseCode struct</summary>
        public CourseCode(string code, int credits)
        {
            Code = code;
            Credits = credits;
        }

        /// <summary>Returns CourseCode details as a string.</summary>
        public override string ToString() => $"{Code} ({Credits} Credits)";
    }

    /// <summary>
    /// Delegate for handling mark addition events.
    /// </summary>
    /// <param name="msg">Event message</param>
    public delegate void MarkHandler(string msg);

    /// <summary>
    /// Abstract base class representing students and other persons.
    /// </summary>
    public abstract class Person
    {
        /// <summary>Name of the person</summary>
        public string Name { get; set; }

        /// <summary>Protected field accessible by derived classes</summary>
        protected string IdentifierInfo;

        /// <summary>Constructor initializing the Person class.</summary>
        protected Person(string name)
        {
            Name = name;
            IdentifierInfo = "ID-" + Guid.NewGuid().ToString().Substring(0, 6);
        }

        /// <summary>Abstract method that derived classes must override.</summary>
        public abstract void Describe();
    }

    /// <summary>
    /// Student class inheriting from Person.
    /// </summary>
    public class Student : Person
    {
        private int _mark;

        /// <summary>Event triggered when a mark is added</summary>
        public event MarkHandler OnMarkAdded;

        /// <summary>
        /// Student's mark (validated between 0-100).
        /// </summary>
        public int Mark
        {
            get => _mark;
            set
            {
                if (value >= 0 && value <= 100)
                {
                    _mark = value;
                    // Trigger the event exactly once when a valid mark is added
                    OnMarkAdded?.Invoke($"Mark {_mark} was successfully added for student {Name}.");
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Mark must be between 0 and 100.");
                }
            }
        }

        /// <summary>Enrolled course information for the student (Struct usage)</summary>
        public CourseCode EnrolledCourse { get; set; }

        /// <summary>
        /// Constructor for Student class calling the base class (Person) constructor.
        /// </summary>
        public Student(string name, CourseCode course) : base(name)
        {
            EnrolledCourse = course;
        }

        /// <summary>
        /// Overrides the abstract Describe method from Person to print student details.
        /// </summary>
        public override void Describe()
        {
            Console.WriteLine($"Student Name: {Name}, Mark: {Mark}, Course: {EnrolledCourse}");
        }
    }

    class Program
    {
        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        static void Main(string[] args)
        {
            // Step 8: Struct usage (CourseCode)
            CourseCode csharpCourse = new CourseCode("CENG102", 5);
            CourseCode webCourse = new CourseCode("CENG204", 4);

            // Step 6: Creating 2 student objects
            Student student1 = new Student("Ahmet", csharpCourse);
            Student student2 = new Student("Ali", webCourse);

            // Step 5: Subscribing to events
            student1.OnMarkAdded += message => Console.WriteLine($"[NOTIFICATION] {message}");
            student2.OnMarkAdded += message => Console.WriteLine($"[NOTIFICATION] {message}");

            Console.WriteLine("--- Student Register App Initializing ---\n");

            try
            {
                // Adding marks (triggers setter validation and event invocation)
                student1.Mark = 85;
                student2.Mark = 92;

                // Step 4: Calling the overridden Describe method
                Console.WriteLine("\n--- Student Details ---");
                student1.Describe();
                student2.Describe();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Occurred: {ex.Message}");
            }

            Console.WriteLine("\nProcess completed. Press any key to exit.");
            Console.ReadKey();
        }
    }
}