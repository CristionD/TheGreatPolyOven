/*
 * Provides an enumerator to distinguish buttons on the Poly Oven and a public function to animate the buttons.
 * 
 * Author: Cristion Dominguez
 * Latest Revision: 27 Sept. 2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Distinguishes buttons on Poly Oven.
/// </summary>
public enum ButtonType
{
    CAKE,
    COOKIE,
    ICE_CREAM,
    SPECIAL,
    BAKE,
    CLEAR
}

// Each button on the Poly Oven has their own instance of the ButtonBehavior class.
public class ButtonBehavior : MonoBehaviour
{
    public ButtonType buttonType;  // the type of button

    private Animator buttonAnim;  // button Animator

    /// <summary>
    /// Assigns the Animator for the button before program completely starts.
    /// </summary>
    private void Awake() => buttonAnim = this.gameObject.GetComponent<Animator>();

    /// <summary>
    /// Plays the Press animation for the button.
    /// </summary>
    public void Press() => buttonAnim.Play(AnimationTags.PRESS);
}
