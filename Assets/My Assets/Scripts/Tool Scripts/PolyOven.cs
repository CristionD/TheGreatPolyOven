/*
 * Provides functionality for the buttons on the PolyOven GameObject, allowing the player to modify a pastry, bake it, and transport it to a plate
 * on the counter.
 * 
 * Author: Cristion Dominguez
 * Latest Revision: 2 Oct. 2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolyOven : MonoBehaviour
{
    #region ClassVariables
    [Header("Pastry Icons")]  // icons of normal pastries to be displayed on PolyOven screen
    [SerializeField]
    private GameObject cakeIcon;

    [SerializeField]
    private GameObject cookieIcon;

    [SerializeField]
    private GameObject iceCreamIcon;

    [Header("Special Pastry Icons")]  // icons of special pastries to be displayed on PolyOven screen
    [SerializeField]
    private GameObject specialCakeIcon;

    [SerializeField]
    private GameObject specialCookieIcon;

    [SerializeField]
    private GameObject specialIceCreamIcon;

    [Header("Oven Text")]
    [SerializeField]
    private Text sweetnessText;  // Text displaying sweetness for current pastry if there is any

    private pastryInfo currentPastry;  // pastry currently being concocted
    #endregion

    #region AwakeAndHelperFunctions
    /// <summary>
    /// Initializes the name of the current pastry to an empty string to avoid baking a pastry with no name.
    /// </summary>
    private void Awake() => currentPastry.name = "";

    /// <summary>
    /// Disables all pastry icons on PolyOven screen.
    /// </summary>
    public void ResetDisplay()
    {
        cakeIcon.SetActive(false);
        cookieIcon.SetActive(false);
        iceCreamIcon.SetActive(false);
        specialCakeIcon.SetActive(false);
        specialCookieIcon.SetActive(false);
        specialIceCreamIcon.SetActive(false);
    }

    /// <summary>
    /// Updates the text above the Poly Oven displaying the sweetness of the current pastry if the received sweetness value is not -1; otherwise, a sweetness
    /// value is not displayed.
    /// </summary>
    /// <param name="sweetness"> sweetness of current pastry </param>
    /// <param name="isEnhanced"> bool whether pastry underwent special technique </param>
    private void UpdateSweetnessText(int sweetness, bool isEnhanced)
    {
        // If sweetness == -1, then set the sweetness text color to black, do not display a sweetness value and then break out of function.
        if (sweetness == -1)
        {
            sweetnessText.color = Color.black;
            sweetnessText.text = "Sweetness Level: ";
            return;
        }

        // If the pastry isEnhanced, then set the sweetness text color to yellow; otherwise, set it to black.
        if (isEnhanced) sweetnessText.color = Color.yellow;
        else sweetnessText.color = Color.black;
        
        // Add the sweetness value to the end of the sweetness text.
        sweetnessText.text = "Sweetness Level: " + sweetness;
    }
    #endregion

    #region SelectPastryFunctions
    /// <summary>
    /// Displays icon for cake, sets the current pastry to the values of cake and updates the sweetness text.
    /// </summary>
    public void SelectCake()
    {
        ResetDisplay();
        cakeIcon.SetActive(true);
        currentPastry.name = PastryNames.CAKE;
        currentPastry.sweetnessLevel = 1;
        UpdateSweetnessText(1, false);
    }

    /// <summary>
    /// Displays icon for cookie and sets the current pastry to the values of cookie and updates the sweetness text.
    /// </summary>
    public void SelectCookie()
    {
        ResetDisplay();
        cookieIcon.SetActive(true);
        currentPastry.name = PastryNames.COOKIE;
        currentPastry.sweetnessLevel = 2;
        UpdateSweetnessText(2, false);
    }

    /// <summary>
    /// Displays icon for ice-cream and sets the current pastry to the values of ice-cream and updates the sweetness text.
    /// </summary>
    public void SelectIceCream()
    {
        ResetDisplay();
        iceCreamIcon.SetActive(true);
        currentPastry.name = PastryNames.ICE_CREAM;
        currentPastry.sweetnessLevel = 3;
        UpdateSweetnessText(3, false);
    }
    #endregion

    #region PastryModificationFunctions
    /// <summary>
    /// If there is no pastry selected, displays Text to player demanding to select a pastry; otherwise, increases the selected
    /// pastry's sweetness level by 1 if the pastry's sweetness level is at its initial value and updates the sweetness text.
    /// </summary>
    public void AddSpecialTechnique()
    {
        if (currentPastry.name == "")
        {
            StartCoroutine(PlayerCanvas.Instance.DisplaySelectPastryText());
            return;
        }

        ResetDisplay();

        // Increase sweetness level only if the pastry selected has its respective initial sweetness level.
        switch (currentPastry.name)
        {
            case PastryNames.CAKE:
                specialCakeIcon.SetActive(true);
                if (currentPastry.sweetnessLevel == 1)
                {
                    currentPastry.sweetnessLevel++;
                    UpdateSweetnessText(2, true);
                }
                break;
            case PastryNames.COOKIE:
                specialCookieIcon.SetActive(true);
                if (currentPastry.sweetnessLevel == 2)
                {
                    currentPastry.sweetnessLevel++;
                    UpdateSweetnessText(3, true);
                }
                break;
            case PastryNames.ICE_CREAM:
                specialIceCreamIcon.SetActive(true);
                if (currentPastry.sweetnessLevel == 3)
                {
                    currentPastry.sweetnessLevel++;
                    UpdateSweetnessText(4, true);
                }
                break;
        }
    }

    /// <summary>
    /// If there is no pastry selected, displays Text to player demanding to select a pastry; otherwise transfers pastry to the Counter,
    /// clears the current pastry selection and resets the sweetness text.
    /// </summary>
    public void Bake()
    {
        if (currentPastry.name == "")
        {
            StartCoroutine(PlayerCanvas.Instance.DisplaySelectPastryText());
            return;
        }

        Counter.Instance.PlaceOrder(currentPastry);

        ClearSelection();
    }

    /// <summary>
    /// If there is no pastry selected, displays Text to player demanding to select a pastry; otherwise disables all icons on PolyOven screen,
    /// resets the values of current pastry to their default values and resets the sweetness text.
    /// </summary>
    public void ClearSelection()
    {
        if (currentPastry.name == "")
        {
            StartCoroutine(PlayerCanvas.Instance.DisplaySelectPastryText());
            return;
        }

        ResetDisplay();
        UpdateSweetnessText(-1, false);

        currentPastry.name = "";
        currentPastry.sweetnessLevel = 0;
    }
    #endregion
}
