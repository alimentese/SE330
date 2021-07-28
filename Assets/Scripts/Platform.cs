using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    
    public GameObject player;
    public BoxCollider2D left, right; // belongs to plaftorm
    public EdgeCollider2D top;


    public List<GameObject> plants;
    public Sprite DeathPlant;
    public bool sizechanging = false;

    public bool moving = false;
    public bool movingleft = false;
    public bool movingright = false;

    // Start is called before the first frame update
    void Start()
    {
        movingleft = true;
        player = GameObject.FindGameObjectWithTag("Player");
        plants = new List<GameObject>(GameObject.FindGameObjectsWithTag("PoisonousPlant"));

    }

    // Update is called once per frame
    void Update() {
        if(moving == true) {
            if (movingleft == true) {
                gameObject.transform.Translate(new Vector2(-5f * Time.deltaTime, 0f));
            }
            if (movingright == true) {
                gameObject.transform.Translate(new Vector2(5f * Time.deltaTime, 0f));
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        foreach (GameObject plant in plants) {
            if (collision.CompareTag("Player") && plant != null) {
                if (plant.GetComponent<SpriteRenderer>().sprite != DeathPlant) {
                    if (plant.GetComponent<BoxCollider2D>().IsTouching(collision)) {
                        plant.GetComponent<SpriteRenderer>().sprite = DeathPlant;
                        plant.transform.position = new Vector3(plant.transform.position.x, plant.transform.position.y - 0.25f, plant.transform.position.z);
                        plant.transform.localScale = new Vector3(plant.transform.localScale.x + 0.5f, plant.transform.localScale.y + 0.5f, plant.transform.localScale.z);
                        //Physics2D.IgnoreCollision(plant.GetComponent<BoxCollider2D>(), collision);
                        collision.GetComponent<Player>().health -= 10;
                    }
                }
            }
            if(collision.CompareTag("PlayerHitbox") && plant != null) {
                if (plant.GetComponent<SpriteRenderer>().sprite != DeathPlant) {
                    if (plant.GetComponent<BoxCollider2D>().IsTouching(collision)) {
                        plant.GetComponent<SpriteRenderer>().sprite = DeathPlant;
                        plant.transform.position = new Vector3(plant.transform.position.x, plant.transform.position.y - 0.25f, plant.transform.position.z);
                        plant.transform.localScale = new Vector3(plant.transform.localScale.x + 0.5f, plant.transform.localScale.y + 0.5f, plant.transform.localScale.z);
                    }
                }                        
            }
        }
        
        if (collision.CompareTag("LeftCollider")) {
            if (left.IsTouching(collision)) {
                movingleft = false;
                movingright = true;
            }
        }

        if (collision.CompareTag("RightCollider")) {
            if (right.IsTouching(collision)) {
                movingleft = true;
                movingright = false;
            }
        }

        
    }

    
}
