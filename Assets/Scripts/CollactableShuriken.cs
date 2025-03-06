using System;
using UnityEngine;

public class CollacTableShuriken : MonoBehaviour, IItem
{
    public static event Action<int> OnShurikenCollected;
    public int worth = 1;
    public void Collect()
    {
        OnShurikenCollected?.Invoke(worth);
        gameObject.SetActive(false);
    }
}
