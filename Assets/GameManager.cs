using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private Camera cam;
    private List<Vector3> cellCenters;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        cellCenters = new List<Vector3>
        {
            new Vector3(-2.0f,  2.0f, -10.0f),
            new Vector3( 0.0f,  2.0f, -10.0f),
            new Vector3( 2.0f,  2.0f, -10.0f),
            new Vector3(-2.0f,  0.0f, -10.0f),
            new Vector3( 0.0f,  0.0f, -10.0f),
            new Vector3( 2.0f,  0.0f, -10.0f),
            new Vector3(-2.0f, -2.0f, -10.0f),
            new Vector3( 0.0f, -2.0f, -10.0f),
            new Vector3( 2.0f, -2.0f, -10.0f)
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            int closestCell = GetClosestCell(clickPosition);
            Debug.Log("Closest cell: " + closestCell.ToString());
        }
    }

    /// <summary>
    /// Finds the cell center closest to the clickPosition.
    /// </summary>
    /// <param name="clickPosition">The position of the mouse click in World coordinates.</param>
    /// <returns>The id of the closest cell.</returns>
    int GetClosestCell(Vector3 clickPosition)
    {
        int currentCell = 0;
        float minDistance = 10000;
        int closestCell = 0;

        // Iterate over the list of cell center locations
        foreach (Vector3 cellCenter in cellCenters)
        {
            currentCell++;

            // Calculate the distance between the click and the cell center
            float distanceToCell = (clickPosition - cellCenter).sqrMagnitude;

            // Keep track of which cell is closest
            if (distanceToCell < minDistance)
            {
                closestCell = currentCell;
                minDistance = distanceToCell;
            }
        }

        return closestCell;
    }
}
