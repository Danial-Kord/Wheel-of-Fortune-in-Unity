using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHandler : MonoBehaviour
{


    public void onClick()
    {
        StartCoroutine(move());
    }

    private IEnumerator move()
    {
        int i = 0;
        while (i < 200)
        {
         transform.Translate(transform.up*5);
         i++;
         yield return null;
        }
        Destroy(gameObject);
    }
}
