/*
 * Handles the creation of orders, the incoming pastries and the pastry animations.
 * 
 * Author: Cristion Dominguez
 * Latest Revision: 2 Oct. 2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Counter : MonoSingleton<Counter>
{
    #region Class Variables
    private pastryInfo[] desiredPastries;  // array of pastries the Player should bake
    private pastryInfo[] bakedPastries;  // array of pastries the Player has baked

    private static int PLATES = 3;  // number of plates on the counter; represents how many pastries the Player bakes at a time
    private int currentPlate;  // the next plate that is not occupied by a pastry

    [Header("Oven")]
    [SerializeField]
    private PolyOven oven;

    [Header("Pastries")]
    [SerializeField]
    private GameObject cake;
    [SerializeField]
    private GameObject cookie;
    [SerializeField]
    private GameObject iceCream;

    [Header("Special Pastries")]
    [SerializeField]
    private GameObject specialCake;
    [SerializeField]
    private GameObject specialCookie;
    [SerializeField]
    private GameObject specialIceCream;

    [Header("Pastry Spawnpoints")]
    [SerializeField]
    private Transform pastry1Spawn;
    [SerializeField]
    private Transform pastry2Spawn;
    [SerializeField]
    private Transform pastry3Spawn;

    // Distances for each pastry to rise from their spawnpoint.
    [Header("Pastry Rise Distances")]
    [SerializeField]
    private float cakeDistance = 0.215f;
    [SerializeField]
    private float cookieDistance = 0.205f;
    [SerializeField]
    private float iceCreamDistance = 0.387f;

    // Speed at which the pastries rise.
    [Header("Pastry Rise Speed")]
    [SerializeField]
    private float speed = 0.5f;
    #endregion

    #region ProgramStartFunctions
    /// <summary>
    /// Before the program completely starts, initializes pastry arrays and sets the current plate to 0.
    /// </summary>
    protected override void Init()
    {
        desiredPastries = new pastryInfo[PLATES];
        bakedPastries = new pastryInfo[PLATES];
        currentPlate = 0;
    }

    /// <summary>
    /// Upon program start, creates an order of pastries for the Player to fulfill and displays this order to the Player.
    /// </summary>
    private void Start()
    {
        CreateOrders();

        PlayerCanvas.Instance.DisplayOrder(desiredPastries);
    }
    #endregion

    #region PastryOrderFunctions
    /// <summary>
    /// Creates a random order of pastries the Player must bake. The number of pastries depends on the number of plates available.
    /// </summary>
    private void CreateOrders()
    {
        // For each plate, generate a random number for the pastry type and its sweetness level.
        for (int i = 0; i < PLATES; i++)
        {
            int pastry = Random.Range(0, 3);
            int enhanceSweetness = Random.Range(0, 2);

            if (pastry == 0)
            {
                desiredPastries[i].name = PastryNames.CAKE;
                desiredPastries[i].sweetnessLevel = 1;
            }
            else if (pastry == 1)
            {
                desiredPastries[i].name = PastryNames.COOKIE;
                desiredPastries[i].sweetnessLevel = 2;
            }
            else if (pastry == 2)
            {
                desiredPastries[i].name = PastryNames.ICE_CREAM;
                desiredPastries[i].sweetnessLevel = 3;
            }

            if(enhanceSweetness == 1)
            {
                desiredPastries[i].sweetnessLevel++;
            }
        }
    }

    /// <summary>
    /// Adds a new pastry to the baked pastries array, displays the new pastry on the Player canvas, and animates the pastry onto the current plate.
    /// If the current plate is the last plate, orders are checked; otherwise, the current plate becomes the next empty plate.
    /// </summary>
    /// <param name="newPastry"> the latest pastry baked by the Player </param>
    public void PlaceOrder(pastryInfo newPastry)
    {
        // Add pastry to baked pastries array.
        bakedPastries[currentPlate].name = newPastry.name;
        bakedPastries[currentPlate].sweetnessLevel = newPastry.sweetnessLevel;

        // Display new pastry info to Player.
        PlayerCanvas.Instance.DisplayBakedPastry(bakedPastries[currentPlate]);

        // Assign the spawnpoint for the first plate to the new pastry spawnpint. If the current plate is not the first plate, then assign it to the spawnpoint
        // for that particular plate.
        Transform spawnPoint = pastry1Spawn;
        if (currentPlate == 1) spawnPoint = pastry2Spawn;
        else if (currentPlate == 2) spawnPoint = pastry3Spawn;

        // Instantiate the new pastry's GameObject depending on its type and sweetness level, then animate it onto the plate.
        switch(newPastry.name)
        {
            case PastryNames.CAKE:
                GameObject newCake = Instantiate(cake, spawnPoint.position, Quaternion.identity, this.transform);
                StartCoroutine(RisePastry(newCake.transform, cakeDistance));
                break;
            case PastryNames.COOKIE:
                GameObject newCookie = Instantiate(cookie, spawnPoint.position, Quaternion.identity, this.transform);
                StartCoroutine(RisePastry(newCookie.transform, cookieDistance));
                break;
            case PastryNames.ICE_CREAM:
                GameObject newIceCream = Instantiate(iceCream, spawnPoint.position, Quaternion.identity, this.transform);
                StartCoroutine(RisePastry(newIceCream.transform, iceCreamDistance));
                break;
        }

        // If the current plate is the last order, compare the baked pastries array and the desired pastries array; otherwise, move current plate to the next empty plate.
        if (currentPlate == 2) CheckOrders();
        else currentPlate++;
    }

    /// <summary>
    /// Compares the baked pastries array and the desired pastries array. If the contents are the same, then congratulate the Player, destroy the pastries on the plates, clear the
    /// baked pastries array and generate new pastries for the desired pastries array; otherwise, notify the Player that their pastries are incorrect, destroy the pastries on the
    /// plates and clear the baked pastries array.
    /// </summary>
    private void CheckOrders()
    {
        for (int i = 0; i < PLATES; i++)
        {
            // If baked pastries does not contain a pastry in desired pastries at a particular index, then notify Player they are incorrect, clear their baked pastries and break
            // out of function.
            if (!bakedPastries.Contains(desiredPastries[i]))
            {
                StartCoroutine(PlayerCanvas.Instance.DisplayOrderStatusText(0));

                StartCoroutine(DestroyPastries());

                return;
            }
        }

        // If baked pastries contains all the pastries in desired pastries, then notify Player they are correct, clear their baked pastries, create new pastry orders and display
        // the new desired pastries to Player.
        StartCoroutine(PlayerCanvas.Instance.DisplayOrderStatusText(1));

        StartCoroutine(DestroyPastries());

        CreateOrders();

        PlayerCanvas.Instance.DisplayOrder(desiredPastries);
    }
    #endregion

    #region PastryGameObjectFuntions
    /// <summary>
    /// Raises a pastry GameObject a certain distance.
    /// </summary>
    /// <param name="pastry"> transform of pastry to rise </param>
    /// <param name="distance"> distance to rise pastry </param>
    private IEnumerator RisePastry(Transform pastry, float distance)
    {
        float risePosition = pastry.position.y + distance;

        while (pastry.position.y < risePosition)
        {
            yield return new WaitForFixedUpdate();
            pastry.position += Vector3.up * speed / 1000;
        }
    }

    /// <summary>
    /// Clears baked pastries array, set the current plate to the first plate, reset the baked pastries text on the Player canvas and destroy each pastry on the plates.
    /// </summary>
    private IEnumerator DestroyPastries()
    {
        // Wait 1 second before executing statements below to prevent RisePastry from accessing a nonexistent Transform.
        yield return new WaitForSeconds(1f);

        // Clear baked pastries array.
        for (int j = 0; j < PLATES; j++)
        {
            bakedPastries[j].name = "";
            bakedPastries[j].sweetnessLevel = 0;
        }
        
        // Set current plate to first plate.
        currentPlate = 0;

        // Reset display for baked pastries.
        PlayerCanvas.Instance.DisplayBakedPastry(bakedPastries[0]);

        // Destroy each child of Counter with the Pastry tag.
        foreach (Transform child in transform)
            if (child.tag == GameObjectTags.PASTRY)
                Destroy(child.gameObject);
    }
    #endregion
}
