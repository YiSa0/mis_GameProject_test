using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    // 當這個物件被啟動時，訂閱「場景載入完成」的廣播
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 當這個物件被關閉或銷毀時，取消訂閱（避免記憶體洩漏）
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 每次有任何場景載入「完成」時，就會自動觸發這個函數
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("【重生系統】場景 [" + scene.name + "] 載入完成！準備讀取重生點：[" + GlobalData.targetSpawnPoint + "]");

        if (!string.IsNullOrEmpty(GlobalData.targetSpawnPoint))
        {
            GameObject spawnPoint = GameObject.Find(GlobalData.targetSpawnPoint);
            
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
                Debug.Log("【重生系統】✅ 成功將小魔女傳送到：" + spawnPoint.name);
            }
            else
            {
                Debug.LogError("【重生系統】❌ 糟糕！在這個場景找不到名為 [" + GlobalData.targetSpawnPoint + "] 的物件！");
            }
            
            GlobalData.targetSpawnPoint = ""; 
        }
    }
}