using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;

    private Vector3 nextPosition;

    void Start()
    {
        nextPosition = pointA.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);

        if (transform.position == nextPosition)
        {
            nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CoroutineHandler.StartStaticCoroutine(ResetParent(collision.transform));
        }
    }

    private static IEnumerator ResetParent(Transform playerTransform)
    {
        yield return new WaitForEndOfFrame();
        playerTransform.SetParent(null);
    }
}

public static class CoroutineHandler
{
    private class CoroutineHolder : MonoBehaviour { }

    private static CoroutineHolder _coroutineHolder;

    public static void StartStaticCoroutine(IEnumerator coroutine)
    {
        if (_coroutineHolder == null)
        {
            GameObject obj = new GameObject("CoroutineHolder");
            _coroutineHolder = obj.AddComponent<CoroutineHolder>();
            Object.DontDestroyOnLoad(obj);
        }
        _coroutineHolder.StartCoroutine(coroutine);
    }
}
