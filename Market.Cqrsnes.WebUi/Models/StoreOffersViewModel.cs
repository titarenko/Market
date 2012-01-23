using System.Collections.Generic;
using System.Web.Mvc;
using Market.Cqrsnes.Projection;

namespace Market.Cqrsnes.WebUi.Models
{
    public class StoreOffersViewModel
    {
        public IEnumerable<SelectListItem> Articles { get; set; }
        public StoreOffers StoreOffers { get; set; } 
    }
}