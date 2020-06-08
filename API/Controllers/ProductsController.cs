using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult<List<Product>>> GetProductsAsync()
        {
            
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            var spec = new ProductsWithTypesAndBrandsSpecification();
            
            IReadOnlyList<Product> products = await _productsRepo.ListAsync(spec);

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            Product product = await _productsRepo.GetByIdAsync(id);

            return Ok(product);
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