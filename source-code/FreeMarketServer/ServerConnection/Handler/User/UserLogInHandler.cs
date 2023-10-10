using BusinessLogic;

namespace ServerConnection.Handler.User
{
    public class UserLogInHandler : UserHandler
    {
        protected override async Task HandleUserSpecificOperationAsync()
        {
            var oc = new OwnerController();

            if (UserDto == null || ResponseDto == null)
                return;

            try
            {
                oc.LogIn(UserDto.UserName, UserDto.Password);
                ResponseDto.StatusCode = 200;
                ResponseDto.Message = $"Welcome back, {UserDto.UserName}";
            }
            catch (AuthenticatorException ex)
            {
                Console.WriteLine(ex.Message);

                ResponseDto.StatusCode = 400;
                ResponseDto.Message = ex.Message;
            }
        }
    }
}