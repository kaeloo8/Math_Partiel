using UnityEngine;

public class setparam : MonoBehaviour
{

    [SerializeField] private Lightcalculator lightcalculator;
    [SerializeField] private Camera[] _liste_camera;
    private void Start()
    {
        lightcalculator.SetCameraList(_liste_camera);
        lightcalculator.Init();
        lightcalculator.StartUpdating(this, 0.3f);
    }

}
