using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [Header("Objectif sélectionné dans le HUB (avant de lancer un niveau)")]
    [SerializeField] private QuestData selectedQuest;

    [Header("Objectif actif dans le niveau (mission en cours)")]
    [SerializeField] private QuestData activeQuest;

    [Header("État de la mission en cours")]
    [SerializeField] private bool hasCollectedTarget;

    public QuestData SelectedQuest => selectedQuest;
    public QuestData ActiveQuest => activeQuest;
    public bool HasCollectedTarget => hasCollectedTarget;
    public bool IsInMission => activeQuest != null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Choisir / changer l'objectif dans le HUB.
    public void SelectQuest(QuestData quest)
    {
        selectedQuest = quest;

        if (quest != null)
        {
            Debug.Log($"[QuestManager] Objectif sélectionné : {quest.title}");
        }
        else
        {
            Debug.Log("[QuestManager] Aucun objectif sélectionné.");
        }
    }

    public void StartMission(string levelSceneName)
    {
        if (selectedQuest == null)
        {
            Debug.LogWarning("[QuestManager] Impossible de lancer une mission : aucun objectif sélectionné dans le HUB.");
            return;
        }

        activeQuest = selectedQuest;
        hasCollectedTarget = false;

        Debug.Log($"[QuestManager] Mission lancée avec objectif : {activeQuest.title}. Chargement scène : {levelSceneName}");
        SceneManager.LoadScene(levelSceneName);
    }

    // Vérifie si la mission active correspond à ce questId (utilisé par l'objet à récolter).
    public bool IsActiveQuest(string questId)
    {
        return activeQuest != null && activeQuest.questId == questId;
    }

    // Appelé quand le joueur ramasse l'objet d'objectif dans le niveau.
    public void CollectQuestTarget()
    {
        if (activeQuest == null)
        {
            Debug.LogWarning("[QuestManager] CollectQuestTarget appelé, mais aucune mission active.");
            return;
        }

        hasCollectedTarget = true;
        Debug.Log("[QuestManager] Objet de mission récupéré !");
    }

    // Mission réussie : le joueur a l'objet et sort du niveau.
    public void CompleteMission()
    {
        if (activeQuest == null)
        {
            Debug.LogWarning("[QuestManager] CompleteMission appelé, mais aucune mission active.");
            return;
        }

        Debug.Log($"[QuestManager] Mission réussie : {activeQuest.title}");

        activeQuest = null;
        hasCollectedTarget = false;

        // Retour au HUB
        SceneManager.LoadScene("Scene_Hub");
    }

    // Mission ratée
    public void FailMission()
    {
        if (activeQuest == null)
        {
            Debug.LogWarning("[QuestManager] FailMission appelé, mais aucune mission active.");
            return;
        }

        Debug.Log($"[QuestManager] Mission échouée : {activeQuest.title}");

        activeQuest = null;
        hasCollectedTarget = false;

        SceneManager.LoadScene("Scene_Hub");
    }
}
