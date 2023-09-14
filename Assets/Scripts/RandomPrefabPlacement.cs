using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabInfo
{
    public GameObject prefab;
    public int probability;
}

public class RandomPrefabPlacement : MonoBehaviour
{
    public List<PrefabInfo> prefabInfos; // List of different prefab types with associated probabilities.
    public int gridSizeX = 10; // Number of cells in the X direction.
    public int gridSizeY = 10; // Number of cells in the Y direction;
    public int maxPrefabCount = 80; // Maximum number of instances of all prefabs combined.
    public GameObject parent; // Parent for the spawned prefabs

    private ObjectPlacement2d objectPlacement2d; // Reference to ObjectPlacement2d script.
    private List<Vector3> occupiedCells = new List<Vector3>();

    private void Start()
    {
        objectPlacement2d = GetComponent<ObjectPlacement2d>();

        if (objectPlacement2d == null)
        {
            Debug.LogError("ObjectPlacement2d script not found on the same GameObject.");
            return;
        }

        // Place prefabs randomly.
        PlaceRandomPrefabs();
    }

    private void PlaceRandomPrefabs()
    {
        int totalProbability = 0;

        // Calculate the total probability.
        foreach (PrefabInfo info in prefabInfos)
        {
            totalProbability += info.probability;
        }

        for (int i = 0; i < maxPrefabCount; i++)
        {
            int randomValue = Random.Range(0, totalProbability);
            int accumulatedProbability = 0;

            foreach (PrefabInfo info in prefabInfos)
            {
                accumulatedProbability += info.probability;

                if (randomValue < accumulatedProbability)
                {
                    // Calculate the position based on the grid size.
                    int x = Random.Range(0, gridSizeX);
                    int y = Random.Range(0, gridSizeY);
                    Vector3 position = new Vector3(x, y, 0f);

                    // Check if the selected cell is already occupied.
                    if (!IsCellOccupied(position))
                    {
                        // Instantiate the selected prefab at the position and set the parent.
                        GameObject instantiatedPrefab = Instantiate(info.prefab, position, Quaternion.identity, parent.transform);

                        // Mark the cell as occupied.
                        occupiedCells.Add(position);

                        // Add the position to the ObjectPlacement2d script's occupiedCells list.
                        objectPlacement2d.AddOccupiedCell(position);
                    }

                    break;
                }
            }
        }
    }

    private bool IsCellOccupied(Vector3 cellPosition)
    {
        return occupiedCells.Contains(cellPosition);
    }
}
