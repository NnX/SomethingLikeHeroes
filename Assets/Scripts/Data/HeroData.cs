using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroData", menuName = "ScriptableObjects/HeroData", order = 1)]
public class HeroData : ScriptableObject
{
    public List<GameObject> heroUnitPrefabs;

}
