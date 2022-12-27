namespace Musference.Models.EndpointModels
{
    public class ResetPasswordModel
    {
        public string ResetCode { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
    }
}
