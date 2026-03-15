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
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private GameObject arrow;
    
    // private fields
    private int arrowDirection;
    private Vector3 arrowOriginalScale;
    
    // components
    private Animator animator;
    private Collider2D Collider;
    private CircleCollider2D battleCollider;
    

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initializes some fields
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
    
    // flips arrows horizontally depending on the button that needs to be pressed
    private void FlipArrowHorizontally(KeyCode input)
    {
        arrowDirection = 1;
        if (input == KeyCode.RightArrow)
        {
            arrowDirection = -1;
        }
        arrow.transform.localScale = new Vector3(
            arrowDirection * arrowOriginalScale.x,
            arrowOriginalScale.y,
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
                FlipArrowHorizontally(inputs[i]);
                yield return null;
            }
        }

        arrow.transform.localScale = new Vector3(0, 0, 0);
        other.GetComponent<CharacterMovementController>().speed = 0;
        animator.SetTrigger("Dead");
        Collider.enabled = false;
        
        yield return new WaitForSeconds(1f);
        
        Time.timeScale = 1f;
        other.GetComponent<CharacterMovementController>().speed = 10;
        camera.orthographicSize = 13;


    }
    
}



