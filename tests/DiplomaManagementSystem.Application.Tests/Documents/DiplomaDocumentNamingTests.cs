using DiplomaManagementSystem.Application.Documents;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Tests.Documents;

public sealed class DiplomaDocumentNamingTests
{
    [Theory]
    [InlineData(DefenceSessionType.Bachelor, "Bakalavrska_Robota")]
    [InlineData(DefenceSessionType.Master, "Magisterska_Robota")]
    public void GetWorkTypeToken_ReturnsExpected(DefenceSessionType sessionType, string expected)
    {
        Assert.Equal(expected, DiplomaDocumentNaming.GetWorkTypeToken(sessionType));
    }

    [Fact]
    public void BuildFileName_StudentWork_IncludesTransliteratedStudentName()
    {
        string fileName = DiplomaDocumentNaming.BuildFileName(
            DefenceSessionType.Bachelor,
            DiplomaDocumentKind.StudentWork,
            1,
            "pdf",
            "Іваненко Іван");

        Assert.Equal("Bakalavrska_Robota_Ivanenka_Ivana_v1.pdf", fileName);
    }

    [Fact]
    public void BuildFileName_SupervisorFeedback_IncludesActorAndStudentNames()
    {
        string fileName = DiplomaDocumentNaming.BuildFileName(
            DefenceSessionType.Bachelor,
            DiplomaDocumentKind.SupervisorFeedback,
            1,
            ".PDF",
            "Іваненко Іван",
            "Мельничук Олена");

        Assert.Equal(
            "Vidguk_Kerivnyka_Melnychuka_Olena_na_robotu_Ivanenka_Ivana_v1.pdf",
            fileName);
    }

    [Fact]
    public void BuildFileName_ExternalReview_IncludesReviewerAndStudentNames()
    {
        string fileName = DiplomaDocumentNaming.BuildFileName(
            DefenceSessionType.Master,
            DiplomaDocumentKind.ExternalReview,
            2,
            "pdf",
            "Іваненко Іван",
            "Шевченко Анна");

        Assert.Equal(
            "Recenziya_Shevchenka_Anna_na_robotu_Ivanenka_Ivana_v2.pdf",
            fileName);
    }

    [Fact]
    public void BuildFileName_AntiPlagiarismReport_IncludesOfficerAndStudentNames()
    {
        string fileName = DiplomaDocumentNaming.BuildFileName(
            DefenceSessionType.Bachelor,
            DiplomaDocumentKind.AntiPlagiarismReport,
            1,
            "pdf",
            "Іваненко Іван",
            "Бондаренко Андрій");

        Assert.Equal(
            "Antyplagiat_Bondarenka_Andriia_na_robotu_Ivanenka_Ivana_v1.pdf",
            fileName);
    }

    [Theory]
    [InlineData(DiplomaDocumentKind.StudentWork, true)]
    [InlineData(DiplomaDocumentKind.SupervisorFeedback, true)]
    [InlineData(DiplomaDocumentKind.ExternalReview, true)]
    [InlineData(DiplomaDocumentKind.AntiPlagiarismReport, true)]
    public void RequiresFile_ForDocumentKind_ReturnsExpected(DiplomaDocumentKind kind, bool expected)
    {
        Assert.Equal(expected, DiplomaDocumentNaming.RequiresFile(kind));
    }

    [Fact]
    public void RequiresFile_ForFormattingStep_ReturnsFalse()
    {
        Assert.False(DiplomaDocumentNaming.RequiresFile(AdmissionStep.FormattingReview));
    }

    [Theory]
    [InlineData(AdmissionStep.SupervisorFeedback, DiplomaDocumentKind.SupervisorFeedback)]
    [InlineData(AdmissionStep.ExternalReview, DiplomaDocumentKind.ExternalReview)]
    [InlineData(AdmissionStep.AntiPlagiarismClearance, DiplomaDocumentKind.AntiPlagiarismReport)]
    public void ToDocumentKind_MapsAdmissionStep(AdmissionStep step, DiplomaDocumentKind expected)
    {
        Assert.Equal(expected, DiplomaDocumentNaming.ToDocumentKind(step));
    }

    [Fact]
    public void ToDocumentKind_UnsupportedStep_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            DiplomaDocumentNaming.ToDocumentKind(AdmissionStep.FormattingReview));
    }

    [Theory]
    [InlineData(AdmissionStep.SupervisorFeedback, true)]
    [InlineData(AdmissionStep.ExternalReview, true)]
    [InlineData(AdmissionStep.AntiPlagiarismClearance, true)]
    public void RequiresFile_ForAdmissionStep_ReturnsExpected(AdmissionStep step, bool expected)
    {
        Assert.Equal(expected, DiplomaDocumentNaming.RequiresFile(step));
    }

    [Fact]
    public void GetWorkTypeToken_InvalidSessionType_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            DiplomaDocumentNaming.GetWorkTypeToken((DefenceSessionType)99));
    }
}
