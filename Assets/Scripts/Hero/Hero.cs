using System.Collections.Generic;
using UnityEngine;
public class Hero : MonoBehaviour
{
    public const int MaxPackAmount = 7;
    public Dictionary<EUnitType, int> UnitTypeAmount;

    [SerializeField] private HeroData heroData;
    [SerializeField] private List<UnitPackData> unitPackData;


    public void Init()
    {
        //TODO instantiate unit prefabs
        //TODO fill unit parameters
        //TODO unit amount view
    }
}
