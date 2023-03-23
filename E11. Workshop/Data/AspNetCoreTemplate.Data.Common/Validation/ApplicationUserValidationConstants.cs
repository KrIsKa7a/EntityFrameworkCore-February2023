namespace Blog.Data.Common.Validation
{
    public class ApplicationUserValidationConstants
    {
        public const int UsernameMaxLength = 20;
        public const int EmailMaxLength = 50;
        public const int PasswordMaxLength = 256; // DB Encrypted Password
        public const int PasswordSaltMaxLength = 256;
    }
}
