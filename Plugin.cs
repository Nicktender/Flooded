using System;
using BepInEx;
using UnityEngine;
using System.ComponentModel;
using Utilla;

namespace Flooded
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [Description("HauntedModMenu")]
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private bool inRoom;
        private bool modEnabled;
        private GameObject beachObject;
        private GameObject forestToBeachPrefabObject;
        private GameObject caveWater;
        private Vector3 caveWaterInitialPosition;
        private Quaternion caveWaterInitialRotation;
        private Vector3 caveWaterInitialScale;

        private GameObject oceanWater;
        private Vector3 oceanWaterInitialPosition;
        private Vector3 oceanWaterInitialScale;

        private void Update()
        {
            // Assuming you have a reference to the parent object
            Transform rigParent = GameObject.Find("Global/RigCache/Rig Parent").transform;

            // Check for active child and quit the game if found
            foreach (Transform child in rigParent)
            {
                if (child.gameObject.activeSelf)
                {
                    if(inRoom && modEnabled)
                    {
 
                        Application.Quit();
                        break; // Exit the loop once an active child is found and the game is quit.
                    }
                }
            }
            // Assuming the GameObjects are present in the scene or instantiated before this script runs.
            if (beachObject == null)
            {
                beachObject = GameObject.Find("beach");
            }
            if (forestToBeachPrefabObject == null)
            {
                forestToBeachPrefabObject = GameObject.Find("ForestToBeach_Prefab_V4");
            }

            // enable beach and beach entrance if they aren't already.
            if (!beachObject.activeSelf)
            {
                beachObject.SetActive(true);
            }

            if (!forestToBeachPrefabObject.activeSelf)
            {
                forestToBeachPrefabObject.SetActive(true);
            }
        }

        private void OnEnable()
        {
            FindAndStoreCaveWater();
            FindAndStoreOceanWater();

            SetCaveWaterTransform(new Vector3(-2.2866f, 6.0803f, -40.1154f), new Vector3(1000f, 1000f, 1000f));
            SetOceanWaterTransform(new Vector3(26.8862f, -43.9815f, 33.4743f), new Vector3(10000f, 100f, 10000f));

            modEnabled = true;
        }

        private void OnDisable()
        {
            modEnabled = false;
            Revert();
        }

        private void Revert()
        {
            SetCaveWaterTransform(caveWaterInitialPosition, caveWaterInitialScale, caveWaterInitialRotation);
            SetOceanWaterTransform(oceanWaterInitialPosition, oceanWaterInitialScale);
        }

        private void FindAndStoreCaveWater()
        {
            caveWater = GameObject.Find("B_CaveWaterV2");
            if (caveWater == null)
            {
                Debug.LogWarning("CaveWater object not found!");
            }
            else
            {
                caveWaterInitialPosition = caveWater.transform.position;
                caveWaterInitialRotation = caveWater.transform.rotation;
                caveWaterInitialScale = caveWater.transform.localScale;
            }
        }

        private void FindAndStoreOceanWater()
        {
            oceanWater = GameObject.Find("OceanWater");
            if (oceanWater == null)
            {
                Debug.LogWarning("OceanWater object not found!");
            }
            else
            {
                oceanWaterInitialPosition = oceanWater.transform.position;
                oceanWaterInitialScale = oceanWater.transform.localScale;
            }
        }

        private void SetCaveWaterTransform(Vector3 position, Vector3 scale)
        {
            if (caveWater != null)
            {
                caveWater.transform.position = position;
                caveWater.transform.localScale = scale;
            }
        }

        private void SetCaveWaterTransform(Vector3 position, Vector3 scale, Quaternion rotation)
        {
            if (caveWater != null)
            {
                caveWater.transform.position = position;
                caveWater.transform.localScale = scale;
                caveWater.transform.rotation = rotation;
            }
        }

        private void SetOceanWaterTransform(Vector3 position, Vector3 scale)
        {
            if (oceanWater != null)
            {
                oceanWater.transform.position = position;
                oceanWater.transform.localScale = scale;
            }
        }

        private void SetOceanWaterTransform(Vector3 position, Vector3 scale, Quaternion rotation)
        {
            if (oceanWater != null)
            {
                oceanWater.transform.position = position;
                oceanWater.transform.localScale = scale;
                oceanWater.transform.rotation = rotation;
            }
        }
        [ModdedGamemodeJoinAttribute]
        void OnLeave(string gamemode)
        {
            inRoom = false;
            Revert();
        }
        [ModdedGamemodeLeaveAttribute]
        void OnJoin(string gamemode)
        {
            inRoom = true;
        }
    }
}
