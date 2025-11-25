using System.Collections;
using UnityEngine;

public class SceneReadyOnStart : MonoBehaviour
{
    [SerializeField] private float minLoadingTime = 1f;

    private IEnumerator Start()
    {
        if (minLoadingTime > 0f)
        {
            yield return new WaitForSeconds(minLoadingTime);
        }

        if (SceneFlow.Instance != null)
        {
            SceneFlow.Instance.SignalSceneReady();
        }
    }
}
