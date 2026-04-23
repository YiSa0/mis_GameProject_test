using UnityEngine;
using UnityEngine.UI;
using TMPro; // 如果你用的是 TextMeshPro (推薦)

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("問答任務 UI 元件")]
    public GameObject qaPanel;          // 整個問答視窗的底圖
    public TMP_Text questionText;       // 顯示問題描述的文字
    public Button[] optionButtons;      // 存放三個選項按鈕的陣列
    public TMP_Text[] optionTexts;      // 按鈕上的文字

    private QAQuestData currentQAQuest; // 記住現在正在答哪一題

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 遊戲一開始先隱藏視窗
        if (qaPanel != null) qaPanel.SetActive(false);
    }

    // 由 QuestManager 呼叫，把任務資料丟進來顯示
    public void ShowQAPanel(QAQuestData qaQuest)
    {
        currentQAQuest = qaQuest;
        
        // 1. 設定問題文字
        questionText.text = qaQuest.description;

        // 2. 設定按鈕文字與狀態
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < qaQuest.options.Count)
            {
                optionButtons[i].gameObject.SetActive(true); // 顯示按鈕
                optionTexts[i].text = qaQuest.options[i];    // 填入選項文字
                
                // 綁定按鈕點擊事件 (先清空舊的，再綁新的)
                int index = i; // 避免閉包問題
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false); // 如果選項不到三個，隱藏多餘按鈕
            }
        }

        // 3. 打開視窗
        qaPanel.SetActive(true);
    }

   // 當玩家點擊任何一個選項時觸發
    private void OnOptionSelected(int selectedIndex)
    {
        // 啟動「協程」來處理視覺效果和延遲
        StartCoroutine(ShowResultCoroutine(selectedIndex));
    }

    // 處理結果動畫與延遲的協程
    private System.Collections.IEnumerator ShowResultCoroutine(int selectedIndex)
    {
        // 1. 鎖死所有按鈕，防止玩家在動畫期間狂點引發 Bug
        foreach (var btn in optionButtons)
        {
            btn.interactable = false; 
        }

        // 判斷是否答對
        bool isCorrect = (selectedIndex == currentQAQuest.correctAnswerIndex);

        // 2. 給予視覺回饋 (改變文字與按鈕顏色)
        if (isCorrect)
        {
            // 使用 Rich Text 改變文字顏色 (奶油白)
            questionText.text = "<color=#FFF8DC>答對了！你是校園通！</color>"; 
            
            // 將玩家點擊的按鈕染成綠色
            optionButtons[selectedIndex].GetComponent<UnityEngine.UI.Image>().color = Color.green;
        }
        else
        {
            // 使用 Rich Text 改變文字顏色 (奶油白)
            questionText.text = "<color=#FFF8DC>答錯了...你還不夠了解校園噢！</color>"; 
            
            // 將玩家點擊的按鈕染成紅色
            optionButtons[selectedIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 0.5f, 0.5f); // 淺紅色
            
            // 💡 貼心設計：把正確答案的按鈕亮綠色，告訴玩家正解
            optionButtons[currentQAQuest.correctAnswerIndex].GetComponent<UnityEngine.UI.Image>().color = Color.green;
        }

        // 3. 魔法時間：讓程式在這裡「等 1.5 秒」，讓玩家看清楚結果
        yield return new WaitForSeconds(1.5f);

        // 4. 恢復原狀 (非常重要！不然下一題打開時按鈕會是紅綠色而且不能按)
        foreach (var btn in optionButtons)
        {
            btn.interactable = true;
            btn.GetComponent<UnityEngine.UI.Image>().color = Color.white; // 恢復預設顏色
        }

        // 5. 關閉視窗
        if (qaPanel != null) qaPanel.SetActive(false);

        // 6. TODO: 結算邏輯
        if (isCorrect)
        {
            Debug.Log("發放獎勵！");
            // 這裡之後可以呼叫 EconomyManager 給金幣
        }
    }
    // 給一般跑點任務用的追蹤器更新功能 (目前先當作佔位符)
    public void UpdateQuestTracker(QuestData quest)
    {
        // 之後如果有做右上角的任務清單 UI，就可以寫在這裡
        Debug.Log("📋 UI 系統收到通知！準備將【" + quest.questName + "】加入畫面的任務追蹤清單裡。");
    }
    
}