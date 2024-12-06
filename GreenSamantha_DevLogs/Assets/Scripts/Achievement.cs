using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement
{
    public string name;
    public string description;
    public bool unlocked;
    public int points;
    public int spriteIndex;
    public GameObject achievementRef;

    public Achievement(string name, string description, int points, int spriteIndex, GameObject achievementRef)
    {
        this.name = name;
        this.description = description;
        this.unlocked = false;
        this.points = points;
        this.spriteIndex = spriteIndex;
        this.achievementRef = achievementRef;
        
    }

    public bool EarnAchievement()
    {
        if (!unlocked)
        {
            unlocked = true;
            return true;
        }
        return false;
    }

}
