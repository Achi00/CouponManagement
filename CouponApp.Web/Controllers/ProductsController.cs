using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CouponApp.Application.DTOs.Product;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Web.Models;

namespace CouponApp.Web.Controllers
{
    //public class ProductsController : Controller
    //{
    //    private readonly IProductservice _productservice;

    //    public ProductsController(IProductservice productservice)
    //    {
    //        _productservice = productservice;
    //    }
    //    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    //    {
    //        var products = await _productservice.GetAllAsync(cancellationToken);
    //        var productView = products.Adapt<List<DisplayProductViewModel>>();
    //        return View(productView);
    //    }

    //    [Authorize(Roles = "Admin")]
    //    [HttpGet]
    //    public IActionResult Create()
    //    {
    //        return View();
    //    }

    //    [Authorize(Roles = "Admin")]
    //    [HttpPost]
    //    public async Task<IActionResult> Create(CreateProductViewModel model, CancellationToken cancellationToken = default)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return View(model);
    //        }

    //        var dto = model.Adapt<CreateProductRequest>();

    //        await _productservice.CreateAsync(dto, cancellationToken);
    //        return RedirectToAction("Index");
    //    }

    //    // update product
    //    [Authorize(Roles = "Admin")]
    //    [HttpGet]
    //    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken = default)
    //    {
    //        if (id <= 0)
    //        {
    //            return NotFound();
    //        }

    //        var product = await _productservice.GetByIdAsync(id, cancellationToken);

    //        if (product == null)
    //        {
    //            return NotFound();
    //        }
    //        var productViewModel = product.Adapt<UpdateProductViewModel>();
    //        return View(productViewModel);
    //    }

    //    [Authorize(Roles = "Admin")]
    //    [HttpPost]
    //    public async Task<IActionResult> Edit(int id, UpdateProductViewModel model, CancellationToken cancellationToken = default)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return View(model);
    //        }

    //        var dto = model.Adapt<UpdateProductRequest>();
    //        await _productservice.UpdateAsync(id, dto, cancellationToken);
            
    //        return RedirectToAction("Index");
    //    }

    //    [Authorize(Roles = "Admin")]
    //    [HttpPost]
    //    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    //    {
    //        if (id <= 0)
    //        {
    //            return NotFound();
    //        }
    //        await _productservice.DeleteAsync(id, cancellationToken);

    //        return RedirectToAction("Index");
    //    }

    //    [Authorize]
    //    [HttpGet]
    //    public async Task<IActionResult> Details([FromQuery] int id, CancellationToken cancellationToken = default)
    //    {
    //        if (id <= 0)
    //        {
    //            return NotFound();
    //        }
    //        var product = await _productservice.GetByIdAsync(id, CancellationToken.None);

    //        if (product == null)
    //        {
    //            return NotFound();
    //        }

    //        var viewModel = product.Adapt<DisplayProductViewModel>();

    //        return View(viewModel);
    //    }
    //}
}
