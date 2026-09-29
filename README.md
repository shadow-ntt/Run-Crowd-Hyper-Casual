# Run Crowd Hyper Casual 🏃‍♂️👥

> Một tựa game **3D Hyper-Casual Runner** theo phong cách điều khiển đám đông (Crowd Runner / Count Masters) được xây dựng trên nền tảng **Unity 6 (URP)**.

---

## 📖 1. Giới thiệu dự án

**Run Crowd Hyper Casual** đưa người chơi vào trải nghiệm điều khiển một nhân vật khởi đầu, chạy qua các cánh cổng toán học nhân bản quân số, né tránh các bẫy chướng ngại vật nguy hiểm và giao chiến với các nhóm kẻ địch trên đường chạy. Mục tiêu cuối cùng là bảo toàn số lượng runner đông nhất có thể để vượt qua vạch đích và nhận lượng tiền thưởng (Coins) khổng lồ.

Dự án được thiết kế theo hướng module hóa cao (Modular Architecture), áp dụng chặt chẽ kiến trúc hướng sự kiện (Event-Driven) và hệ thống ScriptableObject giúp dễ dàng mở rộng màn chơi, tùy biến skin và quản lý dữ liệu người chơi.

---

## 🌟 2. Tính năng nổi bật (Key Features)

### 👥 Cơ chế Đám đông & Thuật toán Fermat's Spiral
- Quân số đám đông được tự động tính toán và sắp xếp vị trí tự nhiên theo mô hình xoắn ốc **Fermat's Spiral**:
  $$r = \text{radius} \cdot \sqrt{n}, \quad \theta = n \cdot 137.5^\circ$$
- Đám đông tự động co dãn, di chuyển mượt mà (Lerp) về hình khối khi tăng thêm thành viên hoặc khi bị phân tán do va quẹt địa hình.

### 🚪 Cổng toán học tương tác (Math Doors)
- Hỗ trợ 4 loại phép tính:
  - **Cộng (`+`)**: Tăng trực tiếp số lượng runner.
  - **Trừ (`-`)**: Giảm số lượng runner (giữ tối thiểu 1).
  - **Nhân (`x`)**: Nhân số lượng runner hiện tại lên nhiều lần.
  - **Chia (`/`)**: Chia giảm số lượng runner.
- Tự động thay đổi màu sắc trực quan (Xanh dương cho tăng, Đỏ cho giảm).
- Tự động xác định cánh cổng người chơi đi qua dựa trên tọa độ trục $X$.

### ⚔️ Kẻ địch & Chiến đấu (Combat System)
- Nhóm kẻ địch (`EnemyGroup`) được bố trí dọc đường chạy với số lượng định sẵn.
- **Tự động quét người chơi (`ScanPlayer`)**: Khi người chơi tiến vào phạm vi nhận diện (`radiusScan`), kẻ địch chuyển trạng thái tấn công.
- **Thuật toán chia mục tiêu (`SetTargetRunner`)**: Mỗi kẻ địch sẽ bám đuổi 1 runner tương ứng và tiêu diệt lẫn nhau khi va chạm trực tiếp.

### 🧱 Địa hình Modular (Chunk-based Generation)
- Màn chơi được chia nhỏ thành các khối đường chạy (`Chunk`) độc lập.
- Sử dụng `LevelSO` (ScriptableObject) để cấu hình danh sách các chunk cho từng màn chơi, giúp thiết kế thêm hàng chục màn mới mà không cần chạm vào code.
- Tự động ghép nối các chunk liên tiếp và đặt vạch đích `EndLine` ở cuối.

### 💰 Kinh tế & Nâng cấp (Upgrades & Economy)
- **Tiền tệ (Coins)**: Lưu trữ liên tục qua `PlayerPrefs` (`SaveLoadManager`).
- **Nâng cấp quân khởi đầu (`AddPlayRunner`)**: Tăng số lượng runner xuất phát ban đầu khi bắt đầu ván đấu.
- **Nâng cấp thu nhập (`HandleUpLevelIncome`)**: Tăng hệ số tiền thưởng nhận được khi qua màn.
- **Công thức tính thưởng qua màn**:
  $$\text{Reward} = \lfloor \sqrt{\text{Runners còn sống}} \cdot (0.1 \cdot \text{LevelIncome} + 1) \rfloor$$

### 👕 Cửa hàng & Trang phục (Store & Skins)
- Quản lý skin nhân vật qua `SkinItemSO` (Prefab, Icon, Tên).
- Cơ chế quay skin ngẫu nhiên (`UnlockRandom`) bằng Coin.
- Tự động lưu skin đang trang bị và tải đúng Prefab nhân vật vào màn chơi.

### 🗂️ Hệ thống UI Stack-Based Navigation
- Quản lý giao diện dạng ngăn xếp (LIFO Stack) thông qua `MenuPopup`:
  - `PushMenu`: Mở menu mới, ẩn menu cũ.
  - `PopMenu`: Đóng menu hiện tại, quay lại menu liền trước.
  - Tự động bắt phím **ESC** trên PC hoặc nút **Back** trên Android để đóng menu.
- Bao gồm các menu: `StartGameUI`, `GameUI`, `GameCompletedUI`, `GameOverUI`, `StoreUI`, `SettingUI`.

### 🔊 Âm thanh, Rung & Quảng cáo
- **Audio Mixer & Slider**: Điều chỉnh âm lượng theo hàm Logarithmic/Exponential phi tuyến chuẩn cho tai người.
- **Haptic Feedback (Rung)**: Kích hoạt hiệu ứng rung nhẹ khi chạm cổng, runner ngã gục hoặc khi chiến thắng/thua cuộc.
- **Unity Ads**:
  - `BannerAds`: Hiển thị banner cố định ở menu.
  - `InterstitialAds`: Tự động hiện quảng cáo xen kẽ sau mỗi 3 lượt chơi.
  - `RewardedAds`: Xem quảng cáo nhận thưởng Coin miễn phí.

---

## 📂 3. Cấu trúc thư mục dự án

```text
Assets/
├── Animation/             # Animator Controller (Income, Units, Menus)
├── AssetsPack/            # 3D Models, Characters, Coins textures
├── AudioMixer.mixer       # Mixer quản lý Master, SFX, BGM
├── Prefabs/
│   ├── Base/              # Prefab cơ sở (Enemy, Runner...)
│   ├── ChunkLevel/        # Prefab các biến thể Chunk địa hình
│   ├── GamePlay/          # Prefab Cổng, Tường, Nhóm Enemy...
│   ├── Player Skins/      # Danh sách các Prefab skin nhân vật (Skin 01 - 09)
│   └── UI/                # Prefab Button, Coin popup...
├── Scenes/
│   └── SampleScene.unity  # Scene Gameplay chính
├── Scripts/
│   ├── Ads/               # Bộ script tích hợp Unity Ads (Banner, Interstitial, Rewarded)
│   ├── GamePlay/          # Logic cốt lõi (Chunk, Door, Doors, Enemy, Obstacle, Road, Runner)
│   ├── Managers/          # Các Singleton quản lý (Audio, Data, Game, SaveLoad, Store, Vibration)
│   ├── Player/            # Điều khiển Runner, Animator, Va chạm và Di chuyển
│   ├── ScriptableObject/  # Định nghĩa LevelSO, SkinItemSO
│   ├── UI/                # Hệ thống giao diện (Menu, MenuPopup, StoreUI, SettingUI...)
│   └── Ulties/            # Singleton pattern generic
└── SO/                    # Dữ liệu ScriptableObject cụ thể (Levels, SkinItems)
```

---

## 🏗️ 4. Kiến trúc mã nguồn & Design Patterns

### 1. Singleton Pattern
Áp dụng mẫu `Singleton<T>` an toàn cho các Manager trọng yếu:
- `GameManager.Instance`: Quản lý Game State (`Menu`, `Game`, `LevelComplete`, `GameOver`).
- `DataManager.Instance`: Quản lý dữ liệu Coin, cấp độ nâng cấp.
- `StoreManager.Instance`: Quản lý danh mục Skin, mở khóa ngẫu nhiên.
- `Player.Instance`: Quản lý nhóm runner hiện tại.
- `MenuPopup.Instance`: Quản lý ngăn xếp giao diện UI.

### 2. Observer / Event-Driven Pattern
Toàn bộ logic tương tác giữa các hệ thống được ghép nối lỏng (loosely coupled) thông qua C# `Action`:
- `GameManager.OnChangeGameState` -> Cập nhật UI, kích hoạt âm thanh, đổi trạng thái di chuyển của Player.
- `DataManager.onChangeCoin` -> Cập nhật text Coin ở Header và trạng thái các nút mua.
- `PlayerCollision.onDoorHit` -> Phát âm thanh, kích hoạt rung haptic.
- `StoreManager.onSelectedSkin` -> Tự động instantiate lại toàn bộ runner trên sân với skin mới.

### 3. Data-Driven Design
Dữ liệu cấu hình cấp độ (`LevelSO`) và Skin nhân vật (`SkinItemSO`) được tách biệt hoàn toàn khỏi logic code thông qua Unity ScriptableObject.

---

## 🚀 5. Cài đặt & Hướng dẫn chạy dự án

### Yêu cầu môi trường
- **Unity Version**: `6000.5.7f1` (Unity 6) hoặc mới hơn.
- **Render Pipeline**: Universal Render Pipeline (URP).
- **Target Platform**: Android / iOS / WebGL / Standalone PC.

### Các bước mở dự án
1. Clone dự án về máy tính:
   ```bash
   git clone https://github.com/shadow-ntt/Run-Crowd-Hyper-Casual.git
   ```
2. Mở **Unity Hub** -> Chọn **Add** -> Trỏ đến thư mục dự án vừa tải về.
3. Chọn phiên bản Unity `6000.5.x`.
4. Trong cửa sổ **Project**, mở Scene chính tại:
   ```text
   Assets / Scenes / SampleScene.unity
   ```
5. Nhấn nút **Play (▶)** trên thanh công cụ Unity để trải nghiệm game.
   - Giữ và kéo chuột trái (hoặc vuốt màn hình cảm ứng) để điều khiển đám đông di chuyển ngang.

---

## 🛠️ 6. Công nghệ & Thư viện sử dụng

| Công nghệ / Package | Phiên bản | Mục đích sử dụng |
| :--- | :--- | :--- |
| **Unity 6 (URP)** | 6000.5.7f1 | Engine đồ họa & Render Pipeline tối ưu di động |
| **TextMesh Pro** | Tích hợp sẵn | Hiển thị typography sắc nét, tối ưu drawcall |
| **Unity Ads** | 4.19.0 | Hệ sinh thái quảng cáo (Banner, Interstitial, Rewarded) |
| **Cinemachine** | 3.1.7 | Camera theo dõi chuyển động đám đông mượt mà |
| **MOST Haptic Feedback** | Plugin | Rung phản hồi vật lý trên thiết bị di động |

---

## 👤 Tác giả

- **Repository**: [shadow-ntt/Run-Crowd-Hyper-Casual](https://github.com/shadow-ntt/Run-Crowd-Hyper-Casual)
- **Engine**: Unity 6
