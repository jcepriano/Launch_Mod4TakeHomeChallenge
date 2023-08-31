using CommerceAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;
using CommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CommerceAPI.Controllers
{
    [Route("/api/merchants/{merchantId}/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly CommerceApiContext _context;

        public ProductsController(CommerceApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult GetProducts(int merchantId)
        {
            var merchantProducts = _context.Products
                .Where(p => p.Merchant.Id == merchantId)
                .Include(p => p.Merchant);


            return new JsonResult(merchantProducts);
        }

        [HttpGet("{id}")]
        public ActionResult GetProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            return new JsonResult(product);
        }

        [HttpPost]
        public ActionResult CreateProduct(int merchantId)
        {
            var merchant = _context.Merchants
                .Where(m => m.Id == merchantId)
                .Include(m => m.Products)
                .FirstOrDefault();

            var product = new Product();
            merchant.Products.Add(product);
            _context.SaveChanges();

            return new JsonResult(merchant);
        }
    }
}
