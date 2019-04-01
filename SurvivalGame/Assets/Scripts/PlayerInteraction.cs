using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using UnityEngine.AI;
using System;

/// <summary>
/// Handles the player's interaction with the world, ie placing/selecting buildings, selecting people etc.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
  GameObject currentPlaceableObject;
  private GameObject population;
  public GameObject buildingList, propList;
  private bool showsInformation = false;
  private GameObject clickedBuilding;
  private GameObject oldClickedBuilding;
  private Material[] oldMaterials;
  private bool isColliding;
  private Color[] defaultColors;

  [SerializeField]
  Color HighlightBuildingColor;

  private void Start()
  {
    population = GameObject.Find("Population");
    propList = GameObject.Find("Props");
  }

  void Update()
  {
    if (Input.GetMouseButtonDown(0))
      HandleWorldInteraction();
    HandleBuildingPlacement();
    if (currentPlaceableObject)
    {
      CheckCollisions();
      if (isColliding)
      {
        for (int i = 0; i < currentPlaceableObject.GetComponent<MeshRenderer>().materials.Length; i++)
          currentPlaceableObject.GetComponent<MeshRenderer>().materials[i].color = Color.red;
      }
      else
      {
        for (int i = 0; i < currentPlaceableObject.GetComponent<MeshRenderer>().materials.Length; i++)
          currentPlaceableObject.GetComponent<MeshRenderer>().materials[i].color = defaultColors[i];
      }
    }
  }

  /// <summary>
  /// Handles the interaction between the world and the mouse cursor.
  /// </summary>
  private void HandleWorldInteraction()
  {
    RaycastHit clickTarget;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    Physics.Raycast(ray, out clickTarget);
    if (clickTarget.collider)
    {
      if (clickTarget.collider.gameObject.layer != 5 && !EventSystem.current.IsPointerOverGameObject(-1))
      {
        // The player clicked on a placed building or worker - show its information.
        if (clickTarget.collider != null && clickTarget.collider.tag.Contains("Building") && currentPlaceableObject == null && clickTarget.collider.gameObject.name != "BuildingIdle")
        {
          if (clickedBuilding != null)
          {  // Closes the information box about the last clicked building/worker.
            clickedBuilding.GetComponent<Building>().ShowInformation(false);
            clickedBuilding = null;
          }
          // Opens information box about this building/worker.
          clickTarget.collider.gameObject.GetComponent<Building>().ShowInformation(true);
          showsInformation = true;
          clickedBuilding = clickTarget.collider.gameObject;
          DeHighlightBuilding(oldClickedBuilding);
          GameObject go = clickTarget.collider.gameObject;
          HighlightBuilding(go);
        }
        // The player clicked away an information box.
        else if ((clickTarget.collider == null || !clickTarget.collider.tag.Contains("Building") || !clickTarget.collider.tag.Contains("Human")) && showsInformation)
        {
          DeselectCurrentSelection();
          DeHighlightBuilding(oldClickedBuilding);
        }
      }
    }
  }

  private void HighlightBuilding(GameObject currentGob)
  {
    if (clickedBuilding != currentGob)
    {
      DeHighlightBuilding(clickedBuilding);
    }
    else
    {
      Renderer r = clickedBuilding.GetComponentInChildren<Renderer>();
      Material[] materials = r.materials;
      oldMaterials = materials;
      foreach (Material mat in materials)
      {
        //mat.color = Color.green;
        mat.color = HighlightBuildingColor;
      }
      oldClickedBuilding = clickedBuilding;
    }
  }

  private void DeHighlightBuilding(GameObject oldGob)
  {
    if (oldGob != null)
    {
      foreach (Material oldMat in oldMaterials)
      {
        oldMat.color = Color.white;
      }
    }
  }
  /// <summary>
  /// Handle building placement.
  /// </summary>
  private void HandleBuildingPlacement()
  {
    if (currentPlaceableObject != null)
    {
      UpdateCurrentPlaceableObjectToMouse();
      if (!isColliding)
      {
        if (Input.GetMouseButtonDown(0) && currentPlaceableObject.GetComponent<Building>().AttemptPurchase(currentPlaceableObject.gameObject, true))
        {
          currentPlaceableObject.layer = 0;
          currentPlaceableObject.GetComponent<NavMeshObstacle>().enabled = true;
          currentPlaceableObject.GetComponent<Building>().enabled = true;
          currentPlaceableObject.transform.parent = GameObject.Find("BuildingsUnderConstruction").transform;
          currentPlaceableObject.gameObject.AddComponent<Construction>();
          currentPlaceableObject = null;
          transform.GetComponentInParent<CameraMovement>().SwitchCameraZoom(true);
          buildingList.GetComponent<BuildingManager>().SwitchRaycast(true);
        }
      }
      else if (isColliding && Input.GetMouseButtonDown(0))
      {
        MessageDisplay.DisplayTooltip("No space to build here.", false);
      }
      if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Escape))
      {
        Destroy(currentPlaceableObject.gameObject);
        currentPlaceableObject = null;
        transform.GetComponentInParent<CameraMovement>().SwitchCameraZoom(true);
        buildingList.GetComponent<BuildingManager>().SwitchRaycast(true);
      }
    }
  }

  /// <summary>
  /// Initiates placement mode.
  /// </summary>
  /// <param name="building">Building to place</param>
  public void InitiateBuildingPlacement(GameObject buildingPrefab)
  {
    if (buildingPrefab.GetComponent<Building>().AttemptPurchase(buildingPrefab.gameObject, false))
    {
      SoundManager.PlaySound("buildingSelect");
      currentPlaceableObject = Instantiate(buildingPrefab);
      currentPlaceableObject.GetComponent<Building>().enabled = false;
      currentPlaceableObject.layer = 2;
      transform.GetComponentInParent<CameraMovement>().SwitchCameraZoom(false);
      buildingList.GetComponent<BuildingManager>().SwitchRaycast(false);
      defaultColors = new Color[currentPlaceableObject.GetComponent<MeshRenderer>().materials.Length];
      for (int i = 0; i < currentPlaceableObject.GetComponent<MeshRenderer>().materials.Length; i++)
        defaultColors[i] = currentPlaceableObject.GetComponent<MeshRenderer>().materials[i].color;
    }
  }

  /// <summary>
  /// Clamps the building to the mouse.
  /// </summary>
  private void UpdateCurrentPlaceableObjectToMouse()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit placementRay;
    if (Physics.Raycast(ray, out placementRay))
    {
      currentPlaceableObject.transform.position = placementRay.point;
      currentPlaceableObject.transform.position = new Vector3(Mathf.Round(currentPlaceableObject.transform.position.x), Mathf.Round(currentPlaceableObject.transform.position.y), Mathf.Round(currentPlaceableObject.transform.position.z));  // Free placement: remove Mathf.Round
                                                                                                                                                                                                                                              //currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, placementRay.normal);   // Turns on building following terrain.
      if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        currentPlaceableObject.transform.Rotate(0, 45, 0);
      else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        currentPlaceableObject.transform.Rotate(0, -45, 0);
    }
  }

  /// <summary>
  /// Checks if the building being placed is colliding.
  /// </summary>
  private void CheckCollisions()
  {
    if (buildingList.GetComponent<BuildingManager>().CheckCollisions(currentPlaceableObject.GetComponent<BoxCollider>().bounds) &&
        propList.GetComponent<PropsManager>().CheckCollisions(currentPlaceableObject.GetComponent<BoxCollider>().bounds))
      isColliding = false;
    else
      isColliding = true;
  }

  /// <summary>
  /// Returns the building the player is selecting.
  /// </summary>
  /// <returns></returns>
  public GameObject GetSelectedBuilding()
  {
    return clickedBuilding;
  }

  /// <summary>
  /// Deselects the selection.
  /// </summary>
  public void DeselectCurrentSelection()
  {
    if (showsInformation)
    {
      showsInformation = false;
      clickedBuilding.GetComponent<Building>().ShowInformation(false);
      clickedBuilding = null;
    }
  }
}
