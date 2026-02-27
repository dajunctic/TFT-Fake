# TFT-Fake

[English](#-english) | [Tiếng Việt](#-tiếng-việt)

---

## 🇺🇸 English

### 🏗️ Architecture

A Teamfight Tactics (TFT) clone built with Unity, featuring a modern **System-Based** architecture with **Async Data Loading** via Addressables and **Event-Driven** communication.

#### Boot Flow
1. **[Boot Scene]**: `InitializeLauncher` init core services (Ticker, Pool, EventDispatcher).
2. **[Launcher Scene]**: `GameSystemManager` creates systems, loads SO data via Addressables, and initializes cross-system wiring.
3. **[Gameplay Scene]**: Scene Binders (`BenchAreaBinder`, `FieldAreaBinder`) connect specific scene references to the systems.

#### Systems Overview

| System | ScriptableObject Data | Purpose |
|---|---|---|
| `SettingsSystem` | `SettingsData` | Cursor, Audio, Graphics, Gameplay settings |
| `EconomySystem` | `EconomySystemData` | Gold, XP, Level logic |
| `ShopSystem` | `ShopSystemData` | Card probability, Reroll, Buy/Sell |
| `BenchSystem` | `BenchSystemData` | Bench slot management, Star upgrades |
| `FieldSystem` | *(no-op)* | Board unit counts and placement |
| `ItemSystem` | `ItemSystemData` | Item bench and combination recipes |
| `EmotionSystem` | `EmotionSystemData` | Manages player emotes (Pi-Menu) |
| `TraitSystem` | `TraitSystemData` | Calculates active traits and applies stat bonuses |

#### Communication
- **Event-Driven**: Systems listen for `IEvent` (e.g., `RequestBuyHeroEvent`).
- **Decoupled**: Use `this.Raise(event)` or `this.GetSystem<T>()` from any MonoBehaviour.

---

### ✨ Features
- **Hero System**: 5-slot Shop with rarity odds, 3-star upgrade chain, and grid-based placement.
- **Economy**: Gold management and XP-based leveling system.
- **Item System**: Drag-and-drop item combination with recipe database.
- **Emotion System**: Radial menu (Pi-Menu) to trigger emotes on your character.
- **Trait System**: Dynamic trait calculation, tier-based bonuses (Bronze/Silver/Gold/Chromatic), and UI integration.
- **Combat Actor**: Custom AI state machine (Dummy vs Heroes).

---

### � Setup Guide
1. **Create Assets**: Right-click → `Create/Dajunctic/Systems/` → Create all required `SystemData` SOs.
2. **Addressables**: In Unity, mark all created `SystemData` SOs as **Addressable**.
3. **GameSystemManager**: Assign your `GameSystemManagerData` SO to the Manager in the Launcher scene.
4. **Scene Binders**: Add `BenchAreaBinder` & `FieldAreaBinder` to your Gameplay scene objects.

---

## 🇻🇳 Tiếng Việt

### 🏗️ Kiến trúc

Bản clone Teamfight Tactics (TFT) được phát triển bằng Unity với kiến trúc **System-Based** hiện đại, sử dụng **Addressables** để tải dữ liệu bất đồng bộ và **Event-Driven** để giao tiếp giữa các thành phần.

#### Quy trình khởi động (Boot Flow)
1. **[Boot Scene]**: `InitializeLauncher` khởi tạo các dịch vụ lõi (Ticker, Pool, EventDispatcher).
2. **[Launcher Scene]**: `GameSystemManager` tự động tạo các System, load dữ liệu ScriptableObject qua Addressables và thực hiện kết nối (`Initialize`).
3. **[Gameplay Scene]**: Các `Scene Binder` kết nối các đối tượng trong scene với hệ thống tương ứng.

#### Tổng quan các Hệ thống

| Hệ thống | ScriptableObject Data | Chức năng |
|---|---|---|
| `SettingsSystem` | `SettingsData` | Cài đặt Chuột, Âm thanh, Đồ họa |
| `EconomySystem` | `EconomySystemData` | Quản lý Vàng, XP và Cấp độ |
| `ShopSystem` | `ShopSystemData` | Tỉ lệ rơi tướng, Reroll, Mua/Bán |
| `BenchSystem` | `BenchSystemData` | Quản lý hàng chờ, nâng cấp tướng 2/3 sao |
| `FieldSystem` | *(no-op)* | Quản lý bàn cờ và số lượng quân |
| `ItemSystem` | `ItemSystemData` | Kho đồ và công thức ghép trang bị |
| `EmotionSystem` | `EmotionSystemData` | Hệ thống cảm xúc/emote (Radial Menu) |
| `TraitSystem` | `TraitSystemData` | Tính toán kích hoạt Tộc/Hệ và cộng chỉ số |

#### Giao tiếp (Decoupling)
- **Event-Driven**: Sử dụng các sự kiện yêu cầu như `this.Raise(new RequestBuyHeroEvent { ... })`.
- **Truy cập System**: Sử dụng Extension method `this.GetSystem<T>()` cực kỳ tiện lợi.

---

### ✨ Tính năng
- **Hệ thống Tướng**: Cửa hàng 5 ô với tỉ lệ theo cấp độ, tự động hợp nhất tướng 3 con (1★→2★→3★).
- **Kinh tế**: Quản lý vàng và cơ chế mua kinh nghiệm lên cấp.
- **Trang bị**: Kéo thả trang bị vào tướng, tự động ghép theo bảng công thức.
- **Cảm xúc (Emotions)**: Menu xoay (Pi-Menu) để thể hiện cảm xúc trên nhân vật chính.
- **Hệ thống Tộc/Hệ (Traits)**: Tự động tính toán mốc kích hoạt, cộng chỉ số (Stat Modifiers) và hiển thị UI chuẩn TFT.
- **Combat Actor**: Hệ thống AI tinh gọn (Dummy vs Heroes).

---

### 🚀 Hướng dẫn Setup
1. **Tạo Assets**: Chuột phải → `Create/Dajunctic/Systems/` → Tạo các file `SystemData` tương ứng.
2. **Addressables**: Đánh dấu các file `SystemData` vừa tạo là **Addressable** trong cửa sổ Unity.
3. **GameSystemManager**: Gán file `GameSystemManagerData` vào trường `config` của Manager trong scene Launcher.
4. **Scene Binders**: Thêm `BenchAreaBinder` và `FieldAreaBinder` vào các đối tượng tương ứng trong scene Gameplay.

---

## 📄 License
@2026 Dajunctic
