namespace InternSystem.Application.Features.InternManagement.InternManagement.Models
{
    public class TemplateData
    {
        // User fields
        //public string Id { get; set; } = "ex10009";
        public string Email { get; set; } = "user109@example.com";
        public bool EmailConfirmed { get; set; } = true;
        //public string PasswordHash { get; set; } = "Hashedpassword1";
        public string PhoneNumber { get; set; } = "1234567890";
        public bool PhoneNumberConfirmed { get; set; } = true;
        //public string HoVaTen { get; set; } = "Example User";
        public bool IsConfirmed { get; set; } = true;
        public string TrangThaiThucTap { get; set; } = "Active";

        // InternInfo
        //public string UserId { get; set; } = "ex10001";
        public string HoTen { get; set; } = "Example User";
        public DateTime NgaySinh { get; set; } = DateTime.Now;
        public bool GioiTinh { get; set; } = true;
        public string MSSV { get; set; } = "ex10001";
        public string EmailTruong { get; set; } = "USER@EXAMPLE.COM";
        public string SdtNguoiThan { get; set; } = "0987654321";
        public string DiaChi { get; set; } = "123 street 1";
        public decimal GPA { get; set; } = 4;
        public string TrinhDoTiengAnh { get; set; } = "Good";
        public string LinkFacebook { get; set; } = "facebook.com/123";
        public string LinkCv { get; set; } = "google.docs.123";
        public string NganhHoc { get; set; } = "Category 1";
        public string TrangThai { get; set; } = "Application Submitted";
        public int Round { get; set; } = 1;
        public string ViTriMongMuon { get; set; } = "Position 1";
        //public DateTime? StartDate { get; set; } = DateTime.Now;
        //public DateTime? EndDate { get; set; } = DateTime.Now;
        public int IdTruong { get; set; } = 1;
        //public int? KyThucTapId { get; set; } = 1;
        //public int? DuAnId { get; set; } = 2;

    }
}
