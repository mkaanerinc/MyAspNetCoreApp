using System.Xml.Linq;

namespace MyAspNetCoreApp.Web.Models
{
    public class ProductRepository
    {
        private static List<Product> _products = new List<Product>()
        {
            new Product()
            {
                Id = 1,
                Name = "Kalem",
                Price = 30,
                Stock = 10
            },
            new Product()
            {
                Id = 2,
                Name = "Defter",
                Price = 30,
                Stock = 10
            },
            new Product()
            {
                Id = 3,
                Name = "Silgi",
                Price = 30,
                 Stock = 10
            },
        };

        public List<Product> GetAll()
        {
            return _products;
        }

        public void Add(Product product) 
        {
            _products.Add(product);
        }

        public void Remove(int id)
        {
            var hasProduct = _products.FirstOrDefault(x => x.Id == id);

            if(hasProduct == null)
            {
                throw new Exception($"Bu id({id})'ye sahip ürün bulunmamaktadır.");
            }

            _products.Remove(hasProduct);
           
        }

        public void Update(Product productToUpdate)
        {
            var hasProduct = _products.FirstOrDefault(x => x.Id == productToUpdate.Id);

            if (hasProduct == null)
            {
                throw new Exception($"Bu id({productToUpdate.Id})'ye sahip ürün bulunmamaktadır.");
            }
            
             hasProduct.Name = productToUpdate.Name;
             hasProduct.Price = productToUpdate.Price;
             hasProduct.Stock = productToUpdate.Stock;

             var index = _products.FindIndex(x => x.Id == productToUpdate.Id);

             _products[index] = hasProduct;        
        }
    }
}
