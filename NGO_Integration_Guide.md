# Hướng Dẫn Tích Hợp Netcode for GameObjects (NGO) Từ A -> Z cho TFT-Fake

Tài liệu này cung cấp hướng dẫn toàn diện để chuyển đổi project `TFT-Fake` từ một game offline (giả lập player) sang một game Multiplayer Client-Server thực thụ sử dụng **Unity Netcode for GameObjects (NGO)**.

---

## 1. Cài Đặt và Thiết Lập Ban Đầu (Setup)

### 1.1. Cài đặt Package NGO
1. Mở Unity, vào **Window > Package Manager**.
2. Đổi tuỳ chọn ở góc trái trên cùng thành **Packages: Unity Registry**.
3. Tìm kiếm **Netcode for GameObjects** (Nên sử dụng bản mới nhất được Verified). Nhấn **Install**.
4. *(Tuỳ chọn cực kỳ khuyên dùng)* Hãy cài thêm **Multiplayer Tools** để có các công cụ Network Profiler và Network Simulator giúp giả lập ping/lag.

### 1.2. Tạo NetworkManager
1. Tạo một Scene mới làm Bootstrapper, hoặc ở `HomeScene`.
2. Tạo một GameObject rỗng tên là `NetworkManager`.
3. Add component **NetworkManager** vào GameObject đó.
4. Ở mục `NetworkTransport`, bấm **Select Transport...** và chọn **UnityTransport** (Sau đó nó sẽ tự add component UnityTransport nằm bên dưới).
5. Tick chọn **Dont Destroy on Load** trên NetworkManager để nó tồn tại xuyên suốt các scene.

---

## 2. Kiến Trúc "1 Máy Host" Cho Game Auto-Battler (TFT)

Đối với thể loại Cờ Nhân Phẩm, hệ thống của bạn sẽ được thiết kế theo mô hình **Listen Server (Host - Client)** với 1 máy tính đóng vai trò là Host tuyệt đối. Do thẩm quyền tính toán nằm hoàn toàn ở Host nên có thể loại trừ can thiệp gian lận từ các Client khác.

*   **Host (Máy Chủ Kiêm Người Chơi):** Một máy tính sẽ đóng vai trò vừa là Server vừa là Client 1.
    - **Ở vai trò Server (`IsServer == true`):** Chạy duy nhất khối Logic Game như đếm thời gian (`Gameplay`), quản lý AI đánh nhau, tính toán máu, nhận lệnh mua tướng, trừ vàng. 
    - **Ở vai trò Client (`IsClient == true`):** Đồng thời cũng render Graphics, giao diện, và cho phép chính chủ máy bấm nút (Input). Thao tác của Host cũng bị bắt buộc gửi Request về lại nội bộ Host xử lý nhằm chuẩn hoá logic.
*   **Client (Các Người Chơi Còn Lại - `IsClient == true`, `IsServer == false`):** Kết nối vào máy Host để chơi. Chỉ mang vai trò "màn hình hiển thị". Tức là nhận toạ độ tướng đi từ Host thông qua cập nhật `NetworkTransform`, nhận Data Máu từ `NetworkVariable` để cập nhật UI. Mọi thao tác bắt buộc gởi Request `[ServerRpc]` tới Host duyệt.

---

## 3. Code Giai Đoạn 1: Đồng Bộ GameState & Phase

Hiện tại system của bạn dùng `Gameplay.cs` là một MonoBehaviour thuần túy, mọi thứ chạy dựa trên `Time.deltaTime` local. Giờ ta phải chuyển sang cho Server điều khiển.

### Biến đổi `Gameplay` thành `NetworkBehaviour`

```csharp
using Unity.Netcode;
using UnityEngine;

// Sửa BaseView / MonoBehaviour thành NetworkBehaviour
public class GameplayNetwork : NetworkBehaviour
{
    // Sử dụng NetworkVariable để Client tự cập nhật giá trị mà không cần gửi gói tin liên tục thủ công
    public NetworkVariable<GameplayPhase> CurrentPhase = new NetworkVariable<GameplayPhase>();
    public NetworkVariable<float> Timer = new NetworkVariable<float>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Chỉ Server mới được quyền StartPhase
            StartPhaseServer(GameplayPhase.Planning);
        }
        
        // Client có thể lắng nghe sự kiện khi Phase thay đổi
        CurrentPhase.OnValueChanged += OnPhaseChanged;
    }

    private void Update()
    {
        // Nhớ RULE QUAN TRỌNG NHẤT: Logic đếm thời gian chỉ Server mới chạy
        if (!IsServer) return;

        if (Timer.Value > 0)
        {
            Timer.Value -= Time.deltaTime;
            if (Timer.Value <= 0)
            {
                OnTimerCompleteServer(); // Hàm này nằm nội bộ trên Server, gọi chuyển sang combat...
            }
        }
    }
    
    private void StartPhaseServer(GameplayPhase phase)
    {
        CurrentPhase.Value = phase;
        Timer.Value = GetDurationForPhase(phase); // Tự viết hàm map thời gian
    }
}
```

---

## 4. Code Giai Đoạn 2: Quản Lý Player, Input và Kinh Tế (Economy)

Trong NGO, mỗi người chơi kết nối vào gọi là 1 `Client`, được định danh bằng một `ClientId` (ulong). Server phải biết ClientId nào là PlayerId (ví dụ 1, 2, 3...) trong game.

### Server lưu trạng thái người chơi
Chuyển đổi `EconomySystem` và `PlayerHp` dùng NetworkVariable.

```csharp
public class PlayerStateSync : NetworkBehaviour
{
    public NetworkVariable<int> Health = new NetworkVariable<int>(100);
    public NetworkVariable<int> Gold = new NetworkVariable<int>(0);
    public NetworkVariable<int> Level = new NetworkVariable<int>(1);

    // Bất kỳ lúc nào Server bị đánh, nó trừ Health.Value, hệ thống của Netcode 
    // sẽ tự động gửi sự kiện OnValueChanged về đến TẤT CẢ client để client update thanh máu UI.
}
```

### Xử lý Input từ Client (Sử dụng ServerRpc)
Khi người chơi thao tác, họ không gọi hàm mua đồ trực tiếp, mà họ "xin" Server.

```csharp
public class PlayerInputHandler : NetworkBehaviour
{
    // Bắt sự kiện người chơi bấm nút Mua Cửa hàng. (Host cũng gọi hàm này vì Host cũng là Client)
    public void OnBuyChampionClicked(int shopSlotIndex)
    {
        if (IsClient)
        {
            // Cả Host lẫn Client thường đều gửi Request tới block logic máy chủ xử lý
            BuyChampionServerRpc(shopSlotIndex); 
        }
    }

    // Hàm Rpc được thực thi duy nhất TẠI MÁY HOST (Server-side)
    [ServerRpc]
    private void BuyChampionServerRpc(int shopSlotIndex, ServerRpcParams rpcParams = default)
    {
        // 1. Phân biệt người vừa gọi là ai (SenderClientId)
        ulong senderId = rpcParams.Receive.SenderClientId;
        int playerId = GetPlayerIdFromClient(senderId);

        // 2. Server kiểm tra trong ShopSystem, check xem player này có đủ vàng không?
        // 3. Nếu ĐỦ vàng -> Server trừ Vàng trên NetworkVariable.
        // 4. Lấy Prefab Tướng bay về Bench. (Xem mục 5)
        // 5. Cập nhật số lượng tướng toàn Server (Pool System).
    }
}
```

---

## 5. Code Giai Đoạn 3: Netcode GameObjects (Sinh Ra Tướng)

Bất kỳ Tướng/Quái/Item nào muốn tồn tại trên cả Server và Client đều phải:
1. Thêm `NetworkObject` Component.
2. Thêm `NetworkTransform` Component vào prefab (Để đồng bộ Vị Trí / Xoay).
3. Kéo prefab đó thả vào mục **NetworkPrefabs** trong Component `NetworkManager`.

### Quy Trình Nhặt Tướng Của Server
```csharp
// Khi Server quyết định Player 1 mua được tướng:
public void SpawnChampionForPlayer(string championId, ulong clientIdPlayer, Vector3 spawnPos)
{
    // Dĩ nhiên chạy ở IsServer == true
    GameObject championPrefab = Resources.Load<GameObject>($"Prefabs/{championId}");
    GameObject spawnedChamp = Instantiate(championPrefab, spawnPos, Quaternion.identity);
    
    // Gán NetworkObject mới với Owner là người mua nó
    var netObj = spawnedChamp.GetComponent<NetworkObject>();
    netObj.SpawnWithOwnership(clientIdPlayer);
}
```

---

## 6. Code Giai Đoạn 4: Đồng Bộ Combat (Tính Toán & Hình Ảnh)

Chúng ta có `TravelSystem` mang tướng qua sân nhau, hoặc Matchmaking đưa `GuestId` gặp `HomeId`.
Auto-battlers sinh ra cả trăm cục vật lý, nếu dùng `NetworkTransform` đồng bộ mọi frame có thể làm nghẽn băng thông. Tuy nhiên trong quy mô TFT 1v1 đến 8v8 cục bộ, NGO của Unity dư sức gánh nếu tối ưu tốt NetworkTransform (chỉ Sync Position & Rotation, tắt Scale).

### Cách Làm Tốt Nhất Cho Combat TFT bằng NGO:
1. Trạng thái `Actor` (Tướng): Gắn `NetworkVariable<int> HP` và `NetworkVariable<int> Mana`.
2. Logic Combat tắt ở Client: Tách logic tìm Target, di chuyển (`NavMeshAgent`), ra chiêu... bao bọc hoàn toàn bởi `if (!IsServer) return;`. Nghĩa là chỉ Server mới tự đánh nhau.
3. Chuyển Động Hình Ảnh: Sử dụng `NetworkTransform`. Server đi đến đâu, Client thấy tướng lướt tới đó theo thời gian thực (có thể bật interpolation để mượt).
4. Ra Chiêu (Hiệu ứng Vfx): Khi Server thực thi vụ nổ, gọi `ClientRpc` để báo tất cả Client tự bật Particle.

```csharp
public class ChampionActorNetwork : NetworkBehaviour
{
    public NetworkVariable<int> Health = new NetworkVariable<int>(1000);

    // Update này chỉ Server chạy State Machine
    private void Update()
    {
        if (!IsServer) return;
        
        // Target tìm kiếm, Di chuyển, Attack...
        if (CanAttack)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        // Trừ máu nội bộ Server
        Target.Health.Value -= CalculateDamage();

        // Gọi mọi Client chạy Animation chém
        PlayAttackAnimationClientRpc();
    }

    // Hàm gọi từ server truyền lệnh ép các Client phải thực thi
    [ClientRpc]
    private void PlayAttackAnimationClientRpc()
    {
        // Code này sẽ chạy trên tất cả các máy Client
        animator.SetTrigger("Attack");
        // AudioManager.PlaySound("Slash");
    }
}
```

---

## 7. Khởi Chạy Mô Hình 1 Máy Host (Local / Internet)

Để một máy tính trở thành Host, chúng ta có 2 cách thực thi thiết thực nhất:

1. **Mạng LAN / Localhost (Dùng để Test song song):** 
   - Máy Host bấm nút gọi hàm: `NetworkManager.Singleton.StartHost();`
   - Các máy Client/Editor Test khác gọi: `NetworkManager.Singleton.StartClient();` (Và đảm bảo trỏ địa chỉ của Transport về địa chỉ IP nội bộ của máy Host đó).
   
2. **Kết nối xuyên Internet để chơi ngoài đời (Dịch vụ Relay):** 
   - Không cần mở Port lằng nhằng ở Modem cho máy Host, ta sử dụng package `com.unity.services.relay`.
   - Máy Host sẽ gọi API Unity để báo là mình là Cụm Máy Chủ Host, lấy 1 chuỗi `Join Code` và sau đó gọi lệnh Host bình thường.
   - Các máy Client xin `Join Code` (ví dụ thông qua Discord chat), nhập `Join Code` vào UI của mình, và NetworkManager sẽ tự động thiết lập đường ống P2P về lại đúng máy Host đó.

---

## 🔥 Tóm Lược Kế Hoạch Hiện Thực Tuần Tự (Checklist):

> [!IMPORTANT]
> Quy tắc vàng trong Multiplayer: **Luôn đặt nghi vấn "Đoạn code này ai đang chạy? Server hay Client?"**

- [ ] **1. Bootstrapping:** Cài đặt NGO vào Project. Gắn `NetworkManager` vào Boot Scene. Thêm toàn bộ các Prefab Champions, Projectiles (chứ không phải UI) vào danh sách _NetworkPrefabs_.
- [ ] **2. Master State:** Đổi các Singleton Systems (`Gameplay.cs`, `EconomySystem.cs`) sang Inherit `NetworkBehaviour`. Đồng bộ Phase Timer bằng biến `NetworkVariable`.
- [ ] **3. Input Validation:** Dùng phương thức `[ServerRpc]` cho các hành động người chơi: Mua sắm, Roll, Kéo thả tướng trên sân, kéo đồ đạc. Cấm mọi hành vi logic local thay đổi game. Giữa Client và Server giao tiếp thông qua ID chứ không phải Reference gốc (Ex: gửi tên hàm `"Qiyana"`, vị trí `Vector2Int(2,3)` chứ không gửi Object).
- [ ] **4. Core Combat Loop:** Kẹp `if(IsServer)` cho tất cả Behaviour AI chạy pathfinding. 
- [ ] **5. Visual FX:** Tích hợp `ClientRpc` thay thế cho logic Particle System, Float Text, để giảm tải Network Sync. Biến Client trở thành "màn hình Livestream" của Server.
- [ ] **6. Testing:** Sử dụng Package **ParrelSync** để chia ra 2 cửa sổ Unity Editor ảo song song cùng chạy và kiểm tra việc bắn đạn trên NGO. Trang chủ: [ParrelSync GitHub](https://github.com/VeriorPies/ParrelSync).
