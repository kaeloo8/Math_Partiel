using UnityEngine;
using UnityEngine.InputSystem;

public class aaaa : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;

    [SerializeField] private InputActionReference _move;

    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        await Awaitable.WaitForSecondsAsync(1f);
        
        Debug.Log("aaaa");
        

    }

}
