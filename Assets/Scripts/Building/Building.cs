using UnityEngine;

public class Building : MonoBehaviour, IPlaceable
{
    [SerializeField] int widthInBlocks;
    [SerializeField] int heighInBlocks;

    public int WidthInBlocks => widthInBlocks;
    public int HeightInBlocks => heighInBlocks;

    public void Place(Vector3 position)
    {
        transform.position = position;
    }

}

