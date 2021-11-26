using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] wasteArray;
    public float rate=1f;
    void Start()
    {
        StartCoroutine(CreateWaste(rate));
    }
    IEnumerator CreateWaste(float rate)
    {
        while (true)
        {
            var index = Random.Range(0, 3);
            Instantiate(wasteArray[index], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(rate);
        }
    }
}