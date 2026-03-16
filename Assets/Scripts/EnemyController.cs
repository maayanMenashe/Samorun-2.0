using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


public class EnemyController : MonoBehaviour
{
    // for testing
    public float slowedTime = 0.1f;
    public float shrinkedCameraSize = 5f;
    public float battleAreaRadius = 5f;
    
    
    
    
    
    
    
    // serialized fields
    private Camera camera;
    [SerializeField]
    private GameObject arrow;


    
    // private fields
    private int arrowHorizontalDirection;
    private int arrowVerticalDirection;
    private Vector3 arrowOriginalScale;
    private float originalCameraSize;

    
    // components
    private Animator animator;
    private Collider2D Collider;
    private CircleCollider2D battleCollider;
    

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initializes some fields
        camera = Camera.main;
        originalCameraSize = camera.orthographicSize;
        battleCollider = GetComponentInChildren<CircleCollider2D>();
        battleCollider.radius = battleAreaRadius;
        animator = GetComponent<Animator>();
        Collider = GetComponent<Collider2D>();
        animator.SetInteger("AnimationNum", -1);
        arrowOriginalScale = arrow.transform.localScale; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void KillEnemy()
    {
        Destroy(gameObject);
    }
    
    
    
    // flips arrows horizontally depending on the button that needs to be pressed
    private void FlipArrow(KeyCode input)
    {
        arrowHorizontalDirection = 1;
        arrowVerticalDirection = 1;
        
        if (input == KeyCode.RightArrow)
        {
            arrowHorizontalDirection = -1;
        }
        
        if (input == KeyCode.DownArrow)
        {
            arrowVerticalDirection = -1;
        }
        
        arrow.transform.localScale = new Vector3(
            arrowHorizontalDirection * arrowOriginalScale.x,
            arrowVerticalDirection * arrowOriginalScale.y,
            arrowOriginalScale.z);
    }



    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        { 
            StartCoroutine(Fight(other));
        }
    }
    
    
    IEnumerator Fight(Collider2D other)
    {
        camera.orthographicSize = shrinkedCameraSize;
        Time.timeScale = 0.1f;
        
        
        KeyCode[] inputs = new KeyCode[4];
        inputs[0] = KeyCode.LeftArrow;
        inputs[1] = KeyCode.RightArrow;
        inputs[2] = KeyCode.LeftArrow;
        inputs[3] = KeyCode.RightArrow;

        for (int i = 0; i < 4; i++)
        {
            animator.SetInteger("AnimationNum", i-1);
            camera.orthographicSize = shrinkedCameraSize + 2 - i;

            while (!Input.GetKeyDown(inputs[i]))
            {
                FlipArrow(inputs[i]);
                yield return null;
            }
        }

        arrow.transform.localScale = new Vector3(0, 0, 0);
        other.GetComponent<CharacterMovementController>().speed = 0;
        GetComponent<CharacterMovementController>().speed = 0;
        animator.SetTrigger("Dead"); 
        
        yield return new WaitForSeconds(0.16f);
        
        Time.timeScale = 1f;
        other.GetComponent<CharacterMovementController>().speed = 10;
        camera.orthographicSize = originalCameraSize;
    }
    
}



