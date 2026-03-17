using System;
using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    
    private Vector3 startingPosition;
    private Vector3 respawnPoint;
    private Camera Camera;
    private float originalCameraSize;
    private float originalSpeed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalSpeed = GetComponent<CharacterMovementController>().speed;
        startingPosition = transform.position;
        respawnPoint = startingPosition;
        Camera = Camera.main;
        originalCameraSize = Camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyKillRange"))
        {
            GameObject[] allGameObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            foreach (var gameObject in allGameObjects)
            {
                if (gameObject.CompareTag("Enemy"))
                {
                    Destroy(gameObject);
                }
                
            }
            
            Camera.orthographicSize = originalCameraSize;
            GetComponent<CharacterMovementController>().speed = originalSpeed;
            Time.timeScale = 1f;
            audioManager.PlayPlayerDeathSFX();
            transform.position = respawnPoint;
            


        }

        if (other.CompareTag("Checkpoint"))
        {
            respawnPoint = other.transform.position;
        }
    }
}
