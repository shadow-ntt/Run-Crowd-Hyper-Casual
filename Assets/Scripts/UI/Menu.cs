using System;
using UnityEngine;

/// <summary>
/// Lớp cơ sở (Base Class) cho tất cả các Menu/Panel/Popup.
/// Thiết kế Generic, độc lập, có thể tái sử dụng cho mọi dự án Unity.
/// </summary>
public abstract class Menu : MonoBehaviour
{
    [Header("Base Menu Settings")]
    [Tooltip("Tự động ẩn Menu này khi vừa khởi tạo (Awake)")]
    [SerializeField] protected bool hideOnAwake = false;

    // Sự kiện đóng/mở tiện lợi cho bên ngoài lắng nghe
    public event Action<Menu> OnMenuOpened;
    public event Action<Menu> OnMenuClosed;

    public bool IsOpen => gameObject.activeSelf;

    protected virtual void Awake()
    {
        if (hideOnAwake)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Mở Menu và kích hoạt Hook OnOpen()
    /// </summary>
    public virtual void Open()
    {
        if (gameObject.activeSelf) return;
        gameObject.SetActive(true);
        OnOpen();
        OnMenuOpened?.Invoke(this);
    }

    /// <summary>
    /// Đóng Menu và kích hoạt Hook OnClose()
    /// </summary>
    public virtual void Close()
    {
        if (!gameObject.activeSelf) return;

        OnClose();
        gameObject.SetActive(false);
        OnMenuClosed?.Invoke(this);
    }

    /// <summary>
    /// Đảo ngược trạng thái Mở/Đóng
    /// </summary>
    public virtual void Toggle()
    {
        if (IsOpen) Close();
        else Open();
    }

    /// <summary>
    /// Hook vòng đời: Được gọi ngay khi Menu vừa mở.
    /// Override ở lớp con để nạp dữ liệu, reset vị trí, chạy animation...
    /// </summary>
    protected virtual void OnOpen() { }

    /// <summary>
    /// Hook vòng đời: Được gọi ngay trước khi Menu bị ẩn.
    /// Override ở lớp con để dọn dẹp, hủy đăng ký sự kiện...
    /// </summary>
    protected virtual void OnClose() { }
}
