using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject player;
    Animator anim;
    float squareSize = 1.28f;

    bool isMoving = false;
    bool isCrate = false;
    GameObject crate = null;

    int moveNumber = 0;

    class Move
    {
        public Vector2 direction;
        public bool isCrate;
        public GameObject crate;

        public Move(Vector2 _dir, bool _iC, GameObject _c)
        {
            direction = _dir;
            isCrate = _iC;
            crate = _c;
        }
    }

    List<Move> pastMoves = new List<Move>();

    Goal[] allGoals;
    public enum GoalColour
    {
        None,
        Brown,
        Red,
        Blue,
        Green,
        Grey
    }
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponent<Animator>();
        allGoals = FindObjectsOfType<Goal>();
    }

    public void Undo()
    {
        if (!isMoving)
        {
            if (moveNumber != 0)
            {
                Move undo = pastMoves[moveNumber - 1];
                if (undo.isCrate)
                {
                    undo.crate.transform.position += (Vector3)(-undo.direction * squareSize);
                }
                player.transform.position += (Vector3)(-undo.direction * squareSize);
                pastMoves.RemoveAt(moveNumber - 1);
                moveNumber--;
            }
        }
    }

    public void ChosenDirection(Vector2 direction)
    {
        if (direction == Vector2.zero || isMoving)
            return;


        RaycastHit2D ray = Physics2D.BoxCast(player.transform.position + (Vector3)(direction * squareSize), new Vector2(squareSize / 2, squareSize / 2), 0, Vector2.zero);
        
        if (ray.collider != null)
        {

            if (ray.collider.transform.tag == "Wall")
            {
                return;
            }
            else if (ray.collider.transform.tag == "Moveable")
            {                
                if(Physics2D.BoxCast(ray.collider.transform.position + (Vector3)(direction * squareSize), new Vector2(squareSize / 2, squareSize / 2), 0, Vector2.zero))
                {
                    return;
                }
                else
                {
                    StartCoroutine(MoveObject(ray.collider.gameObject, direction, false));
                    PlayerMove(direction);
                    isCrate = true;
                    crate = ray.collider.gameObject;

                }
            }
        }
        else
        {
            PlayerMove(direction);
        }

    }

    void PlayerMove(Vector2 direction)
    {
        if (direction == Vector2.up)
            anim.SetTrigger("Up");
        if (direction == Vector2.down)
            anim.SetTrigger("Down");
        if (direction == Vector2.left)
            anim.SetTrigger("Left");
        if (direction == Vector2.right)
            anim.SetTrigger("Right");

        StartCoroutine(MoveObject(player, direction, true));
    }

    IEnumerator MoveObject(GameObject gObject, Vector2 direction, bool isPlayer)
    {   
        if (isPlayer)  
            isMoving = true;
        Vector3 originalPos = gObject.transform.position;

        Vector3 directionPerStep = (direction * squareSize) * Time.fixedDeltaTime * 4;
        float t = 0;
        while (t < 0.25f)
        {
            t += Time.fixedDeltaTime;
            gObject.transform.position += directionPerStep;
            yield return new WaitForEndOfFrame();
        }

        gObject.transform.position = originalPos + (Vector3)(direction * squareSize);
        if(isPlayer)
            FinishMove(direction);

        yield break;
    }

    void FinishMove(Vector2 direction)
    {
        foreach (Goal g in allGoals)
        {
            g.MoveMade(squareSize);
        }

        if (IsAllTrue())
        {
            FindObjectOfType<LevelManagment>().LoadLevelComplete();
        }
        pastMoves.Add(new Move(direction, isCrate, crate));
        moveNumber++;

        isMoving = false;
        isCrate = false;
        crate = null;
    }

    bool IsAllTrue()
    {
        foreach (Goal g in allGoals)
        {
            if (g.isCorrect == false)
            {
                return false;
            }
        }
        return true;
    }
}
