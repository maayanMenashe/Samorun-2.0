using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private GameObject fire;

    private bool firstTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!firstTime)
            {
                fire.transform.localScale *= 5;
                firstTime = true;
            }
        }
    }
}
