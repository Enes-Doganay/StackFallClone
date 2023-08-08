using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    bool hit;


    float currentTime;

    bool invincible;
    [SerializeField]
    GameObject fireEffect;

    public enum PlayerState
    {
        Prepare,
        Playing,
        Died,
        Finish
    }
    [HideInInspector]
    public PlayerState playerState = PlayerState.Prepare;

    [SerializeField]
    AudioClip win, death, iDestroy, destroy, bounce;

    int currentObstacleNumber;
    int totalObstacleNumber;

    public Image invictableSlider;
    public GameObject invictableObj;

    public GameObject gameOverUI;
    public GameObject finishUI;
    GameObject rotateManager;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentObstacleNumber = 0;
    }
    private void Start()
    {
        rotateManager = GameObject.Find("RotateManager");
        totalObstacleNumber = rotateManager.transform.childCount;
    }
    private void Update()
    {
        if (playerState == PlayerState.Prepare)
        {
            if (Input.GetMouseButton(0))
                playerState = PlayerState.Playing;
        }

        if (playerState == PlayerState.Finish)
        {
            if (Input.GetMouseButton(0))
                FindObjectOfType<LevelSpawner>().NextLevel();
        }

        if (playerState == PlayerState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                hit = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                hit = false;
            }

            if (invincible)
            {
                currentTime -= Time.deltaTime * .35f;
                if (!fireEffect.activeInHierarchy)
                    fireEffect.SetActive(true);
            }
            else
            {
                if (fireEffect.activeInHierarchy)
                    fireEffect.SetActive(false);

                if (hit)
                {
                    currentTime += Time.deltaTime * .8f;
                }
                else
                {
                    currentTime -= Time.deltaTime * 0.5f;
                }
            }

            if (currentTime >= 0.15f || invictableSlider.color == Color.red)
            {
                invictableObj.SetActive(true);
            }
            else
            {
                invictableObj.SetActive(false);
            }

            if (currentTime >= 1)
            {
                currentTime = 1;
                invincible = true;
                invictableSlider.color = Color.red;
            }
            else if (currentTime <= 0)
            {
                currentTime = 0;
                invincible = false;
                invictableSlider.color = Color.white;
            }

            if (invictableObj.activeInHierarchy)
            {
                invictableSlider.fillAmount = currentTime / 1;
            }

        }

    }
    private void FixedUpdate()
    {
        if(playerState == PlayerState.Playing) 
        { 
            if (hit)
            {
                rb.velocity = new Vector3(0,-100 * Time.fixedDeltaTime * 7 , 0);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hit)
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }

        if(hit)
        {
            if (invincible)
            {
                if(collision.gameObject.tag=="Untagged" || collision.gameObject.tag == "Enemy")
                {
                    collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles();
                    //SoundManager.instance.PlaySoundFX(iDestory,0.5f);
      //              currentObstacleNumber++;
                }
            }
            if (collision.gameObject.tag == "Untagged" && !invincible)
            {
                collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles();
                //SoundManager.instance.PlaySoundFX(destroy,0.5f);
          //      currentObstacleNumber++;
            }
            else if (collision.gameObject.tag == "Enemy" && !invincible)
            {
                Debug.Log("game over");
                gameOverUI.SetActive(true);
                gameOverUI.transform.GetChild(0).GetComponent<Text>().text = ScoreManager.instance.score.ToString();
                playerState = PlayerState.Finish;
                ScoreManager.instance.ResetScore();
                //SoundManager.instance.PlaySoundFX(death,0.5f);
            }
        }
        currentObstacleNumber = totalObstacleNumber - rotateManager.transform.childCount;
        FindObjectOfType<GameUI>().LevelSliderFill( (float)(currentObstacleNumber) / (float)totalObstacleNumber);

        if (collision.gameObject.tag == "Finish" && playerState == PlayerState.Playing)
        {
            playerState = PlayerState.Finish;
            finishUI.SetActive(true);
            finishUI.transform.GetChild(0).GetComponent<Text>().text = "Level :"+ PlayerPrefs.GetInt("Level");
            ScoreManager.instance.ResetScore();
            invictableObj.SetActive(false);
            //SoundManager.instance.PlaySoundFX(win,0.5f);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!hit || collision.gameObject.tag == "Finish")
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            //SoundManager.instance.PlaySoundFX(bounce,0.5f);
        }
    }
}
