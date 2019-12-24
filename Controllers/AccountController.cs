using Microsoft.AspNet.Identity;
using SigmaTestTask.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Data.Entity;
using System.Dynamic;

namespace SigmaTestTask.Controllers
{
    public class AccountController : ApiController
    {
        OrderContext db;

        public AccountController()
        {
            db = new OrderContext();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        [Authorize]
        public IHttpActionResult Get()
        {
            var userDb = db.Users.Include(e=> e.Orders.Select(s => s.ProductsInCart)).Where((userObj) => userObj.UserName == User.Identity.Name).FirstOrDefault();
            dynamic userData = new ExpandoObject();
            userData.PersonalDiscount = userDb.PersonalDiscount;
            userData.Orders = userDb.Orders;
            return Ok(new { results = userData }); ;
        }

    }
}