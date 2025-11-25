using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Gère le volume global du jeu (Master, Music, SFX).
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Référence vers l'AudioMixer global")]
    [SerializeField] private AudioMixer mainMixer;

    // Noms des paramètres exposés dans l'AudioMixer
    private const string MASTER_PARAM = "MasterVolume";

    // Keys pour sauvegarder les valeurs dans PlayerPrefs
    private const string MASTER_PREF_KEY = "VOLUME_MASTER";

    // Propriété pour que l'UI puisse lire la valeur actuelle (0..1)
    public float MasterVolume { get; private set; } = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        LoadVolumes();
    }

    private void LoadVolumes()
    {
        float savedMaster = PlayerPrefs.GetFloat(MASTER_PREF_KEY, 0.8f);
        SetMasterVolume(savedMaster, saveInPrefs: false);
    }

    public void SetMasterVolume(float value, bool saveInPrefs = true)
    {
        MasterVolume = Mathf.Clamp01(value);

        float volumeDb;

        if (MasterVolume <= 0.0001f)
        {
            volumeDb = -80f;
        }
        else
        {
            volumeDb = Mathf.Log10(MasterVolume) * 20f;
        }

        if (mainMixer != null)
        {
            mainMixer.SetFloat(MASTER_PARAM, volumeDb);
        }

        if (saveInPrefs)
        {
            PlayerPrefs.SetFloat(MASTER_PREF_KEY, MasterVolume);
        }
    }
}
