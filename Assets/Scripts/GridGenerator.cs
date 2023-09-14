using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int gridSizeX = 10; // Number of cells in the X direction.
    public int gridSizeY = 10; // Number of cells in the Y direction.
    public float cellSize = 1.0f; // Size of each cell.
    public Material lineMaterial; // Material for the grid lines.

    private void Start()
    {
        // Calculate half grid size.
        float halfGridSizeX = ((gridSizeX - 1) * cellSize) / 2.0f;
        float halfGridSizeY = ((gridSizeY - 1) * cellSize) / 2.0f;

        // Calculate the position of the parent object.
        Vector3 parentPosition = new Vector3(100.0f - halfGridSizeX, 100.0f - halfGridSizeY, 0.0f);
        transform.position = parentPosition;

        // Generate horizontal grid lines.
        for (int y = 0; y <= gridSizeY; y++)
        {
            Vector3 startPoint = new Vector3(-halfGridSizeX, y * cellSize - halfGridSizeY, 0);
            Vector3 endPoint = new Vector3(halfGridSizeX, y * cellSize - halfGridSizeY, 0);
            CreateLine(startPoint, endPoint);
        }

        // Generate vertical grid lines.
        for (int x = 0; x <= gridSizeX; x++)
        {
            Vector3 startPoint = new Vector3(x * cellSize - halfGridSizeX, -halfGridSizeY, 0);
            Vector3 endPoint = new Vector3(x * cellSize - halfGridSizeX, halfGridSizeY, 0);
            CreateLine(startPoint, endPoint);
        }
    }

    private void CreateLine(Vector3 startPoint, Vector3 endPoint)
    {
        GameObject line = new GameObject("GridLine");
        line.transform.parent = transform; // Set the grid line as a child of the "Grid" GameObject.
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.05f; // Adjust line width as needed.
        lineRenderer.endWidth = 0.05f;
        lineRenderer.SetPositions(new Vector3[] { startPoint, endPoint });
    }
}
