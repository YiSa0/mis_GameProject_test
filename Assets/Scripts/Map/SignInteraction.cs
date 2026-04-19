using UnityEngine;
using UnityEngine.SceneManagement; // 加入這行才能控制場景切換！

public class SignInteraction : MonoBehaviour
{
    [Header("UI 參考")]
    public GameObject signUI;

    [Header("玩家判定設定")]
    [Tooltip("把你的玩家物件拖曳到這裡。如果留空，系統會自動偵測 Tag 為 'Player' 的物件。")]
    public GameObject specificPlayer;

    [Header("場景切換設定")]
    [Tooltip("請輸入你要進入的場景名稱，大小寫必須完全一致！")]
    public string targetSceneName = "CCULibrary_Inside"; 

    // 👇 新增的重生點設定，現在乖乖待在 class 裡面了
    [Header("重生點設定")]
    [Tooltip("玩家進入新場景後，要出現在哪個標記點？")]
    public string spawnPointName; 

    [Header("浮動動畫設定")]
    public float floatSpeed = 3f;
    public float floatAmount = 0.1f;
    
    private Vector3 initialPos;
    private bool isPlayerNearby = false;

    void Start()
    {
        if (signUI != null)
        {
            initialPos = signUI.transform.localPosition;
            signUI.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNearby)
        {
            // 1. 處理 UI 浮動動畫
            if (signUI != null)
            {
                float newY = initialPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
                signUI.transform.localPosition = new Vector3(initialPos.x, newY, initialPos.z);
            }

            // 2. 偵測玩家是否按下 E 鍵
            if (Input.GetKeyDown(KeyCode.E))
            {
                EnterBuilding();
            }
        }
    }

    // 👇 更新後的 EnterBuilding，加入了寫入記憶體的核心魔法
    private void EnterBuilding()
    {
        Debug.Log("準備進入空間：" + targetSceneName);
        
        // 【核心魔法】在換場景前，先把目標重生點的名字寫入全域記憶體！
        GlobalData.targetSpawnPoint = spawnPointName;
        
        // 執行場景切換
        SceneManager.LoadScene(targetSceneName);
    }

    // 玩家進入感應區
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || (specificPlayer != null && other.gameObject == specificPlayer))
        {
            isPlayerNearby = true;
            if (signUI != null) signUI.SetActive(true);
        }
    }

    // 玩家離開感應區
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || (specificPlayer != null && other.gameObject == specificPlayer))
        {
            isPlayerNearby = false;
            if (signUI != null) signUI.SetActive(false);
        }
    }
}