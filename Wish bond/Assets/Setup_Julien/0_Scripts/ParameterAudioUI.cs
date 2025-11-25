using UnityEngine;
using UnityEngine.UI;

public class ParameterAudioUI : MonoBehaviour
{
    [Header("Références UI")]
    [SerializeField] private Slider masterSlider;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            masterSlider.value = AudioManager.Instance.MasterVolume;
        }
        masterSlider.onValueChanged.AddListener(OnMasterSliderChanged);
    }

    private void OnDestroy()
    {
        masterSlider.onValueChanged.RemoveListener(OnMasterSliderChanged);
    }

    private void OnMasterSliderChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(value);
        }
    }
}
