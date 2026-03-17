using System;
using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    //serilized fields
    [SerializeField]
    private AudioManager audioManager;
    
    // private variables
    private Vector3 startingPosition;
    private Vector3 respawnPoint;
    private float originalCameraSize;
    private float originalSpeed;
    private float originalTimeScale = 1f;

    
    // components
    private Camera Camera;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initializes some fields
        originalSpeed = GetComponent<CharacterMovementController>().speed;// saves players original speed
        startingPosition = transform.position;// sets starting position
        respawnPoint = startingPosition;// sets respawn point as starting position
        Camera = Camera.main;// sets camer
        originalCameraSize = Camera.orthographicSize;// saves original camera size
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // when entering the enemy's killing range, the player dies, reverts everything to the way it was in the last checkpoint
        if (other.CompareTag("EnemyKillRange"))
        {
            // destroys all the enemies that spawned
            GameObject[] allGameObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            foreach (var gameObject in allGameObjects)
            {
                if (gameObject.CompareTag("Enemy"))
                {
                    Destroy(gameObject);
                }
                
            }
            
            // reverts everytjing back
            Camera.orthographicSize = originalCameraSize;
            GetComponent<CharacterMovementController>().speed = 10f;
            Time.timeScale = originalTimeScale;
            audioManager.PlayPlayerDeathSFX();// plays death SFX
            transform.position = respawnPoint;// player returns to respawn point
            


        }

        // sets respawn point as checkpoint when is reached
        if (other.CompareTag("Checkpoint"))
        {
            respawnPoint = other.transform.position;
        }
    }
}
