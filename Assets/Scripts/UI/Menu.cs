using System;
using UnityEngine;

// Lớp cơ sở (Base Class) cho tất cả các Menu/Panel/Popup
public abstract class Menu : MonoBehaviour
{
    [Header("Base Menu Settings")]
    [Tooltip("Tự động ẩn Menu khi vừa vào game (Awake) nếu lỡ để Active trong Scene khi thiết kế")]
    [SerializeField]
    protected bool hideOnAwake = false;

    // Sự kiện đóng/mở tiện lợi cho bên ngoài lắng nghe
    public event Action<Menu> OnMenuOpened;
    public event Action<Menu> OnMenuClosed;

    public bool IsOpen => gameObject.activeSelf;

    // Cờ đánh dấu: Menu đang được hàm Open() chủ động bật lên
    // Mục đích: Nếu GameObject bị Inactive từ đầu trong Hierarchy, Unity chưa chạy Awake().
    // Khi Open() gọi SetActive(true), Unity sẽ nhảy vào chạy Awake() lần đầu tiên.
    // Cờ này báo cho Awake() biết là "đang được mở có chủ đích, KHÔNG được tự tắt nữa!"
    private bool isOpening = false;

    protected virtual void Awake()
    {
        // Chỉ tự ẩn khi Awake chạy tự nhiên lúc Scene vừa load (game vừa Play).
        // Nếu Awake bị kích hoạt do hàm Open() gọi SetActive(true), bỏ qua không tắt.
        if (hideOnAwake && !isOpening)
        {
            gameObject.SetActive(false);
        }

        // Đã qua giai đoạn khởi tạo ban đầu thì vô hiệu hóa cờ này để không bao giờ tự tắt lại
        hideOnAwake = false;
    }

    // Mở Menu và kích hoạt Hook OnOpen()
    public virtual void Open()
    {
        if (gameObject.activeSelf)
            return;

        // Bật cờ isOpening trước khi SetActive(true) để phòng trường hợp Awake() chạy lúc này
        isOpening = true;
        gameObject.SetActive(true);
        isOpening = false;

        Debug.Log("Menu open: " + gameObject.name);
        OnOpen();
        OnMenuOpened?.Invoke(this);
    }

    // Đóng Menu và kích hoạt Hook OnClose()
    public virtual void Close()
    {
        if (!gameObject.activeSelf)
            return;

        OnClose();
        gameObject.SetActive(false);
        OnMenuClosed?.Invoke(this);
    }

    // Đảo ngược trạng thái Mở/Đóng của Menu
    public virtual void Toggle()
    {
        if (IsOpen)
            Close();
        else
            Open();
    }

    // Hook vòng đời: Được gọi ngay khi Menu vừa mở (override ở lớp con nếu cần)
    protected virtual void OnOpen() { }

    // Hook vòng đời: Được gọi ngay trước khi Menu bị ẩn (override ở lớp con nếu cần)
    protected virtual void OnClose() { }
}
