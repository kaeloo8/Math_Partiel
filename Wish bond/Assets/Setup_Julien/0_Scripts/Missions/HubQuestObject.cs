using UnityEngine;

// Objet cliquable dans le HUB qui permet de sélectionner un objectif.
public class HubQuestObject : MonoBehaviour, I_Interactable
{
    [Header("Objectif associé à cet objet dans le HUB")]
    [SerializeField] private QuestData questData;

    public void Interact()
    {
        if (questData == null)
        {
            Debug.LogWarning("[HubQuestObject] Aucune QuestData assignée.");
            return;
        }

        if (QuestManager.Instance == null)
        {
            Debug.LogError("[HubQuestObject] Aucun QuestManager trouvé (Scene_Persistent ?).");
            return;
        }

        // On demande au QuestManager de sélectionner cet objectif.
        QuestManager.Instance.SelectQuest(questData);

        Debug.Log($"[HubQuestObject] Objectif sélectionné : {questData.title}");
    }
}
