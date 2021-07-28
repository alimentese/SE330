using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{
    public GameObject player;
    public CapsuleCollider2D playerbody;
    public BoxCollider2D shurikencollider;
    public GameObject flag;
    public GameObject[] platforms;
    public GameObject hazard;
    public GameObject shuriken;
    public GameObject health;
    public GameObject time;
    public GameObject menu;

    GameObject newshuriken = null;
    public GameObject frozer;
    Vector3 direction;



    public Text scoreUI;
    public Image healthbar;
    [SerializeField] float score;

    public bool shurikenmoving = false;

    // Start is called before the first frame update
    void Start() {
        
        StartCoroutine(HazardWave());
        score = player.GetComponent<Player>().score;
        playerbody = player.GetComponent<CapsuleCollider2D>();
        shurikencollider = shuriken.GetComponent<BoxCollider2D>();
        StartCoroutine(ShurikenThrower());

    }

    // Update is called once per frame
    void Update() {
        Debug.Log("player position: " + player.transform.position);
        Debug.Log("player localposition: " + player.transform.localPosition);
        Debug.Log("time sclae:: " + Time.timeScale);

        if (shurikenmoving == true && newshuriken != null) {
            newshuriken.transform.localEulerAngles = new Vector3(newshuriken.transform.localEulerAngles.x, newshuriken.transform.localEulerAngles.y, newshuriken.transform.localEulerAngles.z + 360f * Time.deltaTime);
            newshuriken.transform.position = Vector3.MoveTowards(newshuriken.transform.position, direction, 5f * Time.deltaTime);
        } 

        Debug.Log("Direction: " + direction);

        player.GetComponent<Player>().score += 1 * Time.deltaTime;
        if (player.transform.position.y + 2 >= flag.transform.position.y) {
            flag.transform.position = new Vector3(flag.transform.position.x, flag.transform.position.y + 8.5f, 0);
            int random = Random.Range(0, platforms.Length);
            GameObject platform = Instantiate(platforms[random]);
            platform.transform.position = new Vector3(flag.transform.position.x, flag.transform.position.y - 0.5f, 0);
            platform.transform.parent = GameObject.FindGameObjectWithTag("Spawned").transform;
            platform.SetActive(true);

        }
        score = (int)player.GetComponent<Player>().score;
        healthbar.fillAmount = (float)player.GetComponent<Player>().health / 100f;
        Debug.Log((float)player.GetComponent<Player>().health);
        scoreUI.text = score.ToString();
        if (player.GetComponent<Player>().health <= 0) {
            Time.timeScale = 0;
            menu.SetActive(true);
        }
    }

    
    IEnumerator HazardWave() {
        while(true) {
            yield return new WaitForSeconds(0.25f);
            if (hazard.transform.localScale.x == 1) {
                hazard.transform.localScale = new Vector3(-1, hazard.transform.localScale.y, hazard.transform.localScale.z);
            }
            else if (hazard.transform.localScale.x == -1) {
                hazard.transform.localScale = new Vector3(1, hazard.transform.localScale.y, hazard.transform.localScale.z);
            }
        }
    }

    IEnumerator ShurikenThrower() {
        while(true) {
            yield return new WaitForSeconds(5f);
            if(newshuriken == null) {
                shurikenmoving = false;
                Vector3 ShurikenPosition;
                float x = Random.Range(-21, 3);
                float y = Random.Range(-5, 9);
                float z = 0f;
                ShurikenPosition = new Vector3(x, y, z);
                newshuriken = Instantiate(shuriken);
                newshuriken.transform.parent = gameObject.transform;
                newshuriken.transform.position = ShurikenPosition;
                direction = player.transform.position;
                shurikenmoving = true;
            }
        }       
    }
    

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            if(newshuriken != null &&newshuriken.GetComponent<BoxCollider2D>().IsTouching(collision)) {
                player.GetComponent<Player>().health -= 10;
                shurikenmoving = false;
                Destroy(newshuriken);
                newshuriken = null;
            }
            
        }

    }

    public void Exit() {
        Application.Quit();
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

}
