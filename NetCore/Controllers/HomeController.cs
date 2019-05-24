using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore.Common;
using NetCore.Common.Tool;
using NetCore.Models;
using NetCore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;


namespace NetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext dbContext;
        private static int ids = 1;
        public HomeController(MyDbContext myDbContext)
        {
            try
            {
                dbContext = myDbContext;
                //初始化
                Initialize.MovieDataInit(dbContext);

            }
            catch (Exception ex)
            {
            }
        }
        public IActionResult Index()
        {
            //HttpContext.Session.SetString("test", "123");
            //string result = HttpContext.Session.GetString("test");
            SetSessionId();
            var result = HttpContext.Session.GetString("test_id");
            // List<Movies> movies = dbContext.Movies.ToList();
            //隐藏属性
            var movies1 = dbContext.Movies.OrderBy(b => EF.Property<DateTime>(b, "LastUpdated"));
            //
            List<MovieViewModel> movies = dbContext.Movies.Join(dbContext.MoviePrices, a => a.name, b => b.MovieName, (a, b) => new MovieViewModel { Id = a.Id, Name = a.name, RowVersion = a.RowVersion, SynTime = a.SynTime, Price = b.Price }).Where(d => !string.IsNullOrEmpty(d.Name)).ToList();
            return View(movies);
        }
        public async Task EditSave(int id, byte[] rowVersion)
        {
            try
            {
                Movies model = dbContext.Movies.Where(d => d.Id == id && d.name == "movie111").FirstOrDefault();

                model.SynTime = DateTime.Now;
                //dbContext.Entry(Movies).Property("RowVersion").OriginalValue = rowVersion;
                dbContext.Movies.Update(model);
                await dbContext.SaveChangesAsync();
            }

            catch (Exception ex)
            {

            }

        }


        public string HelloWord(string name, int numTimes = 1)
        {

            var result = HttpContext.Session.Get<string>("test");
            return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        }

        #region seseion

        private void SetSessionId()
        {
            var result = HttpContext.Session.GetString("test_id");
            if (result == null)
            {
                HttpContext.Session.SetString("test_id", ids.ToString());
                ids++;
            }
        }
        #endregion

    }
}