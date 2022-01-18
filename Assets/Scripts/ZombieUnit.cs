using UnityEngine;
public class ZombieUnit : MonoBehaviour
{
    [SerializeField] private Unit unitType;

    private int _unitHealth;

    private void Awake()
    {
        _unitHealth = unitType.health1;
    }
}
