using NetCore.Models;
using System;
using System.Linq;

namespace NetCore.Common
{
    public static class Initialize
    {
        public static void MovieDataInit(MyDbContext context)
        {
            try
            {
                if (context.Movies.Any())
                {
                    return;
                }

                context.Movies.AddRange(new Movies { name = "movie11", SynTime = DateTime.Now }, new Movies { name = "movie22", SynTime = DateTime.Now });
                var result = context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
                
        }
    }
}
