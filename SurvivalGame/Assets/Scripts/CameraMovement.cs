using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles camera movement and navigation across the map.
/// </summary>
public class CameraMovement : MonoBehaviour {
    private float maxCameraZoom, maxCameraDistance, groundLevel, panCamera;
    private bool cameraZoomEnabled;

    void Start() {
        groundLevel = transform.position.y;
        maxCameraDistance = groundLevel + 11f;
        maxCameraZoom = groundLevel - 11f;
        panCamera = maxCameraZoom + 5;
        cameraZoomEnabled = true;
        // TODO: Set camera position to oilrig's origin
    }

    /// <summary>
    /// Updates camera position depending on its target.
    /// </summary>
    void Update () {
        // Map navigation
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * 0.28f);
        }
        if (Input.GetKey(KeyCode.S) ||Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * 0.28f);
        }
        if (Input.GetKey(KeyCode.A) ||Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * 0.28f);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * 0.28f);
        }
        // Camera rotation
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, 0.7f);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.down, 0.7f);
        }

        // Zoom in/out
        if (Input.GetAxis("Mouse ScrollWheel") != 0 && cameraZoomEnabled) {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            if (!EventSystem.current.IsPointerOverGameObject(-1) || results[0].gameObject.layer != 5) {
                if (Input.GetAxis("Mouse ScrollWheel") < 0f && transform.position.y < maxCameraDistance) {  // Scroll DOWN
                    transform.Translate(Vector3.up);
                    //if (transform.position.y < panCamera)
                    //    transform.Rotate(Vector3.right, 4);
                }
                if (Input.GetAxis("Mouse ScrollWheel") > 0f && transform.position.y > maxCameraZoom) {  // Scroll UP
                    transform.Translate(Vector3.down);
                    //if (transform.position.y < panCamera)
                    //    transform.Rotate(Vector3.right, -4);
                }
            }
        }       
    }

    /// <summary>
    /// Switches camera zooming on/off.
    /// </summary>
    /// <param name="enabled">Zoom on if enabled == true.</param>
    public void SwitchCameraZoom(bool enabled) {
        cameraZoomEnabled = enabled;
    }
}