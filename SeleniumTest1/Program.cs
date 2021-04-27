using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;


namespace SeleniumTest1
{
    class Program
    {
        static void Main(string[] args)
        {

            //set the location of chrome browser
            //System.setProperty("webdriver.chrome.driver", "C:\\chromedriver.exe");

            using (IWebDriver driver = new ChromeDriver())
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("http://w12public/TrustPortal/secure/login.aspx?p=1");
                //driver.Navigate().GoToUrl("http://gweswaf.cal.fi-tek.co.in/trustportal/secure/Login.aspx?p=1");

                //driver.Url = "http://w12public/TrustPortal/secure/login.aspx?p=1";

                Console.WriteLine(driver.Title);
                string title = driver.Title;

                if (driver.Title.Substring(11, 15) == "Global WealthES")
                {
                    Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();

                    string Runname = title.Substring(11, 15) + " " + DateTime.Now.ToString("yy-MM-dd-HH_mm_ss");
                    string screenshotfilename = "E:\\FT\\SeleniumTest\\SC\\" + Runname + ".jpg";
                    ss.SaveAsFile(screenshotfilename, ScreenshotImageFormat.Jpeg);

                    // Find the element that's ID attribute is 'log' (Username)
                    // Enter Username on the element found by above desc.
                    driver.FindElement(By.Id("txtUserName")).SendKeys("anunay");

                    // Find the element that's ID attribute is 'pwd' (Password)
                    // Enter Password on the element found by the above desc.
                    driver.FindElement(By.Id("txtPassword")).SendKeys("022589");

                    // Now submit the form.
                    driver.FindElement(By.Id("btnLogIn")).Click();
                    wait.Timeout = TimeSpan.FromSeconds(60);
                    Thread.Sleep(3000);

                    Console.WriteLine(driver.Title);

                    var src = driver.PageSource;

                    Console.Write(driver.PageSource);
                    //driver.FindElement(By.XPath(//";
                    //driver.SwitchTo().Window(driver.CurrentWindowHandle);
                    //var elem = driver.FindElement(By.Id("Newtpmenu1_tpAspMenun0"));
                    //driver.SwitchTo().Window(driver.WindowHandles.Last());
                    //var wh = driver.WindowHandles.Last();
                    string browserURL = driver.Url;
                    string browserPageSource = driver.PageSource;
                    //selenium.WindowFocus();

                    int previousWinCount = driver.WindowHandles.Count;
                    //var elem = driver.FindElements(By.LinkText("Account Enrollment"));
                    // Wait until the page is fully loaded via JavaScript
                    WebDriverWait wdw = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

                    wait.Until((x) =>
                    {
                        return ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete");
                    });

                    //String target_xpath = "//a[linkedtext='Account Enrollment']";
                    //String target_xpath = "//Frame[headerandbody]";

                    driver.SwitchTo().Frame("headerandbody");

                    //driver.FindElement(By.LinkText("Maintain Ticklers")).Click();
                    /////////////////////////////
                    //Actions action1 = new Actions(driver);
                    //IWebElement FirstmenuAdmin = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("Administration")));

                    //action1.MoveToElement(FirstmenuAdmin).Perform();


                    //IWebElement SubmenuElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Name("Maintain Ticklers...")));

                    //SubmenuElement.Click();
                    ////////////////////////////

                    IWebElement we = wdw.Until(x => x.FindElement(By.LinkText("Administration")));//Administration
                    Actions action = new Actions(driver);
                    action.MoveToElement(we).Perform();

                    //IWebElement SubmenuElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Name("Maintain Ticklers...")));
                    //SubmenuElement.Click();

                    //Clicking the SubMenu on MouseHover   
                    //var menuelement = driver.FindElement(By.XPath(SubMenu), 10);
                    //menuelement.Click();
                    //IWebElement wec1 = we.FindElement(By.PartialLinkText("Maintain Ticklers..."));
                    IWebElement SubMenu = wdw.Until(x => x.FindElement(By.LinkText("Maintain Ticklers...")));
                    //IWebElement wec = we.FindElement(By.XPath(".//a[contains(text(), 'Maintain Ticklers')]"));
                    SubMenu.Click();

                    driver.SwitchTo().Window(driver.WindowHandles[1]);

                    Thread.Sleep(3000);

                    Screenshot ss2 = ((ITakesScreenshot)driver).GetScreenshot();
                    Runname = title.Substring(11, 15) + " " + DateTime.Now.ToString("yy-MM-dd-HH_mm_ss");
                    screenshotfilename = "E:\\FT\\SeleniumTest\\SC\\" + Runname + ".jpg";
                    ss2.SaveAsFile(screenshotfilename, ScreenshotImageFormat.Jpeg);


                    driver.SwitchTo().Window(driver.WindowHandles[0]);
                    driver.SwitchTo().Frame("headerandbody");

                    //// Administrator > New > Account
                    IWebElement menuAdministration = wdw.Until(x => x.FindElement(By.LinkText("Administration")));
                    Actions actionAdministration = new Actions(driver);
                    actionAdministration.MoveToElement(menuAdministration).Perform();
                    IWebElement SubMenuNew = wdw.Until(x => x.FindElement(By.LinkText("New")));
                    Actions actionNew = new Actions(driver);
                    actionNew.MoveToElement(SubMenuNew).Perform();
                    IWebElement SubMenuNeAccount = wdw.Until(x => x.FindElement(By.LinkText("Account...")));
                    SubMenuNeAccount.Click();

                    driver.SwitchTo().Window(driver.WindowHandles[2]);
                    Thread.Sleep(3000);
                    driver.FindElement(By.Id("txtAccountNumber")).SendKeys("Selenium Account");
                    driver.FindElement(By.Id("txtShortName")).SendKeys("Selenium");
                    driver.FindElement(By.Id("txtAlphaSort")).SendKeys("Selenium");
                    driver.FindElement(By.Id("chkSweep")).Click();
                    SelectElement selectSweepProfile = new SelectElement(driver.FindElement(By.Id("ddlSweepProfile")));
                    selectSweepProfile.SelectByIndex(1);
                    SelectElement selectNettingRule = new SelectElement(driver.FindElement(By.Id("ddlNettingRule")));
                    selectNettingRule.SelectByIndex(2);
                    SelectElement selectInvestmentAuthority = new SelectElement(driver.FindElement(By.Id("ddlInvestmentAuthority")));
                    selectInvestmentAuthority.SelectByIndex(1);
                    SelectElement selectInvestmentObjective = new SelectElement(driver.FindElement(By.Id("ddlInvestmentObjective")));
                    selectInvestmentObjective.SelectByIndex(1);

                    SelectElement selectType = new SelectElement(driver.FindElement(By.Id("ddlType")));
                    selectType.SelectByIndex(1);
                    SelectElement selectBranch = new SelectElement(driver.FindElement(By.Id("ddlBranch")));
                    selectBranch.SelectByIndex(1);
                    SelectElement selectCapacity = new SelectElement(driver.FindElement(By.Id("ddlCapacity")));
                    selectCapacity.SelectByIndex(1);
                    SelectElement selectRegistration = new SelectElement(driver.FindElement(By.Id("ddlRegistration")));
                    selectRegistration.SelectByIndex(1);
                    SelectElement selectLocation = new SelectElement(driver.FindElement(By.Id("ddlLocation")));
                    selectLocation.SelectByIndex(1);

                    Thread.Sleep(3000);

                    Screenshot ss4 = ((ITakesScreenshot)driver).GetScreenshot();
                    Runname = title.Substring(11, 15) + " " + DateTime.Now.ToString("yy-MM-dd-HH_mm_ss");
                    screenshotfilename = "E:\\FT\\SeleniumTest\\SC\\" + Runname + ".jpg";
                    ss4.SaveAsFile(screenshotfilename, ScreenshotImageFormat.Jpeg);

                    ///////go back to portal dashboard

                    driver.SwitchTo().Window(driver.WindowHandles[0]);
                    driver.SwitchTo().Frame("headerandbody");
                    IWebElement logoutLink = wdw.Until(x => x.FindElement(By.LinkText("Logout")));
                    logoutLink.Click();

                    Thread.Sleep(3000);

                    //driver.CurrentWindowHandle

                    /* Explicit Wait */
                    /* WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); */
                    //DefaultWait<IWebDriver> wdw = new DefaultWait<IWebDriver>(driver);
                    //wdw.Timeout = TimeSpan.FromSeconds(30);
                    //wdw.PollingInterval = TimeSpan.FromMilliseconds(250);
                    /* Ignore the exception - NoSuchElementException that indicates that the element is not present */
                    //wdw.IgnoreExceptionTypes(typeof(NoSuchElementException));
                    //wdw.Message = "Element to be searched not found";

                    /* Explicit Wait */
                    /* IWebElement SearchResult = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(target_xpath))); */
                    //IWebElement we = wdw.Until(x => x.FindElement(By.XPath(target_xpath)));
                    //var we = wdw.Until(x => x.FindElements(By.Name("headerandbody")));
                    //var we = driver.FindElement(By.Name("Account Enrollment"));

                    //var we1 = wdw.Until(x => x.FindElement(By.Name("Account Enrollment")));



                    Screenshot ss3 = ((ITakesScreenshot)driver).GetScreenshot();
                    Runname = title.Substring(11, 15) + " " + DateTime.Now.ToString("yy-MM-dd-HH_mm_ss");
                    screenshotfilename = "E:\\FT\\SeleniumTest\\SC\\" + Runname + ".jpg";
                    ss3.SaveAsFile(screenshotfilename, ScreenshotImageFormat.Jpeg);



                    //var elem = driver.FindElements(By.XPath("//a[text() = 'Account Enrollment']"));
                    var elem = driver.FindElements(By.XPath("//div['divMainMenuBar']"));

                    title = driver.Title;
                    // Find the element that's ID attribute is 'account_logout' (Log Out)
                    //driver.FindElement(By.XPath(".//*[@id='account_logout']/a")).Click();
                }
                else
                    Console.WriteLine("Wrong Application Title: " + driver.Title);

                // Close the driver
                //driver.Quit();



                //driver.FindElement(By.Name("q")).SendKeys("cheese" + Keys.Enter);
                //    wait.Until(webDriver => webDriver.FindElement(By.CssSelector("h3>div")).Displayed);
                //    IWebElement firstResult = driver.FindElement(By.CssSelector("h3>div"));
                //    Console.WriteLine(firstResult.GetAttribute("textContent"));
            }
        }

    }
}
