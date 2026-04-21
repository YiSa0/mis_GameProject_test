using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MinimapRaycaster2D : MonoBehaviour, IPointerClickHandler
{
    [Header("核心綁定")]
    public Camera minimapCamera;        // 實體小地圖攝影機
    public RectTransform rawImageRect;  // 顯示地圖的 RawImage 自己的 RectTransform

    [Header("偵測設定")]
    public LayerMask markerLayer;       // 只偵測小地圖標記的圖層 (例如 MinimapOnly)

    void Start()
    {
        if (rawImageRect == null)
            rawImageRect = GetComponent<RectTransform>();
    }

    // 當玩家點擊小地圖 RawImage 時觸發
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        
        // 偵錯 1：確認 UI 有沒有被點到
        Debug.Log("【偵錯1】成功點擊到小地圖 UI 面板！"); 

        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImageRect, eventData.position, eventData.pressEventCamera, out localCursor))
            return;

        Rect r = rawImageRect.rect;
        float normalizedX = (localCursor.x - r.x) / r.width;
        float normalizedY = (localCursor.y - r.y) / r.height;
        
        // 偵錯 2：確認座標計算是否正常 (數值應該在 0 ~ 1 之間)
        Debug.Log($"【偵錯2】計算出的點擊比例：X={normalizedX}, Y={normalizedY}"); 

        Ray ray = minimapCamera.ViewportPointToRay(new Vector3(normalizedX, normalizedY, 0));
        Collider2D hitCollider = Physics2D.OverlapPoint(ray.origin, markerLayer);

        if (hitCollider != null)
        {
            // 偵錯 3A：打中了！
            Debug.Log("【偵錯3】🎯 成功打中標記：" + hitCollider.gameObject.name);
            HandleMinimapClick(hitCollider.gameObject);
        }
        else
        {
            // 偵錯 3B：沒打中
            Debug.Log("【偵錯3】❌ 射線發射了，但沒打中指定圖層的任何碰撞體");
        }
    }

    private void HandleMinimapClick(GameObject clickedMarker)
    {
        // 取得圖標上的 MinimapTarget 腳本
        MinimapTarget targetData = clickedMarker.GetComponent<MinimapTarget>();
        
        // 檢查是否有綁定成功
        if (targetData != null && targetData.buildingSign != null)
        {
            Debug.Log("🎯 透過小地圖點擊，啟動實體招牌的傳送機制！");
            
            // 透過遙控器，直接呼叫實體招牌的公開函數
            targetData.buildingSign.EnterBuilding(); 
        }
        else
        {
            Debug.LogWarning("❌ 小地圖圖標沒有綁定實體的 SignInteraction 腳本！請去 Inspector 拖曳綁定。");
        }
    }
}