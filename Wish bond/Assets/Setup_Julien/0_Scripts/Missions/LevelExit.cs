using UnityEngine;

public class LevelExit : MonoBehaviour, I_Interactable
{
    public void Interact()
    {
        if (QuestManager.Instance == null)
        {
            Debug.LogError("[LevelExit] Aucun QuestManager trouvé.");
            return;
        }

        if (!QuestManager.Instance.IsInMission)
        {
            Debug.Log("[LevelExit] Aucune mission en cours.");
            return;
        }

        if (QuestManager.Instance.HasCollectedTarget)
        {
            Debug.Log("[LevelExit] Tu as l'objet -> mission réussie !");
            QuestManager.Instance.CompleteMission();
        }
        else
        {
            Debug.Log("[LevelExit] Tu dois d'abord récupérer l'objet de mission avant de partir.");
        }
    }
}
