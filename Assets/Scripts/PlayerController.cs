using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject fireEffect;
    private bool hit;

    private float currentTime;
    private bool invincible;

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
    private AudioClip win, death, iDestroy, destroy, bounce;

    private int currentObstacleNumber;
    private int totalObstacleNumber;

    public Image invictableSlider;
    public GameObject invictableObj;
    private GameObject rotateManager;

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
        HandlePlayerState();
        HandleInvincibility();
        UpdateInvincibleSlider();
    }

    private void FixedUpdate()
    {
        if (playerState == PlayerState.Playing && hit)
        {
            rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
        UpdateCurrentObstacleNumber();
        UpdateLevelSlider();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!hit || collision.gameObject.tag == "Finish")
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            //SoundManager.instance.PlaySoundFX(bounce,0.5f);
        }
    }

    private void HandlePlayerState()
    {
        if (playerState == PlayerState.Prepare)
        {
            if (Input.GetMouseButton(0))
                playerState = PlayerState.Playing;
        }

        if (playerState == PlayerState.Finish)
        {
            UIManager.Instance.SetFinishUI();
            if (Input.GetMouseButton(0))
                GameManager.Instance.NextLevel();
        }

        if (playerState == PlayerState.Died)
        {
            UIManager.Instance.SetGameOverUI();
            Time.timeScale = 0;
            if(Input.GetMouseButton(0))
                GameManager.Instance.Retry();
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
        }
    }

    private void HandleInvincibility()
    {
        if (invincible)
        {
            currentTime -= Time.deltaTime * 0.35f;
            if (!fireEffect.activeInHierarchy)
                fireEffect.SetActive(true);
        }
        else
        {
            if (fireEffect.activeInHierarchy)
                fireEffect.SetActive(false);

            if (hit)
            {
                currentTime += Time.deltaTime * 0.8f;
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
            invictableSlider.fillAmount = currentTime;
        }
    }

    private void HandleCollision(Collision collision)
    {
        if (!hit)
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }

        if (hit)
        {
            if (invincible)
            {
                if (collision.gameObject.tag == "Untagged" || collision.gameObject.tag == "Enemy")
                {
                    collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles();
                    //SoundManager.instance.PlaySoundFX(iDestroy,0.5f);
                    //currentObstacleNumber++;
                }
            }
            if (collision.gameObject.tag == "Untagged" && !invincible)
            {
                collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles();
                //SoundManager.instance.PlaySoundFX(destroy,0.5f);
                //currentObstacleNumber++;
            }
            else if (collision.gameObject.tag == "Enemy" && !invincible)
            {
                playerState = PlayerState.Died;
                ScoreManager.Instance.ResetScore();
                //SoundManager.instance.PlaySoundFX(death,0.5f);
            }
        }
        if(collision.gameObject.tag == "Finish")
        {
            playerState = PlayerState.Finish;
        }
    }

    private void UpdateCurrentObstacleNumber()
    {
        currentObstacleNumber = totalObstacleNumber - rotateManager.transform.childCount;
    }

    private void UpdateLevelSlider()
    {
        FindObjectOfType<UIManager>().LevelSliderFill((float)(currentObstacleNumber) / (float)totalObstacleNumber);
    }

    private void UpdateInvincibleSlider()
    {
        if (invictableObj.activeInHierarchy)
        {
            invictableSlider.fillAmount = currentTime / 1;
        }
    }
}