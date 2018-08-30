/*
 * Copyright (c) 2016-2017, Ivan Gadzhega.
 * All rights reserved.
 *
 * This software is the confidential and proprietary information of Ivan Gadzhega.
 * ("Confidential Information"). You shall not disclose such Confidential Information
 * and shall use it only in accordance with the terms of the license agreement
 * you entered into with Ivan Gadzhega.
 */

using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace ua.org.gdg.devfest
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        [SuppressMessage("ReSharper", "InvertIf")]
        public static T Instance 
        {
            get 
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<T>();                    

                    if (instances.Length == 1)
                    {
                        _instance = instances[0]; 
                    }
                    else if (instances.Length > 1)
                    {
                        Debug.LogError(typeof(T) + ": There is more than 1 instance in the scene.");
                    }
                    else
                    {
                        Debug.LogError(typeof(T) + ": Instance doesn't exist in the scene.");
                    }
                }

                return _instance;
            }
        }
	
    }
}