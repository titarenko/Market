using System.Web.Mvc;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Projection;

namespace Market.Cqrsnes.WebUi.Controllers
{
    public class OfferController : Controller
    {
        private readonly IRepository repository;

        public OfferController(IRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult List()
        {
            return View(repository.GetAll<Offer>());
        }
    }
}
