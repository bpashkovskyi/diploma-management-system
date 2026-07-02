using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;

namespace DiplomaManagementSystem.Domain.Services;

public sealed class DiplomaAdmissionService
{
    public void Admit(
        Diploma diploma,
        DefenceSession defenceSession,
        DateOnly defenceDate,
        DiplomaLifecycleStatus currentLifecycleStatus)
    {
        ArgumentNullException.ThrowIfNull(diploma);
        ArgumentNullException.ThrowIfNull(defenceSession);

        if (defenceSession.Status == DefenceSessionStatus.Archived)
        {
            throw new DomainException("Defence session is archived.");
        }

        if (diploma.AdmissionStatus == DiplomaAdmissionStatus.Admitted)
        {
            throw new DomainException("Diploma is already admitted.");
        }

        if (currentLifecycleStatus != DiplomaLifecycleStatus.ReadyForAdmission)
        {
            throw new DomainException("Diploma is not ready for admission.");
        }

        diploma.AdmissionStatus = DiplomaAdmissionStatus.Admitted;
        diploma.DefenceDate = defenceDate;
        diploma.UpdatedAt = DateTimeOffset.UtcNow;
    }
}
