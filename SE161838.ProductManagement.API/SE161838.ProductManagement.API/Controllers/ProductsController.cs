using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SE161838.ProductManagement.Repo.Interface;
using SE161838.ProductManagement.Repo.Models;
using SE161838.ProductManagement.Repo.Repository;
using SE161838.ProductManagement.Repo.ResponeModel;
using SE161838.ProductManagement.Repo.ResponeModels;
using SE161838.ProductManagement.Repo.ViewModels.Product;
using System.Linq.Expressions;

namespace SE161838.ProductManagement.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private int? page_size = 10;
        private int? index_page = 1;
        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModels>>> GetProducts([FromQuery] ProductSearchViewModel search)
        {
            if (_unitOfWork.ProductsRepository == null)
            {
                return NotFound(new FailedResponseModel()
                {
                    Status = NotFound().StatusCode,
                    Message = "List is not found !"
                });
            }
            if (search.page_size != null)
            {
                page_size = search.page_size;
            }
            if (search.index_page != null)
            {
                index_page = search.index_page;
            }

            Expression<Func<Product, bool>> filter = null;
            if (search. categories_id.HasValue)
            {
                filter = p => p.CategoryId == search. categories_id.Value;
            }
            if (search.name != null)
            {
                filter = filter.And(p => p.ProductName.Contains(search.name));
            }
            if (search. unit_price_min.HasValue || search. unit_price_max.HasValue)
            {
                if (filter == null)
                {
                    filter = p => true;
                }
                if (search. unit_price_min.HasValue)
                {
                    filter = filter.And(p => p.UnitPrice >= search. unit_price_min);
                }
                if (search. unit_price_max.HasValue)
                {
                    filter = filter.And(p => p.UnitPrice <= search. unit_price_max);
                }
            }

            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null;

            if (search.sort_by_price == true && search.descending == true)
            {

                orderBy = p => p.OrderByDescending(p => p.UnitPrice);
            }
            else if (search.sort_by_price == true && search.descending == false)
            {
                orderBy = p => p.OrderBy(p => p.UnitPrice);
            }
            else if (search.sort_by_price == false && search.descending == true)
            {
                orderBy = p => p.OrderByDescending(p => p.ProductName);
            }
            else
            {
                orderBy = p => p.OrderBy(p => p.ProductName);
            }

            string includeProperties = "Category";
            var list = _unitOfWork.ProductsRepository.Get(filter, orderBy, includeProperties, index_page, page_size).ToList();
            var result = _mapper.Map<IEnumerable<ProductViewModels>>(list);
            foreach (var item in result)
            {
                item.CategoryName =list.FirstOrDefault(p=>p.ProductId == item.ProductId).Category.CategoryName;
            }
            return Ok(new ResponeModel()
            {
                Status = Ok().StatusCode,
                Message = "Get Product by Id Success",
                Result = result
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                if(_unitOfWork.ProductsRepository == null)
                {
                    return NotFound(new FailedResponseModel()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Product is not found !"
                    });
                }
                var product = await _unitOfWork.ProductsRepository.GetByIdAsync(id);
                if (product != null)
                {
                    var result = _mapper.Map<ProductViewModels>(product);
                    var cata = await _unitOfWork.CategorysRepository.GetByIdAsync(product.CategoryId.Value);
                    result.CategoryName = cata.CategoryName;
                    return Ok(new ResponeModel()
                    {
                        Status = Ok().StatusCode,
                        Message = "Get Product by Id Success",
                        Result = result
                    });
                    
                }
                return NotFound(new FailedResponseModel()
                {
                    Status = NotFound().StatusCode,
                    Message = "Product is not found !"
                });
            }
            catch (DirectoryNotFoundException ex)
            {
                return BadRequest(new FailedResponseModel()
                {
                    Status = NotFound().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(ProductCreateModels productViewModel)
        {
            try
            {
                var exitsProduct = _unitOfWork.ProductsRepository.GetByIdAsync(productViewModel.ProductId);
                if (exitsProduct != null)
                {
                    return BadRequest(new FailedResponseModel()
                    {
                        Status = BadRequest().StatusCode,
                        Message = "Product has exist with id " + productViewModel.ProductId
                    });
                }
                var notExitCatalogy = _unitOfWork.CategorysRepository.GetByIdAsync(productViewModel.CategoryId);
                if (notExitCatalogy == null) {
                    return NotFound(new FailedResponseModel()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Category has not exist with id " + productViewModel.CategoryId
                    });
                }
                var category = _mapper.Map<Product>(productViewModel);
                await _unitOfWork.ProductsRepository.AddAsync(category);
                _unitOfWork.Save();
                return Ok(new ResponeModel
                {
                    Status = Ok().StatusCode,
                    Message = "Add category Succeed",
                    Result = productViewModel
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryById(int Id, ProductUpdateModels productUpdate)
        {
            try
            {
                var exitProduct = await _unitOfWork.ProductsRepository.GetByIdAsync(Id);
                if (exitProduct == null)
                {
                    return BadRequest(new FailedResponseModel()
                    {
                        Status = BadRequest().StatusCode,
                        Message = "Product not exist with id " + Id
                    });
                }
                var notExitCategory = _unitOfWork.CategorysRepository.GetByIdAsync(productUpdate.CategoryId);
                if (notExitCategory == null)
                {
                    return NotFound(new FailedResponseModel()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Category not exist with id " + productUpdate.CategoryId
                    });
                }
                exitProduct.ProductName = productUpdate.ProductName;
                exitProduct.CategoryId = productUpdate.CategoryId;
                exitProduct.Weight = productUpdate.Weight;
                exitProduct.UnitPrice = productUpdate.UnitPrice;
                exitProduct.UnitsInStock = productUpdate.UnitsInStock;                
                _unitOfWork.ProductsRepository.Update(exitProduct);
                _unitOfWork.Save();
                return Ok(new ResponeModel
                {
                    Status = Ok().StatusCode,
                    Message = "Update product Success",
                    Result = productUpdate
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            try
            {
                var result = await _unitOfWork.ProductsRepository.GetByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new FailedResponseModel()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Product not exist with id " + id
                    });
                }
                _unitOfWork.ProductsRepository.Remove(result);
                _unitOfWork.Save();
                return Ok(new ResponeModel
                {
                    Status = Ok().StatusCode,
                    Message = "Delete product Success",
                });
            }
            catch (DirectoryNotFoundException ex)
            {
                return BadRequest(new FailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }
    }
}
