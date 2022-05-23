using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingsGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);

    private Building[,] grid;
    private Building flyingBuilding;
    private Camera mainCamera;

    [SerializeField] private GameObject NotEnoughResources;
    [SerializeField] private Text GoldText;
    [SerializeField] private Text WoodText;
    [SerializeField] private Text IronText;
    [SerializeField] private Text ArmyPowerText;
    

    public float TotalGold = 1000f;
    public float GoldGain = 0f;

    public float TotalWood = 0f;
    public float WoodGain = 0f;

    public float TotalIron = 0f;
    public float IronGain = 0f;

    public float ArmyPower = 0f;

    private bool TownHallPresent = false;

    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];
        
        mainCamera = Camera.main;

        NotEnoughResources.SetActive(false);
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        if (buildingPrefab.Type == Building.EType.TownHall && TownHallPresent)
            return;

        if (TotalGold >= buildingPrefab.GoldPrice && TotalWood >= buildingPrefab.WoodPrice && TotalIron >= buildingPrefab.IronPrice) 
        {
            flyingBuilding = Instantiate(buildingPrefab);
        }
        else
        {
            NotEnoughResources.SetActive(true);
            StartCoroutine(FadeTextToZeroAlpha(1f, NotEnoughResources.GetComponent<Text>()));
        }
    }

    private void Update()
    {
        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool available = true;

                if (x < 0 || x > GridSize.x - flyingBuilding.Size.x) available = false;
                if (y < 0 || y > GridSize.y - flyingBuilding.Size.y) available = false;

                if (available && IsPlaceTaken(x, y)) available = false;

                flyingBuilding.transform.position = new Vector3(x, 0, y);
                flyingBuilding.SetTransparent(available);

                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBuilding(x, y);
                }
            }
        }

        else 
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                if (x >= 0 && x < GridSize.x && y >= 0 && y < GridSize.y)
                {
                    if (grid[x, y] != null && Input.GetMouseButtonDown(0))
                    {
                        grid[x, y].Upgrade(ref TotalGold, ref TotalWood, ref TotalIron, ref GoldGain, ref WoodGain, ref IronGain, ref ArmyPower);
                    }
                    if (grid[x, y] != null && Input.GetMouseButtonDown(1))
                    {
                        Destroy(grid[x, y].gameObject);
                    }
                }
            }

        }

        TotalGold += GoldGain * Time.deltaTime;
        TotalWood += WoodGain * Time.deltaTime;
        TotalIron += IronGain * Time.deltaTime;

        GoldText.text = $"Золото: {(int)TotalGold}";
        WoodText.text = $"Дерево: {(int)TotalWood}";
        IronText.text = $"Железо: {(int)TotalIron}";
        ArmyPowerText.text = $"Сила армии: {(int)ArmyPower}";
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null) return true;
            }
        }

        return false;
    }

    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = flyingBuilding;
            }
        }

        if (flyingBuilding.Type != Building.EType.Tower) GoldGain += 5f;
        
        if(flyingBuilding.Type == Building.EType.Sawmill) WoodGain += 4f;

        if (flyingBuilding.Type == Building.EType.Forge) IronGain += 4f;

        if (flyingBuilding.Type == Building.EType.Tower) ArmyPower += 200f;

        if (flyingBuilding.Type == Building.EType.TownHall) TownHallPresent = true;

        TotalGold -= flyingBuilding.GoldPrice;
        TotalWood -= flyingBuilding.WoodPrice;
        TotalIron -= flyingBuilding.IronPrice;

        flyingBuilding.SetNormal();
        flyingBuilding = null;
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        yield return new WaitForSeconds(2f);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        NotEnoughResources.SetActive(false);
    }
}
