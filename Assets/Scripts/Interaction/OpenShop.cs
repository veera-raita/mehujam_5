using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : Interactable, IDataPersistence
{
    public bool boughtBoost = false;
    public bool boughtFilter = false;
    public bool boughtIntake = false;
    public override void Interact()
    {
        MenuManager.instance.OpenShopUI(boughtBoost, boughtFilter, boughtIntake, this);
    }

    public void LoadData(GameData data)
    {
        this.boughtBoost = data.boughtBoost;
        this.boughtFilter = data.boughtFilter;
        this.boughtIntake = data.boughtIntake;
    }

    public void SaveData(ref GameData data)
    {
        data.boughtBoost = this.boughtBoost;
        data.boughtFilter = this.boughtFilter;
        data.boughtIntake = this.boughtIntake;
    }
}
