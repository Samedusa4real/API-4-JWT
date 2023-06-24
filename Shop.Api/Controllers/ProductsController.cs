using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Dtos.ProductDtos;
using Shop.Core.Entities;
using Shop.Core.Repositories;

namespace Shop.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IBrandRepository brandRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        [HttpPost("")]
        public IActionResult Create(ProductPostDto productPostDto)
        {
            if (_brandRepository.IsExist(x=>x.Id == productPostDto.BrandId))
            {
                ModelState.AddModelError("BrandId", "BrandId is not correct");
                return BadRequest(ModelState);
            }

            Product product = _mapper.Map<Product>(productPostDto);

            _productRepository.Add(product);
            _productRepository.Commit();

            return StatusCode(201, new { product.Id });
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, ProductPutDto productPutDto)
        {
            Product product = _productRepository.Get(x=>x.Id == id);

            if (product == null)
                return NotFound();

            if (_brandRepository.IsExist(x => x.Id == productPutDto.BrandId))
            {
                ModelState.AddModelError("BrandId", "BrandId is not correct");
                return BadRequest(ModelState);
            }

            product.Name = productPutDto.Name;
            product.Price = productPutDto.Price;
            product.DiscountPercent = productPutDto.DiscountPercent;
            product.BrandId = productPutDto.BrandId;

            _productRepository.Commit();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product product = _productRepository.Get(x => x.Id == id);

            if (product == null)
                return NotFound();

            _productRepository.Remove(product);
            _productRepository.Commit();

            return NoContent();
        }

        [HttpGet("all")]
        public ActionResult<List<ProductGetAllItem>> GetAll()
        {
            var data = _mapper.Map<List<ProductGetAllItem>>(_productRepository.GetAll(x=>true,"Brand"));
            return Ok(data);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductGetDto> Get(int id)
        {
            var data = _productRepository.Get(x=>x.Id == id);

            if (data == null)
                return NotFound();

            var productDto = _mapper.Map<ProductGetDto>(data);

            return Ok(productDto);
        }
    }
}
