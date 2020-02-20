using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexSlot : MonoBehaviour //Hex Slot holds slot data and some methods for color and bomb controls
{
    public Color myColor = Color.white;
    public int xCoordinate, yCoordinate;
    [SerializeField] SpriteRenderer myImage;
    [SerializeField] SpriteRenderer selectHighlight;
    [SerializeField] ParticleSystem pcSystem;
    public bool isBomb=false;
    public int bombCounter;
    [SerializeField] TextMesh bombCounterText;
    /// <summary>
    /// Sets the color of the hex slot
    /// </summary>
    /// <param name="_color"></param>
    public void SetColor(Color _color)
    {
        myImage.enabled = true;
        myImage.color = _color;
        myColor = _color;
    }
    /// <summary>
    /// Deactivates bomb for this hex slot
    /// </summary>
    public void DeactivateBomb()
    {
        isBomb = false;
        bombCounterText.gameObject.SetActive(false);
        myImage.sprite = NewManager.instance.normalIcon;
    }
    /// <summary>
    /// Sets this hex to bomb by given move amount
    /// </summary>
    /// <param name="_bombCounter">Moves left before bomb explosion</param>
    public void SetBomb(int _bombCounter)
    {
        bombCounterText.text = "" + _bombCounter;
        bombCounterText.gameObject.SetActive(true); 
        isBomb = true;
        bombCounter = _bombCounter;
        myImage.sprite = NewManager.instance.BombIcon;
    }
    /// <summary>
    /// does the countdown of the bomb
    /// </summary>
    public void BombCountdown()
    {
        bombCounter--;
        if (bombCounter<=0)
        {
            NewManager.instance.LoseGame(true);
        }
        bombCounterText.text = "" + bombCounter;

    }
    /// <summary>
    /// Checks for possible Movements
    /// </summary>
    /// <returns></returns>
    public bool CheckPossibleMoves()
    {
        HexSlot[,] tempHexSlots = NewManager.instance.hexSlots;
        if (myColor == Color.white) return false;
        int xAmount = NewManager.instance.xAmount;
        int yAmount = NewManager.instance.yAmount;
        if (xCoordinate % 2 == 1)
        {
            if (xCoordinate - 1 >= 0)
            {
                if (tempHexSlots[xCoordinate - 1, yCoordinate].myColor == myColor)
                {
                    if (yCoordinate + 1 < yAmount) if (tempHexSlots[xCoordinate, yCoordinate + 1].myColor == myColor) return true;
                    if (yCoordinate + 2 < yAmount) if (tempHexSlots[xCoordinate - 1, yCoordinate + 2].myColor == myColor) return true;
                    if (xCoordinate + 1 < xAmount) if (tempHexSlots[xCoordinate + 1, yCoordinate].myColor == myColor) return true;
                    if (xCoordinate + 1 < xAmount && yCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate + 1, yCoordinate - 1].myColor == myColor) return true;
                }
            }
            if (xCoordinate - 1 >= 0 && yCoordinate + 1 < yAmount)
            {
                if (tempHexSlots[xCoordinate - 1, yCoordinate + 1].myColor == myColor)
                {
                    if (xCoordinate + 1 < xAmount && yCoordinate + 1 < yAmount) if (tempHexSlots[xCoordinate + 1, yCoordinate + 1].myColor == myColor) return true;
                    if (xCoordinate + 1 < xAmount && yCoordinate + 2 < yAmount) if (tempHexSlots[xCoordinate + 1, yCoordinate + 2].myColor == myColor) return true;
                    if (yCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate, yCoordinate - 1].myColor == myColor) return true;
                    if (xCoordinate - 1 >= 0 && yCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate - 1, yCoordinate - 1].myColor == myColor) return true;
                }
            }
            if (yCoordinate - 1 >= 0)
            {
                if (tempHexSlots[xCoordinate, yCoordinate - 1].myColor == myColor)
                {
                    if (xCoordinate + 1 < xAmount && yCoordinate + 1 < yAmount) if (tempHexSlots[xCoordinate + 1, yCoordinate + 1].myColor == myColor) return true;
                    if (xCoordinate + 2 < xAmount) if (tempHexSlots[xCoordinate + 2, yCoordinate].myColor == myColor) return true;
                    if (xCoordinate - 1 >= 0 && yCoordinate + 1 < yAmount) if (tempHexSlots[xCoordinate - 1, yCoordinate + 1].myColor == myColor) return true;
                    if (xCoordinate - 2 >= 0) if (tempHexSlots[xCoordinate - 2, yCoordinate].myColor == myColor) return true;
                }
            }
            if (yCoordinate + 1 < yAmount)
            {
                if (tempHexSlots[xCoordinate, yCoordinate + 1].myColor == myColor)
                {
                    if (xCoordinate + 1 < xAmount) if (tempHexSlots[xCoordinate + 1, yCoordinate].myColor == myColor) return true;
                    if (xCoordinate + 2 < xAmount) if (tempHexSlots[xCoordinate + 2, yCoordinate].myColor == myColor) return true;
                    if (xCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate - 1, yCoordinate].myColor == myColor) return true;
                    if (xCoordinate - 2 >= 0) if (tempHexSlots[xCoordinate - 2, yCoordinate].myColor == myColor) return true;
                }

            }
            if (xCoordinate + 1 < xAmount)
            {
                if (tempHexSlots[xCoordinate + 1, yCoordinate].myColor == myColor)
                {
                    if (yCoordinate + 1 < yAmount) if (tempHexSlots[xCoordinate, yCoordinate + 1].myColor == myColor) return true;
                    if (xCoordinate + 1 < xAmount && yCoordinate + 2 < yAmount) if (tempHexSlots[xCoordinate + 1, yCoordinate + 2].myColor == myColor) return true;
                    if (xCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate - 1, yCoordinate].myColor == myColor) return true;
                    if (xCoordinate - 1 >= 0 && yCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate - 1, yCoordinate - 1].myColor == myColor) return true;
                }
            }
            if (xCoordinate + 1 < xAmount && yCoordinate + 1 < yAmount)
            {
                if (tempHexSlots[xCoordinate + 1, yCoordinate + 1].myColor == myColor)
                {
                    if (xCoordinate - 1 >= 0 && yCoordinate + 1 < yAmount) if (tempHexSlots[xCoordinate - 1, yCoordinate + 1].myColor == myColor) return true;
                    if (xCoordinate - 1 >= 0 && yCoordinate + 2 < yAmount) if (tempHexSlots[xCoordinate - 1, yCoordinate + 2].myColor == myColor) return true;
                    if (yCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate, yCoordinate - 1].myColor == myColor) return true;
                    if (xCoordinate + 1 < xAmount && yCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate + 1, yCoordinate - 1].myColor == myColor) return true;
                }
            }

        }
        else
        {
            if (xCoordinate - 1 >= 0 && yCoordinate - 1 >= 0)
            {
                if (tempHexSlots[xCoordinate - 1, yCoordinate - 1].myColor == myColor)
                {
                    if (yCoordinate + 1 < yAmount) if (tempHexSlots[xCoordinate, yCoordinate + 1].myColor == myColor) return true;
                    if (yCoordinate + 1 < yAmount) if (tempHexSlots[xCoordinate - 1, yCoordinate + 1].myColor == myColor) return true;
                    if (xCoordinate + 1 < xAmount && yCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate + 1, yCoordinate - 1].myColor == myColor) return true;
                    if (xCoordinate + 1 < xAmount && yCoordinate - 2 >= 0) if (tempHexSlots[xCoordinate + 1, yCoordinate - 2].myColor == myColor) return true;
                }
            }
            if (xCoordinate - 1 >= 0)
            {
                if (tempHexSlots[xCoordinate - 1, yCoordinate].myColor == myColor)
                {
                    if (xCoordinate + 1 < xAmount) if (tempHexSlots[xCoordinate + 1, yCoordinate].myColor == myColor) return true;
                    if (xCoordinate + 1 < xAmount && yCoordinate + 1 < yAmount) if (tempHexSlots[xCoordinate + 1, yCoordinate + 1].myColor == myColor) return true;
                    if (yCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate, yCoordinate - 1].myColor == myColor) return true;
                    if (xCoordinate - 1 >= 0 && yCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate - 1, yCoordinate - 1].myColor == myColor) return true;
                }
            }
            if (yCoordinate - 1 >= 0)
            {
                if (tempHexSlots[xCoordinate, yCoordinate - 1].myColor == myColor)
                {
                    if (xCoordinate + 1 < xAmount) if (tempHexSlots[xCoordinate + 1, yCoordinate].myColor == myColor) return true;
                    if (xCoordinate + 2 < xAmount) if (tempHexSlots[xCoordinate + 2, yCoordinate].myColor == myColor) return true;
                    if (xCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate - 1, yCoordinate].myColor == myColor) return true;
                    if (xCoordinate - 2 >= 0) if (tempHexSlots[xCoordinate - 2, yCoordinate].myColor == myColor) return true;
                }
            }
            if (yCoordinate + 1 < NewManager.instance.yAmount)
            {
                if (tempHexSlots[xCoordinate, yCoordinate + 1].myColor == myColor)
                {
                    if (xCoordinate + 1 < xAmount && yCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate + 1, yCoordinate - 1].myColor == myColor) return true;
                    if (xCoordinate + 2 < xAmount) if (tempHexSlots[xCoordinate + 2, yCoordinate].myColor == myColor) return true;
                    if (xCoordinate - 1 >= 0 && yCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate - 1, yCoordinate - 1].myColor == myColor) return true;
                    if (xCoordinate - 2 >= 0) if (tempHexSlots[xCoordinate - 2, yCoordinate].myColor == myColor) return true;
                }
            }
            if (xCoordinate + 1 < NewManager.instance.xAmount && yCoordinate - 1 >= 0)
            {
                if (tempHexSlots[xCoordinate + 1, yCoordinate - 1].myColor == myColor)
                {
                    if (yCoordinate + 1 < yAmount) if (tempHexSlots[xCoordinate, yCoordinate + 1].myColor == myColor) return true;
                    if (xCoordinate + 1 < xAmount && yCoordinate + 1 < yAmount) if (tempHexSlots[xCoordinate + 1, yCoordinate + 1].myColor == myColor) return true;
                    if (xCoordinate - 1 >= 0 && yCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate - 1, yCoordinate - 1].myColor == myColor) return true;
                    if (xCoordinate - 1 >= 0 && yCoordinate - 2 >= 0) if (tempHexSlots[xCoordinate - 1, yCoordinate - 2].myColor == myColor) return true;
                }
            }
            if (xCoordinate + 1 < NewManager.instance.xAmount)
            {
                if (tempHexSlots[xCoordinate + 1, yCoordinate].myColor == myColor)
                {
                    if (xCoordinate - 1 >= 0 ) if (tempHexSlots[xCoordinate - 1, yCoordinate].myColor == myColor) return true;
                    if (xCoordinate - 1 >= 0 && yCoordinate + 1 < yAmount) if (tempHexSlots[xCoordinate - 1, yCoordinate + 1].myColor == myColor) return true;
                    if (yCoordinate - 1 >= 0) if (tempHexSlots[xCoordinate, yCoordinate - 1].myColor == myColor) return true;
                    if (xCoordinate + 1 < xAmount && yCoordinate - 2 >= 0) if (tempHexSlots[xCoordinate + 1, yCoordinate - 2].myColor == myColor) return true;
                }
            }
        }

        return false;
    }
    /// <summary>
    /// Destroys hex slot (takes away the color and the bomb)
    /// </summary>
    public void Destroy()
    {
        NewManager.instance.matchOccured = true;
        myImage.enabled = true;
        myImage.color = Color.white;
        myColor = Color.white;
        pcSystem.gameObject.SetActive(true);
        pcSystem.Play();
        if (isBomb)
        {
            DeactivateBomb();
        }
    }
    /// <summary>
    /// if the hex slot is white pulls the color on top of it
    /// </summary>
    public void PullColorDown()
    {
        if (yCoordinate == 0)
        {
            SetColor(NewManager.instance.colors[Random.Range(0, NewManager.instance.colors.Length)]);
            return;
        }
        for (int i = 0; i < yCoordinate; i++)
        {
            HexSlot tempSlot = NewManager.instance.hexSlots[xCoordinate, (yCoordinate - i - 1)];
            if (tempSlot.myColor != Color.white) {
                if (tempSlot.isBomb) { SetBomb(tempSlot.bombCounter);tempSlot.DeactivateBomb(); }
                SetColor(tempSlot.myColor); tempSlot.SetColor(Color.white); break;
            }

        }
    }
    /// <summary>
    /// Turns on higlighting
    /// </summary>
    public void SelectSlot()
    {
        selectHighlight.enabled = true;
    }
    /// <summary>
    /// Turns off higlighting
    /// </summary>
    public void DeselectSlot()
    {
        selectHighlight.enabled = false;
    }
}
