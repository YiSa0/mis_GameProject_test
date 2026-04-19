using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        // 檢查全域記憶體裡面有沒有指定的重生點名稱
        if (!string.IsNullOrEmpty(GlobalData.targetSpawnPoint))
        {
            // 在場景中尋找那個名字的空物件
            GameObject spawnObj = GameObject.Find(GlobalData.targetSpawnPoint);
            
            if (spawnObj != null)
            {
                // 把玩家的座標瞬間移動到該物件的位置
                transform.position = spawnObj.transform.position;
                Debug.Log("成功將玩家傳送到：" + GlobalData.targetSpawnPoint);
            }
            else
            {
                Debug.LogWarning("找不到指定的重生點：" + GlobalData.targetSpawnPoint);
            }
        }
    }
}