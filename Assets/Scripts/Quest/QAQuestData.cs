using UnityEngine;
using System.Collections.Generic;

// 注意這裡繼承的是 QuestData，而不是 ScriptableObject
[CreateAssetMenu(fileName = "NewQAQuest", menuName = "CCU RPG/Quest Data/Q&A Quest")]
public class QAQuestData : QuestData 
{
    [Header("Q&A Settings")]
    public List<string> options = new List<string>(); // 存放選項
    public int correctAnswerIndex; // 正確答案索引
}