using UnityEngine;
using System;
using System.Collections;

public class Tile : MonoBehaviour
{
    [Header("Timers")]
    public float tileSwapSpeed = .4f;
    public float checkTilesTimer = .3f;

    [Header("Column stuff")]
    private int column;
    private int row;
    public int previousColumn;
    public int previousRow;
    
    private GameObject otherTile;

    public int targetX;
    public int targetY;

    private bool isMatched = false;
    public bool isSwitching = false;
    
    private Board board;

    private Vector2 startTouchPosition;
    private Vector2 finalTouchPosition;

    private Vector2 tilePosition;

    public float swipeAngle = 0;
    public float swipeDistance = 1f;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        targetX = Convert.ToInt32(transform.position.x);
        targetY = Convert.ToInt32(transform.position.y);
        column = targetX;
        row = targetY;
        
        previousColumn = column;
        previousRow = row;
    }

    // Update is called once per frame
    void Update()
    {
        FindMatches();
        if (isMatched == true)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = new Color(0f, 0f, 0f, .2f);
        }
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
        float ySwipeDistance = Mathf.Abs(finalTouchPosition.y - startTouchPosition.y);
        float xSwipeDistance = Mathf.Abs(finalTouchPosition.x - startTouchPosition.x);

        if (isSwitching == false && (xSwipeDistance > swipeDistance || ySwipeDistance > swipeDistance))
        {
            isSwitching = true;
            //finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
            CalculateSwipeDirection();
            StartCoroutine(CheckMovement());
            isSwitching = false;
        }
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - startTouchPosition.y, finalTouchPosition.x - startTouchPosition.x) * Mathf.Rad2Deg;
        //Debug.Log(swipeAngle);
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
            Debug.Log("Swiped out of board!");
    }
    
    public IEnumerator CheckMovement()
    {
        yield return new WaitForSeconds(checkTilesTimer);
        if (otherTile != null)
        {
            if (!isMatched && !otherTile.GetComponent<Tile>().isMatched)
            {
                otherTile.GetComponent<Tile>().row = row;
                otherTile.GetComponent<Tile>().column = column;
                row = previousRow;
                column = previousColumn;
            }
            otherTile = null;
        }

    }

    void FindMatches()
    {
        //Find matches on left and right
        if (column > 0 && column < board.boardWidth - 1)
        {
            GameObject leftTile = board.allBackgroundTiles[column - 1, row];
            GameObject rightTile = board.allBackgroundTiles[column + 1, row];
            if (leftTile != null && rightTile != null)
            {
                if (leftTile.tag == this.gameObject.tag && rightTile.tag == this.gameObject.tag)
                {
                    leftTile.GetComponent<Tile>().isMatched = true;
                    rightTile.GetComponent<Tile>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        //Find matches on up and down
        if (row > 0 && row < board.boardHeight - 1)
        {
            GameObject upperTile = board.allBackgroundTiles[column, row - 1];
            GameObject downTile = board.allBackgroundTiles[column, row + 1];
            if (upperTile != null && downTile != null)
            {
                if (upperTile.tag == this.gameObject.tag && downTile.tag == this.gameObject.tag)
                {
                    upperTile.GetComponent<Tile>().isMatched = true;
                    downTile.GetComponent<Tile>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
}
