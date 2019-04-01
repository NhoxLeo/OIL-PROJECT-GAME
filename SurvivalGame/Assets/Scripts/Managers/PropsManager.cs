using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsManager : MonoBehaviour {

    /// <summary>
    /// Checks if the buildings collides with any other buildings.
    /// </summary>
    /// <param name="placeableBuildingBounds"></param>
    /// <returns>The bounds of the collider.</returns>
    public bool CheckCollisions(Bounds placeableBuildingBounds) {
        foreach (Transform prop in transform) {
            if (prop.GetComponent<BoxCollider>())
                if (prop.GetComponent<BoxCollider>().bounds.Intersects(placeableBuildingBounds))
                    return false;
        }
        return true;
    }
}
