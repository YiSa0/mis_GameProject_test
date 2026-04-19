using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoPortal : MonoBehaviour
{
    [Header("場景切換設定")]
    [Tooltip("請輸入你要進入的場景名稱，大小寫必須完全一致！")]
    public string targetSceneName = "MainCampus"; 

    [Header("重生點設定")]
    [Tooltip("玩家進入新場景後，要出現在哪個標記點？")]
    public string spawnPointName = "Spawn_FromLibrary"; 

    [Header("玩家判定設定")]
    [Tooltip("設定玩家的標籤，預設為 Player")]
    public string playerTag = "Player";

    // 當有物件「剛碰到」這個感應區時，就會瞬間觸發這個函數
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 檢查碰到的是不是玩家
        if (other.CompareTag(playerTag))
        {
            Debug.Log("玩家踩到自動傳送門，準備進入空間：" + targetSceneName);
            
            // 寫入全域記憶體
            GlobalData.targetSpawnPoint = spawnPointName;
            
            // 瞬間切換場景
            SceneManager.LoadScene(targetSceneName);
        }
    }
}