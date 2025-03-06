using System;
using UnityEngine;

public class Gem : MonoBehaviour, IItem
{
    public static event Action<int> OnGemCollected;
    public int worth = 1;
    public void Collect()
    {
        OnGemCollected?.Invoke(worth);
        SoundEffectManager.Play("Coin");
        gameObject.SetActive(false);
    }
}
