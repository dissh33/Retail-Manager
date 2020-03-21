using System.Collections.Generic;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccessLogic
{
    public interface IProductData
    {
        List<ProductModel> GetProducts();
        ProductModel GetProductById(int productId);
    }
}