using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HarborPopup : MonoBehaviour
{
    /// <summary>
    /// Default price of repair.
    /// </summary>
    private const int DefaultRepairPrice = 150;

    /// <summary>
    /// Default price of one unit of anything.
    /// </summary>
    private const int DefaultUnitPrice = 15;

    /// <summary>
    /// Format for the units text.
    /// </summary>
    private const string UnitTextFormat = "{0} units";

    /// <summary>
    /// Label for cloth amount.
    /// </summary>
    private Text m_clothAmountText;

    /// <summary>
    /// Label for gold amount.
    /// </summary>
    private Text m_goldAmountText;

    /// <summary>
    /// Label for amount of metal.
    /// </summary>
    private Text m_metalAmountText;

    /// <summary>
    /// Controller of player's ship.
    /// </summary>
    private ShipController m_playerShipController;

    /// <summary>
    /// Label for amount of wood;
    /// </summary>
    private Text m_woodAmountText;

    /// <summary>
    /// Called when the script is created.
    /// </summary>
    private void Awake()
    {
        Player.OnPlayerResourcesChanged += OnPlayerResourcesChanged;
        Game.IsPaused = true;

        m_playerShipController = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>();

        var currentHarbor = Player.m_currentHarbor;
        var controlsContainer = transform.FindChild("ui_canvas").FindChild("ui_pControls");

        var repairShipText = controlsContainer.Find("ui_bRepairShip/ui_tRepairShip").GetComponent<Text>();
        repairShipText.text = String.Format(repairShipText.text, DefaultRepairPrice * currentHarbor.m_repairMultiplier);

        if (m_playerShipController.m_healthPoints == ShipController.MaximumHealthPoints)
        {
            controlsContainer.Find("ui_bRepairShip").gameObject.SetActive(false);
        }

        var welcomeText = controlsContainer.Find("ui_tWelcome").GetComponent<Text>();
        welcomeText.text = String.Format(welcomeText.text, currentHarbor.m_harborName);

        var clothPanel = controlsContainer.Find("ui_pCloth");
        var clothPriceText = clothPanel.Find("ui_tPricePerUnit").GetComponent<Text>();
        clothPriceText.text = String.Format(clothPriceText.text, DefaultUnitPrice * currentHarbor.m_clothPriceMultiplier);
        m_clothAmountText = clothPanel.Find("ui_tPlayerUnits").GetComponent<Text>();

        if (currentHarbor.m_clothPriceMultiplier == 0.0f)
        {
            clothPanel.gameObject.SetActive(false);
        }

        var goldPanel = controlsContainer.Find("ui_pGold");
        var goldPriceText = goldPanel.Find("ui_tPricePerUnit").GetComponent<Text>();
        goldPriceText.text = String.Format(goldPriceText.text, DefaultUnitPrice * currentHarbor.m_goldPriceMultiplier);
        m_goldAmountText = goldPanel.Find("ui_tPlayerUnits").GetComponent<Text>();

        if (currentHarbor.m_goldPriceMultiplier == 0.0f)
        {
            goldPanel.gameObject.SetActive(false);
        }

        var metalPanel = controlsContainer.Find("ui_pMetal");
        var metalPriceText = metalPanel.Find("ui_tPricePerUnit").GetComponent<Text>();
        metalPriceText.text = String.Format(metalPriceText.text, DefaultUnitPrice * currentHarbor.m_metalPriceMultiplier);
        m_metalAmountText = metalPanel.Find("ui_tPlayerUnits").GetComponent<Text>();

        if (currentHarbor.m_metalPriceMultiplier == 0.0f)
        {
            metalPanel.gameObject.SetActive(false);
        }

        var woodPanel = controlsContainer.Find("ui_pWood");
        var woodPriceText = woodPanel.Find("ui_tPricePerUnit").GetComponent<Text>();
        woodPriceText.text = String.Format(woodPriceText.text, DefaultUnitPrice * currentHarbor.m_woodPriceMultiplier);
        m_woodAmountText = woodPanel.FindChild("ui_tPlayerUnits").GetComponent<Text>();

        if (currentHarbor.m_woodPriceMultiplier == 0.0f)
        {
            woodPanel.gameObject.SetActive(false);
        }

        UpdateResourceAmounts();
    }

    /// <summary>
    /// Called when the player resources has changed.
    /// Updates the amounts.
    /// </summary>
    /// <param name="playerCargoBay">Player's cargo bay.</param>
    private void OnPlayerResourcesChanged(CargoBay playerCargoBay)
    {
        UpdateResourceAmounts();
    }

    /// <summary>
    /// Called when the script is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        Player.OnPlayerResourcesChanged -= OnPlayerResourcesChanged;
        Game.IsPaused = false;
    }

    /// <summary>
    /// Updates amounts of resources of the player.
    /// </summary>
    private void UpdateResourceAmounts()
    {
        var cargo = m_playerShipController.m_cargoBay;

        m_clothAmountText.text = String.Format(UnitTextFormat, cargo.m_cloth);
        m_goldAmountText.text = String.Format(UnitTextFormat, cargo.m_gold);
        m_metalAmountText.text = String.Format(UnitTextFormat, cargo.m_metal);
        m_woodAmountText.text = String.Format(UnitTextFormat, cargo.m_wood);
    }

    /// <summary>
    /// Buys cargo with specified name.
    /// </summary>
    /// <param name="name">Name of the cargo to buy.</param>
    public void BuyCargo(string name)
    {
        switch (name)
        {
            case "cloth":
                var clothPrice = DefaultUnitPrice * Player.m_currentHarbor.m_clothPriceMultiplier;
                if (Player.Money < clothPrice)
                    return;

                Player.Money -= (int)clothPrice;
                ++m_playerShipController.m_cargoBay.m_cloth;
                break;

            case "gold":
                var goldPrice = DefaultUnitPrice * Player.m_currentHarbor.m_goldPriceMultiplier;
                if (Player.Money < goldPrice)
                    return;

                Player.Money -= (int)goldPrice;
                ++m_playerShipController.m_cargoBay.m_gold;
                break;

            case "metal":
                var metalPrice = DefaultUnitPrice * Player.m_currentHarbor.m_metalPriceMultiplier;
                if (Player.Money < metalPrice)
                    return;

                Player.Money -= (int)metalPrice;
                ++m_playerShipController.m_cargoBay.m_metal;
                break;

            case "wood":
                var woodPrice = DefaultUnitPrice * Player.m_currentHarbor.m_woodPriceMultiplier;
                if (Player.Money < woodPrice)
                    return;

                Player.Money -= (int)woodPrice;
                ++m_playerShipController.m_cargoBay.m_wood;
                break;
        }

        Player.TriggerPlayerResourcesChanged(m_playerShipController.m_cargoBay);
    }

    /// <summary>
    /// Closes the harbor popup.
    /// </summary>
    public void Close()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Repairs player's ship.
    /// </summary>
    public void RepairShip()
    {
        if (m_playerShipController.m_healthPoints == ShipController.MaximumHealthPoints)
            return;

        var repairPrice = Player.m_currentHarbor.m_repairMultiplier * DefaultRepairPrice;
        if (Player.Money < repairPrice)
            return;

        Player.Money -= (int)repairPrice;
        m_playerShipController.m_healthPoints = ShipController.MaximumHealthPoints;
        m_playerShipController.UpdateHealthBar();

        transform.FindChild("ui_canvas").Find("ui_pControls/ui_bRepairShip").gameObject.SetActive(false);
    }

    /// <summary>
    /// Sells cargo with specified name.
    /// </summary>
    /// <param name="name">Name of the cargo to sell.</param>
    public void SellCargo(string name)
    {
        var cargo = m_playerShipController.m_cargoBay;
        var currentHarbor = Player.m_currentHarbor;

        switch (name)
        {
            case "cloth":
                Player.Money += (int)(DefaultUnitPrice * currentHarbor.m_clothPriceMultiplier * cargo.m_cloth);
                cargo.m_cloth = 0;
                break;

            case "gold":
                Player.Money += (int)(DefaultUnitPrice * currentHarbor.m_goldPriceMultiplier * cargo.m_gold);
                cargo.m_gold = 0;
                break;

            case "metal":
                Player.Money += (int)(DefaultUnitPrice * currentHarbor.m_metalPriceMultiplier * cargo.m_metal);
                cargo.m_metal = 0;
                break;

            case "wood":
                Player.Money += (int)(DefaultUnitPrice * currentHarbor.m_woodPriceMultiplier * cargo.m_wood);
                cargo.m_wood = 0;
                break;
        }

        Player.TriggerPlayerResourcesChanged(m_playerShipController.m_cargoBay);
    }
}
