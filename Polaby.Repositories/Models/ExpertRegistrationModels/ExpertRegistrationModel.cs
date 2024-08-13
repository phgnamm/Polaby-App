using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Models.ExpertRegistrationModels;

public class ExpertRegistrationModel : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Image { get; set; }
    public string CertificateUrl { get; set; }
    public string ClinicLicenseUrl { get; set; }
    public string CCCDFrontUrl { get; set; }
    public string CCCDBackUrl { get; set; }
    public string ClinicAddress { get; set; }
    public string Description { get; set; }
    public string Education { get; set; }
    public int YearsOfExperience { get; set; }
    public string Workplace { get; set; }
    public Level Level { get; set; }
    public ExpertRegistrationStatus Status { get; set; }
}