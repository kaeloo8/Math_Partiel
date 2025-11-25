using UnityEngine;
using UnityEngine.UIElements;

public class comviewsensor : MonoBehaviour
{
    [SerializeField] private codeguard _cg;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.TryGetComponent<Suspect_interface>(out var d_level))
        {
            _cg.AddTargetView(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Suspect_interface>(out var d_level))
        {
            _cg.RemoveTargetView(other.gameObject);
        }
    }
}
