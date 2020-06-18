using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace HotelMotel.Business.Models
{
    public class Certification : IValidatableObject
    {
        public bool CertComplete { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DateCertified { get; set; }
        [Required]
        public string CeoName { get; set; }
        [Required]
        public string CeoTitle { get; set; }
        [Required]
        public string PreparerName { get; set; }
        [Required]
        public string PreparerTitle { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:###-###-####}")]
        public string PreparerPhone { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string PreparerEmail { get; set; }

        [RequiredIfTrue("Form3DocUploaded", ErrorMessage = "*")]
        public string PcisProviderName { get; set; }

        [RequiredIfTrue("Form3DocUploaded", ErrorMessage = "*")]
        public string PcisProviderTitle { get; set; }

        [RequiredIfTrue("Form3DocUploaded", ErrorMessage = "*")]
        [DisplayFormat(DataFormatString = "{0:###-###-####}")]
        public string PcisProviderPhone { get; set; }

        [RequiredIfTrue("Form3DocUploaded", ErrorMessage = "*")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string PcisProviderEmail { get; set; }

        public bool Form3DocUploaded { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Form3DocUploaded && PcisProviderName == "")
            {
                yield return new ValidationResult("PCIS Provider must be provided.");
            }
            if (Form3DocUploaded && PcisProviderTitle == "")
            {
                yield return new ValidationResult("PCIS Title must be provided.");
            }
            if (Form3DocUploaded && PcisProviderEmail == "")
            {
                yield return new ValidationResult("PCIS Provider Email must be provided.");
            }
            if (Form3DocUploaded && PcisProviderPhone == "")
            {
                yield return new ValidationResult("PCIS Provider Phone must be provided.");
            }
        }
    }
}
