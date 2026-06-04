using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic; // 🌟 เพิ่มสำหรับ List
using System.IO;
using System.Threading.Tasks;
using static Microsoft.Playwright.Assertions;
using TestProject2.Pages;

namespace TestProject2
{
    [TestClass]
    public class LoginTest
    {
        // ==========================================
        // ส่วนที่ 1: เตรียมข้อมูล (โมเดล + ฟังก์ชันอ่านไฟล์)
        // ==========================================

        public class LoginDataModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string ExpectedResult { get; set; }
        }

        public static IEnumerable<object[]> GetLoginDataFromJson()
        {
            // ชี้ไปที่ไฟล์ JSON ตัวใหม่ที่เราเพิ่งสร้าง
            string filePath = "../../../LoginTestData.json";
            string jsonString = File.ReadAllText(filePath);

            var dataList = System.Text.Json.JsonSerializer.Deserialize<List<LoginDataModel>>(jsonString);

            var testCases = new List<object[]>();
            foreach (var item in dataList)
            {
                testCases.Add(new object[] { item.Username, item.Password, item.ExpectedResult });
            }

            return testCases;
        }

        // ==========================================
        // ส่วนที่ 2: ฟังก์ชันเทสต์หลัก
        // ==========================================

        [DataTestMethod]
        [DynamicData(nameof(GetLoginDataFromJson), DynamicDataSourceType.Method)]
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
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
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