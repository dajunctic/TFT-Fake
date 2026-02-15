# TFT-Fake

Một game clone Teamfight Tactics (TFT) được xây dựng bằng Unity, bao gồm hệ thống hero, combat, shop, và economy.

---

## 📋 Tổng quan hệ thống

### 🎮 Core Systems

#### **Hero System** (`Assets/Dajunctic/Scripts/Systems/HeroSystem/`)
Hệ thống quản lý hero, bao gồm mua bán, nâng cấp, và di chuyển giữa bench/field.

| File | Chức năng |
|------|-----------|
| `HeroCombatActor.cs` | Combat actor cho hero, hỗ trợ drag-drop, swap, và sell |
| `HeroData.cs` | ScriptableObject chứa thông tin hero (rarity, traits, prefab) |
| `BenchManager.cs` | Quản lý bench (khu vực dự bị), xử lý upgrade logic |
| `FieldManager.cs` | Quản lý field (chiến trường), giới hạn số lượng unit theo level |
| `ShopController.cs` | Quản lý shop, reroll, và mua hero |
| `EconomyManager.cs` | Quản lý gold, XP, và level |

---

## ✨ Tính năng đã implement

### 1. **Hero Management**

#### **Mua hero** 
- Shop hiển thị 5 hero ngẫu nhiên dựa trên level và rarity
- Chi phí = `hero.rarity` (1-5 gold)
- Tự động refresh mỗi round Planning phase
- Reroll thủ công tốn 2 gold

#### **Bán hero** 
- **Kéo hero ra ngoài bench/field** → bán tự động
- Giá bán:
  - **1★**: `rarity × 1` gold
  - **2★**: `rarity × 3` gold  
  - **3★**: `rarity × 9` gold

#### **Nâng cấp tự động (3-to-1 merge)**
- **3 hero giống nhau + cùng star level** → tự động merge thành hero star cao hơn
- Ưu tiên giữ vị trí hero trên **field** (nếu có)
- Hỗ trợ **chain upgrade**: 3x 1★ → 1x 2★ → (nếu đủ 3x 2★) → 1x 3★
- **Cho phép mua khi bench full** nếu việc mua đó trigger upgrade (giống TFT)

#### **Drag & Drop**
- Kéo hero giữa **bench ↔ field**
- **Swap** tự động khi thả lên ô đã có hero khác
- **Cross-zone cleanup**: Tự động xóa coordinate cũ khi chuyển zone

---

### 2. **Economy System**

| Tính năng | Mô tả |
|-----------|-------|
| **Gold** | Dùng để mua hero, reroll shop |
| **XP** | Tích lũy để tăng level |
| **Level** | Quyết định số unit tối đa trên field và tỷ lệ rarity trong shop |

**Level progression:**
```
Level 1→2: 2 XP
Level 2→3: 2 XP
Level 3→4: 6 XP
Level 4→5: 10 XP
...
Level 9→10: 100 XP
```

---

### 3. **Combat System**

#### **Field Placement**
- Giới hạn số unit = `EconomyManager.Level`
- Không thể thêm unit khi đã đạt giới hạn (trừ khi swap)
- Hex-based grid cho chiến trường

#### **Behavior Tree AI**
- Hero chỉ hoạt động khi ở **field** và trong **Combat phase**
- Tự động tìm target, di chuyển, và sử dụng skill
- Hỗ trợ Ultimate, Skill, và Basic Attack

---

## 🛠️ Kiến trúc code

### **Manager Pattern**
Tất cả manager đều kế thừa `Singleton<T>` để truy cập global:
```csharp
BenchManager.Instance.AddHeroToBench(heroData);
FieldManager.Instance.CanAddUnit();
EconomyManager.Instance.SpendGold(cost);
```

### **Event System**
Sử dụng `IEvent` interface cho communication giữa các system:
```csharp
public struct HeroBoughtEvent : IEvent { public HeroData Hero; }
public struct HeroSoldEvent : IEvent { public HeroData Hero; public int GoldRefunded; }
public struct ShopRefreshedEvent : IEvent { }
```

### **Coordinate System**
- **Bench**: `Vector2Int` square grid coordinates
- **Field**: `Vector2Int` hex grid coordinates
- Mỗi hero track cả 2 coords, chỉ 1 trong 2 valid tại 1 thời điểm

---

## 🐛 Các bug đã fix

### **Cross-zone coordinate bug**
- **Vấn đề**: Hero swap từ bench → field vẫn giữ `CurrentBenchCoord`, dẫn đến `IsOnBench = true` sai
- **Fix**: `RegisterHeroToTile()` tự động clear coordinate của zone cũ

### **Bench full blocks upgrades**
- **Vấn đề**: Không mua được hero thứ 3 để trigger upgrade khi bench full
- **Fix**: `CanAcceptHero()` check xem việc mua có trigger upgrade không

### **MoveAgent cleanup**
- **Vấn đề**: Destroy hero khi merge/sell gây lỗi navigation system
- **Fix**: Disable và null `MoveAgent` trước khi `Destroy()`

### **Duplicate swap methods**
- **Vấn đề**: `SwapWithBench()` và `SwapWithField()` code giống hệt nhau
- **Fix**: Merge thành 1 method `SwapOccupant()`

---

## 🎯 Workflow điển hình

### **Mua và nâng cấp hero**
1. Mua hero từ shop → hero xuất hiện trên bench
2. Mua thêm 2 bản nữa (cùng hero, cùng star) → **auto merge 2★**
3. Lặp lại để có 3x 2★ → **auto merge 3★**

### **Quản lý bench khi full**
1. **Kéo hero không cần thiết ra ngoài** → bán để lấy gold
2. Hoặc **kéo hero lên field** để free slot bench
3. Hoặc **mua hero để trigger upgrade** (nếu đã có 2 bản)

### **Chiến đấu**
1. Kéo hero từ bench lên field
2. Chờ Combat phase → hero tự động combat theo Behavior Tree
3. Planning phase → điều chỉnh đội hình, mua thêm hero

---

## 📁 File structure

```
Assets/Dajunctic/Scripts/
├── Systems/
│   ├── HeroSystem/
│   │   ├── BenchManager.cs          # Quản lý bench + upgrade logic
│   │   ├── FieldManager.cs          # Quản lý field + unit limit
│   │   ├── HeroCombatActor.cs       # Hero behavior + drag/drop/sell
│   │   ├── HeroData.cs              # Hero ScriptableObject
│   │   ├── ShopController.cs        # Shop + reroll + buy
│   │   └── EconomyManager.cs        # Gold + XP + Level
│   └── CombatActorSystem/
│       └── GameManager.cs           # Game phase management
├── Enviroment/
│   └── DragSystem/
│       └── DragManager.cs           # Drag & drop handler
└── Inputs/
    └── InputManager.cs              # Input handling
```

---

## 🚀 Cách sử dụng

### **Setup Scene**
1. Tạo `BenchManager` GameObject với `SquareAreaView` component
2. Tạo `FieldManager` GameObject với `HexAreaView` component  
3. Tạo `ShopController` GameObject, assign `ShopData` và list `HeroData`
4. Tạo `EconomyManager` GameObject, set initial gold/level

### **Tạo Hero mới**
1. Tạo ScriptableObject `HeroData` (Right-click → Create → Panthera → HeroData)
2. Set `heroId`, `displayName`, `rarity`, `traits`, `prefab`
3. Hero prefab phải có `HeroCombatActor` component

### **Testing**
- Chạy game → Planning phase
- Shop tự động refresh
- Mua hero → xuất hiện trên bench
- Kéo hero lên field → combat khi chuyển sang Combat phase

---

## 📝 Notes

### **Upgrade Logic**
- Merge ưu tiên hero trên **field** làm primary (giữ vị trí)
- Nếu không có hero nào trên field → lấy hero đầu tiên trong list
- Chain upgrade tự động: 1★→2★→3★ trong 1 lần mua

### **Sell Value Formula**
```csharp
int multiplier = (int)Mathf.Pow(3, StarLevel - 1);
int sellValue = heroData.rarity * multiplier;
```

### **Shop Rarity Chances**
Xác định trong `ShopData.cs`, phụ thuộc vào `EconomyManager.Level`

---

## 🔧 Dependencies

- **Unity 2021.3+**
- **Odin Inspector** (optional, cho better inspector)
- **KBCore.Refs** (GuidReference system)
- Custom packages: `CombatActor`, `SquareAreaView`, `HexAreaView`

---

## 📌 TODO / Future Features

- [ ] Trait system (synergies)
- [ ] Item system (equip items cho hero)
- [ ] AI opponent
- [ ] Round progression + PvE/PvP
- [ ] Health system + damage calculation
- [ ] Visual effects cho upgrade/sell
- [ ] Sound effects
- [ ] Save/Load system

---

## 👨‍💻 Development

**Last Updated**: 2026-02-15  
**Unity Version**: 2021.3+  
**Status**: Core systems complete, combat AI functional

---

## 📄 License

[Your License Here]
