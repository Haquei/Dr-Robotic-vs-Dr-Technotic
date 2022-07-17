using UnityEngine;

// Little preview helper thing for when you're throwing out buildings.
public class PlacementPreview : MonoBehaviour
{
    [SerializeField] LineRenderer abilityLineRendererPrefab;
    [SerializeField] float abilityLineHeight = 3f;
    [SerializeField] int abilityLinePoints = 10;

    private LineRenderer abilityLineRenderer;

    void Awake()
    {
        abilityLineRenderer = Instantiate(abilityLineRendererPrefab, this.transform) as LineRenderer;
        abilityLineRenderer.enabled = false;
    }

    // Draws a parabola from start to end position
    public void DrawParabola(Vector3 startPosition, Vector3 endPosition)
    {

        // Determine positions along parabola
        Vector3[] positions = new Vector3[abilityLinePoints];
        for (int i = 0; i < abilityLinePoints; i++)
        {
            positions[i] = ParabolaHelper.LerpParabola(startPosition, endPosition, abilityLineHeight, (float)i / abilityLinePoints);
        }

        abilityLineRenderer.positionCount = abilityLinePoints;
        abilityLineRenderer.SetPositions(positions);
        abilityLineRenderer.enabled = true;
    }

    // Draws a line from start to end position
    public void DrawLine(Vector3 startPosition, Vector3 endPosition)
    {
        abilityLineRenderer.positionCount = 2;
        abilityLineRenderer.SetPositions(new Vector3[] { startPosition, endPosition });
        abilityLineRenderer.enabled = true;
    }

    // Clears preview
    public void ClearPreview()
    {
        abilityLineRenderer.enabled = false;
    }
}