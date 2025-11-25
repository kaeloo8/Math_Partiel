using UnityEngine;


[CreateAssetMenu(fileName = "player data", menuName = "player data")]
public class playerscript : ScriptableObject
{
    [SerializeField] public float sp_walk = 1f;
    [SerializeField] public float sp_run = 1f;
    [SerializeField] public float sp_crouch = 1f;
    [SerializeField] public float jump = 1f;
    [SerializeField] public float distance_throw = 1f;
    [SerializeField] public float min_noise = 0f;
    [SerializeField] public float max_noise = 1f;
    [SerializeField] public float min_light_lv = 0f;
    [SerializeField] public float max_light_lv = 1f;

}
