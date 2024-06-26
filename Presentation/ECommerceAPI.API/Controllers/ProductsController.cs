﻿using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Consts;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Features.Commands.ProductCommands.CreateProduct;
using ECommerceAPI.Application.Features.Commands.ProductCommands.DeleteProduct;
using ECommerceAPI.Application.Features.Commands.ProductCommands.UpdateProduct;
using ECommerceAPI.Application.Features.Commands.ProductImageFileCommands.ChangeShowcase;
using ECommerceAPI.Application.Features.Commands.ProductImageFileCommands.DeleteImage;
using ECommerceAPI.Application.Features.Commands.ProductImageFileCommands.UploadProductImage;
using ECommerceAPI.Application.Features.Queries.ProductImageFileQueries.GetImages;
using ECommerceAPI.Application.Features.Queries.ProductQueries.GetActiveUserProducts;
using ECommerceAPI.Application.Features.Queries.ProductQueries.GetAllProducts;
using ECommerceAPI.Application.Features.Queries.ProductQueries.GetByIdProduct;
using ECommerceAPI.Application.Features.Queries.ProductQueries.GetProductsByCategory;
using ECommerceAPI.Application.Features.Queries.ProductQueries.GetProductsWithPriceFilter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        readonly IProductService productService;
        readonly IMediator _mediator;
        public ProductsController(IMediator mediator, IProductService productService)
        {

            _mediator = mediator;
            this.productService = productService;
        }

        [HttpGet]
        
        public async Task<IActionResult> Get([FromQuery] GetAllProductsQueryRequest getAllProductsQueryRequest)
        {
            GetAllProductsQueryResponse response = await _mediator.Send(getAllProductsQueryRequest);
            return Ok(response);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
            GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response.Product);
        }
        [HttpGet("active-users-products")]
        [Authorize(AuthenticationSchemes = "Admin")]
        
        public async Task<IActionResult> GetActiveUsersProducts([FromQuery]GetActiveUsersProductsCommandRequest request)
        {
            GetActiveUsersProductsCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }       
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut("update-product")]
        [Authorize(AuthenticationSchemes = "Admin")]
      
        public async Task<IActionResult> Put(UpdateProductCommandRequest request)
        {
            UpdateProductCommandResponse response = await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        
        public async Task<IActionResult> Delete([FromRoute]DeleteProductCommandRequest request)
        {
            DeleteProductCommandResponse response = await _mediator.Send(request);
            return Ok();
        }
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, Definition = "Upload a product image", Action = ActionType.Writing)]

        public async Task<IActionResult> Upload([FromQuery]UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            UploadProductImageCommandResponse response = await _mediator.Send(uploadProductImageCommandRequest);

            return Ok(response);
        }

        [HttpGet("[action]/{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, Definition = "Get product images", Action = ActionType.Reading)]
        public async Task<IActionResult> GetImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
        {
           List<GetProductImagesQueryResponse> response = await _mediator.Send(getProductImagesQueryRequest);
            return Ok(response);
        }

        [HttpDelete("[action]/{ProductId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, Definition = "Remove product image", Action = ActionType.Deleting)]
        public async Task<IActionResult> DeleteImage([FromRoute]DeleteImageCommandRequest deleteImageCommandRequest, [FromQuery] string imageId)
        {
            deleteImageCommandRequest.ImageId = imageId;
            DeleteImageCommandResponse response = await _mediator.Send(deleteImageCommandRequest);
            return Ok();
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, Definition = "Change a showcase of a product", Action = ActionType.Updating)]
        public async Task<IActionResult> ChangeShowcase([FromQuery]ChangeShowcaseCommandRequest request)
        {
            ChangeShowcaseCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("get-product-by-category")]
        public async Task<IActionResult> GetProductsByCategory([FromQuery]GetProductsByCategoryQueryRequest request)
        {
            GetProductsByCategoryQueryResponse response =  await _mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("get-product-with-filter")]
        public async Task<IActionResult> GetProductWithPriceFilter([FromQuery]GetProductsWithPriceFilterQueryRequest request)
        {
            GetProductsWithPriceFilterQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
