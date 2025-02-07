using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    GameObject PrefabToInstantiate;

    [SerializeField]
    RectTransform UIDragElement;

    [SerializeField]
    RectTransform Canvas;

    [SerializeField]
    LayerMask clickableLayer;

    public bool pestItems;

    public Camera camera;

    private CameraMovementController cameraController;
    private Vector2 mOriginalLocalPointerPosition;
    private Vector3 mOriginalPanelLocalPosition;
    private Vector2 mOriginalPosition;

    private void Start()
    {
        mOriginalPosition = UIDragElement.localPosition;
        cameraController = Camera.main.GetComponent<CameraMovementController>();
    }

    public void OnBeginDrag(PointerEventData data)
    {
        mOriginalPanelLocalPosition = UIDragElement.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Canvas,
            data.position,
            data.pressEventCamera,
            out mOriginalLocalPointerPosition);

        // Disable camera movement when dragging starts
        if (cameraController != null)
        {
            cameraController.enabled = false;
        }
    }

    public void OnDrag(PointerEventData data)
    {
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Canvas,
            data.position,
            data.pressEventCamera,
            out localPointerPosition))
        {
            Vector3 offsetToOriginal =
                localPointerPosition -
                mOriginalLocalPointerPosition;
            UIDragElement.localPosition =
                mOriginalPanelLocalPosition +
                offsetToOriginal;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Enable camera movement when dragging ends
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        StartCoroutine(Coroutine_MoveUIElement(UIDragElement, mOriginalPosition, 0.5f));

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(
            Input.mousePosition);

        Debug.DrawRay(camera.transform.position, ray.direction * 10f, Color.red, 10f);

        if (Physics.Raycast(ray, out hit, 30f, clickableLayer))
        {
            if (pestItems)
            {
                Destroy(hit.collider.gameObject);
            }
            else
            {
                Debug.Log(hit.point);
                CreateObject(hit.point);
            }
        }
    }

    public IEnumerator Coroutine_MoveUIElement(RectTransform r, Vector2 targetPosition, float duration = 0.1f)
    {
        float elapsedTime = 0;
        Vector2 startingPos = r.localPosition;
        while (elapsedTime < duration)
        {
            r.localPosition =
                Vector2.Lerp(
                    startingPos,
                    targetPosition,
                    (elapsedTime / duration));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        r.localPosition = targetPosition;
    }

    public void CreateObject(Vector3 position)
    {
        if (PrefabToInstantiate == null)
        {
            Debug.Log("No prefab to instantiate");
            return;
        }

        if (PrefabToInstantiate.activeInHierarchy)
        {
            Debug.Log("cock");
            return;
        }

        Debug.Log("Work");
        PrefabToInstantiate.transform.position = position;
        PrefabToInstantiate.SetActive(true);
    }
}
