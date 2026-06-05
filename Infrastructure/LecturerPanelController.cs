using CoreApp.Authorization;
using CoreApp.Dto;
using CoreApp.Entities;
using CoreApp.Enums;
using CoreApp.Interfaces;
using Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("api/lecturer-panel")]
[Authorize(Policy = nameof(AppPolicies.CanManageGrades))]
public class LecturerPanelController : ControllerBase
{
    private readonly UniversityDbContext _context;

    public LecturerPanelController(UniversityDbContext context)
    {
        _context = context;
    }

    /// <summary>Pobiera listę studentów zapisanych na dany kurs (posiadających jakiekolwiek oceny na tym kursie).</summary>
    [HttpGet("courses/{courseId}/students")]
    public async Task<IActionResult> GetStudentsInCourse(Guid courseId)
    {
        var students = await _context.Grades
            .Include(g => g.Student)
            .Where(g => g.CourseId == courseId)
            .Select(g => g.Student!)
            .Distinct()
            .Select(s => new { s.Id, s.FirstName, s.LastName, s.Email }) // Zwracamy uproszczone DTO dla wygody widoku
            .ToListAsync();

        return Ok(students);
    }

    /// <summary>Dodawanie nowej oceny z zapisem w historii.</summary>
    [HttpPost("students/{studentId}/grades")]
    public async Task<IActionResult> AddGrade(Guid studentId, [FromBody] GradeCreateDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        // Upewniamy się, że prowadzący wystawia ocenę z własnego ramienia (chyba że to pracownik dziekanatu)
        if (!User.IsInRole(UserRole.DeaneryWorker.ToString()) && !User.IsInRole(UserRole.Administrator.ToString()))
        {
            if (userId != dto.LecturerId.ToString())
                return Forbid("Wykładowca może wystawiać oceny tylko we własnym imieniu.");
        }

        var grade = new Grade
        {
            Id = Guid.NewGuid(),
            CourseId = dto.CourseId,
            LecturerId = dto.LecturerId,
            GradeValue = GradeValueExtensions.FromDouble(dto.GradeValue),
            DateOfIssue = dto.DateOfIssue,
        };

        // Przypisanie ID studenta
        grade.StudentId = studentId;
        _context.Grades.Add(grade);

        // Rejestrowanie w historii
        _context.GradeHistories.Add(new GradeHistory
        {
            GradeId = grade.Id,
            OldValue = 0, // 0 oznacza brak wcześniejszej oceny
            NewValue = dto.GradeValue,
            ChangedByUserId = userId,
            Action = "Created"
        });

        await _context.SaveChangesAsync();
        return Ok(new { GradeId = grade.Id });
    }

    /// <summary>Edycja istniejącej oceny z zapisem w historii.</summary>
    [HttpPut("grades/{gradeId}")]
    public async Task<IActionResult> UpdateGrade(Guid gradeId, [FromBody] GradeUpdateDto dto)
    {
        var grade = await _context.Grades.FindAsync(gradeId);
        if (grade == null) return NotFound("Nie znaleziono oceny.");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var oldValue = grade.GradeValue.Value();

        // Aktualizacja rekordu oceny
        grade.GradeValue = GradeValueExtensions.FromDouble(dto.GradeValue);
        grade.DateOfIssue = dto.DateOfIssue;
        grade.Type = dto.Type;

        // Zapis w logu historycznym
        _context.GradeHistories.Add(new GradeHistory { GradeId = grade.Id, OldValue = oldValue, NewValue = dto.GradeValue, ChangedByUserId = userId, Action = "Updated" });

        await _context.SaveChangesAsync();
        return NoContent();
    }
}