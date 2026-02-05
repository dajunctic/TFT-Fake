# 🆔 Identity System (Unity)

Hệ thống quản lý ID thông minh dựa trên **ScriptableObject**, giúp tự động hóa việc định danh Asset và cung cấp bộ công cụ tìm kiếm ID ngay trong Inspector.

---

## 🚀 Tính năng chính
- **Smart Reference**: Tìm kiếm và gán ID thông qua cửa sổ Search thay vì nhập chuỗi thủ công.
- **Interface Filtering**: Lọc danh sách ID theo Interface (ví dụ: chỉ hiện ID của Item, Skill, hoặc Enemy).

---

## 🛠 Hướng dẫn sử dụng

### Bước 1: Khởi tạo Database trung tâm
Tạo một lớp kế thừa từ `IdDatabase`. File này đóng vai trò là kho lưu trữ dữ liệu tập trung tạo Id không trực tiếp từ ScriptableObject. Có thể khai báo `list<string>` hoặc `string` dùng attribute `DummyId`.

```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "PhIdDatabase", menuName = "Identity System/PhIdDatabase")]
public class PhIdDatabase : IdDatabase
{
    [SerializeField, DummyId] public string testId;
    [SerializeField, DummyId] public List<string> testIds;
}
```

### Bước 2: Định nghĩa Thực thể
Mọi đối tượng cần có `ID` nên kế thừa từ `AssetId`. Có thể sử dụng OnValidate để đảm bảo ID luôn khớp với tên file, giúp tránh việc nhập sai ID thủ công.

using UnityEngine;

```cs
[CreateAssetMenu(fileName = "TestEntity", menuName = "Identity System/TestEntity")]
public class TestAsset : AssetId
{
    [SerializeField, ReadOnly] private string id;
    public override string Id => id;

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        // Tự động đồng bộ ID với tên File trong Unity
        if (id != name)
        {
            id = name;
        }
    }
#endif
}
```

### Bước 3: Sử dụng GuidReference để tham chiếu
Bạn có thể dùng `GuidReference` trong bất kỳ ScriptableObject hoặc MonoBehaviour nào để tạo ô chọn ID thông minh. Bạn có thể lọc ID theo Interface cụ thể theo prefix hoặc không.
```cs
using UnityEngine;

// Định nghĩa Interface để phân loại (Ví dụ: Chỉ dành cho Entity)
public interface IEntity : IAssetId { }

[CreateAssetMenu(fileName = "TestScriptable", menuName = "Identity System/TestScriptable")]
public class TestScriptable : ScriptableObject
{
    [Header("Basic Reference")]
    // Tham chiếu ID thông thường
    [SerializeField, GuidReference(typeof(IDummyId))] 
    public string id;

    [Header("Filtered References")]
    // Chỉ hiển thị các Asset thực thi IAssetId
    [SerializeField, GuidReference("asset", typeof(IAssetId))] 
    public string assetId;

    // Chỉ hiển thị các Asset thực thi IEntity
    [SerializeField, GuidReference("entity", typeof(IEntity))] 
    public string entityId;
}
```

### Bước 4: Tạo Entity có Interface cụ thể
Để bộ lọc ở Bước 3 hoạt động chính xác (ví dụ chỉ hiện `IEntity`), hãy cho Class của bạn thực thi Interface đó.
```cs
using UnityEngine;

[CreateAssetMenu(fileName = "EntityAsset", menuName = "Identity System/Entity Asset")]
public class EntityAsset : AssetId, IEntity // Đánh dấu thuộc nhóm IEntity
{
    [SerializeField, ReadOnly] private string id;
    public override string Id => id;

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (id != name)
        {
            id = name;
        }
    }
#endif

public interface IEntity: IAssetId
{
    
}
}
```

## Chú ý: 
Luôn bấm `Update Guild` trong `PhIdDatabase` hoặc `Dajunctic/IdentifySystem/Refresh` để cập nhật ID.