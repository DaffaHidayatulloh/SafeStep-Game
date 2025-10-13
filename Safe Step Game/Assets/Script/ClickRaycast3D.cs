using UnityEngine;
using UnityEngine.InputSystem;

public class ClickRaycast3D : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    Camera cam;

    HomeScreenManager homeScreenManager;

    public GameObject[] levelTriggers;

    [SerializeField] private InputAction press, screenPos;

    private Vector3 cursorToScreenPos;

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray = cam.ScreenPointToRay(cursorToScreenPos);

        // also check if it does not confluict with UI click
        if (press.WasPressedThisFrame() && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Click at " + hit.transform.name);
                for (int i = 0; i < levelTriggers.Length; i++)
                {
                    if (hit.transform.name == levelTriggers[i].name)
                    {
                        AudioManager.instance.PlaySFX(2); // suara klik
                                                          // homeScreenManager.LoadLevel(i + 1);
                        homeScreenManager.GoToLevel1();
                    }
                }
                
            }
            
        }
    }

    void Awake()
    {
        press.Enable();
        screenPos.Enable();
        screenPos.performed += context => { cursorToScreenPos = context.ReadValue<Vector2>(); };

        homeScreenManager = FindFirstObjectByType<HomeScreenManager>();
    }
}
