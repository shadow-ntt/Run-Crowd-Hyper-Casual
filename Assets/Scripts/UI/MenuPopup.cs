using System.Collections.Generic;
using UnityEngine;

// Quản lý ngăn xếp Menu/Popup (Stack-based UI Navigation).
// Tái sử dụng cho mọi dự án. Kế thừa từ Singleton<MenuPopup>.
// Hỗ trợ LIFO: Push (mở menu mới, ẩn menu cũ), Pop (đóng menu hiện tại, khôi phục menu liền trước), CloseAll,
// và tự động bắt nút Back (Android) / phím ESC (PC).
public class MenuPopup : Singleton<MenuPopup>
{
    [Header("Settings")]
    [Tooltip("Tự động đóng menu trên cùng khi bấm phím ESC hoặc nút Back trên Android")]
    [SerializeField]
    private bool handleBackButton = true;

    [Tooltip("GameObject container/panel nền (nếu có). Để trống nếu không dùng.")]
    [SerializeField]
    private GameObject container;

    [Tooltip("Tự động tắt GameObject container khi không còn menu nào trong stack")]
    [SerializeField]
    private bool autoHideContainer = true;

    [Header("MenuUI")]
    [SerializeField]
    private GameCompletedUI GameCompletedUI;

    [SerializeField]
    private StartGameUI StartGameUI;

    [SerializeField]
    private GameOverUI GameOverUI;

    // Ngăn xếp lưu các Menu đang mở
    private readonly Stack<Menu> menuStack = new Stack<Menu>();

    public int MenuCount => menuStack.Count;
    public Menu CurrentMenu => menuStack.Count > 0 ? menuStack.Peek() : null;

    void OnDisable()
    {
        GameManager.OnChangeGameState -= OnChangeGameStateCallBack;
    }

    void OnEnable()
    {
        GameManager.OnChangeGameState += OnChangeGameStateCallBack;
    }

    // Điều hướng và hiển thị Menu tương ứng theo trạng thái của game
    private void OnChangeGameStateCallBack(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.LevelComplete:
                CloseAll();
                PushMenu(GameCompletedUI);
                break;
            case GameManager.GameState.Menu:
                CloseAll();
                PushMenu(StartGameUI);
                break;
            case GameManager.GameState.GameOver:
                CloseAll();
                PushMenu(GameOverUI);
                break;
            case GameManager.GameState.Game:
            default:
                CloseAll();
                break;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    //hard code
    void Start()
    {
        PushMenu(StartGameUI);
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

    // Đẩy Menu mới vào ngăn xếp (ẩn menu hiện tại và mở menu mới)
    public void PushMenu(Menu newMenu)
    {
        if (newMenu == null)
        {
            Debug.LogWarning("[MenuPopup] Menu truyền vào PushMenu bị null!");
            return;
        }

        // Chống lỗi Push trùng: Nếu menu này đã đang mở trên đỉnh Stack thì bỏ qua
        if (menuStack.Count > 0 && menuStack.Peek() == newMenu)
        {
            Debug.LogWarning(
                $"[MenuPopup] Menu '{newMenu.name}' đã đang mở trên đỉnh Stack, bỏ qua Push trùng!"
            );
            return;
        }

        // Bật container trước nếu có cấu hình
        if (autoHideContainer && container != null && !container.activeSelf)
        {
            container.SetActive(true);
        }

        // Tạm thời đóng menu hiện tại
        if (menuStack.Count > 0)
        {
            menuStack.Peek().Close();
        }

        menuStack.Push(newMenu);
        newMenu.Open();

        Debug.Log(
            $"[MenuPopup] PushMenu: {newMenu.name}. Tổng số menu trong stack: {menuStack.Count}"
        );
    }

    // Đóng Menu trên đỉnh Stack và mở lại Menu liền trước nó
    public void PopMenu()
    {
        if (menuStack.Count == 0)
        {
            if (autoHideContainer && container != null)
                container.SetActive(false);
            return;
        }

        Menu topMenu = menuStack.Pop();
        if (topMenu != null)
        {
            topMenu.Close();
        }

        if (menuStack.Count > 0)
        {
            Menu previousMenu = menuStack.Peek();
            if (previousMenu != null)
            {
                previousMenu.Open();
            }
        }
        else if (autoHideContainer && container != null)
        {
            container.SetActive(false);
        }

        Debug.Log(
            $"[MenuPopup] PopMenu: {(topMenu != null ? topMenu.name : "null")}. Còn lại trong stack: {menuStack.Count}"
        );
    }

    // Đóng tất cả các Menu và dọn sạch Stack
    public void CloseAll()
    {
        while (menuStack.Count > 0)
        {
            Menu menu = menuStack.Pop();
            if (menu != null)
            {
                menu.Close();
            }
        }

        if (autoHideContainer && container != null)
        {
            container.SetActive(false);
        }
    }
}
