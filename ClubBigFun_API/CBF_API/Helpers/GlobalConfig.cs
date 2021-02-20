namespace CBF_API.Helpers
{
    public class GlobalConfig
    {


        //////////***************development
        //  public static string Get_LogoUrl = "http://cbfpublic-nowgray-com.nt1-p2stl.ezhostingserver.com/assets/CBF-logo.png";
        //  public static string PublicWebUrl = "http://cbfpublic-nowgray-com.nt1-p2stl.ezhostingserver.com";
        //  public static string AdminWebUrl = "http://cbfadmin-nowgray-com.nt1-p2stl.ezhostingserver.com";
        //  public static string MemberProfileUploadUrl = "../cbfpublic/assets/profile/members/";
        //  public static string MemberProfileUploadUrlBackend = "../cbfAdmin/assets/profile/members/";

        //  public static string PoolProfileUploadUrlPublic = "../cbfpublic/assets/profile/pool/";
        //  public static string PoolProfileUploadUrlBackend = "../cbfAdmin/assets/profile/pool/";

        //  public static string AdminProfileUploadUrl = "../cbfAdmin/assets/profile/Administrators/";

        //  internal static string TeamUploadUrlPublic = @"../cbfpublic/assets/logos/";
        //  internal static string TeamUploadUrlAdmin = @"../cbfAdmin/assets/logos/";
        ////  public static string ConnectionString = @"data source=sql1-p2stl.ezhostingserver.com;initial catalog=ClubBigFun;user id=ClubBigFun;password=Nowgray@2019;multipleactiveresultsets=True;application name=EntityFramework";
        //  //public static string ConnectionString = @"data source=NOWGRAY-SERVER\MSSQL2012;initial catalog=ClubBigFun_DB;user id=sa;password=abc@123;multipleactiveresultsets=True;application name=EntityFramework";
        //   public static string ConnectionString = @"data source=AKSHAY-NG\SQLEXPRESS;initial catalog=CBF_Prod3;user id=sa;password=abc@123;multipleactiveresultsets=True;application name=EntityFramework";
        //  ////SMTP
        //  public static string FromAddress = "nowgray@gmail.com";
        //  public static string SMTPHost = "smtp.gmail.com";
        //  public static int SMTPPort = 587;
        //  public static bool SSL = true;
        //  public static string SenderAddress = "nowgray@gmail.com";
        //  public static string SenderPassword = "sxmhunopzzloywyp";
        //  public static string ScoreAdmin = "askdev@nowgray.com";
        //public static string APIKey = "447d66f1-ab64-4c9f-b51d-50388b";

        //////////***********uat
        public static string Get_LogoUrl = "http://uat-public.clubbigfun.com//assets/CBF-logo.png";
        public static string PublicWebUrl = "http://uat-public.clubbigfun.com/";
        public static string AdminWebUrl = "http://uat-admin.clubbigfun.com/";
        public static string MemberProfileUploadUrl = "../uat-public/assets/profile/members/";
        public static string MemberProfileUploadUrlBackend = "../uat-admin/assets/profile/members/";

        public static string PoolProfileUploadUrlPublic = "../uat-public/assets/profile/pool/";
        public static string PoolProfileUploadUrlBackend = "../uat-admin/assets/profile/pool/";

        public static string AdminProfileUploadUrl = "../uat-admin/assets/profile/Administrators/";
        internal static string TeamUploadUrlPublic = @"../uat-public/assets/logos/";
        internal static string TeamUploadUrlAdmin = @"../uat-admin/assets/logos/";
        public static string ConnectionString = @"data source=sql1-p2stl.ezhostingserver.com;initial catalog=CBF_UAT;user id=CBF_UAT;password=Nowgray@2019;multipleactiveresultsets=True;application name=EntityFramework";

        //////SMTP
        public static string FromAddress = "theclubbigfun@gmail.com";
        public static string SMTPHost = "smtp.gmail.com";
        public static int SMTPPort = 587;
        public static bool SSL = true;
        public static string SenderAddress = "theclubbigfun@gmail.com";
        public static string SenderPassword = "!@nT0rn0";
        public static string ScoreAdmin = "theclubbigfun@gmail.com";

        public static string APIKey = "447d66f1-ab64-4c9f-b51d-50388b";//"447d66f1-ab64-4c9f-b51d-50388b";

        ////***********Loacal

        //public static string ConnectionString = @"data source=AKSHAY-NG\SQLEXPRESS;initial catalog=CBF_Prod9;user id=sa;password=abc@123;multipleactiveresultsets=True;application name=EntityFramework";


        //////////***********Production
        //public static string Get_LogoUrl = "https://clubbigfun.com/assets/CBF-logo.png";
        //public static string PublicWebUrl = "https://clubbigfun.com";
        //public static string AdminWebUrl = "https://admin.clubbigfun.com";
        //public static string MemberProfileUploadUrl = "../../wwwroot/assets/profile/members/";
        //public static string MemberProfileUploadUrlBackend = "../admin/assets/profile/members/";

        //public static string PoolProfileUploadUrlPublic = "../../wwwroot/assets/profile/pool/";
        //public static string PoolProfileUploadUrlBackend = "../admin/assets/profile/pool/";


        //public static string AdminProfileUploadUrl = "../admin/assets/profile/Administrators/";
        //internal static string TeamUploadUrlPublic = @"../../wwwroot/assets/logos/";
        //internal static string TeamUploadUrlAdmin = @"../admin/assets/logos/";

        //////Change this later
        //public static string ConnectionString = @"data source=sql1-p2stl.ezhostingserver.com;initial catalog=CBF_Prod;user id=cbf_sa;password=Nowgray@2019;multipleactiveresultsets=True;application name=EntityFramework";

        //////SMTP
        //public static string FromAddress = "admin@clubbigfun.com";
        //public static string SMTPHost = "MAIL02-P2.ezhostingserver.com";
        //public static int SMTPPort = 587;
        //public static bool SSL = false;
        //public static string SenderAddress = "admin@clubbigfun.com";
        //public static string SenderPassword = "Nowgray@2019";
        //public static string ScoreAdmin = "admin@clubbigfun.com";
        //public static string APIKey = "447d66f1-ab64-4c9f-b51d-50388b";
    }
}
