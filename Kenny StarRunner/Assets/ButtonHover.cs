using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (hoverMenu is not null)
            hoverMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hovering");
        if (hoverMenu is not null)
            hoverMenu.SetActive(true);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("No longer hovering");
        if (hoverMenu is not null)
            hoverMenu.SetActive(false);
    }
}
