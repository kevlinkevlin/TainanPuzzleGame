using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartCoroutine : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Example());

    }

    IEnumerator Example()
    {
        print(Time.time);
        
        yield return 3;
        print(Time.time);
    }
}