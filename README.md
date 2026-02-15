# TFT-Fake

Một game clone Teamfight Tactics (TFT) được xây dựng bằng Unity, với kiến trúc **System-Based** hiện đại, **Event-Driven**, giúp code decoupled và dễ mở rộng.

---

## 🏗️ Kiến trúc & Design Pattern (New!)

Dự án đã được Refactor hoàn toàn từ mô hình **Manager Singleton** cũ sang mô hình **Centralized System Manager** kết hợp **Event-Driven Communication**.

### 1. **GameSystemManager (The Core Hub)**
- Thay vì mỗi Manager là một Singleton (`BenchManager.Instance`, `FieldManager.Instance`...), giờ đây chỉ có **duy nhất 1 Singleton**: `GameSystemManager`.
- `GameSystemManager` quản lý vòng đời (`Initialize`, `Shutdown`) của tất cả các System con.

### 2. **Các System (IGameSystem)**
Các tính năng cốt lõi được tách thành các System độc lập, implement interface `IGameSystem`:
- **`BenchSystem`**: Quản lý hàng chờ, logic merge/upgrade (thay thế `BenchManager`).
- **`FieldSystem`**: Quản lý bàn cờ, giới hạn unit (thay thế `FieldManager`).
- **`ShopSystem`**: Quản lý Shop, Reroll, xác suất tướng (thay thế `ShopController`).
- **`EconomySystem`**: Quản lý Vàng, XP, Level (thay thế `EconomyManager`).

### 3. **Cơ chế giao tiếp (Decoupling)**
- **Event-Driven Actions**: UI không gọi hàm trực tiếp của System để thay đổi dữ liệu. Thay vào đó, UI bắn **Request Event**:
  ```csharp
  // UI Code
  this.Raise(new RequestBuyHeroEvent { SlotIndex = 0 });
  this.Raise(new RequestRerollEvent());
  ```
  System lắng nghe và xử lý logic.
- **Data Access**: Để lấy dữ liệu hiển thị (View), sử dụng Extension Method tiện lợi:
  ```csharp
  // Truy cập System bất kỳ đâu
  var shop = this.GetSystem<ShopSystem>();
  var gold = this.GetSystem<EconomySystem>().Gold;
  ```

---

## ✨ Tính năng chính

### 1. **Hero System**
- **Shop**: Random 5 tướng theo tỉ lệ Rarity/Level. Reroll tốn vàng.
- **Mua/Bán**: Kéo thả để bán hoặc mua từ Shop.
- **Upgrade**: Tự động ghép 3 tướng 1★ -> 2★, 3 tướng 2★ -> 3★ (Chain Upgrade).
- **Bench & Field**: Logic quản lý slot thông minh, hỗ trợ swap vị trí đa dạng.

### 2. **Economy System**
- **Gold**: Tài nguyên chính để mua tướng/XP.
- **XP & Level**: Mua XP để lên cấp. Cấp độ quyết định số lượng tướng tối đa trên bàn cờ và tỉ lệ ra tướng xịn trong Shop.

### 3. **Combat UI/UX**
- **Drag & Drop**: Kéo thả mượt mà giữa Bench và Field.
- **Visuals**: Hiển thị thông tin, màu sắc theo Rarity (Xám, Lá, Dương, Tím, Vàng).

---

## 📁 Cấu trúc thư mục (Refactored)

```
Assets/Dajunctic/Scripts/
├── Systems/
│   ├── Core/                        # Lõi hệ thống mới
│   │   ├── GameSystemManager.cs     # Manager tổng
│   │   ├── GameSystemExtensions.cs  # Extension this.GetSystem<T>()
│   │   ├── IGameSystem.cs           # Interface chung
│   │   └── GameEvents.cs            # Định nghĩa các Request Event
│   ├── HeroSystem/                  # Logic game cụ thể
│   │   ├── BenchSystem.cs           # (Renamed from BenchManager)
│   │   ├── FieldSystem.cs           # (Renamed from FieldManager)
│   │   ├── ShopSystem.cs            # (Renamed from ShopController)
│   │   ├── EconomySystem.cs         # (Renamed from EconomyManager)
│   │   └── HeroCombatActor.cs       # Logic Unit
├── UI/
│   ├── Popups/
│   │   └── GameplayPopup.cs         # UI chính (đã update dùng Event)
│   └── Shop/
│       └── ShopSlotView.cs          # UI Slot Shop
```

---

## 🚀 Hướng dẫn Setup Scene (Quan trọng!)

Do thay đổi kiến trúc, Scene cần được setup lại như sau:

1.  **Tạo GameObject**: Đặt tên `GameSystemManager`.
2.  **Add Component**:
    - Thêm script `GameSystemManager`.
    - Thêm script `BenchSystem`.
    - Thêm script `FieldSystem`.
    - Thêm script `EconomySystem`.
    - Thêm script `ShopSystem`.
3.  **Link References (Inspector)**:
    - Kéo các System (`Bench`, `Field`...) vào slot tương ứng trong component `GameSystemManager` (hoặc để nó tự tìm `GetComponentInChildren`).
    - Trong `BenchSystem` / `FieldSystem`: Assign `AreaView` tương ứng.
    - Trong `ShopSystem`: Assign `ShopData` và list `HeroData`.

---

## 🐛 Các Bug đã fix gần đây

1.  **Architecture Refactor**: Loại bỏ Singleton rác, chuyển sang System Manager tập trung.
2.  **Bench Full Logic Check**: Fix lỗi check tọa độ `x` gây sai logic khi bench full (đã chuyển sang check `y` hoặc valid coord chuẩn).
3.  **UI Decoupling**: UI không còn phụ thuộc chặt vào logic game, giảm thiểu lỗi null reference khi khởi tạo sai thứ tự.
4.  **Resource Loading**: Fix lỗi Shop không hiện tướng nếu chưa assign list HeroData (đã thêm fallback `Resources.LoadAll`).

---

## 📝 Scripting Guide (Cho Dev)

**1. Muốn gọi một System:**
```csharp
// Cũ (Don't use): BenchManager.Instance.DoSomething();
// Mới (Recommended):
var bench = this.GetSystem<BenchSystem>();
bench.DoSomething();
```

**2. Muốn thực hiện hành động (Mua, Reroll...):**
```csharp
// Bắn Event Request
this.Raise(new RequestByHeroEvent { SlotIndex = 0 });
this.Raise(new RequestRerollEvent());
```

**3. Muốn nghe sự kiện từ System:**
```csharp
// Trong method Initialization/ListenEvents
this.RegisterListener<GoldChangedEvent>(OnGoldChanged);

private void OnGoldChanged(GoldChangedEvent evt) {
    // Update UI
}
```

---

## 📄 License
@2026 Dajunctic
