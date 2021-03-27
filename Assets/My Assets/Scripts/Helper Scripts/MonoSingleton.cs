/*
 * Changes scripts into a singleton.
 * 
 * Author: Cristion Dominguez
 * Latest Revision: 7 Aug. 2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;

    // Returns instance of class if it is not null; otherwise, prints an error message.
    public static T Instance
    {
        get
        {
            if (instance == null) Debug.LogError(typeof(T).ToString() + " is NULL.");
            return instance;
        }
    }

    /// <summary>
    /// Assigns instance to a script of class type T.
    /// </summary>
    private void Awake()
    {
        instance = (T)this;

        Init();
    }

    /// <summary>
    /// Replaces the Awake function for classes inheriting the MonoSingleton class.
    /// </summary>
    protected virtual void Init() { }
}