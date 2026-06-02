using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace TestProject1
{
    [TestClass]
    public class BuuRegLoginTest
    {
        [TestMethod]
        public async Task TestStudentLogin()
        {

            Console.WriteLine("🚀 กำลังเริ่มต้น Playwright...");
            using var playwright = await Playwright.CreateAsync();

            // เปิดเบราว์เซอร์ให้มองเห็นหน้าต่าง
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 1000 // หน่วงเวลาให้มองทัน 0.5 วินาทีต่อคำสั่ง
            });

            var page = await browser.NewPageAsync();

            Console.WriteLine("🔗 กำลังเข้าสู่เว็บไซต์สำนักทะเบียน ม.บูรพา...");
            await page.GotoAsync("https://eng.buu.ac.th/");
            await page.GetByRole(AriaRole.Link, new() { Name = "เกี่ยวกับเรา " }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "ภาควิชา/หน่วยงาน" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "เว็บไซต์ภาควิชา" }).First.ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "หลักสูตร", Exact = true }).ClickAsync();
            await page.GetByRole(AriaRole.Heading, new() { Name = "ข่าวประชาสัมพันธ์" }).ClickAsync();
            await page.GetByText("ข่าวประชาสัมพันธ์ภาควิชาวิศวกรรมอุตสาหการ ม.บูรพา นำคณาจารย์และนิสิตโชว์ศักยภาพ ").ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "ติดต่อภาควิชา" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "หน้าแรก" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "หลักสูตร", Exact = true }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "เกี่ยวกับภาควิชา" }).HoverAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "เจ้าหน้าที่ภาควิชา" }).ClickAsync();









            //Console.WriteLine("⏸️ หยุดชั่วคราว! เครื่องมือ Inspector จะเด้งขึ้นมาให้คุณสำรวจ...");
            //// คำสั่งนี้คือทีเด็ด! มันจะหยุดบอทชั่วคราวและเปิดเครื่องมือช่วยหาชื่อช่องกรอกข้อมูลให้คุณ
            //await newPage.PauseAsync();



        }


    }
}