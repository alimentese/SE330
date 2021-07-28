using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] public float health = 100f;
    [SerializeField] public float score = 0f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;

    // Cached component references
    Rigidbody2D PlayerRigidbody;
    BoxCollider2D PlayerFeetCollider;
    Animator PlayerAnimator;

    public GameObject hazard;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerFeetCollider = GetComponent<BoxCollider2D>();
        PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if(health > 0) {
            Attack1();
            Run();
            Jump();
            FlipSprite();
        }
       

    }

    private void Attack1() {
        if (Input.GetButtonDown("Fire1")) {          
            PlayerAnimator.SetTrigger("Attack1");    
        }
    }

    private void Run() {
            float controlThrow = Input.GetAxisRaw("Horizontal"); // value is between -1 to +1
            Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, PlayerRigidbody.velocity.y);
            PlayerRigidbody.velocity = playerVelocity;
            bool stateOfRunning = Mathf.Abs(PlayerRigidbody.velocity.x) > Mathf.Epsilon;
            if (PlayerFeetCollider.IsTouchingLayers()) {
                PlayerAnimator.SetBool("Running", stateOfRunning);
            }        
    }

    private void FlipSprite() {
        // if the player is moving horizontally
        if (!PlayerAnimator.GetBool("Crouch") && PlayerAnimator.GetBool("Running") || !PlayerAnimator.GetBool("Crouch") && PlayerAnimator.GetBool("DrawSwordRunning")) {
            bool playerHorizontalSpeed = Mathf.Abs(PlayerRigidbody.velocity.x) > Mathf.Epsilon;
            if (playerHorizontalSpeed) {
                transform.localScale = new Vector2(Mathf.Sign(PlayerRigidbody.velocity.x) * 5f, 5f);
            }
        }
    }

    private void Jump() {
        if (PlayerFeetCollider.IsTouchingLayers()) { //(LayerMask.GetMask("Ground"))
            PlayerAnimator.SetBool("Jumping", false);
            //print("Touching ground!");
            if (Input.GetButtonDown("Jump") && !PlayerAnimator.GetBool("Crouch")) {
                PlayerAnimator.SetBool("Jumping", true);
                //effector.rotationalOffset = 0f;
                Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
                PlayerRigidbody.velocity += jumpVelocityToAdd;
            }           
        }
        else {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Coin")) {
            score += 10;
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Frozer")) {
            hazard.GetComponent<Hazard>().stop = true;
            Destroy(collision.gameObject);
            StartCoroutine(TimeController());

        }

        if (collision.CompareTag("Health")) {
            health += 10;
            Destroy(collision.gameObject);

        }

        if (collision.CompareTag("Time")) {
            Destroy(collision.gameObject);
            StartCoroutine(TimeSlower());
        }
    }

    IEnumerator TimeController() {
        if (hazard.GetComponent<Hazard>().stop == true) {
            yield return new WaitForSeconds(2f);
            hazard.GetComponent<Hazard>().stop = false;
        }
    }

    IEnumerator TimeSlower() {
        if (Time.timeScale == 1.0f) {
            Time.timeScale = 0.5f;
            yield return new WaitForSeconds(10f);
        }
        Time.timeScale = 1f;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Platform")) {
            if(collision.otherCollider == PlayerFeetCollider) {
                if (collision.gameObject.GetComponent<Platform>().moving == true) {
                    Debug.Log("player platform ustunde");
                    transform.parent = collision.transform;
                }
            }            
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Platform")) {
            transform.parent = null;
        }
    }

}
