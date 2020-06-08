using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productsBrandRepo;
        private readonly IGenericRepository<ProductType> _productsTypeRepo;

        public ProductsController(IGenericRepository<Product> productsRepo, 
                                  IGenericRepository<ProductBrand> productsBrandRepo,
                                  IGenericRepository<ProductType> productsTypeRepo)
        {
            _productsRepo = productsRepo;
            _productsBrandRepo = productsBrandRepo;
            _productsTypeRepo = productsTypeRepo;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductToReturnDto>>> GetProductsAsync()
        {
            
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            var spec = new ProductsWithTypesAndBrandsSpecification();
            
            IReadOnlyList<Product> products = await _productsRepo.ListAsync(spec);

            return products.Select(product => new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price =  product.Price,
                ProductBrand =  product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            }).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            
            Product product = await _productsRepo.GetEntityWithSpec(spec);

            return new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price =  product.Price,
                ProductBrand =  product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            };
        }

        [HttpGet("brands")]
        public async Task<ActionResult<ProductBrand>> GetBrands()
        {
            IReadOnlyList<ProductBrand> brands = await _productsBrandRepo.ListAllAsync();

            return Ok(brands);
        }


        [HttpGet("types")]
        public async Task<ActionResult<ProductType>> GetTypes()
        {
            IReadOnlyList<ProductType> types = await _productsTypeRepo.ListAllAsync();

            return Ok(types);
        }
    }

}