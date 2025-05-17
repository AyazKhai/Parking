namespace Identity.Service.Roles
{
    public static class RolesTypes
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";

        public static IEnumerable<string> GetAll()
        {
            return new[] { Admin, Manager, User };
        }
    }
}
