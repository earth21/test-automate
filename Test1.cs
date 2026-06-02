using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using static Microsoft.Playwright.Assertions;

namespace TestProject2
{
    [TestClass]
    public class SauceDemoLoginTest
    {
        [DataTestMethod]
        // ใส่ครบทั้ง 8 เคส พร้อม Expected Result ในคอลัมน์ที่ 3
        [DataRow("standard_user", "secret_sauce", "Logged in successfully")]
        [DataRow("", "secret_sauce", "Show an error message")]
        [DataRow("standard_user", "", "Show an error message")]
        [DataRow("", "", "Show an error message")]
        [DataRow("problem_user", "secret_sauce", "Logged in successfully")]
        [DataRow("performance_glitch_user", "secret_sauce", "Logged in successfully")]
        [DataRow("locked_out_user", "secret_sauce", "Show an error message")]
        [DataRow("testt_user", "test_sauce", "Show an error message")]
        public async Task TestMultipleLogins(string username, string password, string expectedResult)
        {
            // ตัวแปรเก็บสถานะ (เริ่มมาให้เป็น Fail ไว้ก่อน)
            string status = "Fail";
            string date = DateTime.Now.ToString("MM/dd/yyyy, HH:mm:ss");

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 200 // หน่วงเวลาให้มองทัน 0.5 วินาทีต่อคำสั่ง
            });

            var page = await browser.NewPageAsync();

            try
            {
                await page.GotoAsync("https://www.saucedemo.com/");
                await page.Locator("[data-test='username']").FillAsync(username);
                await page.Locator("[data-test='password']").FillAsync(password);
                await page.Locator("[data-test='login-button']").ClickAsync();

                // 🧠 แยกแยะการตรวจสอบตาม Expected Result
                if (expectedResult == "Logged in successfully")
                {
                    // ถ้าคาดหวังให้ผ่าน ต้องเข้าหน้าสินค้าได้
                    // ปรับ Timeout เป็น 6000 เพื่อเผื่อเวลาให้ performance_glitch_user ด้วย
                    await Expect(page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html", new() { Timeout = 6000 });
                }
                else
                {
                    // ถ้าคาดหวังว่าต้อง Error เช็คว่ามีกล่องข้อความสีแดงเด้งขึ้นมา (รอแค่ 2 วินาทีพอ)
                    await Expect(page.Locator("[data-test='error']")).ToBeVisibleAsync(new() { Timeout = 2000 });
                }

                // รันรอดมาถึงตรงนี้ได้ แปลว่าระบบทำงานได้ตรงตาม Expectation
                status = "Pass";
            }
            catch (Exception)
            {
                status = "Fail";
                throw;
            }
            finally
            {
                // 💾 เซฟข้อมูลลงไฟล์ CSV (รูปแบบ: Username : Password, Expected Result, Status, Date)
                string reportLine = $"{username} : {password},{expectedResult},{status},{date}\n";
                File.AppendAllText("../../../TestReport.csv", reportLine);
            }

            await browser.CloseAsync();
        }
    }
}