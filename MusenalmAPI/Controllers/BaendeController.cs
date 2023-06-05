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

    public BaendeController(MusenalmContext DB) {
        _DB = DB;
    }

    [HttpGet]
    public async Task<IList<Band>> OnGetAsync()
    {
        return await _DB.Baende
            .ToListAsync();
    }

    [HttpGet]
    [Route("/[controller]/{id?}")]
    public ActionResult<Band> OnGetID(long id) {
        var band = _DB.Baende
            .Include(x => x.Akteure)
            .Include(x => x.Reihen)
            .Include(x => x.Orte)
            .SingleOrDefault(x => x.ID == id);
        if (band == null) return NotFound();
        return Ok(band);
        // return Ok(new ResultModel() {
        //     ID = id,
        //     Band = band,
        //     // Akteure = band.Akteure,
        //     // Orte = band.Orte,
        //     // Reihen = band.Reihen
        // });
    }
}


public class ResultModel {
    public long ID;
    public Band Band;
    // public ICollection<REL_Band_Akteur> Akteure;
    // public ICollection<REL_Band_Ort> Orte;
    // public ICollection<REL_Band_Reihe> Reihen;
}