//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelMotel.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class HMT_Certifications
    {
        public HMT_Certifications()
        {
            this.HMT_PCIS_Docs = new HashSet<HMT_PCIS_Docs>();
        }
    
        public string CICOID { get; set; }
        public string Fiscal_Year { get; set; }
        public string ATCode { get; set; }
        public bool OrdinanceCorrect { get; set; }
        public bool P1_Complete { get; set; }
        public bool P2_Complete { get; set; }
        public bool P3_Complete { get; set; }
        public bool Cert_Complete { get; set; }
        public Nullable<System.DateTime> Date_Certified { get; set; }
        public string CEO_Name { get; set; }
        public string CEO_Title { get; set; }
        public string Preparer_Name { get; set; }
        public string Preparer_Title { get; set; }
        public string Preparer_Phone { get; set; }
        public string Preparer_Email { get; set; }
        public string PCIS_Provider_Name { get; set; }
        public string PCIS_Provider_Title { get; set; }
        public string PCIS_Provider_Phone { get; set; }
        public string PCIS_Provider_Email { get; set; }
        public string HMTRID { get; set; }
        public Nullable<int> HMSurvey_ID { get; set; }
        public bool ShortTermRentals { get; set; }
    
        public virtual ICollection<HMT_PCIS_Docs> HMT_PCIS_Docs { get; set; }
    }
}
