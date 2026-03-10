# Unity Setup Guide - TFT Missing Systems

Sau khi mình triển khai code, bạn cần thực hiện các bước sau trong Unity Editor để hệ thống hoạt động chính xác:

## 1. Cấu hình ScriptableObjects
- **RoundSystemData**: Đảm bảo các `RoundData` đã được tick vào ô `hasAugment` ở các vòng 2-1, 3-2, 4-2.
- **GameSystemManagerData**: Bạn không cần kéo thả tay các System mới. Hệ thống sẽ tự động tạo GameObject cho `PlayerSystem`, `AugmentSystem`, etc. khi chạy.

## 2. Thiết lập UI
- **Scoreboard (Bảng xếp hạng)**:
    - Kéo thả Prefab `PlayerUI` vào một Vertical Layout Group trong Canvas Chính.
    - Đảm bảo mỗi `PlayerUI` có các component `HPBarFill` được gán đúng.
- **Augment Popup**:
    - Gán prefab `AugmentPopup` vào `PopupControllerData`.
    - Thiết lập các `AugmentData` (ScriptableObjects) trong folder Data và kéo vào `AugmentSystemData`.

## 3. Carousel (Vòng đi chợ)
- Tạo một Scene hoặc một khu vực trong Scene hiện tại có gắn `CarouselCenter` (một Transform rỗng).
- Kéo thả `CarouselCenter` này vào `CarouselSystem` trong Inspector (hoặc để nó tự tìm theo tag "CarouselCenter").

## 4. AI Opponents
- Hệ thống sẽ tự động tạo 7 đối thủ AI. 
- Bạn có thể điều chỉnh cấu hình độ khó của AI (tần suất mua tướng, lên cấp) trong `AISystemData`.

---
**Lưu ý**: Nếu gặp lỗi "System not found", hãy kiểm tra lại xem bạn đã thêm System đó vào danh sách khởi tạo của `GameSystemManager.cs` chưa (mình sẽ cố gắng tự động cập nhật file này cho bạn).
