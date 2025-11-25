using UnityEngine;
using UnityEngine.UI;

public class Player_Stats : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private int damageMultiplicator;
    [SerializeField] private int healthMultiplicator;
    [SerializeField] private int defenseMultiplicator;

    public int Level
    {
        get => level;
        set => level = value;
    }

    public int DamageMultiplicator
    {
        get => damageMultiplicator;
        set => damageMultiplicator = value;
    }

    public int HealthMultiplicator
    {
        get => healthMultiplicator;
        set => healthMultiplicator = value;
    }

    public int DefenseMultiplicator
    {
        get => defenseMultiplicator;
        set => defenseMultiplicator = value;
    }
}
