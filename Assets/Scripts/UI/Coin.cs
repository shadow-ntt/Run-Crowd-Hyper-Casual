using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    TMP_Text coinText;

    void OnEnable()
    {
        DataManager.onChangeCoin += HandleChangeCoin;
    }

    void OnDisable()
    {
        DataManager.onChangeCoin -= HandleChangeCoin;
    }

    void Start()
    {
        UpdateCoinDisplay();
    }

    // Lắng nghe sự kiện thay đổi coin để cập nhật text hiển thị
    void HandleChangeCoin(int coins)
    {
        coinText.text = coins.ToString();
    }

    // Đồng bộ và hiển thị số coin hiện tại từ DataManager
    public void UpdateCoinDisplay()
    {
        coinText.text = DataManager.Instance.Coins.ToString();
    }
}
