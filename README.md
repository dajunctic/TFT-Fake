# TFT-Fake

A Teamfight Tactics (TFT) clone built with Unity, featuring a modern **System-Based** architecture with **Async Data Loading** via Addressables and **Event-Driven** communication.

*Clone game Teamfight Tactics (TFT) bằng Unity, kiến trúc System-Based hiện đại, load data bất đồng bộ qua Addressables và giao tiếp Event-Driven.*

---

## 🏗️ Architecture / Kiến trúc

### Boot Flow

```
[Boot Scene] InitializeLauncher
  → Init core services (Ticker, Pool, EventDispatcher)
  → Load Launcher Scene

[Launcher Scene]
  BaseApplication  → DontDestroyOnLoad
  GameSystemManager:
    1. CreateSystems()      → AddComponent<T>() per system (code-based, no drag-drop)
    2. LoadAllDataAsync()   → Each system loads its ScriptableObject data via Addressables
    3. InitializeSystems()  → Cross-system wiring (Initialize)
    4. AllSystemsReady = true
  Launcher → waits for AllSystemsReady → LoadScene(homeScene)

[Gameplay Scene]
  BenchAreaBinder → binds scene refs to BenchSystem
  FieldAreaBinder → binds scene refs to FieldSystem
```

### 1. **GameSystemManager (The Core Hub)**

Only **1 Singleton** exists: `GameSystemManager` — manages the entire system lifecycle.

*Chỉ có 1 Singleton duy nhất quản lý toàn bộ vòng đời hệ thống.*

- Systems are **created in code** via `CreateSystems()` — no Inspector drag-drop needed.
- Only **1 ScriptableObject** to assign: `GameSystemManagerData` — holds Addressable references to all data assets.
- `AllSystemsReady` property lets `Launcher` know when it's safe to load the Home Scene.

### 2. **IGameSystem — Common Interface**
```csharp
public interface IGameSystem
{
    Task LoadDataAsync();                    // Load data via Addressables
    void Initialize(GameSystemManager mgr); // Cross-system wiring
    void Shutdown();                         // Cleanup
}
```

### 3. **Systems Overview**

| System | ScriptableObject Data | Purpose |
|---|---|---|
| `SettingsSystem` | `SettingsData` | Cursor, Audio, Graphics, Gameplay settings |
| `EconomySystem` | `EconomySystemData` | Gold, XP, Level |
| `ShopSystem` | `ShopSystemData` | Shop, Reroll, hero rarity odds |
| `BenchSystem` | `BenchSystemData` + `BenchAreaBinder` | Bench slots, merge/upgrade logic |
| `FieldSystem` | *(no-op)* + `FieldAreaBinder` | Board, unit cap |
| `ItemSystem` | `ItemSystemData` | Item bench, combine recipes |

### 4. **Scene Binders**

Scene-specific references (SquareAreaView, HexAreaView) can't live in ScriptableObjects. Solution:

*Các scene refs không thể nằm trong SO → dùng Scene Binder MonoBehaviours:*

- **`BenchAreaBinder`**: Placed in gameplay scene → calls `BenchSystem.BindArea(area, fxGuid)` on `Awake()`.
- **`FieldAreaBinder`**: Placed in gameplay scene → calls `FieldSystem.BindArea(area)` on `Awake()`.

### 5. **Communication (Decoupling)**

- **Event-Driven Actions**: UI fires Request Events, Systems listen and handle logic:
  ```csharp
  this.Raise(new RequestBuyHeroEvent { SlotIndex = 0 });
  this.Raise(new RequestRerollEvent());
  ```
- **Data Access**: Convenient extension methods:
  ```csharp
  var shop = this.GetSystem<ShopSystem>();
  var gold = this.GetSystem<EconomySystem>().Gold;
  ```

---

## ✨ Features / Tính năng

### 1. **Hero System**
- **Shop**: Randomizes 5 heroes based on Rarity/Level odds. Reroll costs gold.
- **Buy/Sell**: Drag-and-drop to sell or buy from Shop.
- **Upgrade**: Auto-merge 3× 1★ → 2★, 3× 2★ → 3★ (Chain Upgrade).
- **Bench & Field**: Smart slot management with multi-directional swapping.

### 2. **Economy System**
- **Gold**: Primary resource for buying heroes/XP.
- **XP & Level**: Buy XP to level up. Level determines max units on board and shop odds.

### 3. **Item System**
- **Item Bench**: Manages items on bench UI slots.
- **Combine**: Drag items onto heroes, auto-combine based on recipe database.

### 4. **Combat UI/UX**
- **Drag & Drop**: Smooth drag-and-drop between Bench and Field.
- **Visuals**: Info display with Rarity-based colors (Gray, Green, Blue, Purple, Gold).

---

## 📁 Project Structure / Cấu trúc thư mục

```
Assets/Dajunctic/Scripts/
├── Cores/                              # Core utilities
│   ├── ServiceLocator.cs
│   ├── IApplication.cs
│   ├── Singleton.cs, Ticker.cs, Pool.cs, EventDispatcher.cs
│   └── ...
├── Systems/
│   ├── Core/                           # System core
│   │   ├── GameSystemManager.cs       # Central hub (code-based binding)
│   │   ├── GameSystemManagerData.cs   # SO with Addressable refs
│   │   ├── GameSystemExtensions.cs    # this.GetSystem<T>() extension
│   │   ├── IGameSystem.cs            # Interface (LoadDataAsync + Init + Shutdown)
│   │   └── GameEvents.cs             # Request Events
│   ├── HeroSystem/
│   │   ├── BenchSystem.cs + BenchSystemData.cs + BenchAreaBinder.cs
│   │   ├── FieldSystem.cs + FieldAreaBinder.cs
│   │   ├── ShopSystem.cs + ShopSystemData.cs
│   │   ├── EconomySystem.cs + EconomySystemData.cs
│   │   └── HeroCombatActor.cs
│   ├── ItemSystem/
│   │   ├── ItemSystem.cs + ItemSystemData.cs
│   │   └── DraggableItem.cs
│   ├── SettingsSystem/
│   │   ├── SettingsSystem.cs + SettingsData.cs
│   ├── LoadingSystem/Runtime/
│   │   ├── InitializeLauncher.cs      # Boot scene entry
│   │   ├── BaseApplication.cs         # DontDestroyOnLoad app
│   │   └── Launcher.cs               # Waits AllSystemsReady → load home
│   └── ...
├── UI/, Entity/, SkillSystem/, ...
└── ...
```

---

## 🚀 Setup Guide / Hướng dẫn Setup

### Step 1: Create ScriptableObject Assets
*Bước 1: Tạo SO assets*

Right-click → Create → Dajunctic/Systems/:
- `GameSystemManagerData`
- `ShopSystemData`, `EconomySystemData`, `ItemSystemData`, `BenchSystemData`

### Step 2: Configure Data
*Bước 2: Cấu hình dữ liệu*

- Assign existing data into the new SOs (e.g. `shopData` → `ShopSystemData.shopData`)
- Mark each SO as **Addressable**

### Step 3: Launcher Scene
*Bước 3: Scene Launcher*

- Create/keep `GameSystemManager` GameObject with `GameSystemManager` component
- Assign the single `GameSystemManagerData` SO to its `config` field
- *(No need to drag individual systems — they're created automatically in code)*

### Step 4: Gameplay Scene
*Bước 4: Scene Gameplay*

- Add `BenchAreaBinder` to the GameObject with `SquareAreaView` → assign `benchArea` + `fxGuid`
- Add `FieldAreaBinder` to the GameObject with `HexAreaView` → assign `fieldArea`

### Step 5: Cleanup
*Bước 5: Dọn dẹp*

- Remove old System MonoBehaviour components (BenchSystem, FieldSystem...) from scene objects
- They're now auto-created via `AddComponent` by `GameSystemManager`

---

## 📝 Scripting Guide

**1. Access a System:**
```csharp
var bench = this.GetSystem<BenchSystem>();
var gold  = this.GetSystem<EconomySystem>().Gold;
// Or directly:
GameSystemManager.Instance.Bench.DoSomething();
```

**2. Perform Actions:**
```csharp
this.Raise(new RequestBuyHeroEvent { SlotIndex = 0 });
this.Raise(new RequestRerollEvent());
this.Raise(new RequestBuyXPEvent());
```

**3. Listen to Events:**
```csharp
this.RegisterListener<GoldChangedEvent>(OnGoldChanged);

private void OnGoldChanged(GoldChangedEvent evt) {
    goldText.text = evt.NewGold.ToString();
}
```

**4. Add a New System:**
```csharp
// 1. Create SO data class
[CreateAssetMenu(menuName = "Dajunctic/Systems/MySystemData")]
public class MySystemData : ScriptableObject { ... }

// 2. Create System class implementing IGameSystem
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

// 3. Register in GameSystemManager.CreateSystems() and LoadAllDataAsync()
// 4. Add AssetReference to GameSystemManagerData
```

---

## 📄 License
@2026 Dajunctic
