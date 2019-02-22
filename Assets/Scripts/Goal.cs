using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    GameController.GoalColour colourValue = GameController.GoalColour.None;

    public bool isCorrect = false;

    Crate crateIn;

    public void MoveMade(float squareSize)
    {
        RaycastHit2D ray = Physics2D.BoxCast(transform.position, new Vector2(squareSize / 2, squareSize / 2), 0, Vector2.zero);
        if(ray == false || ray.collider.tag == "Player")
        {
            crateIn = null;
            isCorrect = false;
        }
        else
        {
            crateIn = ray.collider.GetComponent<Crate>();
            if (crateIn != null)
            {
                if (colourValue == GameController.GoalColour.None || colourValue == crateIn.colourValue)
                {
                    isCorrect = true;
                }
                else
                {
                    isCorrect = false;
                }
            }
        }

    }
}
