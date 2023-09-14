using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPlacement2d : MonoBehaviour
{
    public GameObject[] hotbarPrefabs;
    public Button[] hotbarButtons;
    public float gridSizeX = 1f;
    public float gridSizeY = 1f;
    public bool deleting = false;

    private List<Vector3> occupiedCells = new List<Vector3>();
    private List<PlacedPrefabInfo> placedPrefabs = new List<PlacedPrefabInfo>();
    private List<Text> maxCountTexts = new List<Text>();
    private int[] maxCounts;
    private int selectedPrefabIndex = -1;

    private bool placementMode = true; // Variable to control placement mode.

    private void Start()
    {
        int prefabCount = hotbarPrefabs.Length;
        maxCounts = new int[prefabCount];

        for (int i = 0; i < hotbarButtons.Length; i++)
        {
            int index = i;
            hotbarButtons[i].onClick.AddListener(() => SelectPrefab(index));
            Text textComponent = hotbarButtons[i].GetComponentInChildren<Text>();
            maxCountTexts.Add(textComponent);
            UpdateMaxCountText(index);
        }

        for (int i = 0; i < prefabCount; i++)
        {
            maxCounts[i] = 0;
            UpdateMaxCountText(i);
        }

        maxCounts[2] = 1;
        UpdateMaxCountText(2);

        maxCounts[4] = 3;
        UpdateMaxCountText(4);
    }

    private void Update()
    {
        if (Input.GetButton("Circle") || Input.GetKey(KeyCode.D))
        {
            deleting = true;
        }
        else
        {
            deleting = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (deleting)
            {
                if (selectedPrefabIndex >= 0 && selectedPrefabIndex < hotbarPrefabs.Length)
                {
                    RemovePrefab();
                }
            }
            else if (selectedPrefabIndex >= 0 && !deleting)
            {
                if (placementMode)
                {
                    PlacePrefab();
                }
            }
        }
    }

    private void SelectPrefab(int index)
    {
        if (index >= 0 && index < hotbarPrefabs.Length)
        {
            selectedPrefabIndex = index;

            // Enter placement mode when selecting a prefab.
            placementMode = true;
        }
        else
        {
            selectedPrefabIndex = -1;
            placementMode = false; // Exit placement mode when no prefab is selected.
        }
    }

    private void PlacePrefab()
    {
        if (selectedPrefabIndex >= 0 && selectedPrefabIndex < hotbarPrefabs.Length)
        {
            if (maxCounts[selectedPrefabIndex] > 0)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 snappedPosition = new Vector3(
                    Mathf.Round(mousePosition.x / gridSizeX) * gridSizeX,
                    Mathf.Round(mousePosition.y / gridSizeY) * gridSizeY,
                    0f
                );

                if (!IsCellOccupied(snappedPosition))
                {
                    GameObject newPrefab = Instantiate(hotbarPrefabs[selectedPrefabIndex], snappedPosition, Quaternion.identity);

                    PlacedPrefabInfo prefabInfo = new PlacedPrefabInfo
                    {
                        prefab = newPrefab,
                        prefabIndex = selectedPrefabIndex
                    };
                    placedPrefabs.Add(prefabInfo);

                    maxCounts[selectedPrefabIndex]--;
                    UpdateMaxCountText(selectedPrefabIndex);

                    // Exit placement mode after placing a prefab.
                    placementMode = false;
                }
                else
                {
                    Debug.Log("Cannot place prefab on an occupied cell.");
                }
            }
            else
            {
                Debug.Log("Cannot place more of this prefab. Maximum count reached.");
            }
        }
    }

    private void RemovePrefab()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 snappedPosition = new Vector3(
            Mathf.Round(mousePosition.x / gridSizeX) * gridSizeX,
            Mathf.Round(mousePosition.y / gridSizeY) * gridSizeY,
            0f
        );

        PlacedPrefabInfo prefabInfoToRemove = placedPrefabs.Find(prefabInfo => prefabInfo.prefab.transform.position == snappedPosition);

        if (prefabInfoToRemove != null)
        {
            int prefabIndex = prefabInfoToRemove.prefabIndex;

            if(prefabIndex == 2)
            {
                return;
            }

            if (prefabIndex >= 0 && prefabIndex < hotbarPrefabs.Length)
            {
                maxCounts[prefabIndex]++;
                Debug.Log("Max count for prefab " + hotbarPrefabs[prefabIndex].name + " increased to " + maxCounts[prefabIndex]);

                Destroy(prefabInfoToRemove.prefab);

                placedPrefabs.Remove(prefabInfoToRemove);
                UpdateMaxCountText(prefabIndex);
            }
            else
            {
                Debug.LogWarning("Invalid prefab index: " + prefabIndex);
            }
        }
    }

    private void UpdateMaxCountText(int index)
    {
        if (index >= 0 && index < maxCountTexts.Count)
        {
            maxCountTexts[index].text = maxCounts[index].ToString();
            hotbarButtons[index].interactable = maxCounts[index] > 0;
        }
    }

    private bool IsCellOccupied(Vector3 cellPosition)
    {
        return occupiedCells.Contains(cellPosition) || placedPrefabs.Exists(prefabInfo => prefabInfo.prefab.transform.position == cellPosition);
    }

    private class PlacedPrefabInfo
    {
        public GameObject prefab;
        public int prefabIndex;
    }

    public void AddOccupiedCell(Vector3 cellPosition)
    {
        occupiedCells.Add(cellPosition);
    }
}
