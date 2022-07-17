
public class BuildingOrb : Projectile
{
    private Building building;
    private Grid grid;

    public void PassData(Building building, Grid grid)
    {
        this.building = building;
        this.grid = grid;
    }

    protected override void OnHitDestination()
    {
        Building toPlace = Instantiate(building);
        grid.PlaceObjectOnGrid(destination, toPlace);
        Destroy(gameObject);
    }
}
