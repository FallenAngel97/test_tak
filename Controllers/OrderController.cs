using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using SigmaTestTask.Models;
using System.Data.Entity;

namespace SigmaTestTask.Controllers
{
    public class OrderData
    {
        public int order_id { get; set; }
        public string product_name { get; set; }
        public float price { get; set; }
        public int quantity { get; set; }
    }

    public class OrderController: ApiController
    {
        OrderContext db = new OrderContext();

        // Gets all products
        [System.Web.Mvc.HttpGet]
        public IHttpActionResult Get()
        {
            var productsList = db.Products;
            return Ok( new { results = productsList });
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("api/order/add_to_order")]
        public int AddToOrder(OrderData orderData)
        {
            if (HttpContext.Current.Request.HttpMethod == "POST")
            {
                var user = db.Users.Include(u => u.Orders.Select(s=>s.ProductsInCart)).Where(s => s.UserName == User.Identity.Name).FirstOrDefault();
                
                var product = db.Products.Where(s => s.ProductName == orderData.product_name).FirstOrDefault();
                var orderObj = user.Orders.FirstOrDefault((order) => order.Id == orderData.order_id);
                if (orderObj == null)
                {
                    user.Orders.Add(new Order()
                    {
                        ProductsInCart = new List<ProductInCart>()
                    });
                    orderObj = user.Orders.First();
                }
                orderObj.ProductsInCart.Add(new ProductInCart(){ Price=product.Price, ProductName = orderData.product_name, Quantity = orderData.quantity });
                db.SaveChanges();
                return product.Id;
            }
            return -1; // prevent GET
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("api/order/get_checkout/{order_id}")]
        public float GetCheckout(int order_id)
        {
            var user = db.Users.Include(u => u.Orders.Select(s => s.ProductsInCart)).Where(s => s.UserName == User.Identity.Name).FirstOrDefault();
            var order = user.Orders.Where(orderObj => orderObj.Id == order_id).FirstOrDefault();
            if(order != null)
            {
                var sum = 0.0f;
                foreach(var product in order.ProductsInCart)
                {
                    sum += (float)(product.Price * product.Quantity);
                }
                sum *= user.PersonalDiscount;
                return sum;
            }
            return 0;
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.Route("api/order/remove_order/{item_id}")]
        public int RemoveOrder(int item_id)
        {
            int order_id = 1;
            var user = db.Users.Include(u => u.Orders.Select(s => s.ProductsInCart)).Where(s => s.UserName == User.Identity.Name).FirstOrDefault();
            var order = user.Orders.Where(orderObj => orderObj.Id == order_id).FirstOrDefault();
            if (order != null)
            {
                var product = order.ProductsInCart.FirstOrDefault(e => e.Id == item_id);
                order.ProductsInCart.Remove(product);
                db.SaveChanges();
            }
            return 0;
        }
    }
}