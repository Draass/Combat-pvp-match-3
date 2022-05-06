using UnityEngine;
using System;

public class Tile : MonoBehaviour
{
    public float tileSwapSpeed = .4f;

    private int column;
    private int row;
    private GameObject otherTile;

    public int targetX;
    public int targetY;

    private Board board;

    private Vector2 startTouchPosition;
    private Vector2 finalTouchPosition;

    private Vector2 tilePosition;

    public float swipeAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        targetX = Convert.ToInt32(transform.position.x);
        targetY = Convert.ToInt32(transform.position.y);
        column = targetX;
        row = targetY;
    }

    // Update is called once per frame
    void Update()
    {
        targetX = column;
        targetY = row;
        MoveTiles();
    }

    private void MoveTiles()
    {
        //Movement on x axis
        if (Math.Abs(targetX - transform.position.x) > .1)
        {
            //Move towards the target
            tilePosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tilePosition, tileSwapSpeed);
        }
        else
        {
            //Directly set the position
            tilePosition = new Vector2(targetX, transform.position.y);
            transform.position = tilePosition;
            board.allBackgroundTiles[column, row] = this.gameObject;
        }
        //Movement on y
        if (Math.Abs(targetY - transform.position.y) > .1)
        {
            //Move towards the target
            tilePosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tilePosition, tileSwapSpeed);
        }
        else
        {
            //Directly set the position
            tilePosition = new Vector2(transform.position.x, targetY);
            transform.position = tilePosition;
            board.allBackgroundTiles[column, row] = this.gameObject;
        }
    }

    private void OnMouseDown()
    {
        startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
        CalculateSwipeDirection();
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - startTouchPosition.y, finalTouchPosition.x - startTouchPosition.x) * Mathf.Rad2Deg;
        Debug.Log(swipeAngle);
    }

    void CalculateSwipeDirection()
    {

        if (swipeAngle > 45 && swipeAngle <= 135 && row < board.boardHeight)
        {
            //Up swipe
            otherTile = board.allBackgroundTiles[column, row + 1];
            otherTile.GetComponent<Tile>().row -= 1;
            row += 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Down swipe
            otherTile = board.allBackgroundTiles[column, row - 1];
            otherTile.GetComponent<Tile>().row += 1;
            row -= 1;
        }
        else if (swipeAngle > -45 && swipeAngle <= 45 && column < board.boardWidth)
        {
            //Right swipe
            otherTile = board.allBackgroundTiles[column + 1, row];
            otherTile.GetComponent<Tile>().column -= 1;
            column += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left swipe
            otherTile = board.allBackgroundTiles[column - 1, row];
            otherTile.GetComponent<Tile>().column += 1;
            column -= 1;
        }
        else
            Debug.LogError("Something is terribly wrong with mouse swipe angle direction calculation!");
    }
}
