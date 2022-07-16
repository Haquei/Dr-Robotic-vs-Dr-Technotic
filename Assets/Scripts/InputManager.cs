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

    void Start()
    {
        movementComponent = builder.GetComponent<Movement>();

        lastRightClickTime = -clickDelay;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 clickPosition = Camera.main.CastMouseOnFloor();

        if (Input.GetMouseButton(RIGHT_CLICK) && CanRightClick)
        {
            movementComponent.Move(clickPosition);
            lastRightClickTime = Time.time;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            buildGrid.ShowGrid();
            builder.ThrowBuildingPreview(clickPosition);

            if (Input.GetMouseButtonDown(LEFT_CLICK))
            {
                builder.ThrowBuilding(clickPosition, 1);
                builder.CancelPreview();
            }
        
        } 

        // TODO dedup when randomization done.
        if (Input.GetKey(KeyCode.W))
        {
            buildGrid.ShowGrid();
            builder.ThrowBuildingPreview(clickPosition);

            if (Input.GetMouseButtonDown(LEFT_CLICK))
            {
                builder.ThrowBuilding(clickPosition, 2);
                builder.CancelPreview();
            }

        }
        

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Q))
        {
            buildGrid.HideGrid();
            builder.CancelPreview();
        }

        // TODO movement indicator / affordance

        // TODO left click to place buildings
    }

}
