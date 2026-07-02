# Test progress

Оновлюється при імплементації unit-тестів. Деталі кейсів — [test-cases.md](./test-cases.md).

**Останнє оновлення:** 2026-07-01 (хвиля 18)

## Зведення (unit)

| Модуль | xUnit тестів | Статус |
|--------|--------------|--------|
| Domain | 148 | ✅ |
| Application | 306 | ✅ |
| Web | 50 | ✅ |
| **Разом** | **504** | ✅ |

`dotnet test tests/DiplomaManagementSystem.Domain.Tests tests/DiplomaManagementSystem.Application.Tests tests/DiplomaManagementSystem.Web.Tests` — **504 passed**.

## Зведення (integration)

| | |
|--|--|
| Тестів | **111** |
| БД | Testcontainers локально / `DIPLOMA_INTEGRATION_PG` у CI |
| CI job | `integration-test` (PostgreSQL service) |

`dotnet test tests/DiplomaManagementSystem.Integration.Tests` — **111 passed** (Docker / Testcontainers або `DIPLOMA_INTEGRATION_PG`).

## Хвиля 1 ✅

Domain gaps, Authorization, Guidance, Helpers, FluentValidation.

## Хвиля 2 ✅

StudentWorkflowProgress (006–012), AnnualRoleService, ImportResultComposer, Admin form validators.

## Хвиля 3 ✅

| Область | Файли |
|---------|--------|
| Fixtures | `Mapping/MapperTestFixtures.cs` |
| Secretary / Student details | `SecretaryDiplomaDetailsMapperTests`, `StudentDiplomaViewModelMapperTests` |
| List / Dashboard / Reports | `SecretaryListAndDashboardMapperTests` |
| Workflow / Documents / TopicHistory | `WorkflowAndDocumentMapperTests` |
| Employee / Admin sessions | `EmployeeAndAdminMapperTests` |
| Upload / Checkpoint | `Storage/UploadAndCheckpointHelperTests`, `Support/FakeFormFile` |
| Flash messages | `Admin/AdminFlashMessagesTests`, `Support/InMemoryTempDataProvider` |

**+31 тест** (293 → 324).

## Хвиля 4 ✅ (integration)

| Область | Зміни |
|---------|--------|
| Fixture | `DIPLOMA_INTEGRATION_PG` для CI; унікальні email у seed |
| Runner | upload work, `RunUpToTopicSubmitted`, checkpoint prep |
| Негативні сценарії | topic reject, checkpoint reject, wrong role, assign без topic |
| Audit | `SecretaryOverrideAuditScenarioTests` |
| Read model | `MyDiplomaReadScenarioTests` |
| CI | job `integration-test` з PostgreSQL service |
| App fix | EF tracking для `AdmissionStepAttempts` при повторному завантаженні diploma |

**+6 integration-тестів** (8 → 14). Усі 14 проходять локально з Docker Desktop.

## Хвиля 5 ✅ (integration — import / documents)

| Область | Тести |
|---------|--------|
| Import partial failure | `StudentImport_PartialFailure_ImportsValidRowsOnly` |
| Import duplicate email | `StudentImport_DuplicateEmail_SkipsSecondRow` |
| Invalid upload MIME | `DocumentUploadScenarioTests.UploadWork_InvalidFileType_ThrowsDomainException` |

**+3 integration-тестів** (14 → 17).

## Хвиля 6 ✅ (HTTP smoke + secretary read models)

| Область | Тести |
|---------|--------|
| Test auth | `IntegrationTestAuthHandler`, `IntegrationTestWebClient` |
| Area authorization | `AreaAuthorizationEndpointTests` (login redirect, student→secretary forbid, secretary session redirect) |
| POST select supervisor | `StudentSelectSupervisorEndpointTests` |
| Secretary list filters | `SecretaryDiplomaListScenarioTests` |
| Secretary dashboard | `SecretaryDashboardScenarioTests` |

**+7 integration-тестів** (17 → 24).

## Хвиля 7 ✅ (HTTP admit + read gaps)

| Область | Тести |
|---------|--------|
| Runner | `RunUpToReadyForAdmissionAsync` (без admit) |
| Seed | `SeedStudentWithoutDiplomaAsync` |
| POST secretary admit | `SecretaryAdmitEndpointTests` (cookie сесії + antiforgery з Employee home) |
| Admin preview HTTP | `AdminPreviewEndpointTests` |
| Empty student | `GetMyDiploma_WithoutDiploma_ReturnsEmptyComposite` |
| Document ↔ attempt | `DocumentAttemptLinkScenarioTests` |
| Support | `IntegrationAdminHelper`, `SetSecretarySessionCookie` |

**+4 integration-тестів** (24 → 28).

## Хвиля 8 ✅ (override step, reports, employee workflows)

| Область | Тести |
|---------|--------|
| Override admission step | `SecretaryOverrideAdmissionStepScenarioTests` |
| Admitted report + CSV | `AdmittedReportScenarioTests` |
| Head reject topic | `DepartmentHeadTopicRejectionScenarioTests` |
| Supervisor reject student | `SupervisorRejectStudentScenarioTests` |
| Employee home cards | `EmployeeHomeScenarioTests` (supervisor + head) |
| Guidance alignment | `GuidanceAlignmentScenarioTests` |
| Admin preview SetUser | `AdminPreviewEndpointTests.PostSetUser_RedirectsToStudentDiploma` |
| Assertions | `AssertAuditLogExistsByActionAsync`, `AssertTopicRejected` |

**+8 integration-тестів** (28 → 36).

## Хвиля 9 ✅ (checkpoint edges, archive, secretary HTTP fix)

| Область | Тести / fix |
|---------|-------------|
| **App fix** | `AnnualRoleQueries.ListAccessibleSecretarySessionsAsync` — EF-translatable OrderBy |
| **Test host** | `Program.cs` — skip HTTPS redirect у `Testing` |
| Secretary HTTP | `SecretaryDashboardEndpointTests` (Dashboard + Details) |
| Checkpoint edges | `AdmissionCheckpointEdgeScenarioTests` (empty doc, wrong step) |
| Submit topic guard | `SubmitTopicWithoutSupervisorScenarioTests` |
| Archive cascade | `DefenceSessionArchiveScenarioTests` (status + audit) |
| Secretary guidance | `GuidanceAlignmentScenarioTests.BeforeTopicApproval_...` |
| Employee home | formatting + reviewer cards у `EmployeeHomeScenarioTests` |

**+9 integration-тестів** (36 → 45).

## Хвиля 10 ✅ (anti-plagiarism home, list HTTP, guidance admit)

| Область | Тести |
|---------|--------|
| Employee home anti-plagiarism | `EmployeeHomeScenarioTests` |
| Secretary guidance admit / override | `GuidanceAlignmentScenarioTests` |
| Archive blocks upload | `ArchivedSessionScenarioTests.ArchivedSession_BlocksStudentUpload` |
| Secretary list HTTP | `SecretaryDiplomaListEndpointTests` |
| Helper | `GetWritableDiplomaAsync` у assertions |

**+6 integration-тестів** (45 → 51).

## Хвиля 11 ✅ (archive declare ready, reports HTTP, checkpoint HTTP)

| Область | Тести |
|---------|--------|
| Archive blocks declare work ready | `ArchivedSessionScenarioTests.ArchivedSession_BlocksDeclareWorkReady` |
| Secretary reports HTTP | `SecretaryReportsEndpointTests` (Admitted + AdmittedCsv) |
| Supervisor checkpoint HTTP | `SupervisorCheckpointEndpointTests` |
| Guidance after partial checkpoints | `GuidanceAlignmentScenarioTests.AfterPartialCheckpoints_...` |

**+5 integration-тестів** (51 → 56).

## Хвиля 12 ✅ (document download)

| Область | Тести |
|---------|--------|
| View URL + file on disk | `DocumentDownloadScenarioTests` |
| HTTP download auth / 404 | `DocumentDownloadEndpointTests` |

**+4 integration-тестів** (56 → 60).

## Хвиля 13 ✅ (coverage backlog)

| Область | Тести / fix |
|---------|-------------|
| DefenceSession admin | `DefenceSessionAdminScenarioTests` |
| Secretary access | `SecretaryAccessScenarioTests`, `SecretaryAccessEndpointTests` |
| Student admin | `StudentAdminScenarioTests` |
| HTTP checkpoints | `FormattingCheckpointEndpointTests`, `ReviewerCheckpointEndpointTests` |
| Student HTTP | `StudentDiplomaEndpointTests` |
| Admin import HTTP | `ImportEndpointTests` |
| Domain unit gaps | `WorkReadinessServiceTests`, `DiplomaAdmissionServiceTests`, `AdmissionWorkflowServiceTests` |
| Assembler unit | `DiplomaDetailsAssemblerTests` |
| **App fix** | `StorageFolderId` / `StorageFileId` max 1024 + migration `ExpandStoragePathColumns` |
| Runners | `RunUpToFormattingReviewStepAsync`, `RunUpToExternalReviewStepAsync` |

**+17 integration, +8 unit** (60 → 77 integration; 324 → 332 unit).

## Хвиля 14 ✅ (priorities 1–3: unit gaps + admin + HTTP POST)

| Область | Тести |
|---------|--------|
| Import parser | `CanParse`, BOM, empty lines, employee insufficient columns |
| Document helpers | `DiplomaDocumentFormatsTests`, `DiplomaDocumentNamingTests` |
| Labels | `DefenceWorkLabelTests`, `WorkflowUkrainianLabelsTests` (inline, annual role, audit value) |
| Domain | `AdvanceAfterReviewerAssignment` у `AdmissionWorkflowServiceTests` |
| Employee admin | `EmployeeAdminScenarioTests` |
| Study group admin | update + delete empty group у `StudyGroupAdminScenarioTests` |
| Secretary list | combo filter lifecycle + admission step |
| Local storage | duplicate filename suffix у `LocalFileStorageScenarioTests` |
| AdmissionStepQueries | `FindWritableAsync` tracked entity |
| Runners | `RunUpToAntiPlagiarismStepAsync`, `RunUpToReviewerAssignmentStepAsync` |
| HTTP POST | anti-plagiarism, head/supervisor topic, supervisor confirm, secretary assign reviewer / comment / override supervisor, session select |

**+15 integration, +44 unit** (77 → 92 integration; 332 → 376 unit).

## Хвиля 15 ✅ (import xlsx + EmployeeHomeQueries)

| Область | Тести |
|---------|--------|
| Import xlsx | `ImportFileParserTests` — students/employees, header skip (ПІБ/name), missing fields |
| Employee home queries | `EmployeeHomeQueriesScenarioTests` — усі методи `IEmployeeHomeQueries` + empty sessionIds |

**+12 integration, +6 unit** (92 → 104 integration; 376 → 382 unit).

## Хвиля 16 ✅ (domain coverage gaps)

| Область | Тести |
|---------|--------|
| Admission sequence / resolver | `AcceptsOutcome`, prior steps, `GetNextStep` null, reviewer readiness, status overrides |
| Admission workflow | topic/lifecycle guards, wrong step, reviewer assignment, external review, comment trim |
| Readiness / S9 | null topic, not approved, review incomplete, override when not allowed |
| Topic / supervisor | `PendingHead`, `CreateVersion`, confirm/reject guards, archived session |
| Reviewer assignment | admitted diploma, archived session |
| Entity smoke | navigation props (`Diploma`, `StudyGroup`, `AuditLog`, `DiplomaDocument`) |

**+42 unit** (103 → 145 domain; 382 → 424 unit). Domain line coverage (unit only): **96.0%**.

## Хвиля 17 ✅ (Application coverage gaps)

| Область | Тести |
|---------|--------|
| Helpers | `DefenceWorkLabel`, `ImportRowProcessor`, `WorkflowUkrainianLabels`, `PersonNameSort`, `DiplomaDocumentNaming`, `EmailDomainValidator` |
| Workflow progress | `StudentWorkflowProgressBuilderTests_wave3` — checkpoint badges, secretary override, admitted hint |
| Projections | `SecretaryDiplomaListProjectionTests`, `EmployeeDiplomaListProjectionTests` |
| Domain fix | `ResolveCurrentStep` — мертвий guard прибрано, +3 тести |

**+52 unit** (145 → 148 domain, 229 → 278 application; 424 → 476 unit).

## Хвиля 18 ✅ (Application priorities 1–3)

| Область | Тести |
|---------|--------|
| Helpers | `DefenceWorkLabel` throws, `TopicVersionApprovalFormatter`, `WorkflowUkrainianLabels` defaults |
| Guidance | `DiplomaWorkflowGuidanceTests_wave2` — admit updating, override guards |
| Workflow progress | `StudentWorkflowProgressBuilderTests_wave4` — waiting badge, secretary hints |
| Projections | `MapTopicReviewItemsAsync`, reviewer empty list |
| Integration import | session not found / archived / unsupported format (student + employee) |
| Integration secretary list | unknown session, study group + admission filters |

**+28 unit, +7 integration** (476 → 504 unit; 104 → 111 integration).

## Пов’язані файли

- [test-plan.md](./test-plan.md)
- [test-cases.md](./test-cases.md)
