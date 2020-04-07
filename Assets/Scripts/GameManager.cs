using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject[] victoryEffect;
    public static GameManager instance;
    public bool playing = false;
    private void Start()
    {
        instance = this;
        //victoryEffect = GetComponent<DemoController>().m_Particles;
    }

    public void playVictory()
    {
        for (int i = 0; i < victoryEffect.Length; i++)
        {
           // Instantiate(victoryEffect[i],new Vector3(Random.Range(-20,20),Random.Range(-20,20),Random.Range(-20,20)),Quaternion.identity);
            
        }

        playing = true;
        StartCoroutine(startAnimations());
    }

    IEnumerator startAnimations()
    {
        GameObject last = null;
        ArrayList all = new ArrayList();
        while (playing)
        {

            last = Instantiate(victoryEffect[Random.Range(0, victoryEffect.Length)]);
            last.transform.position = new Vector3(Random.Range(-12, 12), Random.Range(-4.5f, 6), 0);
            last.transform.localScale *= Random.Range(2,3.5f);
            all.Add(last);
            if (Random.Range(0, 20) < 12)
            {
                last.GetComponent<AudioSource>().enabled = false;
            }
            yield return new WaitForSeconds(Random.Range(0.05f,0.1f));
        }

        for (int i = 0; i < all.Count; i++)
        {
            Destroy(all[i] as GameObject);
        }
        yield break;
        ;
    }
}
