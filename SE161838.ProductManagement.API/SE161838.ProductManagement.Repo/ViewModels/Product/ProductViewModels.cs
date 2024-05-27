using SE161838.ProductManagement.Repo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SE161838.ProductManagement.Repo.ViewModels.Product
{
    public class ProductViewModels
    {
        public int ProductId { get; set; }

        public string CategoryName { get; set; }

        public string ProductName { get; set; } = null!;

        public string Weight { get; set; } = null!;

        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }
    }
    public class ProductCreateModels
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public string ProductName { get; set; } = null!;

        public string Weight { get; set; } = null!;

        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }
    }
    public class ProductUpdateModels
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public string ProductName { get; set; } = null!;

        public string Weight { get; set; } = null!;

        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }
    }
    public class ProductSearchViewModel
    {
        public string? name { get; set; }
        public int? categories_id { get; set; }
        public decimal? unit_price_min { get; set; }
        public decimal? unit_price_max { get; set; }
        public bool? sort_by_price { get; set; } = false;
        public bool? descending { get; set; } = false;
        public int? index_page { get; set; }
        public int? page_size { get; set; }
    }
}
