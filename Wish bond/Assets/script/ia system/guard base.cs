using UnityEngine;
using UnityEngine.AI;


[CreateAssetMenu(fileName = "guard base", menuName = "guard base")]
public class guardbase : ScriptableObject
{
    [SerializeField] public float speed = 1.0f;
    [SerializeField] public float run = 1.0f;

    [SerializeField] public float hear_distance = 1.0f;

    [SerializeField] public int point_cone = 10;
    [SerializeField] public Material material_cone;
    [SerializeField] public float offset_cone = 0.5f;

    [SerializeField] public float detection_multiplie = 0.2f;
    [SerializeField] public float timer_investigate = 2f;
    [SerializeField] public float suspition_decrease = 0.2f;
    [SerializeField] public float detecte_level = 3f;
    public bool detecter = false;

    [SerializeField] public float error_move_marge = 0.03f;

    [SerializeField] public EnemyState default_recherche_state = EnemyState.Suspicious;

}
