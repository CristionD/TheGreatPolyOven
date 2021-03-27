/*
 * Handles the interactions between the Player and the Poly Oven, displaying descriptions or invoking a Poly Oven function
 * based on the Player's reticle position and used keys.
 * 
 * Author: Cristion Dominguez
 * Latest Revision: 27 Sept. 2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    /// <summary>
    /// Casts a ray from the Main camera every frame. If the ray collides with a Button transform on the Poly Oven, then display the description for that respective button and if the
    /// Player clicks the left mouse button (LMB) whilst looking at that Poly Oven button, then carry out the function associated with the particular button.
    /// </summary>
    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // If the ray collides with a Transform containing the Button tag, then display the description for that particular button.
            if (hit.transform.tag == GameObjectTags.BUTTON)
            {
                var button = hit.transform.GetComponent<ButtonBehavior>();
                var oven = hit.transform.GetComponentInParent<PolyOven>();

                switch (button.buttonType)
                {
                    case ButtonType.CAKE:
                        PlayerCanvas.Instance.DisplayButtonDescription("Select Cake", true);
                        break;
                    case ButtonType.COOKIE:
                        PlayerCanvas.Instance.DisplayButtonDescription("Select Cookie", true);
                        break;
                    case ButtonType.ICE_CREAM:
                        PlayerCanvas.Instance.DisplayButtonDescription("Select Ice-Cream", true);
                        break;
                    case ButtonType.SPECIAL:
                        PlayerCanvas.Instance.DisplayButtonDescription("Add Special Technique (Sweetness +1)", true);
                        break;
                    case ButtonType.BAKE:
                        PlayerCanvas.Instance.DisplayButtonDescription("Bake Selected Pastry", true);
                        break;
                    case ButtonType.CLEAR:
                        PlayerCanvas.Instance.DisplayButtonDescription("Clear Selection", true);
                        break;
                }

                // If the Player presses LMB whilst looking at a Poly Oven button, then initiate the button Press animation and call the Poly Oven function
                // associated with it.
                if (Input.GetMouseButtonDown(0))
                {
                    button.Press();

                    switch(button.buttonType)
                    {
                        case ButtonType.CAKE:
                            oven.SelectCake();
                            break;
                        case ButtonType.COOKIE:
                            oven.SelectCookie();
                            break;
                        case ButtonType.ICE_CREAM:
                            oven.SelectIceCream();
                            break;
                        case ButtonType.SPECIAL:
                            oven.AddSpecialTechnique();
                            break;
                        case ButtonType.BAKE:
                            oven.Bake();
                            break;
                        case ButtonType.CLEAR:
                            oven.ClearSelection();
                            break;
                    }
                }
            }
            // If the ray collides with a Transform not containing the Button tag, then remove the button description text from the Player's view.
            else
            {
                PlayerCanvas.Instance.DisplayButtonDescription("", false);
            }
        }
    }
}
