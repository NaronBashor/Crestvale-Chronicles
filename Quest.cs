using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/Basic Quest")]
public class Quest : ScriptableObject
{
    public string questName;  // Name of the quest
    public string description;  // Description of the quest
    public bool isCompleted;  // If the quest is completed
    public bool isActive;  // If the quest is active
}
