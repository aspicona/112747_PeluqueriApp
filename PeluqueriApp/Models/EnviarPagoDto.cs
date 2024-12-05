namespace PeluqueriApp.Models
{
    public class EnviarPagoDto
    {
        public string Email { get; set; }
        public  string PhoneNumber { get; set; }
        public  string PersonType { get; set; }
        public string IdentificationType { get; set; }
        public  string IdentificationNumber { get; set; }
        public  string BanksList { get; set; }
        public  string TransactionAmount { get; set; }
        public  string Description { get; set; }
        public  string ZipCode { get; set; }
        public  string StreetName { get; set; }
        public  string StreetNumber { get; set; }
        public  string Neighborhood { get; set; }
        public  string City { get; set; }
        public  string FederalUnit { get; set; }
        public  string PhoneAreaCode { get; set; }
    }
}