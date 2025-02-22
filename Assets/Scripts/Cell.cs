using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    // Indica si la celda es una bomba
    public bool isBomb = false;
    public bool isFlagged = false;
    public bool hasBombNearby = false;

    public SpriteRenderer cellSpriteRenderer;
    public Sprite hiddenSprite;
    public int posX, posY;
    public Board gameBoard;


    void Start()
    {
        cellSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Actualización por frame si es necesario
    }

    // Maneja el evento de clic del ratón
    public void OnPointerClick(PointerEventData eventData)
    {
        // Click izquierdo
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            gameBoard.FindNeighbors(gameObject);
        }
        // Click derecho
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Si no hay bandera ponerla
            if (cellSpriteRenderer.sprite == gameBoard.cellSprites[0])
            {
                gameBoard.flags--;
                isFlagged = !isFlagged;
                cellSpriteRenderer.sprite = gameBoard.cellSprites[2];

            }
            // Si hay bandera quitarla
            else if (cellSpriteRenderer.sprite == gameBoard.cellSprites[2])
            {
                gameBoard.flags++;
                isFlagged = !isFlagged;
                cellSpriteRenderer.sprite = gameBoard.cellSprites[0];
            }



        }
    }


    // Método para descubrir la celda
    public void UnCover()
    {
        cellSpriteRenderer.sprite = hiddenSprite;
    }

    public void Click()
    {
        Debug.Log("Clicked!!");
    }
}