using System;
using UnityEngine;

public class HealthItem : MonoBehaviour, IItem
{
    public int healAmount = 1;
    public static event Action<int> OnHealthCollected;
    public void Collect()
    {
        OnHealthCollected.Invoke(healAmount);
        gameObject.SetActive(false);
    }


}
