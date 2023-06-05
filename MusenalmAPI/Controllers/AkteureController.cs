using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusenalmAPI.Data;
using MusenalmAPI.Models;

namespace MusenalmAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AkteureController : ControllerBase
{
    private readonly MusenalmContext _DB;

    public AkteureController(MusenalmContext DB) {
        _DB = DB;
    }

    [HttpGet]
    public async Task<IList<Akteur>> OnGetAsync()
    {
        return await _DB.Akteure
            .ToListAsync();
    }
}
