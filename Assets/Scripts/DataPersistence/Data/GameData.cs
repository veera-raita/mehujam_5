using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    [Header("Saved Variables")]
    public int currency;
    public int jumpUpgrades;
    public int activeJumpUpgrades;
    public int filterUpgrades;
    public int activeFilterUpgrades;
    public int intakeUpgrades;
    public int activeIntakeUpgrades;
    public bool introPlayed;
    public bool boughtBoost;
    public bool boughtFilter;
    public bool boughtIntake;
    public Vector2 position;

    //constructor, ran on starting new game
    public GameData()
    {
        this.currency = 0;
        this.jumpUpgrades = 0;
        this.activeJumpUpgrades = 0;
        this.filterUpgrades = 0;
        this.activeFilterUpgrades = 0;
        this.intakeUpgrades = 0;
        this.activeIntakeUpgrades = 0;
        this.position = new Vector2(0, -1f);
        boughtBoost = false;
        boughtFilter = false;
        boughtIntake = false;
    }
}
