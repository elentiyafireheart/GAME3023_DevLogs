using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public GameObject achievementPrefab;
    public Sprite[] sprites;

    private AchievementButton activeButton;
    public ScrollRect scrollRect;

    public GameObject achievementMenu;
    public GameObject visualAchievement;
    public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();


    // Start is called before the first frame update
    void Start()
    {
        achievements.Add("RunAchievement", new Achievement("Run Forest Run", "Move 10 feet", 10, 0, this.gameObject));

        activeButton = GameObject.Find("GeneralCategory").GetComponent<AchievementButton>();
        CreateAchievement("General", "Adventure Time", "Pressed W!", 25,1);
     

        foreach (GameObject achievementList in GameObject.FindGameObjectsWithTag("AchievementList"))
        {
            achievementList.SetActive(false);
        }

        activeButton.Click();

        achievementMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            achievementMenu.SetActive(!achievementMenu.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            EarnAchievement("Adventure Time");
        }
    }

    public void EarnAchievement(string title)
    {
        if (achievements[title].EarnAchievement())
        {
            // do something
            GameObject achievement = (GameObject)Instantiate(visualAchievement);
           SetAchievementInfo("EarnCanvas", achievement, title);
            StartCoroutine(HideAchievement(achievement));
        }
    }

    public IEnumerator HideAchievement(GameObject achievement)
    {
        yield return new WaitForSeconds(3);
        Destroy(achievement);
    }

    public void CreateAchievement(string parent, string title, string description, int points, int spriteIndex)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);

        Achievement newAchievement = new Achievement(title, description, points, spriteIndex, achievement);
        achievements.Add(title, newAchievement);

        SetAchievementInfo(parent, achievement, title);


    }

    public void SetAchievementInfo(string parent, GameObject achievement, string title)
    {
        // Sets the parent for the achievement we just created
        achievement.transform.SetParent(GameObject.Find(parent).transform);
        achievement.transform.localScale = new Vector3(1, 1, 1);

        achievement.transform.GetChild(0).GetComponent<TMP_Text>().text = title;
        achievement.transform.GetChild(1).GetComponent<TMP_Text>().text = achievements[title].description;
        achievement.transform.GetChild(2).GetComponent<TMP_Text>().text = achievements[title].points.ToString();
        achievement.transform.GetChild(3).GetComponent<Image>().sprite = sprites[achievements[title].spriteIndex];
    }

    public void ChangeCategory(GameObject button)
    {
        AchievementButton achievementButton = button.GetComponent<AchievementButton>();
        scrollRect.content = achievementButton.achievementList.GetComponent<RectTransform>();
        achievementButton.Click();
        activeButton.Click();
        activeButton = achievementButton;
    }

}
