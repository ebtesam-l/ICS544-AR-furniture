using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class FurniturePlacementManager : MonoBehaviour
{
    public GameObject SpawnableFurniture;
    public XROrigin sessionOrigin;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    [Header("Furniture Prefabs")]
    public GameObject whiteSofaPrefab;
    public GameObject lightBrownSofaPrefab;
    public GameObject brownSofaPrefab;

    [Header("UI")]
    public GameObject transformPanel;

    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private bool furniturePlaced = false;
    private bool moveMode = false;
    private GameObject placedObject = null;
    private float currentScale = 1f;
    private float currentRotationY = 0f;

    private void OnEnable() { EnhancedTouchSupport.Enable(); }
    private void OnDisable() { EnhancedTouchSupport.Disable(); }

    private void Update()
    {
        if (Touch.activeTouches.Count == 0) return;
        var touch = Touch.activeTouches[0];
        if (touch.phase != UnityEngine.InputSystem.TouchPhase.Began) return;
        Vector2 screenPos = touch.screenPosition;
        if (IsPointerOverUI(screenPos)) return;

        if (moveMode && furniturePlaced && placedObject != null)
        {
            if (raycastManager.Raycast(screenPos, raycastHits, TrackableType.PlaneWithinPolygon))
            {
                placedObject.transform.position = raycastHits[0].pose.position;
                LiftOffFloor(placedObject);
            }
            return;
        }

        if (furniturePlaced) return;
        if (SpawnableFurniture == null) return;

        if (raycastManager.Raycast(screenPos, raycastHits, TrackableType.PlaneWithinPolygon))
        {
            furniturePlaced = true;
            placedObject = Instantiate(SpawnableFurniture);
            placedObject.transform.position = raycastHits[0].pose.position;

            Vector3 camPos = Camera.main != null ? Camera.main.transform.position : Vector3.zero;
            Vector3 lookDir = placedObject.transform.position - camPos;
            lookDir.y = 0;
            if (lookDir != Vector3.zero)
                placedObject.transform.rotation = Quaternion.LookRotation(lookDir);

            currentRotationY = placedObject.transform.eulerAngles.y;
            currentScale = 1f;
            placedObject.transform.localScale = Vector3.one;
            LiftOffFloor(placedObject);

            foreach (var plane in planeManager.trackables)
                plane.gameObject.SetActive(false);
            planeManager.enabled = false;

            if (transformPanel != null) transformPanel.SetActive(true);
        }
    }

    private void LiftOffFloor(GameObject obj)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;
        float minY = float.MaxValue;
        foreach (var r in renderers)
            if (r.bounds.min.y < minY) minY = r.bounds.min.y;
        float offset = obj.transform.position.y - minY;
        obj.transform.position += new Vector3(0, offset, 0);
    }

    public void ScaleUp()
    {
        if (placedObject == null) return;
        currentScale = Mathf.Min(currentScale + 0.1f, 3f);
        placedObject.transform.localScale = Vector3.one * currentScale;
    }

    public void ScaleDown()
    {
        if (placedObject == null) return;
        currentScale = Mathf.Max(currentScale - 0.1f, 0.2f);
        placedObject.transform.localScale = Vector3.one * currentScale;
    }

    public void RotateLeft()
    {
        if (placedObject == null) return;
        currentRotationY -= 15f;
        placedObject.transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
    }

    public void RotateRight()
    {
        if (placedObject == null) return;
        currentRotationY += 15f;
        placedObject.transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
    }

    public void ToggleMove()
    {
        moveMode = !moveMode;
        if (moveMode)
        {
            planeManager.enabled = true;
            foreach (var plane in planeManager.trackables)
                plane.gameObject.SetActive(true);
        }
        else
        {
            foreach (var plane in planeManager.trackables)
                plane.gameObject.SetActive(false);
            planeManager.enabled = false;
        }
        Debug.Log("Move mode: " + moveMode);
    }

    public void SelectWhiteSofa()      { SpawnableFurniture = whiteSofaPrefab;      ResetPlacement(); }
    public void SelectLightBrownSofa() { SpawnableFurniture = lightBrownSofaPrefab; ResetPlacement(); }
    public void SelectBrownSofa()      { SpawnableFurniture = brownSofaPrefab;       ResetPlacement(); }

    private void ResetPlacement()
    {
        if (placedObject != null) { Destroy(placedObject); placedObject = null; }
        furniturePlaced = false;
        moveMode = false;
        currentScale = 1f;
        currentRotationY = 0f;
        if (transformPanel != null) transformPanel.SetActive(false);
        ReEnablePlanes();
    }

    private void ReEnablePlanes()
    {
        planeManager.enabled = true;
        foreach (var plane in planeManager.trackables)
            plane.gameObject.SetActive(true);
    }

    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        var eventData = new PointerEventData(EventSystem.current) { position = screenPosition };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
