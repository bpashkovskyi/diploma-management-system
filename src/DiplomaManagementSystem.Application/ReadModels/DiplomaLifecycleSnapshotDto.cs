using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.ReadModels;

public sealed record DiplomaLifecycleSnapshotDto(
    DiplomaLifecycleStatus LifecycleStatus,
    DiplomaAdmissionStatus AdmissionStatus,
    AdmissionStep? CurrentAdmissionStep,
    DateOnly? DefenceDate);
