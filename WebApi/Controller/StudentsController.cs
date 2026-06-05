using CoreApp.Dto;
using CoreApp.Services;
using CoreApp.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("/api/students")]
public class StudentsController(IStudentService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllStudents([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        return Ok(await service.FindAllStudentsPaged(page, size));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetStudent(Guid id)
    {
        var dto = await service.FindStudentByIdAsync(id);
        if (dto is null)
            return NotFound();

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(StudentCreateDto dto)
    {
        var result = await service.AddStudent(dto);
        return CreatedAtAction(nameof(GetStudent), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, StudentUpdateDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("ID w adresie URL nie zgadza się z ID w ciele żądania.");
        }

        var result = await service.UpdateStudentAsync(id, dto);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var wasDeleted = await service.DeleteStudentAsync(id);
        if (!wasDeleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{studentId:guid}/grades")]
    [ProducesResponseType(typeof(GradeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddGrade(Guid studentId, [FromBody] GradeCreateDto dto)
    {
        var gradeDto = await service.AddGradeAsync(studentId, dto);
        return CreatedAtAction(nameof(GetGrades), new { studentId }, gradeDto);
    }

    [HttpGet("{studentId:guid}/grades")]
    [ProducesResponseType(typeof(IEnumerable<GradeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGrades([FromRoute] Guid studentId)
    {
        var grades = await service.GetGradesAsync(studentId);
        return Ok(grades);
    }

    [HttpPut("{studentId:guid}/grades/{gradeId:guid}")]
    [ProducesResponseType(typeof(GradeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGrade(Guid studentId, Guid gradeId, [FromBody] GradeUpdateDto dto)
    {
        var updatedGrade = await service.UpdateGradeAsync(studentId, gradeId, dto);
        return Ok(updatedGrade);
    }
}
