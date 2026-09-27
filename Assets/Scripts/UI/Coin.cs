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

    void HandleChangeCoin(int coins)
    {
        coinText.text = coins.ToString();
    }

    public void UpdateCoinDisplay()
    {
        coinText.text = DataManager.Instance.Coins.ToString();
    }
}
