using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGroup : MonoBehaviour //Hex Group holds 3 Hexgrids and has some methods for group control and checks
{
    public HexSlot[] hexGroup = new HexSlot[3];
    private void Start()
    {
        Vector3 offset = Vector3.Lerp(hexGroup[0].transform.position, hexGroup[1].transform.position, 0.4f);
        offset = Vector3.Lerp(offset, hexGroup[2].transform.position, 0.4f);
        transform.position = offset;

    }
    /// <summary>
    /// Checks group for color matches
    /// </summary>
    public void CheckThree()
    {
        if (hexGroup[0].myColor == hexGroup[1].myColor && hexGroup[1].myColor == hexGroup[2].myColor)
        {
            for (int i = 0; i < 3; i++)
            {
                hexGroup[i].Destroy();
                NewManager.instance.whiteExist = true;
            }
            NewManager.instance.AddScore();
        }
    }
    /// <summary>
    /// Rotates group clockwise
    /// </summary>
    public void RotateClockwise()
    {
        Color[] tempHexSlotColors=new Color[3];
        for (int i = 0; i < 3; i++) tempHexSlotColors[i] = hexGroup[i].myColor;
        hexGroup[0].SetColor(tempHexSlotColors[2]);
        hexGroup[1].SetColor(tempHexSlotColors[0]);
        hexGroup[2].SetColor(tempHexSlotColors[1]);
        if (hexGroup[0].isBomb)
        {
            hexGroup[1].SetBomb(hexGroup[0].bombCounter);
            hexGroup[0].DeactivateBomb();
        }
        else if (hexGroup[1].isBomb)
        {
            hexGroup[2].SetBomb(hexGroup[1].bombCounter);
            hexGroup[1].DeactivateBomb();
        }
        else if (hexGroup[2].isBomb)
        {
            hexGroup[0].SetBomb(hexGroup[2].bombCounter);
            hexGroup[2].DeactivateBomb();
        }
    }
    /// <summary>
    /// Rotates group counterclockwise
    /// </summary>
    public void RotateCounterClockwise()
    {
        Color[] tempHexSlotColors = new Color[3];
        for (int i = 0; i < 3; i++) tempHexSlotColors[i] = hexGroup[i].myColor;
        hexGroup[0].SetColor(tempHexSlotColors[1]);
        hexGroup[1].SetColor(tempHexSlotColors[2]); 
        hexGroup[2].SetColor(tempHexSlotColors[0]);
        if (hexGroup[0].isBomb)
        {
            hexGroup[2].SetBomb(hexGroup[0].bombCounter);
            hexGroup[0].DeactivateBomb();
        }
        else if (hexGroup[1].isBomb)
        {
            hexGroup[0].SetBomb(hexGroup[1].bombCounter);
            hexGroup[1].DeactivateBomb();
        }
        else if (hexGroup[2].isBomb)
        {
            hexGroup[1].SetBomb(hexGroup[2].bombCounter);
            hexGroup[2].DeactivateBomb();
        }
    }
    /// <summary>
    /// Highlights slot group
    /// </summary>
    public void Activate()
    {
        for (int i = 0; i < 3; i++)
        {
            hexGroup[i].SelectSlot();
        }
    }
    /// <summary>
    /// Turns off highlighting for slot group
    /// </summary>
    public void Deactivate()
    {
        for (int i = 0; i < 3; i++)
        {
            hexGroup[i].DeselectSlot();
        }
    }

}
