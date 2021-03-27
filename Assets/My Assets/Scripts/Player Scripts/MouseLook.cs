/*
 * Handles the camera rotation and cursor lock mode for the player.
 * 
 * Author: Cristion Dominguez
 * Latest Revision: 27 Sept. 2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Camera Roots")]
    [SerializeField]
    private Transform playerRoot;
    
    [SerializeField]
    private Transform lookRoot; // Transforms of Player and Look Root.

    [Header("Initial Player Rotation")]
    [SerializeField]
    private float horizontalRotationAngles;

    [SerializeField]
    private float verticalRotationAngles;

    [Header("Look Settings")]
    [SerializeField]
    private bool invert; // determines whether the player has an inverted vertical rotations

    [SerializeField]
    private float sensitivity = 5f; // sensitivity at which Player and Look Root rotate.

    [SerializeField]
    private float rollAngle = 10f; // intensity of z-rotations

    [SerializeField]
    private float rollSpeed = 3f; // speed z-angles return to 0

    [SerializeField]
    private Vector2 defaultLookLimits = new Vector2(-70f, 80f); // limits of vertical rotation

    private Vector2 lookAngles; // vector containing new angles for the Player and Look Root to rotate towards

    private Vector2 currentMouseLook; // vector containing inputs of moving mouse

    private float currentRollAngle; // current z-angle

    /// <summary>
    /// Locks cursor to the center of the game window and sets the initial Player's rotation upon program startup.
    /// </summary>
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        lookAngles.y = horizontalRotationAngles;
        lookAngles.x = verticalRotationAngles;
    }

    /// <summary>
    /// Upon each frame, determines whether the cursor should be locked or unlocked based on player input and allow the player to rotate their camera 
    /// whenever the the cursor is locked.
    /// </summary>
    private void Update()
    {
        LockAndUnlockCursor();

        // Only run the LookAround method if the Cursor is in Locked mode.
        if (Cursor.lockState == CursorLockMode.Locked) LookAround();
    }

    /// <summary>
    /// When the Escape key is pressed, unlocks the cursor if the cursor is in Locked; otherwise locks the cursor as well as makes it invisible if the
    /// cursor is not in Locked mode.
    /// </summary>
    internal void LockAndUnlockCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;

            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    /// <summary>
    /// Execute rotations on the Player and the Look Root based on mouse input.
    /// </summary>
    internal void LookAround()
    {
        // Assign currentMouseLook to a 2D Vector that has MOUSE_Y passed as the horizontal value and MOUSE_X passed as the vertical value since both the
        // Player and Look Root rotate horizontally when their Y rotation values are altered and rotate vertically when their X rotation values are altered.
        currentMouseLook = new Vector2(Input.GetAxis(MouseAxis.MOUSE_Y), Input.GetAxis(MouseAxis.MOUSE_X));

        // Calculate the new vertical and horizontal angles (respectively) of lookAngles based on the input from currentMouseLook and senstivity.
        lookAngles.x += currentMouseLook.x * sensitivity * (invert ? 1f : -1f); // if invert is true, then the vertical rotations are inversed
                                                                                    // from the currentMouseLook input
        lookAngles.y += currentMouseLook.y * sensitivity;

        // Constrain lookAngles to a vertical angle interval (defaultLookLimits.x is min and defaultLookLimits.y is max).
        lookAngles.x = Mathf.Clamp(lookAngles.x, defaultLookLimits.x, defaultLookLimits.y);

        // The second line below adds a "drunk" feature to a player's look rotation by allowing rotation on the z-axis.
        // Change currentRollAngle to Input.GetAxisRaw("Mouse X") [1 or -1] * rollAngle in the time span of Time.deltaTime * rollSpeed. 
        currentRollAngle = Mathf.Lerp(currentRollAngle, Input.GetAxisRaw(MouseAxis.MOUSE_X) * rollAngle, Time.deltaTime * rollSpeed);

        // Allow the Look Root to rotate vertically to lookAngles.x and rotate on the z-axis to currentRollAngle.
        lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, currentRollAngle);

        // Allow the Player to rotate horizontally to lookAngles.y.
        playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);
    }
}
