using UnityEngine;

public class Building : MonoBehaviour, IPlaceable
{
    [SerializeField] int widthInBlocks;
    [SerializeField] int heighInBlocks;

    public int WidthInBlocks => widthInBlocks;
    public int HeightInBlocks => heighInBlocks;

    private int gridX;
    private int gridZ;

    public int GridX => gridX;
    public int GridZ => gridZ;

    public void SetGridIndicies(int x, int z)
    {
        gridX = x;
        gridZ = z;
    }

    public void Place(Vector3 position)
    {
        transform.position = position;
    }

}

