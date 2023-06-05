using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusenalmAPI.Data;
using MusenalmAPI.Models;

namespace MusenalmAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ReihenController : ControllerBase
{
    private readonly MusenalmContext _DB;

    public ReihenController(MusenalmContext DB) {
        _DB = DB;
    }

    [HttpGet]
    public async Task<IList<Reihe>> OnGetAsync()
    {
        return await _DB.Reihen
            .Include(x => x.REL_Baende_Reihen)
            .ToListAsync();
    }
}
