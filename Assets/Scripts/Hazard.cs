using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] float RisingRate = 0.3f;
    public GameObject flag;
    public GameObject player;
    public bool stop = false;
    Vector2 dummy;
    // Start is called before the first frame update
    void Start()
    {
         dummy.y = flag.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(stop == false) {
            if (dummy.y != flag.transform.position.y) {
                dummy = flag.transform.position;
                RisingRate += 0.3f;
            }
            transform.Translate(new Vector2(0f, RisingRate * Time.deltaTime));
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            player.GetComponent<Player>().health -= 5;
            if(this.gameObject.transform.position.y > player.transform.position.y) {
                //player.GetComponent<Player>().health = 0;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            player.GetComponent<Player>().health -= 0.1f;
            if (this.gameObject.transform.position.y > player.transform.position.y) {
                //player.GetComponent<Player>().health = 0;
            }
        }

        if(collision.CompareTag("Platform")) {
            Destroy(collision.gameObject);
        }
    }
}
