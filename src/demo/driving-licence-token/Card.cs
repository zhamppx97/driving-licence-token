using System;

namespace driving_licence_token
{
    public class Card
    {
        public string CardId { get; set; }
        public string CardType { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string NationalId { get; set; }
        public string TitlenameTH { get; set; }
        public string FirstnameTH { get; set; }
        public string LastnameTH { get; set; }
        public string TitlenameEN { get; set; }
        public string FirstnameEN { get; set; }
        public string LastnameEN { get; set; }
        public string IssuedCode { get; set; }
        public string IssuedProvince { get; set; }
        public string LicenseImage { get; set; }
        public string UserImage { get; set; }
    }
}
