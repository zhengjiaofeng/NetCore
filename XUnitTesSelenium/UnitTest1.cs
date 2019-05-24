using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using Xunit;

namespace XUnitTesSelenium
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }
        [Fact(DisplayName = "Visit.Cnblogs")]
        public void Visit_Cnblogs()
        {
            IWebDriver driver = new ChromeDriver(@"E:\c#\chromedriver\chromedriver_win32");
            
            driver.Url = "http://www.cnblogs.com/NorthAlan";
            var lnkAutomation = driver.FindElement(By.XPath(@"//*[@id='CatList_LinkList_0_Link_4']"));
            var test= lnkAutomation.Text;
            lnkAutomation.Click();
        }
    }
}
