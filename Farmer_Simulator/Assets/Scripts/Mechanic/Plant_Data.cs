using UnityEngine;

public class Plant_Data : MonoBehaviour , CanWatered
{
    [SerializeField] CheckRobotWork Robottakecare;
    public enum PlantStage{
        Seed,
        Growing,
        Harvest , // can harvest
        Dead ,
        Infected,
        None // no plant
    }

    public enum PotLevel{
        Normal,
        Upgrade
    }

    [Header("Plant Data")]
    public Seed_Scriptable SeedData;
    public PlantStage _plantStage;
    public PotLevel _potLevel; // level of the pot
    [SerializeField] private int currentDay;
    public bool isWatered;
    [SerializeField] private Transform plantLocation; // location to plant the seed

    [Header("Infect")]
    public bool isInfected;

    [Tooltip("Infect Chance Rate Chance to infect the plant 0 is no chance 1 is 100% chance")]
    [SerializeField] [Range(0f,1f)] private float infectChanceRate; // chance to infect the plant

    private GameObject plantPrefab; // prefab of the plant

    private FindDataItem findDataItem;
    private int plantIndex = 0; // index of the plant in the lis


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

        if(SeedData != null)
        {
            plantIndex = 0; // Reset plant index when planting a new seed
            ChangeGameObj(plantIndex);
            _plantStage = PlantStage.Seed;
            currentDay = 0;
            isWatered = false; // Reset watering status for the new seed
        }
        else
        {
            _plantStage = PlantStage.None; // No seed planted
        }
    }

    void Update()
    {
        if (Robottakecare.TakeCared == true)
        {
            Watered();
            PlantCure();
            Debug.Log("Take care !!!");
        }

        if (SeedData == null) {
            GetComponent<BoxCollider>().enabled = true;
            return;
        }
        else if(SeedData != null) {
            GetComponent<BoxCollider>().enabled = false;
            return;
        }
        if(plantIndex >= 2) {
            plantIndex = 2;
        }
    }

    private void HandleOnNewMorning()
    {
        if(_plantStage == PlantStage.None) return; // No seed planted
        Grow();
    }  

    private void Grow(){
        if(_plantStage == PlantStage.Dead) {
            ChangeGameObj(3);
            return;
        }

        if(_plantStage == PlantStage.Harvest) return;

        if(!isWatered && SeedData.WaterNeeded || _plantStage == PlantStage.Infected) {
            ChangeGameObj(3);

            _plantStage = PlantStage.Dead;
            return;
        }

        currentDay++;
        isWatered = false; // Reset watering status for the next day

        if(_plantStage == PlantStage.Seed){
            _plantStage = PlantStage.Growing;
            plantIndex++;
            ChangeGameObj(plantIndex);
        }
        else if(_plantStage == PlantStage.Growing && currentDay >= SeedData.GrowDay){
            _plantStage = PlantStage.Harvest;
            plantIndex++;
            ChangeGameObj(plantIndex);
        }
        else if(_plantStage == PlantStage.Growing && currentDay <= SeedData.GrowDay){
            plantInfect();
        }


        Debug.Log("Plant is now at stage: " + _plantStage);
    }

    // Infect the plant
    private void plantInfect(){
        if(_plantStage == PlantStage.Growing){
            float randomValue = Random.Range(0f, 1f);
            if(randomValue <= infectChanceRate){
                isInfected = true;
                ChangeGameObj(4);
                Debug.Log("Plant is infected!");
                _plantStage = PlantStage.Infected;
            }
        }
    }

    //Update the plant prefab based on the current stage
    private void ChangeGameObj(int index){
        if(plantPrefab == null) {
            plantPrefab = SeedData.growStates[index].plantPrefab;
            GameObject newPlant = Instantiate(plantPrefab, plantLocation.position, Quaternion.identity, plantLocation);
            newPlant.transform.localPosition = Vector3.zero; // Set position to zero
        }
        else if(plantPrefab != null){
            if (plantLocation.childCount > 0) {
                Debug.Log("Destroying old plant prefab: " + plantLocation.GetChild(0).name);
                Destroy(plantLocation.GetChild(0).gameObject); // Destroy the old plant prefab
                plantPrefab = null;
            }
            plantPrefab = SeedData.growStates[index].plantPrefab;
            GameObject newPlant = Instantiate(plantPrefab, plantLocation.position, Quaternion.identity, plantLocation);
            newPlant.transform.localPosition = Vector3.zero; // Set position to zero
        }
    }

    #region for Gun
    // Cure the plant


    public void PlantCure(){
        if(_plantStage == PlantStage.Infected){
            isInfected = false;
            ChangeGameObj(1);

            _plantStage = PlantStage.Growing;
        }
    }

    // Harvest the plant
    //TODO : if Harvest
    public void _Harvest(){
        if(_plantStage == PlantStage.Harvest){
            // findDataItem.AddItemToPlayer(SeedData.product.getID(),FindObjectOfType<PlayerData>()); // Add the harvested product to the player's inventory
            Destroy(plantLocation.GetChild(0).gameObject);
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
            Destroy(plantLocation.GetChild(0).gameObject);
            SeedData = null; // Clear the product after harvesting
            _plantStage = PlantStage.None;
        }
        else if(_plantStage == PlantStage.Dead){
            Destroy(plantLocation.GetChild(0).gameObject);
            SeedData = null; // Clear the product after harvesting
            _plantStage = PlantStage.None;
        }
        else{
            return;
        }
    }

    // Water the plant
    public void Watered(){
        isWatered = true;
    }

    // Plant the seed
    public void PlantSeed(Seed_Scriptable seedData){
        SeedData = seedData;
        Game_Manager.instance.playerData.SubtractMoney(SeedData.BuyPrice); // Subtract money when planting the seed
        plantIndex = 0;
        ChangeGameObj(plantIndex);
        _plantStage = PlantStage.Seed;
        currentDay = 0;
        isWatered = false; // Reset watering status for the new seed
    }

    public void GrowPlant(){
        Game_Manager.instance.playerData.SubtractMoney(100);
        Watered();
        if(_plantStage == PlantStage.Seed){
            Debug.Log("Plant is growing!");
            _plantStage = PlantStage.Growing;
            plantIndex++;
        }
        else if(_plantStage == PlantStage.Growing){
            _plantStage = PlantStage.Harvest;
            plantIndex++;
        }
        ChangeGameObj(plantIndex);
    }

    #endregion

    #region for hand gun

    public void HandleCureAction()
    {
        Debug.Log("Handling cure action on: " + gameObject.name);
        // Add logic for curing objects
        PlantCure();
    }

    public void HandleHarvestAction()
    {
        Debug.Log("Handling harvest action on: " + gameObject.name);
        // Add logic for harvesting crops
        Sell();
    }

    public void HandlePlantAction(Seed_Scriptable seedData)
    {
        Debug.Log("Handling plant action on: " + gameObject.name);
        // Add logic for planting seeds
        PlantSeed(seedData);
    }

    public void HandleWaterAction()
    {
        Debug.Log("Handling water action on: " +gameObject.name);
        // Add logic for watering plants
        Watered();
    }

    public void HandleGrowAction()
    {
        Debug.Log("Handling grow action on: " + gameObject.name);
        // Add logic for accelerating plant growth
        GrowPlant();
    }
    #endregion
}
