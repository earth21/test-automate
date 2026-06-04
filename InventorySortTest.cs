using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using static Microsoft.Playwright.Assertions;
using TestProject2.Pages;

namespace TestProject2
{
    [TestClass]
    public class InventorySortTest
    {
        [DataTestMethod]
        // ทดสอบ 2 ไอดีเทียบกัน สั่งเรียงแบบ "za" (Name (Z to A))
        // ผลที่คาดหวัง: สินค้าชิ้นแรกต้องเปลี่ยนเป็น "Test.allTheThings() T-Shirt (Red)" 
        [DataRow("standard_user", "secret_sauce", "az", "Sauce Labs Backpack")]
        [DataRow("standard_user", "secret_sauce", "za", "Test.allTheThings() T-Shirt (Red)")]
        [DataRow("standard_user", "secret_sauce", "lohi", "Sauce Labs Onesie")]
        [DataRow("standard_user", "secret_sauce", "hilo", "Sauce Labs Fleece Jacket")]

        [DataRow("problem_user", "secret_sauce", "az", "Sauce Labs Backpack")]
        [DataRow("problem_user", "secret_sauce", "za", "Test.allTheThings() T-Shirt (Red)")]
        [DataRow("problem_user", "secret_sauce", "lohi", "Sauce Labs Onesie")]
        [DataRow("problem_user", "secret_sauce", "hilo", "Sauce Labs Fleece Jacket")]

        [DataRow("performance_glitch_user", "secret_sauce", "az", "Sauce Labs Backpack")]
        [DataRow("performance_glitch_user", "secret_sauce", "za", "Test.allTheThings() T-Shirt (Red)")]
        [DataRow("performance_glitch_user", "secret_sauce", "lohi", "Sauce Labs Onesie")]
        [DataRow("performance_glitch_user", "secret_sauce", "hilo", "Sauce Labs Fleece Jacket")]



        public async Task TestSorting(string username, string password, string sortOption, string expectedFirstItem)
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 500 });
            var page = await browser.NewPageAsync();

            // 1. ประกาศเรียกใช้ POM ทั้ง 2 หน้า
            var loginPage = new LoginPage(page);
            var inventoryPage = new InventoryPage(page);

            Console.WriteLine($"🚀 ล็อกอินด้วยไอดี: {username}");

            // 2. สั่งล็อกอิน (โค้ดสั้นนิดเดียวเพราะดึงมาจาก LoginPage!)
            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(username, password);
            


            // 3. เริ่มทดสอบหน้า Inventory
            Console.WriteLine($"🔄 กำลังสั่งจัดเรียงแบบ: {sortOption}");
            await inventoryPage.SortItemsAsync(sortOption);

            // 🛑 พักจอ 1 วินาที ให้คุณมองเห็นว่ามันเรียงจริงไหม
            await page.WaitForTimeoutAsync(1000);

            // 4. ตรวจสอบผลลัพธ์ (Assert)
            // เช็คว่าชื่อสินค้าชิ้นแรกสุด ตรงกับ Expected ที่เราตั้งไว้ใน DataRow ไหม?
            await Expect(inventoryPage.FirstItemName).ToHaveTextAsync(expectedFirstItem);

            await browser.CloseAsync();
        }
    }
}