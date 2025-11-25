using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class SceneFlow : MonoBehaviour
{
    [Header("Noms EXACTS des scènes (BuildSettings)")]
    [SerializeField] private string scenePersistent = "Scene_Persistent";
    [SerializeField] private string sceneMainMenu = "Scene_MainMenu";
    [SerializeField] private string sceneHub = "Scene_Hub";
    [SerializeField] private string sceneTutorial = "Scene_Tutorial";
    [SerializeField] private string sceneLvl1 = "Scene_Lvl1";
    [SerializeField] private string sceneLvl2 = "Scene_Lvl2";
    [SerializeField] private string sceneTestJulien = "Scene_Test_Julien";
    [SerializeField] private string sceneParameter = "Scene_Parameter";
    [SerializeField] private string sceneLoading = "Scene_Loading";

    [Header("Démarrage")]
    [Tooltip("Si vrai : au lancement, si seule Scene_Persistent est chargée, on ouvre automatiquement Scene_MainMenu.")]
    [SerializeField] private bool autoLoadMainMenuOnStart = true;

    [Header("Touche pour ouvrir/fermer les paramètres")]
    [SerializeField] private Key parameterKey = Key.Escape;

    public static SceneFlow Instance { get; private set; }

    private bool isTransitioning = false;
    private bool isParametreOpen = false;
    private bool currentSceneReportedReady = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    private void Start()
    {
        int nonPersistentCount = 0;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            if (s.isLoaded && s.name != scenePersistent)
            {
                nonPersistentCount++;
            }
        }

        if (autoLoadMainMenuOnStart && nonPersistentCount == 0)
        {
            LoadMainMenu();
        }
    }

    private void Update()
    {
        if (isTransitioning || Keyboard.current == null)
            return;

        var keyControl = Keyboard.current[parameterKey];

        if (keyControl != null && keyControl.wasPressedThisFrame)
        {
            if (!isParametreOpen)
                StartCoroutine(OpenParametre());
            else
                StartCoroutine(CloseParametre());
        }
    }


    public void LoadMainMenu()
    {
        if (!isTransitioning)
            StartCoroutine(Go(sceneMainMenu, waitForSceneReady: false));
    }

    public void LoadHub()
    {
        if (!isTransitioning)
            StartCoroutine(Go(sceneHub, waitForSceneReady: false));
    }

    public void LoadTutorial()
    {
        if (!isTransitioning)
            StartCoroutine(Go(sceneTutorial, waitForSceneReady: true));
    }

    public void LoadLvl1()
    {
        if (!isTransitioning)
            StartCoroutine(Go(sceneLvl1, waitForSceneReady: true));
    }

    public void LoadLvl2()
    {
        if (!isTransitioning)
            StartCoroutine(Go(sceneLvl2, waitForSceneReady: true));
    }

    public void LoadTestJulien()
    {
        if (!isTransitioning)
            StartCoroutine(Go(sceneTestJulien, waitForSceneReady: true));
    }

    public void LoadByName(string sceneName, bool waitForSceneReady = true)
    {
        if (string.IsNullOrEmpty(sceneName) || isTransitioning)
            return;

        StartCoroutine(Go(sceneName, waitForSceneReady));
    }

    public void SignalSceneReady()
    {
        currentSceneReportedReady = true;
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        LoadMainMenu();
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator Go(string targetSceneName, bool waitForSceneReady)
    {
        isTransitioning = true;
        currentSceneReportedReady = false;
        Time.timeScale = 1f;

        if (isParametreOpen)
            yield return CloseParametre();

        {
            Scene loading = SceneManager.GetSceneByName(sceneLoading);
            if (!(loading.IsValid() && loading.isLoaded))
            {
                AsyncOperation op = SceneManager.LoadSceneAsync(sceneLoading, LoadSceneMode.Additive);
                while (!op.isDone)
                    yield return null;

                loading = SceneManager.GetSceneByName(sceneLoading);
            }

            if (loading.IsValid() && loading.isLoaded)
                SceneManager.SetActiveScene(loading);
        }

        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene s = SceneManager.GetSceneAt(i);
                if (!s.isLoaded) continue;
                if (s.name == scenePersistent) continue;
                if (s.name == sceneLoading) continue;

                AsyncOperation opUnload = SceneManager.UnloadSceneAsync(s);
                while (opUnload != null && !opUnload.isDone)
                    yield return null;
            }
        }

        Scene targetScene;
        {
            targetScene = SceneManager.GetSceneByName(targetSceneName);
            if (!(targetScene.IsValid() && targetScene.isLoaded))
            {
                AsyncOperation op = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
                while (!op.isDone)
                    yield return null;

                targetScene = SceneManager.GetSceneByName(targetSceneName);
            }

            if (targetScene.IsValid() && targetScene.isLoaded)
                SceneManager.SetActiveScene(targetScene);
        }

        if (waitForSceneReady)
        {
            yield return null;
            while (!currentSceneReportedReady)
                yield return null;
        }
        else
        {
            yield return new WaitForSeconds(0.25f);
        }

        {
            Scene loading = SceneManager.GetSceneByName(sceneLoading);
            if (loading.IsValid() && loading.isLoaded)
            {
                AsyncOperation op = SceneManager.UnloadSceneAsync(sceneLoading);
                while (op != null && !op.isDone)
                    yield return null;
            }
        }

        isTransitioning = false;
    }

    private IEnumerator OpenParametre()
    {
        Scene paramScene = SceneManager.GetSceneByName(sceneParameter);
        if (!(paramScene.IsValid() && paramScene.isLoaded))
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneParameter, LoadSceneMode.Additive);
            while (!op.isDone)
                yield return null;
        }

        isParametreOpen = true;
        Time.timeScale = 0f;
    }

    private IEnumerator CloseParametre()
    {
        Time.timeScale = 1f;

        Scene paramScene = SceneManager.GetSceneByName(sceneParameter);
        if (paramScene.IsValid() && paramScene.isLoaded)
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(sceneParameter);
            while (!op.isDone)
                yield return null;
        }

        isParametreOpen = false;
        yield break;
    }

    public void CloseParametreFromButton()
    {
        if (!isTransitioning && isParametreOpen)
            StartCoroutine(CloseParametre());
    }
}
