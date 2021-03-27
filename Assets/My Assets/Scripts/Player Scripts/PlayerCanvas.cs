/*
 * Provides functions for updating the Player's UI.
 * 
 * Author: Cristion Dominguez
 * Lastest Revision: 2 Oct. 2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoSingleton<PlayerCanvas>
{
    [SerializeField]
    private Text orderStatusText;  // Text indicating whether the Player baked the correct pastries

    [SerializeField]
    private Text orderList;  // Text displaying the pastries to bake

    [SerializeField]
    private Text bakedList;  // Text displaying the pastries baked by Player

    [SerializeField]
    private Text buttonDesciption;  // Text displaying the description of a button on the PolyOven

    [SerializeField]
    private Text selectPastryText;  // Text indicating that a player has not selected a pastry to bake

    /// <summary>
    /// Activates the orderStatusText for 2 seconds. If status == 1, assigns orderStatusText a string indicating the Player baked the correct pastries; otherwise 
    /// assigns orderStatusText a string indicating that the Player's pastries are incorrect and must attempt again.
    /// </summary>
    /// <param name="status"> int representing if the Player baked the correct pastries </param>
    public IEnumerator DisplayOrderStatusText(int status)
    {
        if (status == 1)
        {
            orderStatusText.text = "Good job!";
            orderStatusText.color = Color.green;
        }
        else
        {
            orderStatusText.text = "Start over.";
            orderStatusText.color = Color.red;
        }

        orderStatusText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        orderStatusText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Concatenates pastry names and sweetness levels from the pastries array onto the orderListText.
    /// </summary>
    /// <param name="pastries"> array of pastries </param>
    public void DisplayOrder(pastryInfo[] pastries)
    {
        orderList.text = "Order List:";
        string pastryName = "";

        // Iterate through pastries.
        for (int i = 0; i < pastries.Length; i++)
        {
            // Reformat the pastry name.
            switch (pastries[i].name)
            {
                case PastryNames.CAKE:
                    pastryName = "Cake";
                    break;
                case PastryNames.COOKIE:
                    pastryName = "Cookie";
                    break;
                case PastryNames.ICE_CREAM:
                    pastryName = "Ice-Cream";
                    break;
            }

            orderList.text += "\n" + pastryName + ", Sweetness = " + pastries[i].sweetnessLevel;
        }
    }

    /// <summary>
    /// Concatenates a new pastry's name and sweetness level to bakedListText. If the pastry has a sweetness level of 0, resets the text.
    /// </summary>
    /// <param name="newPastry"> new pastry to be added to baked list </param>
    public void DisplayBakedPastry(pastryInfo newPastry)
    {
        // Reset text.
        if (newPastry.sweetnessLevel == 0)
        {
            bakedList.text = "Current Pastries:";
            return;
        }

        string pastryName = "";

        // Reformat the pastry name.
        switch (newPastry.name)
        {
            case PastryNames.CAKE:
                pastryName = "Cake";
                break;
            case PastryNames.COOKIE:
                pastryName = "Cookie";
                break;
            case PastryNames.ICE_CREAM:
                pastryName = "Ice-Cream";
                break;
        }

        bakedList.text += "\n" + pastryName + ", Sweetness = " + newPastry.sweetnessLevel;
    }

    /// <summary>
    /// Replaces the buttonDescription text with a different description and activates the buttonDesription text if showDesciprtion is true; otherwise deactives
    /// the buttonDescription text.
    /// </summary>
    /// <param name="description"> text to replace buttonDescription text </param>
    /// <param name="showDescription"> bool indicating whether the show the button description text </param>
    public void DisplayButtonDescription(string description, bool showDescription)
    {
        if (showDescription)
        {
            buttonDesciption.text = description;
            buttonDesciption.gameObject.SetActive(true);
        }
        else
        {
            buttonDesciption.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// Activates selectPastryText for 2 seconds.
    /// </summary>
    public IEnumerator DisplaySelectPastryText()
    {
        selectPastryText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        selectPastryText.gameObject.SetActive(false);
    }
}
