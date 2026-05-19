# TFT-Fake

> **Teamfight Tactics clone** — Unity · FishNet · URP · Addressables

[English](#-english) | [Tiếng Việt](#-tiếng-việt)

---

## 🇬🇧 English

### 🏗️ Architecture Overview

A server-authoritative Teamfight Tactics clone built with Unity, using a **System-Based** architecture with **Event-Driven** communication and **Multiplayer (FishNet)** synchronization.

```
Boot Scene
  └─ InitializeLauncher          ← Ticker, Pool, EventDispatcher
       └─ GameSystemManager      ← DontDestroyOnLoad singleton, creates all systems
            ├─ Addressables      ← Async data loading for all SystemData SOs
            └─ Home/Lobby Scene
                 ├─ LobbyNetworkManager   ← FishNet host/join, player list sync
                 └─ Gameplay Scene
                      ├─ Gameplay.cs           ← Phase controller (Planning / Combat)
                      ├─ PlayerDataSync.cs      ← Per-player network state (Gold, Level, Streak…)
                      ├─ Scene Binders          ← Connect scene objects to systems
                      └─ GameplayPopup          ← Main HUD (Shop, Economy, Streak, Player list)
```

---

### 🧩 Systems

| System | Data Asset | Purpose |
|---|---|---|
| `SettingsSystem` | `SettingsData` | Cursor, Audio, Graphics, Gameplay settings |
| `ShopSystem` | `ShopSystemData` | 5-slot shop, rarity odds, reroll, buy/sell |
| `BenchSystem` | `BenchSystemData` | Bench slot management, 3-star merge |
| `FieldSystem` | *(scene-driven)* | Board placement and unit count per arena |
| `ItemSystem` | `ItemSystemData` | Drag-and-drop item bench, combination recipes |
| `TraitSystem` | `TraitSystemData` | Active trait calculation, tier bonuses, UI |
| `EmotionSystem` | `EmotionSystemData` | Radial Pi-Menu emotes, synced via network |
| `RoundSystem` | `RoundSystemData` | Round progression (Carousel → PvP → PvE) |
| `SkillSystem` | *(runtime)* | Node-based (xNode) Gambit skill system, CombatActor integration |
| `PlayerSystem` | *(runtime)* | Player data, LocalPlayer reference, callbacks |
| `GlobalChampionPool` | *(runtime)* | Shared champion pool (server-authoritative) |
| `AugmentSystem` | *(runtime)* | Augment selection and application |
| `CarouselSystem` | *(runtime)* | Shared-draft carousel round logic |
| `TravelSystem` | `TravelSystemData` | Arena travel / spectating other players |
| `AISystem` | *(runtime)* | Bot player behavior |
| `ChatSystem` | *(runtime)* | In-game chat (Vivox) |

---

### 🌐 Multiplayer Architecture (FishNet)

| Component | Role |
|---|---|
| `LobbyNetworkManager` | Host/Client lifecycle, scene loading, player list broadcast |
| `PlayerDataSync` (NetworkBehaviour) | Per-player SyncVars: `Gold`, `Level`, `Exp`, `WinStreak`, `LoseStreak`, `Health` |
| `Gameplay.cs` | Server-side phase controller — rolls shop for each player, fires `ObserversRpc` |
| `ShopSystem` | Client-side only — receives shop data via `TargetRpc`, raises `ShopRefreshedEvent` |
| `TacticianNetworkMovement` | Tactician movement + emote sync (`CmdMoveTo`, `CmdPlayEmote` → `ObserversRpc`) |
| `GameplayPopup` | Subscribes to `PlayerDataSync` events (Gold/Level/Streak) for live HUD updates |

**Emote Flow:**
```
Player taps emote
  → EmotionSystem.ShowEmotion(index)       ← finds LocalPlayer.Tactician
    → TacticianNetworkMovement.CmdPlayEmote  [ServerRpc]
      → RpcPlayEmote(index)                  [ObserversRpc RunLocally=true]
        → EmotionSystem.SpawnEmotionOnActor  ← all clients see it
```

**Shop Flow:**
```
Planning phase starts (server)
  → ServerRollShop(playerDataSync)
    → TargetUpdateShop(connection, ids[])   [TargetRpc → owner only]
      → ShopSystem.SyncShopData(ids)
        → ShopRefreshedEvent raised
          → GameplayPopup.UpdateShop()      ← slots visible
```

---

### 🎮 Gameplay Loop

```
Round N
  ├─ Planning Phase  (timer countdown)
  │    ├─ Shop refresh for each player
  │    ├─ Player: buy heroes, sell, reroll, buy XP, use emotes
  │    └─ Player: drag heroes to board / bench
  │
  └─ Combat Phase
       ├─ Heroes fight automatically on each arena
       ├─ Loser takes damage (base + star level of surviving enemies)
       ├─ Win/Lose streak tracked → bonus gold
       └─ Repeat until 1 player remains
```

---

### 🛠️ Key Design Patterns

- **Event-Driven**: `this.Raise(new RequestBuyHeroEvent { ... })` — zero direct references between systems.
- **System Access**: `this.GetSystem<T>()` extension method available on any `MonoBehaviour`.
- **Async Data**: All `SystemData` ScriptableObjects are loaded via `Addressables.LoadAssetAsync<T>()`.
- **Popup Lifecycle**: `BeforeShow → SetActive(true) → ListenEvents → AfterShow` — safe event subscription order.
- **Network Ownership**: Local player found via `NetworkObject.IsOwner` search, not `Connection.FirstObject`.

---

### ⚙️ Setup Guide

1. **Open** `Assets/Scenes/BootScene.unity` as the startup scene.
2. **Create SystemData assets**: Right-click → `Create/Dajunctic/Systems/` → create each required SO.
3. **Mark Addressable**: Select each SO → Inspector → check **Addressable**, assign the address used in `GameSystemManagerData`.
4. **GameSystemManagerData**: Assign all SO references in the `config` field of `GameSystemManager` in the Launcher scene.
5. **Scene Binders**: Add `BenchAreaBinder` and `FieldAreaBinder` to the appropriate GameObjects in the Gameplay scene.
6. **PopupControllerData**: Assign all popup prefabs (`GameplayPopup`, `LobbyPopup`, `SettingsPopup`…) to the `Prefabs` list.
7. **FishNet**: The `NetworkManager` is in the Boot/Home scene. Use **Unity Multiplayer Play Mode** to test Host + Client in-editor.

---

### 📦 Dependencies

| Package | Version | Purpose |
|---|---|---|
| FishNet | latest (git) | Server-authoritative networking |
| Universal Render Pipeline | 17.3.0 | Rendering |
| Addressables | 2.9.0 | Async asset loading |
| AI Navigation | 2.0.10 | NavMesh (Tactician movement) |
| Input System | 1.18.0 | Joystick + keyboard input |
| DOTween | (imported) | UI animations |
| xNode | latest (git) | Node-based visual scripting for skills |
| Unity Services Vivox | 16.10.0 | Voice / Chat |
| Unity Multiplayer Play Mode | 2.0.2 | Multi-editor testing |

---

## 🇻🇳 Tiếng Việt

### 🏗️ Kiến trúc tổng quan

Clone Teamfight Tactics xây dựng bằng Unity, kiến trúc **System-Based** + **Event-Driven** + **Multiplayer server-authoritative** qua FishNet.

```
Boot Scene
  └─ InitializeLauncher          ← Ticker, Pool, EventDispatcher
       └─ GameSystemManager      ← Singleton DontDestroyOnLoad, tạo tất cả system
            ├─ Addressables      ← Load async data cho mọi SystemData SO
            └─ Home / Lobby Scene
                 ├─ LobbyNetworkManager   ← Host/join, đồng bộ danh sách người chơi
                 └─ Gameplay Scene
                      ├─ Gameplay.cs           ← Điều khiển phase (Planning / Combat)
                      ├─ PlayerDataSync.cs      ← Trạng thái mạng từng người (Vàng, Cấp, Streak…)
                      ├─ Scene Binders          ← Kết nối scene object vào system
                      └─ GameplayPopup          ← HUD chính (Shop, Kinh tế, Streak, Bảng người chơi)
```

---

### 🧩 Danh sách System

| System | Data Asset | Chức năng |
|---|---|---|
| `SettingsSystem` | `SettingsData` | Cài đặt chuột, âm thanh, đồ họa |
| `ShopSystem` | `ShopSystemData` | Cửa hàng 5 ô, tỉ lệ rarity, reroll, mua/bán |
| `BenchSystem` | `BenchSystemData` | Quản lý băng ghế, ghép tướng 3 sao |
| `FieldSystem` | *(scene-driven)* | Quản lý bàn cờ và số lượng quân |
| `ItemSystem` | `ItemSystemData` | Kéo thả trang bị, bảng công thức ghép |
| `TraitSystem` | `TraitSystemData` | Tính trait kích hoạt, tăng chỉ số, UI |
| `EmotionSystem` | `EmotionSystemData` | Biểu cảm radial menu, sync qua mạng |
| `RoundSystem` | `RoundSystemData` | Tiến trình vòng (Carousel → PvP → PvE) |
| `SkillSystem` | *(runtime)* | Hệ thống kỹ năng Node-based (xNode) dùng Gambit, tích hợp CombatActor |
| `PlayerSystem` | *(runtime)* | Dữ liệu người chơi, LocalPlayer, callback |
| `GlobalChampionPool` | *(runtime)* | Pool tướng chung, server quản lý |
| `AugmentSystem` | *(runtime)* | Chọn và áp dụng Augment |
| `CarouselSystem` | *(runtime)* | Logic vòng carousel chọn tướng chung |
| `TravelSystem` | `TravelSystemData` | Đi thăm sân đấu người khác |
| `AISystem` | *(runtime)* | Bot AI |
| `ChatSystem` | *(runtime)* | Chat trong game (Vivox) |

---

### 🌐 Multiplayer (FishNet)

| Thành phần | Vai trò |
|---|---|
| `LobbyNetworkManager` | Vòng đời host/client, load scene, broadcast danh sách player |
| `PlayerDataSync` | SyncVar cho từng player: `Gold`, `Level`, `Exp`, `WinStreak`, `LoseStreak`, `Health` |
| `Gameplay.cs` | Server điều khiển phase — roll shop cho từng người, gửi `ObserversRpc` |
| `ShopSystem` | Client only — nhận data qua `TargetRpc`, bắn `ShopRefreshedEvent` |
| `TacticianNetworkMovement` | Đồng bộ di chuyển + biểu cảm (`CmdMoveTo`, `CmdPlayEmote` → `ObserversRpc`) |
| `GameplayPopup` | Subscribe event `PlayerDataSync` (Gold/Level/Streak) để cập nhật HUD realtime |

---

### 🎮 Vòng chơi

```
Vòng N
  ├─ Phase Planning  (đếm ngược)
  │    ├─ Server roll shop cho từng người
  │    ├─ Người chơi: mua tướng, bán, reroll, mua XP, dùng biểu cảm
  │    └─ Người chơi: kéo tướng lên bàn / ghế chờ
  │
  └─ Phase Combat
       ├─ Tướng tự chiến đấu trên từng sân
       ├─ Người thua nhận sát thương (máu base + level quân sống)
       ├─ Theo dõi chuỗi thắng/thua → vàng thưởng
       └─ Lặp lại đến khi còn 1 người
```

---

### ⚙️ Hướng dẫn Setup

1. Mở `Assets/Scenes/BootScene.unity` làm scene khởi động.
2. Tạo SO: Chuột phải → `Create/Dajunctic/Systems/` → tạo từng `SystemData`.
3. Đánh dấu **Addressable** cho từng SO trong Inspector.
4. **GameSystemManagerData**: Gán đủ reference SO vào `config` của `GameSystemManager` trong scene Launcher.
5. **Scene Binders**: Thêm `BenchAreaBinder` và `FieldAreaBinder` vào GameObject tương ứng trong Gameplay scene.
6. **PopupControllerData**: Gán đủ prefab popup (`GameplayPopup`, `LobbyPopup`, `SettingsPopup`…) vào list `Prefabs`.
7. **Test multiplayer**: Dùng **Unity Multiplayer Play Mode** để chạy Host + Client ngay trong Editor.

---

## 📝 License

© 2026 Dajunctic. All rights reserved.
