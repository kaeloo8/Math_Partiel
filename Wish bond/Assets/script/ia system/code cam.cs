using UnityEngine;

public class codecam : MonoBehaviour
{
    [SerializeField] private cambase _cb;
    void Start()
    {
        if (_cb == null)
        {
            Debug.Log("erreur cam so null destruction");
            Destroy(this);
            return;
        }
    }


    void Update()
    {
        
    }
}
