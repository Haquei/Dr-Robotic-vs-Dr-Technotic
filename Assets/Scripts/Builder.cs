using UnityEngine;

[RequireComponent(typeof(Movement), typeof(PlacementPreview))]
public class Builder : MonoBehaviour
{

    // TODO replace this with randomized buildings.
    [SerializeField] Building buildingPrefab;
    [SerializeField] Building bigBuildingPrefab;

    [SerializeField] BuildingOrb buildingOrbPrefab;

    [SerializeField] float buildRange;
    [SerializeField] Grid buildGrid;

    private PlacementPreview myPlacementPreview;

    [SerializeField] Building[] buildingOptions;

    // Start is called before the first frame update
    void Awake()
    {
        myPlacementPreview = GetComponent<PlacementPreview>();
    }

    public void ThrowBuildingPreview(Vector3 position)
    {
        // Correct position for range
        if (Vector3.Distance(transform.position, position) > buildRange)
        {
            Vector3 direction = (position - transform.position).normalized;
            position = transform.position + direction * buildRange;
        }
        // Map it to the grid
        position = buildGrid.MapPositionToGrid(position, 1, 1);
        // Move to middle of the block 
        // position += new Vector3(1, -0.5f, 1);

        myPlacementPreview.DrawParabola(transform.position + Vector3.up * 2, position);
    }

    public void CancelPreview()
    {
        myPlacementPreview.ClearPreview();
    }

    public void ThrowBuilding(Vector3 position, Building building)
    {
        BuildingOrb orb = Instantiate(buildingOrbPrefab);
        orb.PassData(building, buildGrid);
        orb.Launch(transform.position + Vector3.up * 2, position);
    }

    public Building PickRandomBuilding()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(buildingOptions.Length);
        return buildingOptions[index];
    }
}
