using System.Collections;
using UnityEngine;

public class LearnCoroutine : MonoBehaviour
{
    public Transform ming;

    private void Start()
    {
        StartCoroutine(Test());
        StartCoroutine(Big());
    }

    public IEnumerator Test()
    {
        print("嗨，我是協程");
        yield return new WaitForSeconds(2);
        print("嗨，我試過了兩秒的協程");

    }

    public IEnumerator Big()
    {
        for (int i = 0; i < 10; i++)
        {
            ming.localScale += Vector3.one;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
