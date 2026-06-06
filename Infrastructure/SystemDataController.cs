using Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[ApiController]
[Route("api/system-data")]
[AllowAnonymous]
public class SystemDataController : ControllerBase
{
    private readonly UniversityDbContext _context;

    public SystemDataController(UniversityDbContext context)
    {
        _context = context;
    }

    [HttpGet("courses")]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _context.Courses
            .Select(c => new { c.Id, c.Name, c.EctsCredits })
            .ToListAsync();
            
        return Ok(courses);
    }

    [HttpGet("lecturers")]
    public async Task<IActionResult> GetLecturers()
    {
        var lecturers = await _context.Lecturers
            .Select(l => new { l.Id, l.Title, l.FirstName, l.LastName })
            .ToListAsync();
            
        return Ok(lecturers);
    }
}