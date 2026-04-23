using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestData", menuName = "CCU RPG/Quest Data/Basic Quest")]
public class QuestData : ScriptableObject
{
    [Header("Basic Info")]
    public string questID;
    public string questName;
    [TextArea(3, 5)] 
    public string description;
    public string targetPOI;
    public string rewardBadge;
    
    // 你可以在這裡加入其他共通屬性，例如任務狀態 (未接、進行中、完成)
}