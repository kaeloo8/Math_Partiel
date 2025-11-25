using UnityEngine;

public interface Suspect_interface
{
    public float GetDetectionLevel(); // 1 = obj / joueur entre selon lumiere 0 - 1 (min != 0) !!!
    public int GetImportance_level(); //  0 < 1 importance
}
