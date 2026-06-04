using Microsoft.Playwright;
using System.Threading.Tasks;

namespace TestProject2.Pages
{
    public class InventoryPage
    {
        private readonly IPage _page;

        // ==========================================
        // 🕵️‍♂️ ภารกิจของคุณ: หา Locator มาใส่ในช่องว่าง ("...")
        // ==========================================

        // 1. ช่อง Dropdown สำหรับกดเรียงลำดับ (Sort)
        private ILocator _sortDropdown => _page.Locator("[data-test=\"product-sort-container\"]");

        // 2. ป้ายชื่อสินค้าชิ้นแรก (เอาไว้เช็คว่ามันเรียงถูกต้องไหม)
        // Hint: ลองหา class หรือ data-test ของชื่อสินค้าอันแรกสุดดูครับ
        public ILocator FirstItemName => _page.Locator("[data-test=\"inventory-item-name\"]").First; 

        public InventoryPage(IPage page)
        {
            _page = page;
        }

        // ==========================================
        // 🛠️ Action Methods (สั่งให้บอททำงาน)
        // ==========================================

        // สั่งให้เปลี่ยนค่าใน Dropdown
        public async Task SortItemsAsync(string sortValue)
        {
            // 💡 ทริค: Playwright มีคำสั่งพิเศษสำหรับ Dropdown (<select>)
            // เราไม่ต้องสั่ง Click แล้วค่อยไปหา Click ตัวเลือก แต่ใช้ SelectOptionAsync ได้เลย!
            await _sortDropdown.SelectOptionAsync(new[] { sortValue });
        }
    }
}