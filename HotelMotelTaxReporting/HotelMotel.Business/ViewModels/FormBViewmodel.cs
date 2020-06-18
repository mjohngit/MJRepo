using System.Collections.Generic;
using HotelMotel.Business.Models;

namespace HotelMotel.Business.ViewModels
{
    public class FormBViewmodel
    {
        public FormBViewmodel()
        {
			PurposeAmountResults = new List<PurposeAmount>();
            DmoResultses = new List<DmoResults>();
            ParkResultses = new List<ParksResults>();
		}

        public List<PurposeAmount> PurposeAmountResults { get; set; }
        public List<DmoResults> DmoResultses { get; set; }
        public List<ParksResults> ParkResultses { get; set; } 
    }
}
