using UnityEngine;


[DisallowMultipleComponent]
public class DDOL : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
