namespace CBF_API.Helpers
{
    public static class Messages
    {
        public static string InvalidToken = "Auth Token is not supplied or is invalid.";
        public static string SessionExpired = "User session has been expired due to inactivity. Please refresh and login again.";
        public static string ModelStateInvalid = "Request body is invalid, Please verify the data sent to the API.";
        public static string APIStatusError = "0";
        public static string APIStatusSuccess = "1";
        public static string DeleteSuccess = "Record deleted successfully.";
        public static string SavedSuccess = "Saved successfully.";
        public static string UpdatedSuccess = "Updated successfully.";

        public static string ForgotPasswordEmailSent = "An email has been sent to your email. Please follow the instruction given in the email.";
        public static string ForgotPasswordInvalidEmail = "This email is not registered with us.";

        public static string ResetPasswordSuccess = "Password successfully changed, Please navigate to Login page.";
        internal static string CanNotSelectATeamTwice = "This team is already picked. You can not select a team twice.";
        public static string AdminPasswordResetSuccess = "Password successfully changed.";

        public static string WeekClosed = "This week has been closed, You can not make a pick.";

        public static string ClubClosed = "This club has been closed.";
        internal static string maxticketallowed = "You cannot add more than 50 tickets at a time.";
    }
}
