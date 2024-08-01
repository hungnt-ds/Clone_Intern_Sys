namespace InternSystem.API.Utilities
{
    public static class AppConstants
    {
        public const string AdminRole = "admin";
        public const string StaffRole = "staff";
        public const string LeaderRole = "leader";
        public const string InternRole = "intern";

        public const string AdminStaff = AdminRole + "," + StaffRole;
        public const string AdminStaffLeader = AdminRole + "," + StaffRole + "," + LeaderRole;
        public const string StaffLeader = StaffRole + "," + LeaderRole;
    }
}
