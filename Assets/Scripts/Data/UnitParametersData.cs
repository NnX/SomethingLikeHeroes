using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "UnitParameters", menuName = "ScriptableObjects/UnitParametersData", order = 1)]
public class UnitParametersData
    : ScriptableObject
{
    public int health, damageMin, damageMax, speed, initiative;
    [FormerlySerializedAs("defaultAttackType")] public EAttackType defaultAttackType;
    [FormerlySerializedAs("_unitType")] public EUnitType unitType;
    
}
