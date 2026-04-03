using UnityEngine;
using Unity.Cinemachine; // 注意：Cinemachine 3.x 的命名空間

public class MinimapZoomController : MonoBehaviour
{
    [Header("Cinemachine 設定")]
    public CinemachineCamera minimapCam; // 拖入你的 CM vcam_Minimap

    [Header("縮放設定")]
    public float minSize = 3f;  // 最大放大倍率 (數值越小越近)
    public float maxSize = 15f; // 最大縮小倍率 (數值越大越遠)
    public float zoomSpeed = 2f; // 滾輪縮放速度

    void Update()
    {
        // 透過滑鼠滾輪取得輸入值
        float scrollInput = Input.mouseScrollDelta.y;

        if (scrollInput != 0 && minimapCam != null)
        {
            // 讀取目前的 Orthographic Size
            float currentSize = minimapCam.Lens.OrthographicSize;

            // 根據滾輪方向計算新的大小 (向上滾放大=減小數值，向下滾縮小=增加數值)
            float newSize = currentSize - (scrollInput * zoomSpeed);

            // 限制縮放範圍，避免放到無限大或縮到無限小
            newSize = Mathf.Clamp(newSize, minSize, maxSize);

            // 寫回 Cinemachine 攝影機
            minimapCam.Lens.OrthographicSize = newSize;
        }
    }

    // 也可以提供給 UI 按鈕呼叫的公開方法
    public void ZoomIn()
    {
        if (minimapCam.Lens.OrthographicSize > minSize)
            minimapCam.Lens.OrthographicSize -= 1f;
    }

    public void ZoomOut()
    {
        if (minimapCam.Lens.OrthographicSize < maxSize)
            minimapCam.Lens.OrthographicSize += 1f;
    }
}