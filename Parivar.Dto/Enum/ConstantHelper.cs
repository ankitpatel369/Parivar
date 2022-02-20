namespace Parivar.Dto.Enum
{
    #region Enums
    public enum WorldDbs
    {
        Country = 1,
        State = 2,
        City = 3,
        District = 4,
        County = 5,
        Village = 6
    }

    public enum Genders
    {
        Male = 1,
        Female = 2,
        Other = 3,
    }

    #endregion
    public class UserRoles
    {
        public const string SystemAdmin = "SystemAdmin";
        public const string FamilyMember = "FamilyMember";
        public const string Developer = "Developer";
    }

    public class UserClaims
    {
        public const string UserId = "UserId";
        public const string UserRole = "UserRole";
        public const string DisplayUserRole = "DisplayUserRole";
        public const string FullName = "FullName";
        public const string ProfilePic = "ProfilePic";
    }

    public class FilePathList
    {
        public const string FixProfilePic = @"/UploadFile/UserProfile/user.png";
        public const string FixOrgPic = @"/UploadFile/OrgPic/org.png";
        public const string ProfilePic = @"UploadFile\UserProfile";
    }
    public class EmailTemplateFileList
    {
        public const string ResetPassword = @"ResetPassword.html";
        public const string Congratulation = @"Congratulation.html";
    }

    public class GlobalFormates
    {
        public const string DefaultDate = "MMM/dd/yyyy";
        public const string FullDate = "MM/dd/yyyy hh:mm tt";
    }

    public class StoredProcedureList
    {
        public const string GetContactUsList = "GetContactUsList";
    }

    public class LocalizationConstant
    {
        #region Common
        public const string Login = "Login";
        public const string RegisterFamily = "RegisterFamily"; 
        public const string Home = "Home";
        public const string AboutUs = "AboutUs";
        public const string Events = "Events";
        public const string Gallerys = "Gallerys";
        public const string Business = "Business";
        public const string Committees = "Committees";
        public const string OurCommittee = "OurCommittee";
        public const string Contact = "Contact";
        public const string Select = "Select";
        public const string ReadMore = "ReadMore";
        public const string SeeMore = "SeeMore";
        #endregion

        #region HeaderBar
        public const string AdminDashboard = "Admin Dashboard";
        public const string Dashboard = "Dashboard";

        #endregion

        #region Add Family
        public const string MemberRegistrationForm = "MemberRegistrationForm";
        public const string MainMember = "MainMember";
        public const string FatherName = "FatherName";
        public const string ResidentialAddress = "ResidentialAddress";
        public const string Country = "Country";
        public const string State = "State";
        public const string City = "City";
        public const string District = "District";
        public const string Taluk = "Taluk";
        public const string Village = "Village";
        public const string MobileNumber = "MobileNumber";
        public const string Email = "Email";
        public const string TotalMembersInHouse = "TotalMembersInHouse";
        public const string Gender = "Gender";
        public const string Submit = "Submit";

        #region AddNoOfFamilyMembers
        public const string MemberDetails = "MemberDetails";
        public const string MemberName = "MemberName";
        public const string RelationshipWithMainMember = "RelationshipWithMainMember";
        public const string Relationship = "Relationship";
        public const string BirthDate = "BirthDate";
        public const string BloodGroup = "BloodGroup";
        public const string Married = "Married";
        public const string Yes = "Yes";
        public const string No = "No";
        public const string EducationStudy = "EducationStudy";
        public const string SurnameOfMosal = "SurnameOfMosal";
        public const string MosalVillage = "MosalVillage";
        public const string Member = "Member";

        #endregion
        #endregion

        #region FooterBar

        public const string ContactAddress = "ContactAddress";
        public const string ContactInfo = "ContactInfo";
        public const string ContactUs = "ContactUs";
        public const string DevPlaces = "DevPlaces";

        public const string HighEducatedPeoples = "HighEducatedPeoples";
        public const string Information = "Information";
        public const string NoticeBoard = "NoticeBoard";
        public const string OurParivar = "OurParivar";
        public const string UsefulLinks = "UsefulLinks";
    #endregion
    }
}
