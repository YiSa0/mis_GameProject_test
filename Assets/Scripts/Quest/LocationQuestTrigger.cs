using UnityEngine;

public class LocationQuestTrigger : MonoBehaviour
{
    [Header("要自動觸發的任務資料")]
    public QuestData questToTrigger;

    [Header("觸發後是否失效")]
    [Tooltip("勾選後，任務接取一次後，這塊感應區就會消失，不會一直重複彈出")]
    public bool triggerOnce = true;

    private bool hasTriggered = false; // 紀錄是否已經完成觸發

    // 當有物件進入感應區的「瞬間」就會執行這裡
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 如果已經觸發過了，就甚麼都不做
        if (hasTriggered) return; 

        // 2. 確認撞進來的是不是玩家
        if (collision.CompareTag("Player") || collision.CompareTag("player"))
        {
            TriggerQuest();
        }
    }

    // 觸發任務的核心邏輯
    private void TriggerQuest()
    {
        if (questToTrigger != null)
        {
            // 呼叫任務管理器接取任務 (這會自動幫你彈出 QAPanel！)
            QuestManager.Instance.AcceptQuest(questToTrigger);
            Debug.Log("🚶 玩家靠近！自動觸發了任務：" + questToTrigger.questName);

            // 如果設定為只觸發一次，就讓自己功成身退
            if (triggerOnce)
            {
                hasTriggered = true;
                gameObject.SetActive(false); 
            }
        }
        else
        {
            Debug.LogWarning("⚠️ 你忘記把 QuestData 拖進來感應區了啦！");
        }
    }
}