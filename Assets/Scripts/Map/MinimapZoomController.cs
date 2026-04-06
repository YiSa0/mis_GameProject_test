using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI; // ⚠️ 必須加入這行，才能控制 UI

public class MinimapZoomController : MonoBehaviour
{
    [Header("綁定虛擬攝影機")]
    public CinemachineCamera minimapCam;

    [Header("綁定 UI 介面")]
    public Slider zoomSlider; // 你的拉桿
    public Button zoomInBtn;  // 你的 + 按鈕
    public Button zoomOutBtn; // 你的 - 按鈕

    [Header("縮放參數")]
    public float minSize = 3f;  
    public float maxSize = 15f; 
    public float zoomSpeed = 2f; 

    private void Start()
    {
        // === 初始化設定 ===
        if (zoomSlider != null)
        {
            zoomSlider.minValue = minSize; 
            zoomSlider.maxValue = maxSize; 
            // 讓拉桿一開始的位置，對齊攝影機當前的大小
            zoomSlider.value = minimapCam.Lens.OrthographicSize;

            // 告訴拉桿：當玩家拖動你時，去執行 OnSliderValueChanged 這個動作
            zoomSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        // 告訴按鈕：當玩家點擊你時，去執行放大/縮小動作
        if (zoomInBtn != null) zoomInBtn.onClick.AddListener(ZoomIn);
        if (zoomOutBtn != null) zoomOutBtn.onClick.AddListener(ZoomOut);
    }

    private void Update()
    {
        // === 保留原本的滑鼠滾輪控制 ===
        float scrollInput = Input.mouseScrollDelta.y;

        if (scrollInput != 0 && minimapCam != null)
        {
            float currentSize = minimapCam.Lens.OrthographicSize;
            float newSize = Mathf.Clamp(currentSize - (scrollInput * zoomSpeed), minSize, maxSize);
            
            minimapCam.Lens.OrthographicSize = newSize;

            // 【反向同步】：當玩家用滾輪縮放時，拉桿的滑塊也要跟著移動！
            if (zoomSlider != null)
            {
                zoomSlider.value = newSize;
            }
        }
    }

    // === 方法 1: 拉桿改變 -> 攝影機改變 ===
    private void OnSliderValueChanged(float value)
    {
        if (minimapCam != null)
        {
            minimapCam.Lens.OrthographicSize = value;
        }
    }

    // === 方法 2: 按鈕被點擊 -> 攝影機改變 + 拉桿跟著動 ===
    public void ZoomIn()
    {
        if (minimapCam.Lens.OrthographicSize > minSize)
        {
            minimapCam.Lens.OrthographicSize -= 1f; // 每次放大減少 1
            if (zoomSlider != null) zoomSlider.value = minimapCam.Lens.OrthographicSize;
        }
    }

    public void ZoomOut()
    {
        if (minimapCam.Lens.OrthographicSize < maxSize)
        {
            minimapCam.Lens.OrthographicSize += 1f; // 每次縮小增加 1
            if (zoomSlider != null) zoomSlider.value = minimapCam.Lens.OrthographicSize;
        }
    }
}