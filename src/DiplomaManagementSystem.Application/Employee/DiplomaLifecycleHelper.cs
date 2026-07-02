using DiplomaManagementSystem.Application.Persistence.Contracts;
using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Services;

namespace DiplomaManagementSystem.Application.Employee;

internal static class DiplomaLifecycleHelper
{
    public static async Task RecalculateAsync(
        IAdmissionStepQueries admissionStepQueries,
        ITopicVersionQueries topicVersionQueries,
        DiplomaLifecycleService diplomaLifecycleService,
        Diploma diploma,
        CancellationToken cancellationToken)
    {
        DiplomaTopicVersion? latestTopic = await topicVersionQueries.GetLatestAsync(diploma.Id, cancellationToken);

        List<DiplomaAdmissionStepAttempt> attempts = diploma.AdmissionStepAttempts.Count > 0
            ? [.. diploma.AdmissionStepAttempts]
            : await admissionStepQueries.ListForDiplomaAsync(diploma.Id, cancellationToken);

        diploma.LifecycleStatus = diplomaLifecycleService.Recalculate(diploma, latestTopic, attempts);
        diploma.UpdatedAt = DateTimeOffset.UtcNow;
    }
}
