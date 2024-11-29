using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests = new List<Quest>();  // List of all quests
    public Quest currentQuest;  // The currently active quest

    private void Start()
    {
        if (currentQuest != null && !currentQuest.isActive) {
            currentQuest.isActive = true;
            Debug.Log("Quest Started: " + currentQuest.questName);
        }
    }

    public void CompleteQuest(Quest quest)
    {
        if (quest == currentQuest && !quest.isCompleted) {
            quest.isCompleted = true;
            quest.isActive = false;
            Debug.Log("Quest Completed: " + quest.questName);
            RewardPlayer();
        }
    }

    private void RewardPlayer()
    {
        Debug.Log("You have received your reward from the Bard!");
    }
}
