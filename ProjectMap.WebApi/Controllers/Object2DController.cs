using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMap.WebApi.Interfaces;
using ProjectMap.WebApi.Models;

namespace ProjectMap.WebApi.Controllers;

[ApiController]
[Route("Object2D")]
[Authorize] // Alle methodes in deze controller vereisen authenticatie, tenzij anders aangegeven
public class Object2DController : ControllerBase
{
    private readonly IObject2DRepository _object2DRepository;
    private readonly ILogger<Object2DController> _logger;

    public Object2DController(IObject2DRepository object2DRepository, ILogger<Object2DController> logger)
    {
        _object2DRepository = object2DRepository;
        _logger = logger;
    }

    [HttpGet(Name = "Read2DObjects")]
    public async Task<ActionResult<IEnumerable<Object2D>>> Get()
    {
        var object2Ds = await _object2DRepository.ReadAsync();
        return Ok(object2Ds);
    }

    [HttpGet("{object2DId}", Name = "Read2DObject")]
    public async Task<ActionResult<Object2D>> Get(Guid object2DId)
    {
        var object2D = await _object2DRepository.ReadAsync(object2DId);
        if (object2D == null)
            return NotFound();

        return Ok(object2D);
    }

    [HttpPost(Name = "CreateObject2D")] // Alleen ingelogde gebruikers kunnen objecten aanmaken
    public async Task<ActionResult> Add(Object2D object2D)
    {
        object2D.Id = Guid.NewGuid();

        var createdObject2D = await _object2DRepository.InsertAsync(object2D);
        return Created();
    }

    [HttpPut("{object2DId}", Name = "UpdateObject2D")] // Alleen ingelogde gebruikers kunnen objecten updaten
    public async Task<ActionResult> Update(Guid object2DId, Object2D newObject2D)
    {
        var existingObject2D = await _object2DRepository.ReadAsync(object2DId);

        if (existingObject2D == null)
            return NotFound();

        await _object2DRepository.UpdateAsync(newObject2D);

        return Ok(newObject2D);
    }

    [HttpDelete("{object2DId}", Name = "DeleteObject2DByGuid")] // Alleen ingelogde gebruikers kunnen objecten verwijderen
    public async Task<IActionResult> Delete(Guid object2DId) // Naam aangepast van "Update" naar "Delete"
    {
        var existingObject2D = await _object2DRepository.ReadAsync(object2DId);

        if (existingObject2D == null)
            return NotFound();

        await _object2DRepository.DeleteAsync(object2DId);

        return Ok();
    }
}
