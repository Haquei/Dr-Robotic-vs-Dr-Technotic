using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for things that can be placed on the grid
public interface IPlaceable
{
    int WidthInBlocks { get; }
    int HeightInBlocks { get; }
    int GridX { get; }
    int GridZ { get; }

    void SetGridIndicies(int X, int Z);
    void Place(Vector3 position);
}

/* Note, I'm making a bunch of assumptions here:
        - Blocks are 2x2 (Looks good).
        - Ground is flat so y is always zero.
        - Object anchor points for objects to place are all in their center.

    Could account for them, but do we care right now?
*/

public class Grid : MonoBehaviour
{

    [SerializeField] Vector3 bottomLeft;
    [SerializeField] Vector3 topRight;

    private int[,] grid; // 0 = Free | 1 = Occupied
    
    [SerializeField] MeshRenderer gridPreview;
    [SerializeField] MeshFilter meshFilter = default;

    [SerializeField] MeshRenderer placementPreview;
    [SerializeField] MeshFilter placementPreviewMeshFilter;

    private float blockSize = 2;

    // Precalculated cause these are used all over the place.
    private int m_NumBlocksWidth;
    private int m_NumBlocksHeight;
    private float m_StartX;
    private float m_StartZ;
    private float m_EndX;
    private float m_EndZ;

    public Vector3 Size { get { return topRight - bottomLeft; } }
    public Vector3 Center { get { return (bottomLeft + topRight) / 2; } }

    void Awake()
    {
        float width = topRight.x - bottomLeft.x;
        float height = topRight.z - bottomLeft.z;

        m_NumBlocksWidth = (int)(width / blockSize);
        m_NumBlocksHeight = (int)(height / blockSize);

        m_StartX = bottomLeft.x;
        m_StartZ = bottomLeft.z;

        // Adjust ends for trailing space
        m_EndX = topRight.x - width % blockSize;
        m_EndZ = topRight.z - height % blockSize;

        InitializeGrid();
        InitializeGridPreview();
    }

    public void ShowGrid()
    {
        gridPreview.enabled = true;
    }

    public void HideGrid()
    {
        gridPreview.enabled = false;
    }


    // Updates the grid to mark the given blocks as occupied.
    public void PlaceObjectOnGrid(Vector3 pos, IPlaceable toPlace)
    {
        GridIndex gridIndex = MapPositionToGridIndex(pos);
        Debug.Log($"Placed at [{gridIndex.X}: {gridIndex.Z}]");

        Vector3 centeredPosition = MapPositionToGrid(pos, toPlace.WidthInBlocks, toPlace.HeightInBlocks);
        toPlace.Place(centeredPosition);
        toPlace.SetGridIndicies(gridIndex.X, gridIndex.Z);

        for (int x = gridIndex.X; x < gridIndex.X + toPlace.WidthInBlocks; x++)
        {
            for (int z = gridIndex.Z; z < gridIndex.Z + toPlace.HeightInBlocks; z++)
            {
                MarkGridAsOccupied(new GridIndex(x, z));
            }
        }
    }

    [SerializeField]
    LayerMask gridObstaclesLayer = default;

    // Clears the grid given a position and an iplaceable
    // TODO there are no checks here - just moving fast.
    public void ClearGrid(Vector3 pos, int width, int height)
    {
        GridIndex gridIndex = MapPositionToGridIndex(pos);
        ClearArea(gridIndex, width, height);
        Debug.Log($"cleared [{gridIndex.X}: {gridIndex.Z}] -> {width} ^ {height}");
    }

    public void ClearGrid(int X, int Z, int width, int height)
    {
        GridIndex gridIndex = new GridIndex(X, Z);
        ClearArea(gridIndex, width, height);
    }

    // Whether or not the given object can be placed at the given position.
    public bool CanPlaceObjectOnGrid(Vector3 pos, IPlaceable toPlace)
    {
        GridIndex gridIndex = MapPositionToGridIndex(pos);
        bool isGridEmpty = IsGridEmpty(gridIndex, toPlace.WidthInBlocks, toPlace.HeightInBlocks);
        if (!isGridEmpty) return false;

        var center = MapPositionToGrid(pos, toPlace.WidthInBlocks, toPlace.HeightInBlocks);
        var halfExtents = new Vector3(toPlace.WidthInBlocks * blockSize * 0.5f, 2f, toPlace.HeightInBlocks * blockSize * 0.5f);
        bool isThereExternalObstacle = Physics.CheckBox(center, halfExtents, Quaternion.identity, gridObstaclesLayer);

        return !isThereExternalObstacle;
    }

    public bool CanPlaceObjectOnGrid(Vector3 pos, int width, int height)
    {
        GridIndex gridIndex = MapPositionToGridIndex(pos);
        bool isGridEmpty = IsGridEmpty(gridIndex, width, height);
        if (!isGridEmpty) return false;

        var center = MapPositionToGrid(pos, width, height);
        var halfExtents = new Vector3(width * blockSize * 0.5f, 2f, height * blockSize * 0.5f);
        bool isThereExternalObstacle = Physics.CheckBox(center, halfExtents, Quaternion.identity, gridObstaclesLayer);

        return !isThereExternalObstacle;
    }

    // Map the given position to the grid, centered from the anchor point
    public Vector3 MapPositionToGrid(Vector3 position, int widthInBlocks, int heightInBlocks)
    {
        GridPoint anchorPoint = MapToAnchorPointOnGrid(position);

        float offsetX = widthInBlocks * blockSize / 2f;
        float offsetZ = heightInBlocks * blockSize / 2f;
        return new Vector3(
            anchorPoint.X + offsetX, 
            0, 
            anchorPoint.Z + offsetZ
        );
    }

    public void ShowPlacementPreview(Vector3 position, int width, int height)
    {
        GridPoint blPoint = MapToAnchorPointOnGrid(position);
        
        // TODO Create red/green preview based on if you can build here or not.
        var mesh = new Mesh();
        var verticies = new List<Vector3>();
        var indicies = new List<int>();

        float x = blPoint.X;// - blockSize/2;
        float z = blPoint.Z;// - blockSize / 2;

        verticies.Add(new Vector3(x, 0, z));
        verticies.Add(new Vector3(x + width * blockSize, 0, z));
        verticies.Add(new Vector3(x + width * blockSize, 0, z + height * blockSize));
        verticies.Add(new Vector3(x, 0, z + height * blockSize));
        indicies.Add(0);
        indicies.Add(1);
        indicies.Add(2);
        indicies.Add(3);

        mesh.vertices = verticies.ToArray();
        mesh.SetIndices(indicies.ToArray(), MeshTopology.Quads, 0);
        placementPreviewMeshFilter.mesh = mesh;

        placementPreview.material = new Material(Shader.Find("Sprites/Default"));

        if (CanPlaceObjectOnGrid(position, width, height)) {
            placementPreview.material.color = Color.green;
        } else
        {
            placementPreview.material.color = Color.red;
        }
        placementPreview.enabled = true;
    }

    public void HidePlacePreview()
    {
        placementPreview.enabled = false;
    }

    // Returns given position mapped to anchor point on the grid (Bottom left block).
    private GridPoint MapToAnchorPointOnGrid(Vector3 position)
    {
        GridIndex gridIndex = MapPositionToGridIndex(position);
        return new GridPoint(
            bottomLeft.x + blockSize * gridIndex.X,
            bottomLeft.z + blockSize * gridIndex.Z
        );
    }

    // Returns index for grid array given position.
    private GridIndex MapPositionToGridIndex(Vector3 position)
    {
        return new GridIndex(
            Mathf.FloorToInt((position.x - m_StartX) / blockSize),
            Mathf.FloorToInt((position.z - m_StartZ) / blockSize)
        );
    }

    private bool IsGridEmpty(GridIndex startIndex, int width, int height)
    {
        int endIndexX = startIndex.X + width;
        int endIndexZ = startIndex.Z + height;
        if (
            startIndex.X < 0 ||
            startIndex.Z < 0 ||
            endIndexX > m_NumBlocksWidth ||
            endIndexZ > m_NumBlocksHeight
        ) return false;

        for (int x = startIndex.X; x < endIndexX; x++)
        {
            for (int z = startIndex.Z; z < endIndexZ; z++)
            {
                if (grid[x, z] != 0) return false;
            }
        }
        return true;
    }

    private void ClearArea(GridIndex startIndex, int width, int height)
    {
        for (int x = startIndex.X; x < startIndex.X + width; x++)
        {
            for (int z = startIndex.Z; z < startIndex.Z + height; z++)
            {
                grid[x, z] = 0;
            }
        }
    }

    private void MarkGridAsOccupied(GridIndex gridIndex)
    {
        grid[gridIndex.X, gridIndex.Z] = 1;
    }

    /* ===============================================================
                             Initialization
    =============================================================== */

    private void InitializeGrid()
    {
        int numBlocksWidth = (int)((topRight.x - bottomLeft.x) / blockSize);
        int numBlocksHeight = (int)((topRight.z - bottomLeft.z) / blockSize);

        grid = new int[numBlocksWidth, numBlocksHeight];
    }

    private void InitializeGridPreview()
    {
        var mesh = new Mesh();
        var verticies = new List<Vector3>();
        var indicies = new List<int>();

        int counter = 0;
        for (float i = 0; i <= m_NumBlocksWidth; i++)
        {
            float pos = m_StartX + i * blockSize;

            verticies.Add(new Vector3(pos, 0, m_StartZ));
            verticies.Add(new Vector3(pos, 0, m_EndZ));

            indicies.Add(counter++);
            indicies.Add(counter++);
        }

        for (int j = 0; j <= m_NumBlocksHeight; j++)
        {
            float pos = m_StartZ + j * blockSize;

            verticies.Add(new Vector3(m_StartX, 0, pos));
            verticies.Add(new Vector3(m_EndX, 0, pos));

            indicies.Add(counter++);
            indicies.Add(counter++);
        }

        mesh.vertices = verticies.ToArray();
        mesh.SetIndices(indicies.ToArray(), MeshTopology.Lines, 0);
        meshFilter.mesh = mesh;

        gridPreview.material = new Material(Shader.Find("Sprites/Default"));
        gridPreview.material.color = Color.white;
        HideGrid();
    }


    /* ===============================================================
                                    Gizmos
     =============================================================== */

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(bottomLeft, 0.3f);
        Gizmos.DrawSphere(topRight, 0.3f);

        Gizmos.color = Color.red;
        int numBlocksWidth = (int)((topRight.x - bottomLeft.x) / blockSize);
        int numBlocksHeight = (int)((topRight.z - bottomLeft.z) / blockSize);

        float startX = bottomLeft.x;
        float startZ = bottomLeft.z;

        for (int x = 0; x < numBlocksWidth; x++)
        {
            for (int z = 0; z < numBlocksHeight; z++)
            {
                if (grid != null && grid[x, z] == 1)
                {
                    Gizmos.DrawCube(new Vector3(startX + x * blockSize + blockSize / 2, 0, startZ + z * blockSize + blockSize / 2), Vector3.one * .9f);
                }
                else
                {
                    Gizmos.DrawWireCube(new Vector3(startX + x * blockSize + blockSize / 2, 0, startZ + z * blockSize + blockSize / 2), Vector3.one * .9f);
                }
            }
        }
    }

    /* ===============================================================
                            Domain Stuff
     =============================================================== */

    // Index in grid (int[,])
    struct GridIndex
    {
        public int X { get; }
        public int Z { get; }

        public GridIndex(int x, int z)
        {
            X = x;
            Z = z;
        }

    }


    struct GridPoint
    {
        public float X { get; }
        public float Z { get; }

        public GridPoint(float x, float z)
        {
            X = x;
            Z = z;
        }

    }

}

