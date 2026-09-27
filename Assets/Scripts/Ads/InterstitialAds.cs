using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId;
    [SerializeField] string _iOSAdUnitId;

    string _adUnitId;

    // Callback hành động tiếp theo sau khi tắt/đóng quảng cáo (hoặc khi ad bị lỗi không xem được)
    // Ví dụ: Dùng để Reload Scene chuyển màn tiếp theo sau khi người chơi đã xem xong ads.
    private Action onAdClosedCallback;

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
        #if UNITY_IOS
            _adUnitId = _iOSAdUnitId;
        #elif UNITY_ANDROID || UNITY_EDITOR
            _adUnitId = _androidAdUnitId;
        #endif
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowAd(Action onClosed = null)
    {
        onAdClosedCallback = onClosed;

        if (!Advertisement.isInitialized || string.IsNullOrEmpty(_adUnitId))
        {
            Debug.LogWarning("Unity Ads not initialized or Ad Unit ID is empty.");
            onAdClosedCallback?.Invoke();
            onAdClosedCallback = null;
            return;
        }

        Debug.Log("Showing Ad: " + _adUnitId);
        Advertisement.Show(_adUnitId, this);
        LoadAd();
    }

    #region LoadCallBacks

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
    }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    #endregion LoadCallBacks

    #region ShowCallBacks

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        onAdClosedCallback?.Invoke();
        onAdClosedCallback = null;
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) 
    {
        Debug.Log("Interstitial Ad Completed");
        onAdClosedCallback?.Invoke();
        onAdClosedCallback = null;
    }

    #endregion ShowCallBacks
}
