namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Models
{
    public class GetPagedRolesResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
