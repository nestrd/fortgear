using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public int experience = 300;
    public int maxLevel;

    public void AddXP(int XP)
    {
        if (GetCurrentLevel() < maxLevel)
        {
            experience += XP;
        }
    }

    public void RewardKillXP()
    {
        AddXP(GetMobXPForLevel(GetCurrentLevel()));
    }

    public int GetXP()
    {
        return experience;
    }

    public int GetCurrentLevel()
    {
        int i;
        int c = 0;

        for (i = 1; i < maxLevel; i++)
        {
            c += GetXPForLevel(i);

            if (c > GetXP())
            {
                break;
            }
        }
        return i;
    }

    public int GetXPIntoCurrentLevel()
    {
        int i;
        int c = 0;

        for (i = 1; i < GetCurrentLevel(); i++)
        {
            c += GetXPForLevel(i);

            if (c > GetXP())
            {
                break;
            }
        }

        return GetXPForLevel(GetCurrentLevel()) - (GetXPForLevel(GetCurrentLevel()) - (experience - c));
    }

    public int GetXPForLevel(int level)
    {
        return (8 * level) * GetMobXPForLevel(level);
    }

    public int GetMobXPForLevel(int level)
    {
        return (level * 5) + 45;
    }
}

