using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;

    [SerializeField]
    private float parralaxEffect;

    void Awake()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
     
    void Update()
    {
        float temp = Camera.main.transform.position.x * (1 - parralaxEffect);
        float distance = Camera.main.transform.position.x * parralaxEffect;

        transform.position = new Vector3 (startPos + distance, transform.position.y, transform.position.z);

        print(temp + " " + distance);
        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length) 
        {
            startPos -= length;
        }
    }
}
