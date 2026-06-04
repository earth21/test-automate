using Microsoft.Playwright;
using System.Threading.Tasks;

namespace TestProject2.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        // 1. ประกาศตัวแปรเก็บตำแหน่งปุ่มต่างๆ (Locators) ไว้ที่เดียวกัน
        private ILocator _usernameInput => _page.Locator("[data-test='username']");
        private ILocator _passwordInput => _page.Locator("[data-test='password']");
        private ILocator _loginButton => _page.Locator("[data-test='login-button']");
        public ILocator ErrorMessage => _page.Locator("[data-test='error']"); // เปิดเป็น Public ให้ Test ดึงไปเช็คได้

        // 2. รับค่า 'page' จากไฟล์ Test เข้ามา
        public LoginPage(IPage page)
        {
            _page = page;
        }

        // 3. สร้าง Method สำหรับการทำงานเป็นชุด (Action)
        public async Task NavigateAsync()
        {
            await _page.GotoAsync("https://www.saucedemo.com/");
        }

        public async Task LoginAsync(string username, string password)
        {
            // พ่อครัวจัดการพิมพ์และคลิกให้เสร็จสรรพ
            await _usernameInput.FillAsync(username);
            await _passwordInput.FillAsync(password);
            await _loginButton.ClickAsync();
        }
    }
}