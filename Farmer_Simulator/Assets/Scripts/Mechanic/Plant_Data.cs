using UnityEngine;

public class Plant_Data : MonoBehaviour , CanWatered
{
    public enum PlantStage{
        Seed,
        Growing,
        Harvest , // can harvest
        Dead ,
        None // no plant
    }

    public Seed_Scriptable SeedData;
    [SerializeField] private PlantStage _plantStage;
    [SerializeField] private int currentDay;
    [SerializeField] private bool isWatered;
    [SerializeField] private Transform plantLocation; // location to plant the seed
    private GameObject plantPrefab; // prefab of the plant

    private FindDataItem findDataItem;


    void OnEnable()
    {
        Time_Manager.OnNewMorning += HandleOnNewMorning;
    }

    void OnDisable()
    {
        Time_Manager.OnNewMorning -= HandleOnNewMorning;
    }

    void Start()
    {
        findDataItem = FindObjectOfType<FindDataItem>();
    }

    private void HandleOnNewMorning()
    {
        Grow();
    }   

    // Water the plant
    public void Watered(){
        isWatered = true;
    }

    // Plant the seed
    public void PlantSeed(Seed_Scriptable seedData){
        SeedData = seedData;
        Game_Manager.instance.playerData.SubtractMoney(SeedData.BuyPrice); // Subtract money when planting the seed
        _plantStage = PlantStage.Seed;
        currentDay = 0;
        isWatered = false; // Reset watering status for the new seed
    }

    // Harvest the plant
    //TODO : if Harvest
    public void _Harvest(){
        if(_plantStage == PlantStage.Harvest){
            findDataItem.AddItemToPlayer(SeedData.product.getID(),FindObjectOfType<PlayerData>());
            SeedData = null; // Clear the product after harvesting
            _plantStage = PlantStage.None;
        }
        else{
            return;
        }
    }

    public void Sell(){
        if(_plantStage == PlantStage.Harvest){
            Game_Manager.instance.playerData.AddMoney(SeedData.SellPrice);
            SeedData = null; // Clear the product after harvesting
            _plantStage = PlantStage.None;
        }
        else{
            return;
        }
    }

    private void Grow(){
        if(_plantStage == PlantStage.Dead || _plantStage == PlantStage.Harvest) return;

        if(!isWatered && SeedData.WaterNeeded) _plantStage = PlantStage.Dead;

        currentDay++;
        isWatered = false; // Reset watering status for the next day

        if(_plantStage == PlantStage.Seed){
            _plantStage = PlantStage.Growing;
        }
        else if(_plantStage == PlantStage.Growing && currentDay >= SeedData.GrowDay){
            _plantStage = PlantStage.Harvest;
        }


        Debug.Log("Plant is now at stage: " + _plantStage);
    }

    private void ChangeGameObj(int index){
        if(plantPrefab == null) plantPrefab = SeedData.growStates[index].plantPrefab;
        else if(plantPrefab != null){
            if (plantLocation.childCount > 0) {
                Destroy(plantLocation.GetChild(0).gameObject); // Destroy the old plant prefab
                plantPrefab = null;
            }
            plantPrefab = SeedData.growStates[index].plantPrefab;
            GameObject newPlant = Instantiate(plantPrefab, plantLocation.position, Quaternion.identity, plantLocation);
            newPlant.transform.localPosition = Vector3.zero; // Set position to zero
        }
    }
}
