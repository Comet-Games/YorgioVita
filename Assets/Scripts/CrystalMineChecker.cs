using UnityEngine;

public class CrystalMineChecker : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager script.

    private float gemIncrementTimer = 1f; // Timer for gem increment.

    private void Start()
    {
        // Find and store a reference to the GameManager script.
        gameManager = FindObjectOfType<GameManager>();

        // Check for neighbors when the object is created.
        CheckNeighbors();
    }

    private void Update()
    {
        // Count down the timer.
        gemIncrementTimer -= Time.deltaTime;

        // When the timer reaches zero or less, reset it and increment gems.
        if (gemIncrementTimer <= 0f)
        {
            gemIncrementTimer = 1f; // Reset the timer.

            // Increment "gems" for each neighboring "mine" object.
            IncrementGemsForNeighbors();
        }
    }

    private void CheckNeighbors()
    {
        // Get the current position of this object.
        Vector3 currentPosition = transform.position;

        // Define the positions of neighboring cells.
        Vector3[] neighborOffsets = new Vector3[]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right
        };

        // Loop through each neighboring cell position.
        foreach (Vector3 offset in neighborOffsets)
        {
            // Calculate the position of the neighbor cell.
            Vector3 neighborPosition = currentPosition + offset;

            // Check if there's a game object at the neighbor position.
            Collider2D[] colliders = Physics2D.OverlapPointAll(neighborPosition);

            foreach (Collider2D collider in colliders)
            {
                // Check if the neighboring game object has the specified script.
                if (collider.gameObject != gameObject && collider.GetComponent<CrystalMine>() != null)
                {
                    // Do something with the neighboring object that has the script.
                    Debug.Log("Found neighboring mine with script");

                    // Create and configure a Line Renderer for the neighbor.
                    CreateLineRenderer(neighborPosition);
                }
            }
        }
    }

    private void CreateLineRenderer(Vector3 neighborPosition)
    {
        // Create a new GameObject for the Line Renderer.
        GameObject lineObject = new GameObject("LineRenderer");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        // Set Line Renderer properties.
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, neighborPosition);
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Optional: Adjust Line Renderer properties as needed (color, material, etc.).
    }

    private void IncrementGemsForNeighbors()
    {
        // Get the current position of this object.
        Vector3 currentPosition = transform.position;

        // Define the positions of neighboring cells.
        Vector3[] neighborOffsets = new Vector3[]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right
        };

        // Loop through each neighboring cell position.
        foreach (Vector3 offset in neighborOffsets)
        {
            // Calculate the position of the neighbor cell.
            Vector3 neighborPosition = currentPosition + offset;

            // Check if there's a game object at the neighbor position.
            Collider2D[] colliders = Physics2D.OverlapPointAll(neighborPosition);

            foreach (Collider2D collider in colliders)
            {
                // Check if the neighboring game object has the specified script.
                if (collider.gameObject != gameObject && collider.GetComponent<CrystalMine>() != null)
                {
                    // Increment "gems" in the GameManager for each neighboring "mine" object.
                    gameManager.IncrementGems();
                }
            }
        }
    }
}
