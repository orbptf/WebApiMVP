using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMap.WebApi.Interfaces;
using ProjectMap.WebApi.Models;

namespace ProjectMap.WebApi.Controllers;

[ApiController]
[Route("2DEnvironments")]
[Authorize] //mag alleen met login
public class Environment2DController : ControllerBase
{
    private readonly IEnvironment2DRepository _environment2DRepository;
    private readonly ILogger<Environment2DController> _logger;

    public Environment2DController(IEnvironment2DRepository environment2DRepository, ILogger<Environment2DController> logger)
    {
        _environment2DRepository = environment2DRepository;
        _logger = logger;
    }

    [HttpGet(Name = "Read2DEnvironments")]
    public async Task<ActionResult<IEnumerable<Environment2D>>> Get()
    {
        var environment2Ds = await _environment2DRepository.ReadAsync();
        return Ok(environment2Ds);
    }

    [HttpGet("{environment2DId}", Name = "Read2DEnvironment")]
    public async Task<ActionResult<Environment2D>> Get(Guid environment2DId)
    {
        var environment2D = await _environment2DRepository.ReadAsync(environment2DId);
        if (environment2D == null)
            return NotFound();

        return Ok(environment2D);
    }

    [HttpPost(Name = "Create2Denvironment")]
    public async Task<ActionResult> Add(Environment2D environment2D)
    {
        int minimalNumberOfCharacters = 1;
        int maximumNumberOfCharacters = 25;
        int maximumAmountOfWorlds = 5;

        List<Environment2D> userWorlds = await _environment2DRepository.ReadByUserMailAsync(environment2D.usermail) ?? new List<Environment2D>();

        if (userWorlds.Count() >= maximumAmountOfWorlds)
        {
            return BadRequest("Je kunt niet meer dan 5 werelden aanmaken");
        }

        if (environment2D.Name.Length < 1 || environment2D.Name.Length > 25)
        {
            return BadRequest($"De naam {environment2D.Name} moet minmaal {minimalNumberOfCharacters} en maximaal {maximumNumberOfCharacters} bevatten.");
        }

        environment2D.Id = Guid.NewGuid();
        
        var createdEnvironment2D = await _environment2DRepository.InsertAsync(environment2D);
        return Created();
    }

    [HttpPut("{environment2DId}", Name = "Update2DEnvironment")]
    public async Task<ActionResult> Update(Guid environment2DId, Environment2D newEnvironment2D)
    {
        var existingEnvironment2D = await _environment2DRepository.ReadAsync(environment2DId);

        if (existingEnvironment2D == null)
            return NotFound();

        await _environment2DRepository.UpdateAsync(newEnvironment2D);

        return Ok(newEnvironment2D);
    }

    [HttpDelete("{environment2DId}", Name = "Delete2DEnvironmentByGuid")]
    public async Task<IActionResult> Delete(Guid environment2DId)
    {
        var existingEnvironment2D = await _environment2DRepository.ReadAsync(environment2DId);

        if (existingEnvironment2D == null)
            return NotFound();

        await _environment2DRepository.DeleteAsync(environment2DId);

        return Ok();
    }

}