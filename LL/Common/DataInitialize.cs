using LLBLL.Common.Tool;
using LLBLL.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace LLBLL.Common
{

    public class DataInitialize
    {
        /// <summary>
        /// 初始化添加数据
        /// </summary>
        /// <param name="dbContext"></param>
        public static void DataInit(IServiceProvider service)
        {
            var dbContext = service.GetService<LLDbContext>();
            if (dbContext.Users.Any())
            {
                return;
            }
            //添加用户
            dbContext.Users.AddRange(new Users { UserName = "ZJF", PassWord = Md5Encry.MD5Encry("123456"), Delstate = 0 }, new Users { UserName = "LL", PassWord = Md5Encry.MD5Encry("123456"), Delstate = 0 });
            dbContext.SaveChangesAsync();

        }

    }
}
