using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiationmanager : MonoBehaviour
{
    private void Awake()
    {
        VirtualwalkThrough.SpawnObjects += VirtualwalkThrough_InstantiateObject;

    }

    private void VirtualwalkThrough_InstantiateObject(List<SpawanObject> objList)
    {
//        Debug.Log("Called");
        foreach (SpawanObject instantiation in objList)
        {
            GameObject obj = Instantiate(instantiation.gameObject, instantiation.pos, Quaternion.Euler(instantiation.rotation));


        }
    }

    private void OnDestroy()
    {
        VirtualwalkThrough.SpawnObjects -= VirtualwalkThrough_InstantiateObject;

    }
}
