namespace Frontend.Model
{
    public class Patient
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public int Age { get; set; }

        public string PatientIllness { get; set; }

        public int AssignedDoctorId { get; set; }

        public Doctor? AssignedDoctor { get; set; }
    }
}
