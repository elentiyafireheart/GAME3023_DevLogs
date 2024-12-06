using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class AchievementButton : MonoBehaviour
{
    public GameObject achievementList;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
    
        achievementList.SetActive(true);
    }
}
