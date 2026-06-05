using CoreApp.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.EntityFramework.Entities;

public class AppUser:  IdentityUser, ISystemUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    
    // Zgodnie z prośbą, FullName jest właściwością z setterem.
    // W praktyce często stosuje się tu właściwość obliczeniową: public string FullName => $"{FirstName} {LastName}";
    public required string FullName { get; set; }
    
    public required string Department { get; set; }
    public required SystemUserStatus Status { get; set; }
    public DateTime CreatedAt { get; set;  }
    public DateTime? LastLoginAt { get; private set; }
    public DateTime? DeactivatedAt { get; private set; }
    
    public void Activate()
    {
        // Zmiana statusu tylko gdy użytkownik jest nieaktywny
        if (Status == SystemUserStatus.Inactive)
        {
            Status = SystemUserStatus.Active;
        }
    }

    public void Deactivate(DateTime now)
    {
        // Zmiana statusu tylko gdy użytkownik jest aktywny
        // oraz aktualizacja daty deaktywacji
        if (Status == SystemUserStatus.Active)
        {
            Status = SystemUserStatus.Inactive;
            DeactivatedAt = now;
        }
    }
}