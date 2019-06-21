using System;
using System.Collections.Generic;
using System.Text;

namespace LLBLL.IService
{
  public  interface IRepositoryBaseService
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<T> ToPageList<T>(int pageIndex = 1, int pageSize = 10) where T : class;
    }
}
