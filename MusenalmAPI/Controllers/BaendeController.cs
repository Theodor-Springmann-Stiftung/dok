using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusenalmAPI.Data;
using MusenalmAPI.Models;

namespace MusenalmAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BaendeController : ControllerBase
{
    private readonly MusenalmContext _DB;
    public IList<Band> Baende { get;set; }

    public BaendeController(MusenalmContext DB) {
        _DB = DB;
    }

    [HttpGet]
    public async Task<IList<Band>> OnGetAsync()
    {
        return await _DB.Baende.ToListAsync();
    }
}
