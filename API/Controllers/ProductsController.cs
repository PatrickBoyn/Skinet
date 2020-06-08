using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo, 
                                  IGenericRepository<ProductBrand> productsBrandRepo,
                                  IGenericRepository<ProductType> productsTypeRepo,
                                  IMapper mapper)
        {
            _productsRepo = productsRepo;
            _productsBrandRepo = productsBrandRepo;
            _productsTypeRepo = productsTypeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProductsAsync()
        {
            
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            var spec = new ProductsWithTypesAndBrandsSpecification();
            
            IReadOnlyList<Product> products = await _productsRepo.ListAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            
            Product product = await _productsRepo.GetEntityWithSpec(spec);

            return _mapper.Map<Product, ProductToReturnDto>(product);
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