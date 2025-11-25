using UnityEngine;

public class MissionLauncher : MonoBehaviour, I_Interactable
{
    [Header("Nom de la scène de niveau à charger")]
    [SerializeField] private string levelSceneName = "Scene_Lvl_01";

    public void Interact()
    {
        if (QuestManager.Instance == null)
        {
            Debug.LogError("[MissionLauncher] Aucun QuestManager trouvé.");
            return;
        }

        QuestManager.Instance.StartMission(levelSceneName);
    }
}
