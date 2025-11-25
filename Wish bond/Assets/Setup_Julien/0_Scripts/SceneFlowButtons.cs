using UnityEngine;

/// <summary>
/// Proxy pour appeler SceneFlow depuis des boutons UI.
/// </summary>
public class SceneFlowButtons : MonoBehaviour
{
    private SceneFlow Flow => SceneFlow.Instance;

    public void GoToMainMenu()
    {
        Flow?.LoadMainMenu();
    }

    public void GoToHub()
    {
        Flow?.LoadHub();
    }

    public void GoToTutorial()
    {
        Flow?.LoadTutorial();
    }

    public void GoToLvl1()
    {
        Flow?.LoadLvl1();
    }

    public void GoToLvl2()
    {
        Flow?.LoadLvl2();
    }

    public void GoToTestJulien()
    {
        Flow?.LoadTestJulien();
    }

    public void ResetGame()
    {
        Flow?.ResetGame();
    }

    public void ExitGame()
    {
        Flow?.ExitGame();
    }
}
