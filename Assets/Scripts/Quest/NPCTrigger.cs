using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    [Header("這個 NPC 要發布的任務")]
    public QuestData questToGive;

    // 假設你的遊戲是 2D，使用碰撞體觸發 (例如玩家走進 NPC 的判定範圍)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 確認撞到的是玩家
        if (collision.CompareTag("Player"))
        {
            Debug.Log("玩家靠近了 NPC，準備發布任務...");
            
            // 呼叫我們剛剛寫好的 QuestManager，把任務塞進去！
            if (questToGive != null)
            {
                QuestManager.Instance.AcceptQuest(questToGive);
            }
        }
    }
}