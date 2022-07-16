using UnityEngine;

public class BuildingOrb : MonoBehaviour
{
    private bool isInitialized;
    private Vector3 startingPosition;
    private Vector3 destination;
    private float currentTimeFiring;
    private Building building;
    private Grid grid;

    void Awake()
    {
        this.enabled = false;
    }

    void Update()
    {
        if (!isInitialized) return;

        transform.position = ParabolaHelper.LerpParabola(
                startingPosition, destination, 3, currentTimeFiring / 0.3f);
        currentTimeFiring += Time.deltaTime;

        if (currentTimeFiring / 0.3f > 1)
        {
            Building toPlace = Instantiate(building);
            grid.PlaceObjectOnGrid(destination, toPlace);
            Destroy(gameObject);
        }
    }

    public void Launch(Building building, Grid grid, Vector3 from, Vector3 to)
    {
        this.enabled = true;
        transform.position = from;

        this.building = building;
        this.grid = grid;

        this.startingPosition = from;
        this.destination = to;

        this.currentTimeFiring = 0f;

        isInitialized = true;
    }
}
