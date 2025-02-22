using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static GameObject[,] cellGrid;
    public int width = 0;
    public int height = 0;
    public int totalBombs = 0;
    [SerializeField] public Sprite[] cellSprites;
    public GameManager gameManager;
    public SoundManager soundManager;
    private bool isFirstClick = true;
    private int numberOfUncoveredCells = 0;
    public GameObject explosion;
    public Sprite tileSheet;

    public int flags;


    void Start()
    {
        
        if ( PlayerPrefs.GetInt("LoadCustom") == 1 )
        {
            GameObject camera = GameObject.Find("Main Camera");
            
            width = PlayerPrefs.GetInt("CustomWidth", 10);
            height = PlayerPrefs.GetInt("CustomHeight", 10);
            totalBombs = PlayerPrefs.GetInt("CustomBombs", 10);
            if (width > height)
            {
                camera.GetComponent<Camera>().orthographicSize = width * 16 / 2;    
            }
            else
            {
                camera.GetComponent<Camera>().orthographicSize = height * 16 / 2 + 20;
            }

            
            PlayerPrefs.SetInt("LoadCustom", 0);
        }
       // Cargar todos los sprites desde el sprite sheet
        cellSprites = Resources.LoadAll<Sprite>("Sprites/" + (tileSheet.name.Length > 2 ? tileSheet.name.Substring(0, tileSheet.name.Length - 2) : tileSheet.name));

        cellGrid = CreateGrid(width, height);
        flags = totalBombs;
        
    }



    // Método para crear la cuadrícula del tablero
    public GameObject[,] CreateGrid(int width, int height)
    {
        GameObject[,] grid = new GameObject[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cellObject = new GameObject();
                cellObject.AddComponent<SpriteRenderer>().sprite = cellSprites[0];
                cellObject.AddComponent<BoxCollider2D>();

                Cell cell = cellObject.AddComponent<Cell>();
                cell.gameBoard = this;
                cell.posX = x;
                cell.posY = y;
                cell.hiddenSprite = cellSprites[1];

                cellObject.transform.position = new Vector3((x - (width / 2)) * 16, (y - (height / 2)) * 16, 0);
                grid[x, y] = cellObject;
            }
        }

        return grid;
    }

    // Método para añadir bombas al tablero
    public void AddBombs(int totalBombs, GameObject firstClickedCell)
    {
        int firstCellX = firstClickedCell.GetComponent<Cell>().posX;
        int firstCellY = firstClickedCell.GetComponent<Cell>().posY;

        for (int i = 0; i < totalBombs; i++)
        {
            while (true)
            {
                int randomX = Random.Range(0, width - 1);
                int randomY = Random.Range(0, height - 1);

                // Evitar que las bombas se coloquen alrededor de la primera celda clicada
                if (randomX >= firstCellX - 1 && randomX <= firstCellX + 1 && randomY >= firstCellY - 1 && randomY <= firstCellY + 1)
                {
                    continue;
                }

                if (!cellGrid[randomX, randomY].GetComponent<Cell>().isBomb)
                {
                    cellGrid[randomX, randomY].GetComponent<Cell>().isBomb = true;
                    cellGrid[randomX, randomY].GetComponent<Cell>().hiddenSprite = cellSprites[5];
                    break;
                }
            }
        }
    }

    // Método para encontrar y descubrir celdas vecinas
    public void FindNeighbors(GameObject clickedCell)
    {
        int cellX = clickedCell.GetComponent<Cell>().posX;
        int cellY = clickedCell.GetComponent<Cell>().posY;

        if (isFirstClick)
        {
            isFirstClick = false;
            AddBombs(totalBombs, clickedCell);
            foreach (var cell in cellGrid)
            {
                CountBombs(cell.GetComponent<Cell>());
            }
        }



        UncoverCell(cellX, cellY);

        if (numberOfUncoveredCells == width * height - totalBombs)
        {
            gameManager.Win();
        }
    }

    // Método para descubrir una celda
    private void UncoverCell(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return;

        Cell cell = cellGrid[x, y].GetComponent<Cell>();

        if (cell.cellSpriteRenderer.sprite != cell.hiddenSprite)
        {
            cell.UnCover();
            numberOfUncoveredCells++;

            if (cell.isBomb)
            {
                StartCoroutine(AnimateGameOver(cell));
                gameManager.GameOver();
                return;
            }

            if (!cell.hasBombNearby)
            {
                for (int lx = x - 1; lx <= x + 1; lx++)
                {
                    for (int ly = y - 1; ly <= y + 1; ly++)
                    {
                        if (lx == x && ly == y)
                            continue;

                        UncoverCell(lx, ly);
                    }
                }
            }
        }
    }

    // Método para contar las bombas alrededor de una celda
    public void CountBombs(Cell cell)
    {
        int cellX = cell.posX;
        int cellY = cell.posY;
        int bombCount = 0;

        for (int lx = cellX - 1; lx <= cellX + 1; lx++)
        {
            for (int ly = cellY - 1; ly <= cellY + 1; ly++)
            {
                if (lx > width - 1 || lx < 0 || ly < 0 || ly > height - 1)
                {
                    continue;
                }
                if (cellGrid[lx, ly].GetComponent<Cell>().isBomb)
                {
                    bombCount++;
                }
            }
        }

        if (bombCount > 0 && !cell.isBomb)
        {
            cell.hiddenSprite = cellSprites[bombCount + 7];
            cell.hasBombNearby = true;
        }
    }

    // Método recursivo para manejar la animación del final del juego
    private IEnumerator AnimateGameOver(Cell cell)
    {
        int cellX = cell.posX;
        int cellY = cell.posY;

        cell.cellSpriteRenderer.sprite = cellSprites[6];
        yield return new WaitForSeconds(0.5f);

        List<Cell> bombCells = new List<Cell>();

        // Recopilar todas las celdas con bombas
        for (int lx = 0; lx < width; lx++)
        {
            for (int ly = 0; ly < height; ly++)
            {
                Cell currentCell = cellGrid[lx, ly].GetComponent<Cell>();
                if (currentCell.isBomb && currentCell.cellSpriteRenderer.sprite != cellSprites[6])
                {
                    bombCells.Add(currentCell);
                }
            }
        }

        // Ordenar las celdas por distancia a la celda inicial
        bombCells.Sort((a, b) =>
        {
            float distanceA = Mathf.Sqrt(Mathf.Pow(a.posX - cellX, 2) + Mathf.Pow(a.posY - cellY, 2));
            float distanceB = Mathf.Sqrt(Mathf.Pow(b.posX - cellX, 2) + Mathf.Pow(b.posY - cellY, 2));
            return distanceA.CompareTo(distanceB);
        });

        // Animar las celdas en orden de distancia
        foreach (var bombCell in bombCells)
        {

            GameObject explosionInstance = Instantiate(explosion, new Vector3(
                bombCell.transform.position.x,
                bombCell.transform.position.y, 0),
                Quaternion.identity);

            bombCell.cellSpriteRenderer.sprite = cellSprites[6];
            soundManager.PlayExplosionSound();
            yield return new WaitForSeconds(0.3f);
            yield return new WaitForSeconds(0.35f); // Esperar unos segundos antes de eliminar la animación
            Destroy(explosionInstance); // Eliminar la animación después de que termine
        }
    }
}
