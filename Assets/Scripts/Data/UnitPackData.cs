using UnityEngine;

[CreateAssetMenu(fileName = "UnitPackParams", menuName = "ScriptableObjects/UnitPackData", order = 1)]
public class UnitPackData : ScriptableObject
{
    public EUnitType unitType;
    public int unitCount;
    public ESkillType skillType;
}
