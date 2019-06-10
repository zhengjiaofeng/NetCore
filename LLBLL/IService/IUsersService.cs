using LLBLL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LLBLL.IService
{
    public interface IUsersService
    {
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        List<Users> GetUsers();

        Task<Users> GetUserByUserName(string userName);

    }
}
