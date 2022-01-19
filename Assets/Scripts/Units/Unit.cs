using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitParametersData unitParametersDataType;
    [SerializeField] private UnitPackData unitPackData;
    [SerializeField] private TextMeshProUGUI unitAmountView;
    
    private EAttackType _defaultAttackType;
    private EUnitType _unitType;
    private int _health;
    private int _damageMin;
    private int _damageMax;
    private int _speed;
    private int _initiative;

    private void Awake()
    {
        _defaultAttackType = unitParametersDataType.defaultAttackType;
        _health = unitParametersDataType.health;
        _damageMin = unitParametersDataType.damageMin;
        _damageMax = unitParametersDataType.damageMax;
        _speed = unitParametersDataType.speed;
        _initiative = unitParametersDataType.initiative;
        _unitType = unitParametersDataType.unitType;
    }

    public void UpdateUnitsAmountView(int newAmount)
    {
        unitAmountView.text = newAmount.ToString();
    }
}
