using UnityEngine;

public class MinimapIconBounce : MonoBehaviour
{
    [Header("跳動設定")]
    [Tooltip("跳動的高度幅度")]
    public float bounceHeight = 1f; 

    [Tooltip("跳動的快慢速度")]
    public float bounceSpeed = 3f;

    [Header("距離偵測設定")]
    [Tooltip("把小魔女 (Player) 的物件拖曳到這裡。如果留空，會自動尋找標籤為 Player 或 player 的物件")]
    public Transform playerTarget;
    
    [Tooltip("小魔女靠近到多少距離內，圖標才會開始跳？")]
    public float activationDistance = 5f;

    // 用來儲存圖標最一開始的位置基準點
    private Vector3 startPosition;

    void Start()
    {
        // 記錄初始的 Local 座標
        startPosition = transform.localPosition;

        // 如果忘記在面板上拖曳玩家物件，嘗試用標籤自動尋找
        if (playerTarget == null)
        {
            // 1. 先找大寫的 "Player"
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            // 2. 【新增邏輯】如果大寫找不到，就試著找小寫的 "player"
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("player");
            }

            // 3. 判斷最後有沒有找到
            if (player != null)
            {
                playerTarget = player.transform;
            }
            else
            {
                Debug.LogWarning("找不到玩家！請確認小魔女的物件有設定 'Player' 或 'player' 標籤，或直接拖曳到腳本的 playerTarget 欄位中。");
            }
        }
    }

    void Update()
    {
        // 如果場景裡沒有玩家，就不執行下面的動作
        if (playerTarget == null) return;

        // 計算圖標（在世界空間）與小魔女之間的距離
        // 因為是 2D/2.5D 遊戲，這裡使用 Vector2 計算 XY 軸平面的距離會比較精準
        float distance = Vector2.Distance(transform.position, playerTarget.position);

        if (distance <= activationDistance)
        {
            // 【狀態 1】小魔女在範圍內：開始正弦波跳動
           float newY = startPosition.y + (Mathf.Sin(Time.time * bounceSpeed) * 0.5f + 0.5f) * bounceHeight;
           transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
        }
        else
        {
            // 【狀態 2】小魔女在範圍外：平滑地回到原始位置 (Lerp 插值)
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime * 5f);
        }
    }
}