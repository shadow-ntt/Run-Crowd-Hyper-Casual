using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardedAdsButton : MonoBehaviour
{
    private Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnRewardedAdsButtonClicked);
    }

    public void OnRewardedAdsButtonClicked()
    {
        AdsManager.Instance.rewardedAds.ShowAd();
    }
}
