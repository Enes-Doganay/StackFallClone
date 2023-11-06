using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField]
    Obstacle[] obstacles = null;

    public void ShatterAllObstacles()
    {
        if(transform.parent != null)
        {
            transform.parent = null;
        }

        foreach(Obstacle item in obstacles)
        {
            item.Shatter();
        }
        StartCoroutine(RemoveAllShatterPart());
        ScoreManager.Instance.AddScore(PlayerPrefs.GetInt("Level", 1));
    }

    IEnumerator RemoveAllShatterPart()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
