using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Data : MonoBehaviour
{
    public enum PlantStage{
        Seed,
        Growing,
        Harvest
    }

    public Plant_Scriptable plantData;
    [SerializeField] private PlantStage _plantStage;
    [SerializeField] private int currentDay;
    [SerializeField] private bool isWatered;


    
}
