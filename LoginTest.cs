using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO; // 🌟 อย่าลืม using IO สำหรับเขียนไฟล์
using System.Threading.Tasks;
using static Microsoft.Playwright.Assertions;
using TestProject2.Pages;

namespace TestProject2
{
    [TestClass]
    public class LoginTest
    {
        [DataTestMethod]
        [DataRow("standard_user", "secret_sauce", "Logged in successfully")]
        [DataRow("problem_user", "secret_sauce", "Logged in successfully")]
        
        public async Task TestLoginWithPOM(string username, string password, string expectedResult)
        {
            // 📝 เตรียมตัวแปรสำหรับทำ Report
            string status = "Fail";
            string date = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string filePath = "../../../TestReport.csv";

            // สร้างหัวตารางถ้ายังไม่มีไฟล์
            if (!File.Exists(filePath))
            {
                File.AppendAllText(filePath, "Data Test,Expected Result,Status,Date\n");
            }

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { 
                Headless = false,
                SlowMo = 1000 
            });
            var page = await browser.NewPageAsync();

            // 🧠 เรียกใช้งานพ่อครัว (POM)
            var loginPage = new LoginPage(page);

            try
            {
                // สั่งงานผ่าน POM
                await loginPage.NavigateAsync();
                await loginPage.LoginAsync(username, password);

                // ตรวจสอบผลลัพธ์
                if (expectedResult == "Logged in successfully")
                {
                    await Expect(page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html", new() { Timeout = 6000 });
                }
                else
                {
                    // ใช้ ErrorMessage จาก LoginPage มาตรวจสอบ
                    await Expect(loginPage.ErrorMessage).ToBeVisibleAsync(new() { Timeout = 2000 });
                }

                status = "Pass";
            }
            catch (Exception)
            {
                status = "Fail";
                throw;
            }
            finally
            {
                // 💾 เซฟข้อมูลลงไฟล์ CSV ในขั้นตอนสุดท้ายเสมอ
                string reportLine = $"\"{username} : {password}\",{expectedResult},{status},{date}\n";
                File.AppendAllText(filePath, reportLine);
            }

            await browser.CloseAsync();
        }
    }
}