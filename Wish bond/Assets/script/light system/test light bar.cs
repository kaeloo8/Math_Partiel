using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private Lightcalculator lightcalculator;

    [SerializeField] private Image back_visibility;
    [SerializeField] private Image front_visibility;

    private void Update()
    {
        front_visibility.transform.localScale = new Vector2(back_visibility.transform.localScale.x, back_visibility.transform.localScale.x * lightcalculator.GetLastLightValue());
    }
}
