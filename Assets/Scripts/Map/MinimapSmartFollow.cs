using UnityEngine;
using UnityEngine.EventSystems;

// 已經不需要 IEndDragHandler 了，因為我們不再倒數計時！
public class MinimapSmartFollow : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [Header("目標設定")]
    public Transform player;
    public Camera minimapCamera; 

    [Header("參數設定")]
    public float dragSpeed = 1f;
    public bool isFollowing = true;
    public float smoothReturnSpeed = 10f; // 按下按鈕時的歸位速度 (稍微調快一點)

    [Header("邊界推擠範圍 (防走丟狗鍊)")]
    [Range(0.1f, 0.9f)]
    public float edgeMargin = 0.7f; // 0.7 代表角色可以走到畫面的 70% 邊緣，再過去就會推著地圖走

    private float cameraZ;

    void Start()
    {
        if (minimapCamera != null)
            cameraZ = minimapCamera.transform.position.z;
    }

    void LateUpdate()
    {
        if (minimapCamera == null || player == null) return;

        if (isFollowing)
        {
            // 🌟 模式 A：手動點擊按鈕後，平滑回歸到角色正中央
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, cameraZ);
            minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, targetPos, Time.deltaTime * smoothReturnSpeed);
        }
        else
        {
            // 🌟 模式 B：自由模式 (不會自己動，除非角色撞到邊界)
            
            float camHeight = minimapCamera.orthographicSize;
            float camWidth = camHeight * minimapCamera.aspect;

            // 算出結界大小
            float limitX = camWidth * edgeMargin;
            float limitY = camHeight * edgeMargin;

            Vector3 camPos = minimapCamera.transform.position;

            // 核心魔法：如果攝影機與角色的距離超過極限，就強制把攝影機「拉」跟著角色走
            camPos.x = Mathf.Clamp(camPos.x, player.position.x - limitX, player.position.x + limitX);
            camPos.y = Mathf.Clamp(camPos.y, player.position.y - limitY, player.position.y + limitY);

            // 套用座標 (如果角色沒撞到邊緣，camPos 就不會改變，地圖就會完全靜止)
            minimapCamera.transform.position = camPos;
        }
    }

    // 當滑鼠「剛點下」拖曳時
    public void OnBeginDrag(PointerEventData eventData)
    {
        isFollowing = false; // 徹底關閉自動置中，進入自由模式
    }

    // 當滑鼠「正在拖曳」時
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("滑鼠正在拖曳小地圖！數值：" + eventData.delta);
        if (minimapCamera == null) return;

        float zoomMultiplier = minimapCamera.orthographicSize / 5f;
        Vector3 moveDelta = new Vector3(-eventData.delta.x * dragSpeed, -eventData.delta.y * dragSpeed, 0) * zoomMultiplier;
        
        // 拖曳地圖
        minimapCamera.transform.position += moveDelta;
    }

    // 給 UI 按鈕呼叫的「一鍵回家」
    public void BackToPlayer()
    {
        isFollowing = true; // 只有手動按下按鈕，才會切換回模式 A
    }
}