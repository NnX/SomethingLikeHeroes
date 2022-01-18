using UnityEngine;

[CreateAssetMenu(fileName = "UnitType", menuName = "ScriptableObjects/UnitBase", order = 1)]
public class Unit : ScriptableObject
{
    private int 
        health, attack;
    public int 
        health1, attack1;

    public int Damage(int count)
    {
        return count * attack;
    }
}
