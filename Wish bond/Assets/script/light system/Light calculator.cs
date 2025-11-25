using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Lightcalculator", menuName = "Lightcalculator")]
public class Lightcalculator : ScriptableObject
{
    private RenderTexture[] _liste_capteur;
    [NonSerialized] private Camera[] _liste_camera;
    [SerializeField] private int _size_check = 1; // 1 = 1x1, 2 = 2x2, etc.

    private Texture2D _testTex;
    private float _lastLightValue;

    public void Init()
    {
        _liste_capteur = new RenderTexture[_liste_camera.Length];
        _testTex = new Texture2D(_size_check, _size_check, TextureFormat.RGB24, false);

        for (int i = 0; i < _liste_camera.Length; i++)
        {
            _liste_capteur[i] = new RenderTexture(128, 128, 32);
            _liste_camera[i].targetTexture = _liste_capteur[i];
        }
    }

    public void StartUpdating(MonoBehaviour owner, float updateRate = 0.1f) // a lancer absolument avant de recup les valeur de light 
    {
        owner.StartCoroutine(UpdateLightCoroutine(updateRate));
    }

    public void SetCameraList(Camera[] liste)
    {
        _liste_camera = liste;
    }

    private IEnumerator UpdateLightCoroutine(float updateRate)
    {
        while (true)
        {
            _lastLightValue = CalculateLight();
            Debug.Log(_lastLightValue);
            yield return new WaitForSeconds(updateRate);
        }
    }

    private float CalculateLight()
    {
        float lightValue = 0;
        var prevRT = RenderTexture.active;

        foreach (var capteur in _liste_capteur)
        {
            RenderTexture.active = capteur;
            _testTex.ReadPixels(new Rect(0, 0, _size_check, _size_check), 0, 0, false);
            RenderTexture.active = prevRT;

            float temp = 0;
            for (int y = 0; y < _size_check; y++)
            {
                for (int x = 0; x < _size_check; x++)
                {
                    Color pixel = _testTex.GetPixel(x, y);
                    temp += (pixel.r + pixel.g + pixel.b) / 3f;
                }
            }
            // lightValue += temp / (_size_check * _size_check); // autre methode pour obtenir un moyen de light
            float val = temp / (_size_check * _size_check);
            if (lightValue < val) lightValue = val;
        }
        // return lightValue / _listecapetur.lenght;
        return lightValue;
    }

    public float GetLastLightValue()
    {
        return _lastLightValue;
    }

}
