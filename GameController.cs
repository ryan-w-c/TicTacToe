using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    //regular tictactoe
    public int turn;
    public int turnCounter;
    public int gridSize = 3;
    public int inARow = 3; //how many you need in a row to win
    public GameObject[] playerDisplay; //canvas element for each player inside game
    // "1" "2" "Won" "Draw" "WinDrawDisplay" "PlayerDisplay" 
    public Sprite[] playerIcons; // x and o 0=x 1=o
    public Button[] grid; //initializes the grid of buttons
    public int[] gridMarked; //identifies which player marks the button
    public int[] row = new int [5]; //declares an array for row change to bigger number if gridSize > 5
    public int[] col = new int[5];
    public int[] diag = new int[5];
    public int[] backDiag = new int[5];
    bool won = false;
    

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        turn = 1; //player1 starts
        turnCounter = 0; //counter is 0 at setup
        playerDisplay[0].SetActive(true); //1
        playerDisplay[1].SetActive(false);//2
        playerDisplay[2].SetActive(false);//won
        playerDisplay[3].SetActive(false);//draw
        playerDisplay[4].SetActive(false);//windrawdisplay
        playerDisplay[5].SetActive(true);//playerDisplay
        won = false;
        for(int i = 0; i < grid.Length; i++)
        {
            grid[i].interactable = true; //sets all buttons interactable
            grid[i].GetComponent<Image>().sprite = playerIcons[0]; //makes all buttons blank
            gridMarked[i] = 0;
        }
        //resets the row values so you can check for a winner if they are the same
        row[0] = -5;
        row[1] = -4;
        row[2] = -3;
        row[3] = -2;
        row[4] = -1;
        col[0] = -5;
        col[1] = -4;
        col[2] = -3;
        col[3] = -2;
        col[4] = -1;
        diag[0] = -5;
        diag[1] = -4;
        diag[2] = -3;
        diag[3] = -2;
        backDiag[4] = -1;
        backDiag[0] = -5;
        backDiag[1] = -4;
        backDiag[2] = -3;
        backDiag[3] = -2;
        backDiag[4] = -1;
    }

    public void TicTacToeButton(int ButtonNum)
    {
        //sets the Button to the image of the players marker
        System.Random random = new System.Random();
        int randomNumber = random.Next(0, 4);
        grid[ButtonNum].image.sprite = playerIcons[turn + randomNumber];
        grid[ButtonNum].interactable = false;
        //sets the button to have the same number as the player
        gridMarked[ButtonNum] = turn;
        //adds one to the turn counter
        turnCounter++;
        //checks for winner after 4 moves
        if (turnCounter > inARow + 1)//need to change this formula for bigger grids
        {
            //calls win functions
            bool isHorizontalWin = CheckWinHorizontal(ButtonNum);
            bool isVerticalWin = CheckWinVertical(ButtonNum);
            bool isDiagnolWin = CheckWinDiagnol(ButtonNum);
            bool isBackDiagnolWin = CheckWinBackDiagnol(ButtonNum);
            if (turnCounter == grid.Length)
            {
                if ((isHorizontalWin || isVerticalWin || isDiagnolWin || isBackDiagnolWin) == false)
                { 
                    DrawGame();
                }
            }

        }
        //shows the players turn only if no one won
        if (won == false)
        {
            if (turn == 1)
            {
                turn = 5;
                playerDisplay[0].SetActive(false);
                playerDisplay[1].SetActive(true);
            }
            else
            {
                turn = 1;
                playerDisplay[0].SetActive(true);
                playerDisplay[1].SetActive(false);
            }
        }

    }

    bool CheckWinHorizontal(int ButtonNum)
    {
        if (inARow == gridSize)
        {
            //gives which column the player marked
            int startingPlace = ButtonNum % inARow;
            //reasigns x as 0 for row array to check values
            int x = 0;
            //sets the row array element to gridMarked
            for (int i = 0 - startingPlace; i < inARow - startingPlace; i++)
            {
                row[x] = gridMarked[ButtonNum + i];
                x++;
            }
            //checks to see if 3 in a row are the same through row[]
            bool allTheSame = true;
            for (int i = 0; i < inARow; i++)
             {
                 if (row[i] != row[0])
                 {
                    allTheSame = false;
                 }
            }              
            if (allTheSame == true)
            {
                Win();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    bool CheckWinVertical(int ButtonNum)
    {
        if (inARow == gridSize)
        {
            //gives which column the player marked
            int startingPlace = ButtonNum % inARow;
            //reasigns x as 0 for row array to check values
            int x = 0;
            //sets the row array element to gridMarked
            for (int i = startingPlace; i < grid.Length; i+= gridSize)
            {
                col[x] = gridMarked[i];
                x++;
            }
            //checks to see if 3 in a row are the same through row[]
            bool allTheSame = true;
            for (int i = 0; i < inARow; i++)
            {
                if (col[i] != col[0])
                {
                    allTheSame = false;
                }
            }
            if (allTheSame == true)
            {
                Win();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    bool CheckWinDiagnol(int ButtonNum)
    {
        if (inARow == gridSize)
        {
            while (ButtonNum > gridSize)
            {
                ButtonNum-= gridSize-1;
            }
            if (ButtonNum != gridSize - 1)
            {
                return false;
            }
            else
            {
                //reasigns x as 0 for row array to check values
                int x = 0;
                //sets the row array element to gridMarked
                for (int i = ButtonNum; i <= ButtonNum * gridSize; i += ButtonNum)
                {
                    diag[x] = gridMarked[i];
                    x++;
                }
                //checks to see if 3 in a row are the same through row[]
                bool allTheSame = true;
                for (int i = 0; i < inARow; i++)
                {
                    if (diag[i] != diag[0])
                    {
                        allTheSame = false;
                    }
                }
                if (allTheSame == true)
                {
                    Win();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
    }

    bool CheckWinBackDiagnol(int ButtonNum)
    {
        if (inARow == gridSize)
        {
            while (ButtonNum > gridSize - 1)
            {
                ButtonNum -= (gridSize + 1);
            }
            if (ButtonNum != 0)
            {
                return false;
            }
            else
            {
                //reasigns x as 0 for row array to check values
                int x = 0;
                //sets the row array element to gridMarked
                for (int i = ButtonNum; i <= grid.Length; i += (gridSize + 1))
                {
                    backDiag[x] = gridMarked[i];
                    x++;
                }
                //checks to see if 3 in a row are the same through row[]
                bool allTheSame = true;
                for (int i = 0; i < inARow; i++)
                {
                    if (backDiag[i] != backDiag[0])
                    {
                        allTheSame = false;
                    }
                }
                if (allTheSame == true)
                {
                    Win();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
    }

    void Win()
    {
        //sets won to true and disables turn displays
        won = true;
        playerDisplay[4].SetActive(true);
        playerDisplay[2].SetActive(true);
    }

    void DrawGame()
    {
        won = true;
        playerDisplay[0].SetActive(false);
        playerDisplay[1].SetActive(false);
        playerDisplay[3].SetActive(true);
        playerDisplay[4].SetActive(true);
        playerDisplay[5].SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

}
