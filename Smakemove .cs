using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Snakemove : MonoBehaviour
{
    
    public static ScoreManager Instance;

    public int score = 0;
     // Assign in Inspector

    private Rigidbody2D rb;
    private Vector2 movedirection = Vector2.right;
    private float movetimer = 0;
    private float movetimemax = 0.9f;
    public GameObject bodyPrefab; // Assign this in the Inspector (Prefab of snake body part)
    private List<Transform> snakeBody = new List<Transform>(); // Stores the body parts
    private List<SnakeStep> movementHistory = new List<SnakeStep>();

   private List<Vector3> positionHistory = new List<Vector3>();
  
    public float moveRate = 0.2f;
    private float timer;
    private float GetAngleFromVector(Vector2 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return (n < 0) ? n + 360 : n;
    }



    private List<Transform> snakeBodyTransformlist;
    private Vector2 direction = Vector2.right;

  

    private struct SnakeStep
    {
        public Vector3 position;
        public Vector2 direction;

        public SnakeStep(Vector3 pos, Vector2 dir)
        {
            position = pos;
            direction = dir;
        }
    }


    private void Die()
    {
        Debug.Log("Snake Died!");

        if (CameraShake.instance != null)
        {
            CameraShake.instance.TriggerShake(0.3f, 0.3f);
        }

        GameOverManager.Instance.ShowGameOver(ScoreManager.Instance.score);

        StartCoroutine(StopTimeAfterDelay(0.3f));
    }

  private IEnumerator StopTimeAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 0f;
    }



    public void Grow()
{
        Vector3 spawnPosition;

        if (snakeBody.Count > 0)
    {
            // Use last body part's position
            spawnPosition = snakeBody[snakeBody.Count - 1].position;
        }
    else
    {
            // Place close behind the head (closer than before)
            spawnPosition = transform.position - (Vector3)movedirection * 0.5f; // Reduced offset
    }

        GameObject bodyPart = Instantiate(bodyPrefab, spawnPosition, Quaternion.identity);
        snakeBody.Add(bodyPart.transform);

    }


    private void Awake()
    {
        movetimemax = 0.1f;
        movetimer = movetimemax;
        movedirection = new Vector2(1, 0); // Start moving right



        snakeBodyTransformlist = new List<Transform>();
    }

    private void Start()
    {
    //   timer = movetimemax;
      //  movementHistory.Add(new SnakeStep(transform.position, moveDirection));
    
    positionHistory.Add(transform.position);// Start with head's position
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        // Default movement direction
    }

    private void Update()
    {
        HandleInput();
        transform.position += (Vector3)direction * movetimer * Time.deltaTime;
      //  moveTimer -= Time.deltaTime;
      //  if (moveTimer <= 0f)
        //{
          //  Move();
            //moveTimer = moveStepTime;
        //}
        HandleGridMovement();
        

    }
    //this off kry start     ffffffffff
    public void SetDirection(Vector2 newDirection)
    {
        Debug.Log("Button pressed: " + newDirection);

        // Prevent the snake from going back directly
        if (newDirection + direction != Vector2.zero)
        {
            direction = newDirection;
            movedirection = newDirection; // VERY IMPORTANT
        }
    }


    private void HandleInput()

    {
        Vector2 previousDirection = movedirection;

        if (Input.GetKeyDown(KeyCode.UpArrow) && movedirection.y != -1)
            movedirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) && movedirection.y != 1)
            movedirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && movedirection.x != 1)
            movedirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && movedirection.x != -1)
            movedirection = Vector2.right;

        if (previousDirection != movedirection)
        {
            movementHistory.Add(new SnakeStep(transform.position, movedirection));
        }
    }
       
    //  private void Move()
    //{

    //  Vector3 nextPosition = transform.position + (Vector3)(movedirection * gridSize);
    //transform.position = nextPosition;

    //  positionHistory.Insert(0, transform.position);

    //for (int i = 0; i<snakeBody.Count; i++)
    //{
    //  snakeBody[i].position = positionHistory[Mathf.Min(i + 1, positionHistory.Count - 1)];
    //}

    // Optional: Trim history to size
    //int maxHistory = snakeBody.Count + 5;
    //if (positionHistory.Count > maxHistory)
    //{
    //  positionHistory.RemoveAt(positionHistory.Count - 1);
    //}
    //}




    private void HandleGridMovement()
    {
        movetimer += Time.deltaTime;
        if (movetimer >= movetimemax)
        {
            movetimer -= movetimemax;

            // Save current head position before moving
            movementHistory.Insert(0, new SnakeStep(transform.position, movedirection));


            // Move head
            transform.position = new Vector3(
                Mathf.Round(transform.position.x) + movedirection.x,
                Mathf.Round(transform.position.y) + movedirection.y,
                0f
            );

            // Rotate to direction
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(movedirection) - 90);

            // Move each body part to the past position of the part in front of it
            for (int i = 0; i < snakeBody.Count; i++)
            {
                float spacing = 0.500f; // Smaller spacing
                int index = Mathf.RoundToInt((i + 0) * spacing);
                // You can tweak spacing here

                if (movementHistory.Count > index)
                {
                    SnakeStep step = movementHistory[index];
                    snakeBody[i].position = step.position;

                    float angle = GetAngleFromVector(step.direction) - 90;
                    snakeBody[i].eulerAngles = new Vector3(0, 0, angle);
              
                
                
                
                
                }
            }


            // Keep history size in check
            int historyLimit = snakeBody.Count + 1;
            if (movementHistory.Count > historyLimit)
            {
                movementHistory.RemoveAt(movementHistory.Count - 1);
          
            
            
            }
            for (int i = 0; i < snakeBody.Count; i++)
            {
                Vector3 direction = transform.position - snakeBody[i].position;
                if (direction != Vector3.zero)
                {
                    float bodyAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    snakeBody[i].eulerAngles = new Vector3(0, 0, bodyAngle - 90);
                }
            }


            Debug.Log("Snake moved. Body parts following.");
        }
    }






    // private void MoveBody(Vector3 newPosition)
    //{
    //  for (int i = snakeBody.Count - 1; i > 0; i--)
    //{
    //   snakeBody[i].position = snakeBody[i - 1].position;
    //}
    //if (snakeBody.Count > 0)
    //{
    //    snakeBody[0].position = newPosition;
    //}
    //}




    private float GetAngleFromVector(Vector3 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return (n < 0) ? n + 360 : n;
    }
    //private void OnTriggerEnter2D(Collider2D collision)
   // {
      
    //}



    // private void GrowSnake()
    //{
    //  Vector3 newPartPosition = (snakeBody.Count == 0) ? transform.position : snakeBody[snakeBody.Count - 1].position;
    //GameObject newPart = Instantiate(bodyPrefab, newPartPosition, Quaternion.identity);
    //snakeBody.Add(newPart.transform);
    //}

    //private void FixedUpdate()

    //{


    //  MoveSnake(); // Move using physics
    //}
    // private void MoveSnake()
    //{
    // rb.linearVelocity = movedirection * movetimer; // Uses physics for smooth movement
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            SoundManager.Instance.PlayGameOverSound();

          Debug.Log("Hit a Wall!");
            Die();
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            SoundManager.Instance.StopBGM();

            SoundManager.Instance.PlayGameOverSound();

            Debug.Log("Hit a Wall on: " + collision.contacts[0].normal);

            // Prevent movement in that direction
            rb.linearVelocity = Vector3.zero;
            movedirection = Vector3.zero;
            Debug.Log("Collided with: " + collision.gameObject.name);
        }
        if (collision.gameObject.CompareTag("food1"))
        {
            Debug.Log("Ate food1");
            SoundManager.Instance.PlaySFX(SoundManager.Instance.eatSound);
            //SoundManager.Instance.PlaySFX(SoundManager.Instance.screamSound, 0.3f); // Optional

            ScoreManager.Instance.AddScore(5);
            BloodEffectSpawner.Instance.SpawnBlood(transform.position);
            Destroy(collision.gameObject);


            Grow();
        }
        else if (collision.gameObject.CompareTag("food"))
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.eatSound);
            SoundManager.Instance.PlaySFX(SoundManager.Instance.screamSound, 0.3f); // Optional

            Debug.Log("Ate food");
            ScoreManager.Instance.AddScore(10);
            BloodEffectSpawner.Instance.SpawnBlood(transform.position);
            Destroy(collision.gameObject);


            Grow();

        }
        else if (collision.gameObject.CompareTag("Body"))
        {
            SoundManager.Instance.StopBGM();

            SoundManager.Instance.PlayGameOverSound();

            Debug.Log("Collided with Body!");


            Die();
        }
    }





}
