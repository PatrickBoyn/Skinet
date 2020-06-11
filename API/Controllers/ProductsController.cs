using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
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
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProductsAsync([FromQuery]ProductSpecParams productParams)
        {
            
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            int totalItems = await _productsRepo.CountAsync(countSpec);
            
            IReadOnlyList<Product> products = await _productsRepo.ListAsync(spec);

            IReadOnlyList<ProductToReturnDto> data =
                _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            
            
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageSize, productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            
            Product product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));
            
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