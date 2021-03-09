namespace TestModule.Models
{
    //this throws exception
    public class AnotherPerson : Person
    {
        public AnotherPerson() : base() { }
        public string AdditionalField1 { get; set; }
        public string AdditionalField2 { get; set; }
        public string AdditionalField3 { get; set; }
    }

    //this works

    //public class AnotherPerson
    //{
    //    public int Id { get; set; }
    //    public string Firstname { get; set; }
    //    public string Lastname { get; set; }
    //    public int Age { get; set; }
    //    public bool Anonymous { get; set; }
    //    public int Version { get; set; }

    //    public string AdditionalField1 { get; set; }
    //    public string AdditionalField2 { get; set; }
    //    public string AdditionalField3 { get; set; }
    //}
}
