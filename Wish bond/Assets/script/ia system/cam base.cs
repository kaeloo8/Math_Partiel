using UnityEngine;


[CreateAssetMenu(fileName = "cam base", menuName = "cam base")]
public class cambase : ScriptableObject
{
    [SerializeField] private float _rotation_speed = 1.0f;
    [SerializeField] private float _see_distance = 1.0f;
    [SerializeField] private float _detection_multiplie = 1.0f;
}
