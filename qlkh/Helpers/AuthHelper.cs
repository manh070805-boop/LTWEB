namespace qlkh.Helpers
{
    public class AuthHelper
    {
        public const int AdminRoleId = 1;
        public const int TeacherRoleId = 2;
        public const int StudentRoleId = 3;

        public static bool IsLoggedIn(HttpContext context)
        {
            return context.Session.GetInt32("AccountId") != null;
        }
        public static int? GetAccountId (HttpContext context)
        {
            return context.Session.GetInt32("AccountId");
        }
        public static int? GetRoleId(HttpContext context) {
            return context.Session.GetInt32("RoleId");
        }
        public static bool IsAdmin(HttpContext context)
        {
            return GetRoleId(context) == AdminRoleId;
        }
        public static bool IsTeacher(HttpContext context)        {
            return GetRoleId(context) == TeacherRoleId;
        }
        public static bool IsStudent(HttpContext context)
        {
            return GetRoleId(context) == StudentRoleId;
        }
    
    }
}