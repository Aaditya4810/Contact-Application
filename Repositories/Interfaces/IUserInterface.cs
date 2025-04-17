using ContactApp.Models;

namespace ContactApp.Repositories.Interfaces
{
    public interface IUserInterface
    {
        #region for Registeration
         Task<int>Add(t_User userData);
        #endregion

        #region for Login
        Task<t_User>Login(vm_Login loginData);
        Task<t_User>GetUser(int userId);
        #endregion
    }
}