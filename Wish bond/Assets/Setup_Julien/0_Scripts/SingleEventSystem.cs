using UnityEngine;
using UnityEngine.EventSystems;

[DefaultExecutionOrder(-1000)]
public class SingleEventSystem : MonoBehaviour
{
    void Awake()
    {
        var me = GetComponent<EventSystem>();
        var all = Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        foreach (var es in all) if (es != me) Destroy(es.gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
