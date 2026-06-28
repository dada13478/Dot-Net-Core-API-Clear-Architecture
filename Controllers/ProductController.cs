using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    // 注入 Service，不再直接注入 DbContext
    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.GetAll();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetById(id);
        if (product == null)
            return NotFound("找不到商品");
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        var created = await _service.Create(product);
        return Created($"/api/product/{created.Id}", created);
    }

    [HttpPut("{id}/{test}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        var result = await _service.Update(id, product);
        if (!result)
            return NotFound("找不到商品");
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.Delete(id);
        if (!result)
            return NotFound("找不到商品");
        return NoContent();
    }
}