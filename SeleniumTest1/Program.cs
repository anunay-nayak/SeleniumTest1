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
using SeleniumExtras.WaitHelpers;
using System.IO;

namespace SeleniumTest1
{
    class Program
    {
        static void Main(string[] args)
        {

            //set the location of chrome browser
            //System.setProperty("webdriver.chrome.driver", "C:\\chromedriver.exe");

            StreamWriter sw;
            using (IWebDriver driver = new ChromeDriver())
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                driver.Manage().Window.Maximize();
                sw = new StreamWriter("E:\\FT\\SeleniumTest\\SC\\GWES_Test_Baseline.txt");
                RunTest(driver, wait, "http://w12publictest/TrustPortal/secure/login.aspx?p=1", true, "anunay", "022589", sw);
                sw.Close();

                driver.Quit();
            }
            using (IWebDriver driverNew = new ChromeDriver())
            {
                WebDriverWait waitnew = new WebDriverWait(driverNew, TimeSpan.FromSeconds(10));
                driverNew.Manage().Window.Maximize();
                sw = new StreamWriter("E:\\FT\\SeleniumTest\\SC\\GWES_Test.txt");
                RunTest(driverNew, waitnew, "http://w12public/TrustPortal/secure/login.aspx?p=1", false, "anunay", "022589", sw);
                sw.Close();
                driverNew.Quit();

            }
        }

        private static void RunTest(IWebDriver driver, WebDriverWait wait, string testUrl, bool isBaseline, string userName, string password, StreamWriter sw)
        {
            driver.Navigate().GoToUrl(testUrl);

            Console.WriteLine(driver.Title);
            string title = driver.Title;

            if (driver.Title.Substring(11, 15) == "Global WealthES")
            {
                TakeScreenShot(driver, "Login", isBaseline);

                PortalLogin(driver, wait, userName, password, false);

                Thread.Sleep(2000);

                //Console.WriteLine(driver.Title);
                //var src = driver.PageSource;
                //Console.Write(driver.PageSource);

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

                if (driver.Title.Contains("Today"))
                {
                    TakeScreenShot(driver, "Portal", isBaseline);
                    ////////maintain ticklers
                    //driver.SwitchTo().Frame("headerandbody");
                    //OpenMaintainTickler(driver, wdw);

                    //////// Administrator > New > Account
                    //driver.SwitchTo().Window(driver.WindowHandles[0]);
                    //driver.SwitchTo().Frame("headerandbody");
                    //NewAccount(driver, wdw, "Selen09", "Selen09", "Selen09");

                    ///////Administrator > Open > Account
                    SeleniumTest_DAL.SeleniumTest_DAL_Class newDAL = new SeleniumTest_DAL.SeleniumTest_DAL_Class();
                    newDAL.GetAcccountDetail("0023CA");
                    //int Acctid = newDAL.GetAcctId("0023CA");

                    driver.SwitchTo().Window(driver.WindowHandles[0]);
                    driver.SwitchTo().Frame("headerandbody");
                    OpenAccount(driver, wdw, "0023CA", isBaseline, sw);
                    Thread.Sleep(2000);

                    ///////go back to portal dashboard and logout
                    driver.SwitchTo().Window(driver.WindowHandles[0]);
                    driver.SwitchTo().Frame("headerandbody");
                    IWebElement logoutLink = wdw.Until(x => x.FindElement(By.LinkText("Logout")));
                    logoutLink.Click();

                    Thread.Sleep(2000);
                }
                else if (driver.FindElement(By.Id("errorLabel")).Text.Contains("Someone else is currently logged on using the same Login"))
                {
                    TakeScreenShot(driver, "Login", isBaseline);

                    Thread.Sleep(2000);

                    PortalLogin(driver, wait, userName, password, true);

                    Thread.Sleep(2000);

                    driver.SwitchTo().Frame("headerandbody");
                    IWebElement logoutLink = wdw.Until(x => x.FindElement(By.LinkText("Logout")));
                    logoutLink.Click();

                }

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

            }
            else
                Console.WriteLine("Wrong Application Title: " + driver.Title);
        }

        private static void PortalLogin(IWebDriver driver, WebDriverWait wait, string userName, string password, bool attemptReLogin)
        {
            // Find the element that's ID attribute is 'log' (Username)
            // Enter Username on the element found by above desc.
            if (!attemptReLogin)
                driver.FindElement(By.Id("txtUserName")).SendKeys(userName);

            // Find the element that's ID attribute is 'pwd' (Password)
            // Enter Password on the element found by the above desc.
            driver.FindElement(By.Id("txtPassword")).SendKeys(password);

            // Now submit the form.
            driver.FindElement(By.Id("btnLogIn")).Click();
            wait.Timeout = TimeSpan.FromSeconds(60);
        }

        private static void OpenAccount(IWebDriver driver, WebDriverWait wdw, string AccountNumber, bool isBaseline, StreamWriter sw)
        {
            IWebElement menuAdministration = wdw.Until(x => x.FindElement(By.LinkText("Administration")));
            Actions actionAdministration = new Actions(driver);
            actionAdministration.MoveToElement(menuAdministration).Perform();
            IWebElement SubMenuNew = wdw.Until(x => x.FindElement(By.LinkText("Open")));
            Actions actionNew = new Actions(driver);
            actionNew.MoveToElement(SubMenuNew).Perform();
            IWebElement SubMenuOpenAccount = wdw.Until(x => x.FindElement(By.LinkText("Account...")));
            SubMenuOpenAccount.Click();

            SeleniumTest_DAL.SeleniumTest_DAL_Class newDAL = new SeleniumTest_DAL.SeleniumTest_DAL_Class();
            //newDAL.GetAcccountDetail("0023CA");
            int Acctid = newDAL.GetAcctInfo(AccountNumber, "AcctId");
            int YrEndAmortInd = newDAL.GetAcctInfo(AccountNumber, "YrEndAmortInd");

            if (!isBaseline)
            {
                Console.WriteLine("YrEndAmortInd before Save: " + YrEndAmortInd);
               //StreamWriter sw = new StreamWriter("E:\\FT\\SeleniumTest\\SC\\GWES_Test.txt");
                sw.WriteLine(DateTime.Now.ToString("yy-MM-dd-HH_mm_ss") + " :: " + "ExtrnlAcctId=" + AccountNumber + ", AcctId=" + Acctid + ", YrEndAmortInd before Save: " + YrEndAmortInd);
            }

            //searchAccountHeader_txtAccountNumber
            driver.SwitchTo().Window(driver.WindowHandles[1]); //Account search window
            Thread.Sleep(2000);
            driver.FindElement(By.Id("searchAccountHeader_txtAccountNumber")).SendKeys(AccountNumber);
            driver.FindElement(By.Id("searchAccountHeader_btnFindNow")).Click();
            String RadioButtonId = "searchDisplayAccountAssetContact_" + Acctid.ToString();
            driver.FindElement(By.Id(RadioButtonId)).Click();
            driver.FindElement(By.Id("searchDisplayAccountAssetContact_btnSelectClose")).Click();

            driver.SwitchTo().Window(driver.WindowHandles[2]); //Account window
            Thread.Sleep(2000);
            Actions keyAction = new Actions(driver);
            keyAction.KeyDown(Keys.Alt).SendKeys("z").KeyUp(Keys.Alt).Perform();
            //keyAction.KeyDown(Keys.Alt).KeyDown(Keys.Shift).SendKeys("z").KeyUp(Keys.Alt).KeyUp(Keys.Shift).Perform();
            TakeScreenShot(driver, "Account", isBaseline);
            Thread.Sleep(2000);

            if (!isBaseline)
            {
                driver.FindElement(By.Id("chkYrEndAmrtz")).Click();
                driver.FindElement(By.Id("webToolBar_Save")).Click();
            }
            else
            {
                driver.FindElement(By.Id("webToolBar_Close"));
            }

            Thread.Sleep(2000);
            if (!isBaseline)
            {
                ////check Account.YrEndAmortInd from DB
                YrEndAmortInd = newDAL.GetAcctInfo(AccountNumber, "YrEndAmortInd");
                Console.WriteLine("YrEndAmortInd after Save: " + YrEndAmortInd);
                sw.WriteLine(DateTime.Now.ToString("yy-MM-dd-HH_mm_ss") + " :: " + "ExtrnlAcctId=" + AccountNumber + ", AcctId=" + Acctid + ", YrEndAmortInd after Save: " + YrEndAmortInd);


                //Wait for the alert to be displayed and store it in a variable
                IAlert alert = wdw.Until(ExpectedConditions.AlertIsPresent());
                //Store the alert text in a variable
                string text = alert.Text;
                //Press the OK button
                alert.Accept();
            }
            TakeScreenShot(driver, "Account", isBaseline);

            ////string title = driver.Title;
            //Screenshot ss4 = ((ITakesScreenshot)driver).GetScreenshot();
            ////string Runname = title.Substring(11, 15) + " " + DateTime.Now.ToString("yy-MM-dd-HH_mm_ss");
            //string screenshotfilename = "E:\\FT\\SeleniumTest\\SC\\GWES" + DateTime.Now.ToString("yy-MM-dd-HH_mm_ss") + ".jpg";
            //ss4.SaveAsFile(screenshotfilename, ScreenshotImageFormat.Jpeg);

            Thread.Sleep(2000);

        }

        private static void TakeScreenShot(IWebDriver driver, string fileNameIdentifier, bool isBaseline)
        {
            //string title = driver.Title;
            if (isBaseline)
                fileNameIdentifier += "_baseLine";
            Screenshot ss4 = ((ITakesScreenshot)driver).GetScreenshot();
            //string Runname = title.Substring(11, 15) + " " + DateTime.Now.ToString("yy-MM-dd-HH_mm_ss");
            string screenshotfilename = "E:\\FT\\SeleniumTest\\SC\\GWES_" + fileNameIdentifier + "_" + DateTime.Now.ToString("yy-MM-dd-HH_mm_ss") + ".jpg";
            ss4.SaveAsFile(screenshotfilename, ScreenshotImageFormat.Jpeg);
        }

        private static void OpenMaintainTickler(IWebDriver driver, WebDriverWait wdw)
        {
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

            Thread.Sleep(2000);

            //string title = driver.Title;
            Screenshot ss2 = ((ITakesScreenshot)driver).GetScreenshot();
            //string Runname = title.Substring(11, 15) + " " + DateTime.Now.ToString("yy-MM-dd-HH_mm_ss");
            //string screenshotfilename = "E:\\FT\\SeleniumTest\\SC\\" + Runname + ".jpg";
            string screenshotfilename = "E:\\FT\\SeleniumTest\\SC\\GWES" + DateTime.Now.ToString("yy-MM-dd-HH_mm_ss") + ".jpg";
            ss2.SaveAsFile(screenshotfilename, ScreenshotImageFormat.Jpeg);

            driver.Close();
        }

        private static void NewAccount(IWebDriver driver, WebDriverWait wdw, string AccountNumber, string ShortName, string AlphaShort)
        {
            IWebElement menuAdministration = wdw.Until(x => x.FindElement(By.LinkText("Administration")));
            Actions actionAdministration = new Actions(driver);
            actionAdministration.MoveToElement(menuAdministration).Perform();
            IWebElement SubMenuNew = wdw.Until(x => x.FindElement(By.LinkText("New")));
            Actions actionNew = new Actions(driver);
            actionNew.MoveToElement(SubMenuNew).Perform();
            IWebElement SubMenuNeAccount = wdw.Until(x => x.FindElement(By.LinkText("Account...")));
            SubMenuNeAccount.Click();

            driver.SwitchTo().Window(driver.WindowHandles[1]);
            Thread.Sleep(2000);
            driver.FindElement(By.Id("txtAccountNumber")).SendKeys(AccountNumber);
            driver.FindElement(By.Id("txtShortName")).SendKeys(ShortName);
            driver.FindElement(By.Id("txtAlphaSort")).SendKeys(AlphaShort);
            driver.FindElement(By.Id("chkSweep")).Click();
            SelectElement selectSweepProfile = new SelectElement(driver.FindElement(By.Id("ddlSweepProfile")));
            selectSweepProfile.SelectByIndex(6);
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

            driver.FindElement(By.Id("btnAddOwner")).Click();
            driver.SwitchTo().Window(driver.WindowHandles[2]);
            driver.FindElement(By.Id("contactSearch_contactBase_txtSingleSelect")).SendKeys("Liton");
            driver.FindElement(By.Id("contactSearch_contactBase_txtSingleSelect")).SendKeys(Keys.Tab);
            driver.FindElement(By.Id("webToolBar_SaveClose")).Click();

            driver.SwitchTo().Window(driver.WindowHandles[1]);
            driver.FindElement(By.Id("btnAddAdministrator")).Click();
            driver.SwitchTo().Window(driver.WindowHandles[2]);
            SelectElement selectAdministratorContact = new SelectElement(driver.FindElement(By.Id("ddlContact")));
            selectAdministratorContact.SelectByIndex(1);
            driver.FindElement(By.Id("webToolBar_SaveClose")).Click();

            driver.SwitchTo().Window(driver.WindowHandles[1]);
            driver.FindElement(By.Id("btnAddInvestmentOfficer")).Click();
            driver.SwitchTo().Window(driver.WindowHandles[2]);
            SelectElement selectInvestmentOfficerContact = new SelectElement(driver.FindElement(By.Id("ddlContact")));
            selectInvestmentOfficerContact.SelectByIndex(1);
            driver.FindElement(By.Id("webToolBar_SaveClose")).Click();

            driver.SwitchTo().Window(driver.WindowHandles[1]);

            //string title = driver.Title;
            Screenshot ss4 = ((ITakesScreenshot)driver).GetScreenshot();
            //string Runname = title.Substring(11, 15) + " " + DateTime.Now.ToString("yy-MM-dd-HH_mm_ss");
            string screenshotfilename = "E:\\FT\\SeleniumTest\\SC\\GWES" + DateTime.Now.ToString("yy-MM-dd-HH_mm_ss") + ".jpg";
            ss4.SaveAsFile(screenshotfilename, ScreenshotImageFormat.Jpeg);


            //driver.FindElement(By.Id("webToolBar_Save")).Click(); 

            Thread.Sleep(2000);

            //driver.Close();

        }


    }
}
