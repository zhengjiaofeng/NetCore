using LLBLL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LLBLL.IService
{
   public interface IUsersService
    {
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        List<Users> GetUsers();
    }
}
