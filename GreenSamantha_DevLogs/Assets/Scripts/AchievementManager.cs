using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public GameObject achievementPrefab;


    // Start is called before the first frame update
    void Start()
    {
        CreateAchievement("General", "TestTitle", "Description goes here", 25);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateAchievement(string category, string title, string description, int points)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);
        SetAchievementInfo(category, achievement, title, description, points);
    }

    public void SetAchievementInfo(string category, GameObject achievement, string title, string description, int points)
    {
        // Sets the parent for the achievement we just created
        achievement.transform.SetParent(GameObject.Find(category).transform);
        achievement.transform.localScale = new Vector3(1, 1, 1);

        achievement.transform.GetChild(0).GetComponent<TMP_Text>().text = title;
        achievement.transform.GetChild(1).GetComponent<TMP_Text>().text = description;
        achievement.transform.GetChild(2).GetComponent<TMP_Text>().text = points.ToString();
    }

}
