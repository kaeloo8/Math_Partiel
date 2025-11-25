using UnityEngine;

[CreateAssetMenu(menuName = "Quests/New Quest Objective", fileName = "NewQuestObjective")]
public class QuestData : ScriptableObject
{
    [Header("Identifiant unique de l'objectif")]
    public string questId;

    [Header("Infos affichées au joueur")]
    public string title;
    [TextArea]
    public string description;

    [Header("Récompenses")]
    public int moneyReward;
}
