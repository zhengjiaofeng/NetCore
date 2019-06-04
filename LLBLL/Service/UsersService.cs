using LLBLL.Common;
using LLBLL.IService;
using LLBLL.Model;
using System.Collections.Generic;
using System.Linq;
namespace LLBLL.Service
{
    public class UsersService : IUsersService
    {
        private readonly LLDbContext llDbContext;

        public UsersService(LLDbContext _llDbContext)
        {
            llDbContext = _llDbContext;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public List<Users> GetUsers()
        {
            List<Users> lists = new List<Users>();
            lists = llDbContext.Users.ToList();
            
            return lists;
        }
    }
}
