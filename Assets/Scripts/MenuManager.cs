using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance { get; private set; }
    [SerializeField] private InputReader inputReader;
    private OpenShop lastShop;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private Button buyBoost;
    [SerializeField] private Button buyFilter;
    [SerializeField] private Button buyIntake;
    [SerializeField] private Button addBoost;
    [SerializeField] private Button remBoost;
    [SerializeField] private Button addFilter;
    [SerializeField] private Button remFilter;
    [SerializeField] private Button addIntake;
    [SerializeField] private Button remIntake;
    [SerializeField] PlayerController playerController;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one MenuManager");
        }
        else instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenShopUI(bool _boost, bool _filter, bool _intake, OpenShop _lastShop)
    {
        shopUI.SetActive(true);
        inputReader.SetMenuMode();
        if (!_boost) buyBoost.interactable = true;
        else buyBoost.interactable = false;
        if (!_filter) buyFilter.interactable = true;
        else buyFilter.interactable = false;
        if (!_intake) buyIntake.interactable = true;
        else buyIntake.interactable = false;
        lastShop = _lastShop;
        UpdateButtons();
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        inputReader.SetIslandMovement();
    }

    public void BuyBoost()
    {
        playerController.AddUpgrade(1);
        buyBoost.interactable = false;
        lastShop.boughtBoost = true;
        UpdateButtons();
    }

    public void BuyFilter()
    {
        playerController.AddUpgrade(2);
        buyFilter.interactable = false;
        lastShop.boughtFilter = true;
        UpdateButtons();
    }

    public void BuyIntake()
    {
        playerController.AddUpgrade(3);
        buyIntake.interactable = false;
        lastShop.boughtIntake = true;
        UpdateButtons();
    }

    public void AddBoost()
    {
        playerController.ActivateUpgrade(1);
        UpdateButtons();
    }

    public void AddFilter()
    {
        playerController.ActivateUpgrade(2);
        UpdateButtons();
    }

    public void AddIntake()
    {
        playerController.ActivateUpgrade(3);
        UpdateButtons();
    }

    public void RemBoost()
    {
        playerController.DeactivateUpgrade(1);
        UpdateButtons();
    }

    public void RemFilter()
    {
        playerController.DeactivateUpgrade(2);
        UpdateButtons();
    }

    public void RemIntake()
    {
        playerController.DeactivateUpgrade(3);
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if (playerController.activeJumpUpgrades < playerController.jumpUpgrades)
        addBoost.interactable = true;
        else
        addBoost.interactable = false;

        if (playerController.activeJumpUpgrades > 0)
        remBoost.interactable = true;
        else
        remBoost.interactable = false;

        if (playerController.activeFilterUpgrades < playerController.filterUpgrades)
        addFilter.interactable = true;
        else
        addFilter.interactable = false;

        if (playerController.activeFilterUpgrades > 0)
        remFilter.interactable = true;
        else
        remFilter.interactable = false;

        if (playerController.activeIntakeUpgrades < playerController.intakeUpgrades)
        addIntake.interactable = true;
        else
        addIntake.interactable = false;

        if (playerController.activeIntakeUpgrades > 0)
        remIntake.interactable = true;
        else
        remIntake.interactable = false;
    }
}