using UnityEngine;

public class LevelQuestObject : MonoBehaviour, I_Interactable
{
    [Header("ID de l'objectif pour lequel cet objet est valide")]
    [SerializeField] private string questId;

    public void Interact()
    {
        if (QuestManager.Instance == null)
        {
            Debug.LogError("[LevelQuestObject] Aucun QuestManager trouvé.");
            return;
        }

        if (!QuestManager.Instance.IsInMission)
        {
            Debug.Log("[LevelQuestObject] Aucune mission en cours.");
            return;
        }

        // On vérifie que cette mission utilise bien cet objectif.
        if (!QuestManager.Instance.IsActiveQuest(questId))
        {
            Debug.Log("[LevelQuestObject] Cet objet ne correspond pas à l'objectif de la mission active.");
            return;
        }

        // On signale que l'objet a été récupéré.
        QuestManager.Instance.CollectQuestTarget();

        // On "récolte" l'objet -> il disparaît.
        gameObject.SetActive(false);

        Debug.Log("[LevelQuestObject] Objet de mission ramassé. Tu peux aller à la sortie.");
    }
}
