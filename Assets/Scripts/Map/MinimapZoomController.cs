using UnityEngine;
using UnityEngine.UI;

public class MinimapZoomController : MonoBehaviour
{
    [Header("綁定實體攝影機 (替換掉原本的 Cinemachine)")]
    public Camera minimapCam; 

    [Header("綁定 UI 介面")]
    public Slider zoomSlider; 
    public Button zoomInBtn;  
    public Button zoomOutBtn; 

    [Header("縮放參數")]
    public float minSize = 3f;  
    public float maxSize = 15f; 
    public float zoomSpeed = 2f; 

    private void Start()
    {
        if (zoomSlider != null && minimapCam != null)
        {
            zoomSlider.minValue = minSize; 
            zoomSlider.maxValue = maxSize; 
            // 🌟 改變點：標準 Camera 的寫法是 .orthographicSize
            zoomSlider.value = minimapCam.orthographicSize;

            zoomSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        if (zoomInBtn != null) zoomInBtn.onClick.AddListener(ZoomIn);
        if (zoomOutBtn != null) zoomOutBtn.onClick.AddListener(ZoomOut);
    }

    private void Update()
    {
        float scrollInput = Input.mouseScrollDelta.y;

        if (scrollInput != 0 && minimapCam != null)
        {
            // 🌟 改變點：標準 Camera 的寫法
            float currentSize = minimapCam.orthographicSize;
            float newSize = Mathf.Clamp(currentSize - (scrollInput * zoomSpeed), minSize, maxSize);
            
            minimapCam.orthographicSize = newSize;

            if (zoomSlider != null)
            {
                zoomSlider.value = newSize;
            }
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (minimapCam != null)
        {
            minimapCam.orthographicSize = value;
        }
    }

    public void ZoomIn()
    {
        if (minimapCam != null && minimapCam.orthographicSize > minSize)
        {
            minimapCam.orthographicSize -= 1f; 
            if (zoomSlider != null) zoomSlider.value = minimapCam.orthographicSize;
        }
    }

    public void ZoomOut()
    {
        if (minimapCam != null && minimapCam.orthographicSize < maxSize)
        {
            minimapCam.orthographicSize += 1f; 
            if (zoomSlider != null) zoomSlider.value = minimapCam.orthographicSize;
        }
    }
}