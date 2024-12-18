namespace AuthService.Domain.Constants;

public static class ResponseMessages
{
    public const string LoggedOutSuccessfully = "You have logged out successfully.";
    public const string PasswordUpdatedSuccessfully = "You have updated your password successfully.";
    public const string EmailVerificationSentSuccessfully = "Email verification has been sent successfully.";
    public const string UserUpdatedSuccessfully = "User has been updated successfully.";
    public const string EmailVerifiedSuccessfully = "Email has been verified successfully.";

    public const string UserNotFound = "User not found.";
    public const string ErrorDuringLogout = "An error occurred during logout.";
    public const string ErrorDuringResetPassword = "An error occurred during password reset.";
    public const string ErrorDuringSendVerifyEmail = "An error occurred while sending the verification email.";
}