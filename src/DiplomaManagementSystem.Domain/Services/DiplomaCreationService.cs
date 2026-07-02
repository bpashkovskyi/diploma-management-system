using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;

namespace DiplomaManagementSystem.Domain.Services;

public sealed class DiplomaCreationService
{
    public IReadOnlyList<Diploma> CreateForStudents(
        IEnumerable<DiplomaStudentCandidate> students,
        Guid defenceSessionId,
        IReadOnlySet<Guid> existingStudentIdsInSession)
    {
        ArgumentNullException.ThrowIfNull(students);
        ArgumentNullException.ThrowIfNull(existingStudentIdsInSession);

        List<Diploma> diplomas = [];
        DateTimeOffset now = DateTimeOffset.UtcNow;

        foreach (DiplomaStudentCandidate student in students)
        {
            if (student.UserKind != UserKind.Student)
            {
                throw new DomainException($"User {student.StudentId} is not a student.");
            }

            if (existingStudentIdsInSession.Contains(student.StudentId))
            {
                continue;
            }

            diplomas.Add(new Diploma
            {
                Id = Guid.NewGuid(),
                DefenceSessionId = defenceSessionId,
                StudentId = student.StudentId,
                SupervisorAssignmentStatus = SupervisorAssignmentStatus.Pending,
                ReviewAssignmentStatus = ReviewAssignmentStatus.NotAssigned,
                LifecycleStatus = DiplomaLifecycleStatus.AwaitingSupervisor,
                AdmissionStatus = DiplomaAdmissionStatus.NotAdmitted,
                CreatedAt = now,
                UpdatedAt = now,
            });
        }

        return diplomas;
    }
}
