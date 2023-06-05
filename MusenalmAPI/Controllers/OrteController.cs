using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusenalmAPI.Data;
using MusenalmAPI.Models;

namespace MusenalmAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class OrteController : ControllerBase
{
    private readonly MusenalmContext _DB;

    public OrteController(MusenalmContext DB) {
        _DB = DB;
    }

    [HttpGet]
    public async Task<IList<Ort>> OnGetAsync()
    {
        return await _DB.Orte
            .ToListAsync();
    }
}
