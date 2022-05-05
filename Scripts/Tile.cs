using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 finalTouchPosition;

    public float swipeAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - startTouchPosition.y, finalTouchPosition.x - startTouchPosition.x) * Mathf.Rad2Deg;
        Debug.Log(swipeAngle);
    }
}
