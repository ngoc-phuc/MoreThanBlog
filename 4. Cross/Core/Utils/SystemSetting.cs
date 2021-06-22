namespace Core.Utils
{
    public class SystemSetting
    {
        public static SystemSetting Current { get; set; }
        public static JwtIssuerOptions JwtIssuerOptions { get; set; }
    }
}