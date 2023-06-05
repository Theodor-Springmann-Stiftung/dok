using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusenalmAPI.Data;
using MusenalmAPI.Models;

namespace MusenalmAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class VocabularyController : ControllerBase
{
    private readonly MusenalmContext _DB;

    public VocabularyController(MusenalmContext DB) {
        _DB = DB;
    }

    [HttpGet]
    [Route("TVOC")]
    public IQueryable<TVOC> GetTVOC()
    {
        return _DB.Set<TVOC>();
    }

    [HttpGet]
    [Route("FVOC")]
    public IQueryable<FVOC> GetFVOC()
    {
        return  _DB.Set<FVOC>();
    }
}
