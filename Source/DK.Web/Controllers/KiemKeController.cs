using DK.Application.Repositories;
using System.Web.Mvc;

namespace DK.Web.Controllers
{
    [Authorize]
    public class KiemKeController : Controller
    {
        private readonly ITypeRepository _typeRepository;
        public KiemKeController(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
    }
}
