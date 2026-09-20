using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quản lý ngăn xếp Menu/Popup (Stack-based UI Navigation).
/// Tái sử dụng cho mọi dự án. Kế thừa từ Singleton<MenuPopup>.
/// Hỗ trợ LIFO: Push (mở menu mới, ẩn menu cũ), Pop (đóng menu hiện tại, khôi phục menu liền trước), CloseAll,
/// và tự động bắt nút Back (Android) / phím ESC (PC).
/// </summary>
public class MenuPopup : Singleton<MenuPopup>
{
    [Header("Settings")]
    [Tooltip("Tự động đóng menu trên cùng khi bấm phím ESC hoặc nút Back trên Android")]
    [SerializeField] private bool handleBackButton = true;

    [Tooltip("Tự động tắt GameObject container này khi không còn menu nào trong stack")]
    [SerializeField] private bool autoHideContainer = true;


    [Header("MenuUI")]
    [SerializeField] private GameCompletedUI GameCompletedUI ;
    //
    void OnDisable()
    {
        GameManager.OnChangeGameState -= OnChangeGameStateCallBack;
    }
    void OnEnable()
    {
        GameManager.OnChangeGameState += OnChangeGameStateCallBack;
    }
    private void OnChangeGameStateCallBack(GameManager.GameState gameState)
    {
        if(gameState==GameManager.GameState.LevelComplete) this.PushMenu(GameCompletedUI); 
    }
    //

    // Ngăn xếp lưu các Menu đang mở
    private readonly Stack<Menu> menuStack = new Stack<Menu>();

    public int MenuCount => menuStack.Count;
    public Menu CurrentMenu => menuStack.Count > 0 ? menuStack.Peek() : null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Update()
    {
        if (handleBackButton && Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuStack.Count > 0)
            {
                PopMenu();
            }
        }
    }

    /// <summary>
    /// Đẩy Menu mới vào ngăn xếp:
    /// - Ẩn Menu hiện tại (nếu có).
    /// - Mở Menu mới và đưa vào đỉnh Stack.
    /// </summary>
    public void PushMenu(Menu newMenu)
    {
        if (newMenu == null)
        {
            Debug.LogWarning("[MenuPopup] Menu truyền vào PushMenu bị null!");
            return;
        }

        // Tạm thời đóng menu hiện tại
        if (menuStack.Count > 0)
        {
            menuStack.Peek().Close();
        }

        menuStack.Push(newMenu);
        newMenu.Open();

        if (autoHideContainer && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Đóng Menu trên đỉnh Stack và mở lại Menu liền trước nó.
    /// </summary>
    public void PopMenu()
    {
        if (menuStack.Count == 0)
        {
            if (autoHideContainer) gameObject.SetActive(false);
            return;
        }

        Menu topMenu = menuStack.Pop();
        topMenu.Close();

        if (menuStack.Count > 0)
        {
            menuStack.Peek().Open();
        }
        else if (autoHideContainer)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Đóng tất cả các Menu và dọn sạch Stack.
    /// </summary>
    public void CloseAll()
    {
        while (menuStack.Count > 0)
        {
            Menu menu = menuStack.Pop();
            menu.Close();
        }

        if (autoHideContainer)
        {
            gameObject.SetActive(false);
        }
    }
}
