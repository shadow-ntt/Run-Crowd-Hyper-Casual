using TMPro;
using UnityEngine;
using UnityEngine.UI;

public  class GameUI :MonoBehaviour
{
    [SerializeField] private Slider progressLevel;
    [SerializeField] private TMP_Text textLevel;

    public static GameUI Instance;
        void Awake()
    {
        if (Instance == null)
        {
            Instance =this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        textLevel.text=SaveLoadManager.LoadInt("level", 1).ToString();
    }
    void Update()
    {
        
    }
    public void setProgressLevel(float value)
    {
        progressLevel.value=value;
    }
    
}
