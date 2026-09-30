using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField]
    private float radius;

    [SerializeField]
    private Transform RunnerGroup;

    [SerializeField]
    private Road road;

    [SerializeField]
    private float angle = 137.5f;

    [SerializeField]
    private float speed;

    [SerializeField]
    private TextMeshPro textCount;

    private Transform RunnerPrefab;
    private HashSet<Obstacle> activeObstacles = new HashSet<Obstacle>();
    public bool isLerp = false;

    void OnEnable()
    {
        StoreManager.onSelectedSkin += HandleSelectedSkin;
        GameManager.OnChangeGameState += OnChangeGameStateCallBack;
        DataManager.onUpLevelRunner += HandleUpgradeRunner;
    }

    void OnDisable()
    {
        StoreManager.onSelectedSkin -= HandleSelectedSkin;
        GameManager.OnChangeGameState -= OnChangeGameStateCallBack;
        DataManager.onUpLevelRunner -= HandleUpgradeRunner;
    }

    // Cập nhật lại toàn bộ runner theo prefab của skin vừa được chọn
    void HandleSelectedSkin(SkinItemSO skinItemSO)
    {
        //Clear old skin
        for (int i = 0; i < RunnerGroup.childCount; i++)
            Destroy(RunnerGroup.GetChild(i).gameObject);
        //
        int currentCount = DataManager.Instance.AmoutStartRunner;
        RunnerPrefab = skinItemSO.Prefab;
        SpawnRunner(currentCount);
    }

    // Xử lý tạo thêm runner khi nâng cấp số lượng runner ban đầu
    private void HandleUpgradeRunner(int totalRunners)
    {
        int diff = totalRunners - RunnerGroup.childCount;
        if (diff > 0)
        {
            SpawnRunner(diff);
        }
    }

    // Xử lý cộng thưởng coin khi hoàn thành màn chơi
    private void OnChangeGameStateCallBack(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.LevelComplete)
            DataManager.Instance.AddCoins(CaculateReward());
    }

    // Sinh ra số lượng runner chỉ định vào nhóm
    void SpawnRunner(int number)
    {
        for (int i = 0; i < number; i++)
        {
            Instantiate(RunnerPrefab, RunnerGroup);
        }
        if (!GameManager.Instance.IsGameState())
        {
            GetComponent<PlayerAnimator>().PlayerIdle();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitSkinAndRunners();
    }

    // Khởi tạo skin và số lượng runner ban đầu khi vào game
    public void InitSkinAndRunners()
    {
        SkinItemSO selectedSkin = StoreManager.Instance.GetSkinItemSelected();
        RunnerPrefab = selectedSkin.Prefab;

        int startAmount = DataManager.Instance.AmoutStartRunner;

        for (int i = RunnerGroup.childCount - 1; i >= 0; i--)
        {
            Destroy(RunnerGroup.GetChild(i).gameObject);
        }

        SpawnRunner(startAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (activeObstacles.Count < 1)
        {
            if (isLerp)
                PlaceRunnersMoveEase();
            else
                PlaceRunners();
            GameUI.Instance.setProgressLevel(ProgressEndLine());
            textCount.text = RunnerGroup.childCount.ToString();
        }
        if (RunnerCount() <= 0 && GameManager.Instance.IsGameState())
        {
            GameManager.Instance.ChangeGameState(GameManager.GameState.GameOver);
        }
    }

    //
    // Tính bán kính bao phủ của toàn bộ nhóm runner
    public float GetRadiusGroup()
    {
        int count = RunnerGroup.childCount;

        if (count == 0)
            return 0f;

        return radius * Mathf.Sqrt(count - 1);
    }

    // Tính toán vị trí cục bộ của từng runner theo mô hình Fermat spiral
    private Vector3 GetRunnerLocalPositions(int index)
    {
        float r = radius * Mathf.Sqrt(index);

        float x = r * Mathf.Cos(angle * index * Mathf.Deg2Rad);
        float z = r * Mathf.Sin(angle * index * Mathf.Deg2Rad);

        return new Vector3(x, 0, z);
    }

    // Đặt tức thì các runner vào đúng vị trí hình học của nhóm
    public void PlaceRunners()
    {
        for (int i = 0; i < RunnerCount(); i++)
        {
            RunnerGroup.GetChild(i).localPosition = GetRunnerLocalPositions(i);
        }
    }

    // Di chuyển mượt mà các runner về vị trí hình học sau khi thay đổi số lượng
    public void PlaceRunnersMoveEase()
    {
        for (int i = 0; i < RunnerCount(); i++)
        {
            RunnerGroup.GetChild(i).localPosition = Vector3.Lerp(
                RunnerGroup.GetChild(i).localPosition,
                GetRunnerLocalPositions(i),
                speed * Time.deltaTime
            );
        }
        //kiểm tra về đúng vị trí chưa
        for (int i = 0; i < RunnerCount(); i++)
        {
            if (
                Vector3.Distance(RunnerGroup.GetChild(i).localPosition, GetRunnerLocalPositions(i))
                > 0.2f
            )
                return;
        }
        isLerp = false;
    }

    // Lấy tổng số lượng runner hiện có trong nhóm
    public int RunnerCount()
    {
        return RunnerGroup.childCount;
    }

    // Áp dụng phép toán của cửa (+, -, *, /) lên số lượng runner
    public void ApplyAmount(Door.TypeDoor doorType, int amount)
    {
        switch (doorType)
        {
            case Door.TypeDoor.Plus:
                SetSerialRuner(RunnerCount() + amount);
                break;
            case Door.TypeDoor.Subtrack:
                SetSerialRuner(Mathf.Max(1, RunnerCount() - amount));
                break;
            case Door.TypeDoor.Multiply:
                SetSerialRuner(RunnerCount() * amount);
                break;
            case Door.TypeDoor.Division:
                if (amount <= 0)
                    return;
                SetSerialRuner(Mathf.Max(1, RunnerCount() / amount));
                break;
        }
    }

    // Điều chỉnh số lượng runner tăng hoặc giảm cho bằng đúng giá trị n
    public void SetSerialRuner(int n)
    {
        int runnerCount = RunnerCount();
        //add runner
        if (n > RunnerCount())
        {
            SpawnRunner(n - runnerCount);
        }
        //remove runner
        if (n < runnerCount)
        {
            for (int i = 0; i < runnerCount - n; i++)
            {
                Destroy(RunnerGroup.GetChild(i).gameObject);
            }
        }
    }

    // Tính toán tỉ lệ phần trăm quãng đường đã đi được tới đích
    public float ProgressEndLine()
    {
        return transform.position.z / road.EndLineZ;
    }

    // Đăng ký chướng ngại vật đang tương tác với nhóm runner
    public void RegisterObstacle(Obstacle obstacle)
    {
        activeObstacles.Add(obstacle);
    }

    // Hủy đăng ký chướng ngại vật và kích hoạt căn chỉnh lại đội hình
    public void UnRegisterObstacle(Obstacle obstacle)
    {
        activeObstacles.Remove(obstacle);
        isLerp = true;
    }

    // Tính toán số coin thưởng nhận được khi hoàn thành màn chơi
    public int CaculateReward()
    {
        return (int)
                Math.Floor(
                    Math.Sqrt(RunnerCount()) * (0.1f * DataManager.Instance.LevelIncome + 1f)
                ) * 100;
    }
}
