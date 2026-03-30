using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "CyberSecurity/Question")]
public class QuestionData : ScriptableObject {
    public string questionID;
    [TextArea(3, 10)] public string content; // 題目內容
    public string[] options;                // 選項
    public int correctIndex;                // 正確答案索引
    public string explanation;              // 答案解析（資安教育重點）
}