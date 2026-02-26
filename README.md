# TFT-Fake

Một game clone Teamfight Tactics (TFT) được xây dựng bằng Unity, với kiến trúc **System-Based** hiện đại, **Event-Driven**, hỗ trợ **Async Data Loading** qua Addressables.

---

## 🏗️ Kiến trúc

### Boot Flow

```
[Boot Scene] InitializeLauncher
  → Init core services (Ticker, Pool, EventDispatcher)
  → Load Launcher Scene

[Launcher Scene]
  BaseApplication  → DontDestroyOnLoad
  GameSystemManager:
    1. CreateSystems()      → AddComponent<T>() cho mỗi system (code-based, không kéo thả)
    2. LoadAllDataAsync()   → Từng system load ScriptableObject data qua Addressables
    3. InitializeSystems()  → Cross-system wiring (Initialize)
    4. AllSystemsReady = true
  Launcher → chờ AllSystemsReady → LoadScene(homeScene)

[Gameplay Scene]
  BenchAreaBinder → bind scene refs vào BenchSystem
  FieldAreaBinder → bind scene refs vào FieldSystem
```

### 1. **GameSystemManager (The Core Hub)**
- Duy nhất **1 Singleton**: `GameSystemManager` — quản lý toàn bộ vòng đời hệ thống.
- Systems được **tạo bằng code** trong `CreateSystems()`, không cần kéo thả từng system trên Inspector.
- Chỉ cần gán **1 ScriptableObject** duy nhất: `GameSystemManagerData` — chứa Addressable references tới mọi data asset.
- Property `AllSystemsReady` cho phép `Launcher` biết khi nào đã sẵn sàng vào Home Scene.

### 2. **IGameSystem — Interface chung**
```csharp
public interface IGameSystem
{
    Task LoadDataAsync();                    // Load data qua Addressables
    void Initialize(GameSystemManager mgr); // Cross-system wiring
    void Shutdown();                         // Cleanup
}
```

### 3. **Các System**

| System | ScriptableObject Data | Chức năng |
|---|---|---|
| `SettingsSystem` | `SettingsData` | Cursor, Audio, Graphics, Gameplay settings |
| `EconomySystem` | `EconomySystemData` | Gold, XP, Level |
| `ShopSystem` | `ShopSystemData` | Shop, Reroll, xác suất tướng |
| `BenchSystem` | `BenchSystemData` + `BenchAreaBinder` (scene) | Hàng chờ, merge/upgrade |
| `FieldSystem` | *(no-op)* + `FieldAreaBinder` (scene) | Bàn cờ, giới hạn unit |
| `ItemSystem` | `ItemSystemData` | Item bench, combine recipes |

### 4. **Scene Binders**
Scene-specific references (SquareAreaView, HexAreaView) không thể nằm trong ScriptableObject. Giải pháp:
- **`BenchAreaBinder`**: MonoBehaviour trong gameplay scene → gọi `BenchSystem.BindArea(area, fxGuid)` lúc `Awake()`.
- **`FieldAreaBinder`**: MonoBehaviour trong gameplay scene → gọi `FieldSystem.BindArea(area)` lúc `Awake()`.

### 5. **Cơ chế giao tiếp (Decoupling)**
- **Event-Driven Actions**: UI bắn Request Event, System lắng nghe và xử lý:
  ```csharp
  this.Raise(new RequestBuyHeroEvent { SlotIndex = 0 });
  this.Raise(new RequestRerollEvent());
  ```
- **Data Access**: Extension Method tiện lợi:
  ```csharp
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
- **XP & Level**: Mua XP để lên cấp. Level quyết định số lượng tướng tối đa và tỉ lệ shop.

### 3. **Item System**
- **Item Bench**: Quản lý items trên bench UI.
- **Combine**: Kéo item vào tướng, tự combine theo recipe database.

### 4. **Combat UI/UX**
- **Drag & Drop**: Kéo thả mượt mà giữa Bench và Field.
- **Visuals**: Hiển thị thông tin, màu sắc theo Rarity (Xám, Lá, Dương, Tím, Vàng).

---

## 📁 Cấu trúc thư mục

```
Assets/Dajunctic/Scripts/
├── Cores/                              # Core utilities
│   ├── ServiceLocator.cs              # ServiceLocator pattern
│   ├── IApplication.cs                # Application interface
│   ├── Singleton.cs, Ticker.cs, Pool.cs, EventDispatcher.cs
│   └── ...
├── Systems/
│   ├── Core/                           # Lõi hệ thống
│   │   ├── GameSystemManager.cs       # Manager tổng (code-based binding)
│   │   ├── GameSystemManagerData.cs   # SO chứa Addressable refs
│   │   ├── GameSystemExtensions.cs    # Extension this.GetSystem<T>()
│   │   ├── IGameSystem.cs            # Interface (LoadDataAsync + Init + Shutdown)
│   │   └── GameEvents.cs             # Request Events
│   ├── HeroSystem/
│   │   ├── BenchSystem.cs            # Logic bench + BindArea()
│   │   ├── BenchSystemData.cs        # SO data
│   │   ├── BenchAreaBinder.cs        # Scene binder
│   │   ├── FieldSystem.cs            # Logic field + BindArea()
│   │   ├── FieldAreaBinder.cs        # Scene binder
│   │   ├── ShopSystem.cs             # Logic shop
│   │   ├── ShopSystemData.cs         # SO data (shopData + allHeroes)
│   │   ├── EconomySystem.cs          # Logic economy
│   │   ├── EconomySystemData.cs      # SO data (initialGold, initialLevel)
│   │   └── HeroCombatActor.cs        # Logic Unit
│   ├── ItemSystem/
│   │   ├── ItemSystem.cs             # Logic items
│   │   └── ItemSystemData.cs         # SO data (recipeDB, prefab, slots)
│   ├── SettingsSystem/
│   │   ├── SettingsSystem.cs          # Settings (loads SettingsData via Addressables)
│   │   └── SettingsData.cs            # SO data
│   ├── LoadingSystem/Runtime/
│   │   ├── InitializeLauncher.cs      # Boot scene entry point
│   │   ├── BaseApplication.cs         # DontDestroyOnLoad app
│   │   └── Launcher.cs               # Chờ AllSystemsReady → load home
│   └── ...
├── UI/
│   ├── Popups/GameplayPopup.cs
│   └── Shop/ShopSlotView.cs
└── ...
```

---

## 🚀 Hướng dẫn Setup

### Bước 1: Tạo ScriptableObject Assets
Right-click → Create → Dajunctic/Systems/:
- `GameSystemManagerData`
- `ShopSystemData`, `EconomySystemData`, `ItemSystemData`, `BenchSystemData`

### Bước 2: Cấu hình dữ liệu
- Kéo data cũ vào SO mới (VD: `shopData` → `ShopSystemData.shopData`, `allHeroes` → `ShopSystemData.allHeroes`)
- Đánh dấu **Addressable** cho từng SO asset

### Bước 3: Launcher Scene
- Tạo/giữ GameObject `GameSystemManager` với component `GameSystemManager`
- Gán duy nhất 1 SO: `GameSystemManagerData` vào field `config`
  *(Không cần kéo thả từng system nữa — chúng được tạo tự động bằng code)*

### Bước 4: Gameplay Scene
- Thêm `BenchAreaBinder` lên GameObject có `SquareAreaView` (bench area) — gán `benchArea` + `fxGuid`
- Thêm `FieldAreaBinder` lên GameObject có `HexAreaView` (field area) — gán `fieldArea`

### Bước 5: Xoá cũ
- Xoá các System MonoBehaviour components cũ (BenchSystem, FieldSystem...) khỏi scene objects
- Chúng giờ được `AddComponent` tự động bởi `GameSystemManager`

---

## 📝 Scripting Guide

**1. Truy cập System:**
```csharp
var bench = this.GetSystem<BenchSystem>();
var gold  = this.GetSystem<EconomySystem>().Gold;
// Hoặc:
GameSystemManager.Instance.Bench.DoSomething();
```

**2. Thực hiện hành động:**
```csharp
this.Raise(new RequestBuyHeroEvent { SlotIndex = 0 });
this.Raise(new RequestRerollEvent());
this.Raise(new RequestBuyXPEvent());
```

**3. Lắng nghe sự kiện:**
```csharp
this.RegisterListener<GoldChangedEvent>(OnGoldChanged);

private void OnGoldChanged(GoldChangedEvent evt) {
    goldText.text = evt.NewGold.ToString();
}
```

**4. Thêm System mới:**
```csharp
// 1. Tạo SO data class
[CreateAssetMenu(menuName = "Dajunctic/Systems/MySystemData")]
public class MySystemData : ScriptableObject { ... }

// 2. Tạo System class
public class MySystem : MonoBehaviour, IGameSystem
{
    private MySystemData _data;

    public async Task LoadDataAsync()
    {
        var handle = Addressables.LoadAssetAsync<MySystemData>(...);
        _data = await handle.Task;
    }

    public void Initialize(GameSystemManager manager) { ... }
    public void Shutdown() { ... }
}

// 3. Thêm vào GameSystemManager.CreateSystems() và LoadAllDataAsync()
// 4. Thêm AssetReference vào GameSystemManagerData
```

---

## 📄 License
@2026 Dajunctic
