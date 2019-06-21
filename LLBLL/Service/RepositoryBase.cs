using LLBLL.IService;
using System.Collections.Generic;
using System.Linq;

namespace LLBLL.Common
{
    public class RepositoryBase: IRepositoryBaseService
    {
        private readonly LLDbContext llDbContext;

        public RepositoryBase(LLDbContext _llDbContext)
        {
            llDbContext = _llDbContext;
        }
        public List<T> ToPageList<T>(int pageIndex = 1, int pageSize = 10) where T : class
        {
           
            List<T> result = new List<T>();
            result = llDbContext.Set<T> ().Skip((pageIndex - 1) * pageSize).ToList();
            return result;

        }
    }
}
