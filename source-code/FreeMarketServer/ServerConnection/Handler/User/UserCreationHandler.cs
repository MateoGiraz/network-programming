using BusinessLogic;

namespace ServerConnection.Handler.User
{
    public class UserCreationHandler : UserHandler
    {
        protected override async Task HandleUserSpecificOperationAsync()
        {
            var oc = new OwnerController();

            if (UserDto == null || ResponseDto == null)
                return;

            try
            {
                oc.SignUp(UserDto.UserName, UserDto.Password);
                ResponseDto.StatusCode = 200;
                ResponseDto.Message = $"Registered {UserDto.UserName}";
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