using PlantApp.Data;

namespace PlantApp.Services
{
    public class AuthService
    {
        public User? CurrentUser { get; private set; }

        public void SetUser(User user)
        {
            CurrentUser = user;
        }

        public int GetUserId()
        {
            if (CurrentUser == null)
                throw new Exception("Пользователь не зарегестрирован");

            return CurrentUser.Id;
        }

        public bool IsLoggedIn()
        {
            return CurrentUser != null;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}