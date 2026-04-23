using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // 單例模式 (Singleton)，方便其他腳本 (如 NPC) 直接呼叫 QuestManager.Instance
    public static QuestManager Instance;

    [Header("目前的任務清單")]
    [Tooltip("這裡會顯示玩家目前正在進行的所有任務")]
    public List<QuestData> activeQuests = new List<QuestData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 確保場景中只有一個 QuestManager
        }
    }
    
    // 這個方法未來會讓 NPC 來呼叫：當玩家跟 NPC 講完話，就把任務塞進這個清單
    public void AcceptQuest(QuestData newQuest)
    {
        // 1. 檢查是不是已經接過這個任務了，避免重複接取
        if (!activeQuests.Contains(newQuest))
        {
            activeQuests.Add(newQuest);
            Debug.Log("✅ 叮！成功接取新任務：" + newQuest.questName);

            // 2. 判斷任務類型，決定接下來的 UI 流程
            if (newQuest is QAQuestData qaQuest)
            {
                // 這是一個問答任務
                Debug.Log("開啟選擇題 UI，選項有 " + qaQuest.options.Count + " 個");
                
                // TODO: 呼叫 UIManager 開啟問答面板，並把 qaQuest 傳進去
                UIManager.Instance.ShowQAPanel(qaQuest);
            }
            else
            {
                // 這是一般支線或隱藏事件
                Debug.Log("這是一般跑點任務，開啟一般任務提示 UI");
                
                // TODO: 更新畫面上的任務追蹤清單，或在地圖上標記 Target POI
                UIManager.Instance.UpdateQuestTracker(newQuest);
            }
        }
        else
        {
            Debug.Log("⚠️ 這個任務你已經接過了啦！");
        }
    }
    
    
}