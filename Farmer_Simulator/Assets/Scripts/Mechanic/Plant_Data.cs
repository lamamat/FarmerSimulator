using UnityEngine;

public class Plant_Data : MonoBehaviour
{
    public enum PlantStage{
        Seed,
        Growing,
        Harvest , // can harvest
        Dead
    }

    public Seed_Scriptable SeedData;
    [SerializeField] private PlantStage _plantStage;
    [SerializeField] private int currentDay;
    [SerializeField] private bool isWatered;

    void OnEnable()
    {
        Time_Manager.OnNewMorning += HandleOnNewMorning;
    }

    void OnDisable()
    {
        Time_Manager.OnNewMorning -= HandleOnNewMorning;
    }

    private void HandleOnNewMorning()
    {
        Grow();
    }   

    private void Grow(){
        if(_plantStage == PlantStage.Dead || _plantStage == PlantStage.Growing) return;

        if(!isWatered && SeedData.WaterNeeded) _plantStage = PlantStage.Dead;

        if(_plantStage == PlantStage.Seed){
            _plantStage = PlantStage.Growing;
        }
        else if(_plantStage == PlantStage.Growing){
            _plantStage = PlantStage.Harvest;
        }

        Debug.Log("Plant is now at stage: " + _plantStage);
    }
}
