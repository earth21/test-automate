using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO; // 🌟 ต้องมีตัวนี้สำหรับคำสั่ง File.ReadAllText
using System.Collections.Generic; // 🌟 ต้องมีตัวนี้สำหรับคำสั่ง IEnumerable และ List
using System.Threading.Tasks;
using static Microsoft.Playwright.Assertions;
using TestProject2.Pages;

namespace TestProject2
{
    [TestClass]
    public class InventorySortTest
    {
        // ==========================================
        // ส่วนที่ 1: เตรียมข้อมูล (เอาไว้ด้านบนๆ ของคลาส)
        // ==========================================

        public class TestDataModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string SortOption { get; set; }
            public string ExpectedFirstItem { get; set; }
        }

        public static IEnumerable<object[]> GetTestDataFromJson()
        {
            string filePath = "../../../TestData.json";
            string jsonString = File.ReadAllText(filePath);

            var dataList = System.Text.Json.JsonSerializer.Deserialize<List<TestDataModel>>(jsonString);

            var testCases = new List<object[]>();
            foreach (var item in dataList)
            {
                testCases.Add(new object[] { item.Username, item.Password, item.SortOption, item.ExpectedFirstItem });
            }

            return testCases;
        }

        // ==========================================
        // ส่วนที่ 2: ฟังก์ชันเทสต์หลัก 
        // ==========================================

        [DataTestMethod] // 🌟 ป้ายชื่อต้องแปะติดกับตัวฟังก์ชันเทสต์แบบนี้ครับ
        [DynamicData(nameof(GetTestDataFromJson), DynamicDataSourceType.Method)]
        public async Task TestSorting(string username, string password, string sortOption, string expectedFirstItem)
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 500 });
            var page = await browser.NewPageAsync();

            var loginPage = new LoginPage(page);
            var inventoryPage = new InventoryPage(page);

            Console.WriteLine($"🚀 ล็อกอินด้วยไอดี: {username}");

            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(username, password);

            Console.WriteLine($"🔄 กำลังสั่งจัดเรียงแบบ: {sortOption}");
            await inventoryPage.SortItemsAsync(sortOption);

            await page.WaitForTimeoutAsync(1000);

            await Expect(inventoryPage.FirstItemName).ToHaveTextAsync(expectedFirstItem);

            await browser.CloseAsync();
        }
    }
}