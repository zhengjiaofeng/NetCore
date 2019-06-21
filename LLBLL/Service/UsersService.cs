using LLBLL.Common;
using LLBLL.IService;
using LLBLL.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<Users> GetUserByUserName(string userName)
        {
            return await llDbContext.Users.SingleOrDefaultAsync(d => d.UserName == userName);

        }

    }
}
