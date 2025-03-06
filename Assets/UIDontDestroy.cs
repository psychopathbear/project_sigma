using UnityEngine;

public class UIDontDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
