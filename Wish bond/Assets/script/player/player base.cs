using UnityEngine;

public class playerbase : MonoBehaviour, Suspect_interface
{
    [SerializeField] int _suspect_importance = 10;
    [SerializeField] Lightcalculator _lightcalculator;

    public float GetDetectionLevel()
    {
        if (_lightcalculator == null)
            return 1f;
        Debug.Log(_lightcalculator.GetLastLightValue());
        return _lightcalculator.GetLastLightValue();
    }
    public int GetImportance_level()
    {
        return _suspect_importance;
    }
}
