using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Image levelSlider;
    public Image currentLevelImg;
    public Image nextLevelImg;

    public Text currentLevelText;
    public Text nextLevelText;

    public Material playerMat;

    private void Start()
    {
        playerMat = FindObjectOfType<PlayerController>().transform.GetChild(0).GetComponent<MeshRenderer>().material;

        levelSlider.transform.GetComponent<Image>().color = playerMat.color + Color.gray;

        levelSlider.color = playerMat.color;

        currentLevelImg.color = playerMat.color;

        nextLevelImg.color = playerMat.color;

        currentLevelText.text = PlayerPrefs.GetInt("Level").ToString();
        nextLevelText.text = (PlayerPrefs.GetInt("Level") + 1).ToString();
    }

    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }
}
