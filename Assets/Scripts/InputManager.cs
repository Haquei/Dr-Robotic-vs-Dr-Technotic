using UnityEngine;

public class InputManager : MonoBehaviour
{

    const int LEFT_CLICK = 0;
    const int RIGHT_CLICK = 1;

    // I like auto-right clickers when you have to move dudes...
    // ... Dota has ruined me xD
    [Header("Auto Right Clicker")]
    public float clickDelay = 0.4f;
    private float lastRightClickTime;
    private bool CanRightClick => Time.time - lastRightClickTime > clickDelay;

    [SerializeField] Builder builder;
    private Movement movementComponent;

    [SerializeField] Grid buildGrid;

    // TODO make better
    Building randomBuilding;

    void Start()
    {
        movementComponent = builder.GetComponent<Movement>();

        lastRightClickTime = -clickDelay;
        randomBuilding = builder.PickRandomBuilding();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 clickPosition = Camera.main.CastMouseOnFloor();

        // Making builder face the mouse since he's not moving anymore.
        builder.transform.LookAt(clickPosition);

        if (Input.GetMouseButton(RIGHT_CLICK) && CanRightClick)
        {
            movementComponent.Move(clickPosition);
            lastRightClickTime = Time.time;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            buildGrid.ShowGrid();
            buildGrid.ShowPlacementPreview(clickPosition, randomBuilding.WidthInBlocks, randomBuilding.HeightInBlocks);
            builder.ThrowBuildingPreview(clickPosition);

            bool canPlace = buildGrid.CanPlaceObjectOnGrid(clickPosition, randomBuilding.WidthInBlocks, randomBuilding.HeightInBlocks);

            if (Input.GetMouseButtonDown(LEFT_CLICK) && canPlace)
            {
                builder.ThrowBuilding(clickPosition, randomBuilding);
                builder.CancelPreview();
                randomBuilding = builder.PickRandomBuilding();
            }
        
        } 

        // TODO dedup when randomization done.
        /*if (Input.GetKey(KeyCode.W))
        {
            buildGrid.ShowGrid();
            buildGrid.ShowPlacementPreview(clickPosition, 2, 2);
            builder.ThrowBuildingPreview(clickPosition);

            bool canPlace = buildGrid.CanPlaceObjectOnGrid(clickPosition, 2, 2);

            if (Input.GetMouseButtonDown(LEFT_CLICK) && canPlace)
            {
                builder.ThrowBuilding(clickPosition, 2);
                builder.CancelPreview();
            }

        }*/
        

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Q))
        {
            buildGrid.HideGrid();
            buildGrid.HidePlacePreview();
            builder.CancelPreview();
        }

        // Random testing buttons
        if (Input.GetKeyDown(KeyCode.F))
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.Freeze(2);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.Slow(5, 0.3f);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.TakeDamage(5);
            }
        }


        // TODO movement indicator / affordance

        // TODO left click to place buildings
    }

}
