using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveLoadManager
{
    public static void SaveInt(string key, int value) => PlayerPrefs.SetInt(key, value);

    public static int LoadInt(string key, int defaultValue) => PlayerPrefs.GetInt(key, defaultValue);

    public static void SaveFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);

    public static float LoadFloat(string key, float defaultValue) => PlayerPrefs.GetFloat(key, defaultValue);

    public static void SaveString(string key, string value) => PlayerPrefs.SetString(key, value);

    public static string LoadString(string key, string defaultValue) => PlayerPrefs.GetString(key, defaultValue);
}
