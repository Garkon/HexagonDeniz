using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewManager : MonoBehaviour //General Game Manager Responsible for most of the game
{
    public enum State {WaitingInput,Calculating}
    State currentState = State.Calculating;//State enum for stopping input

    [Header("Size Adjustments"),Tooltip("Grid count on x")]
    public int xAmount = 8;

    [Tooltip("Grid count on y")]
    public int yAmount = 9;

    [Tooltip("Offsets Grid on screen by Y Axis")]
    public float gameAreaOffset = 0;

    [Tooltip("Instantiating Object")]
    [SerializeField] GameObject gridObject, gridGroupObject;

    [Tooltip("Color list for random colors")]
    public Color[] colors;

    [Tooltip("Used to keep all Slots in code")]
    public HexSlot[,] hexSlots;

    [Tooltip("Used to keep selected group"),HideInInspector]
    public HexGroup selectedHexGroup;

    [Tooltip("Singleton Instance")]
    public static NewManager instance;

    [Tooltip("Bounds to calculate game area")]
    Bounds bounds;

    [Tooltip("Used to keep all HexGroups in code")]
    List<HexGroup> hexGroups=new List<HexGroup>();

    [Tooltip("Box Collider for Raycasting in game area")]
    [SerializeField] BoxCollider coll;

    bool creatinglevel=true; //Simply used for not giving score in creating level

    [Tooltip("Icons used in game")]
    public Sprite normalIcon, BombIcon;

    int bombAmount=0; //Spawned bomb amount

    [Header("Score Variables")]
    int score=0;
    int highScore = 0;
    int moves=0;
    [SerializeField] Text scoreText, highScoreText, moveText;
    [SerializeField] GameObject losePanel;
    float swipeLength=25;

    void Start()
    {
        bounds = new Bounds(transform.position, Vector3.zero);
        if (instance == null) instance = this; 
        else Destroy(gameObject);
        if (PlayerPrefs.HasKey("hScore"))highScore= PlayerPrefs.GetInt("hScore");//Loading hScore if exists
        hexSlots = new HexSlot[xAmount, yAmount]; //Creating Array for HexSlots
        CreateGrid();
        coll.size = bounds.size; //Set collider size same with game area
        coll.gameObject.transform.position = bounds.center;
        currentState = State.WaitingInput; //Starts waiting input
        UpdateUI(); //Updating UI for game start
       
    }
    /// <summary>
    /// Creates Hexgrid by wanted sizes
    /// </summary>
    void CreateGrid()
    {
        for (int y = 0; y < yAmount; y++)
            for (int x = 0; x < xAmount; x++)
            {
                GameObject instedObject = Instantiate(gridObject, transform);
                instedObject.transform.localPosition = new Vector2(x * 0.45f, -(y * 0.52f + ((x % 2) * 0.26f))); 
                instedObject.transform.name = "Hex : " + x + " | " + y;
                hexSlots[x, y] = instedObject.GetComponent<HexSlot>();
                hexSlots[x, y].xCoordinate = x;
                hexSlots[x, y].yCoordinate = y;
            }
        RandomizeColors();
        AdjustCamera();
        CreateGridGroups();
       StartCoroutine( CheckColors());
    }
    /// <summary>
    /// Creates Hex Grid Groups for controls and checks
    /// </summary>
    void CreateGridGroups()
    {
        for (int x = 1; x < xAmount; x++)
        {
            for (int y = 0; y < yAmount; y++)
            {
                if (y + 1 <= yAmount - 1)
                {
                    if (x % 2 == 1)
                    {

                        GameObject instedGroup = Instantiate(gridGroupObject);
                        HexGroup instedComp = instedGroup.GetComponent<HexGroup>();
                        hexGroups.Add(instedComp);
                        instedComp.hexGroup[0] = hexSlots[x, y];
                        instedComp.hexGroup[1] = hexSlots[x - 1, y + 1];
                        instedComp.hexGroup[2] = hexSlots[x - 1, y];

                        instedGroup = Instantiate(gridGroupObject);
                        instedComp = instedGroup.GetComponent<HexGroup>();
                        hexGroups.Add(instedComp);
                        instedComp.hexGroup[0] = hexSlots[x, y];
                        instedComp.hexGroup[1] = hexSlots[x, y + 1];
                        instedComp.hexGroup[2] = hexSlots[x - 1, y + 1];


                    }
                    else
                    {
                        GameObject instedGroup = Instantiate(gridGroupObject);
                        HexGroup instedComp = instedGroup.GetComponent<HexGroup>();
                        hexGroups.Add(instedComp);
                        instedComp.hexGroup[0] = hexSlots[x, y];
                        instedComp.hexGroup[1] = hexSlots[x, y + 1];
                        instedComp.hexGroup[2] = hexSlots[x - 1, y];

                        instedGroup = Instantiate(gridGroupObject);
                        instedComp = instedGroup.GetComponent<HexGroup>();
                        hexGroups.Add(instedComp);
                        instedComp.hexGroup[0] = hexSlots[x, y + 1];
                        instedComp.hexGroup[1] = hexSlots[x - 1, y + 1];
                        instedComp.hexGroup[2] = hexSlots[x - 1, y];

                    }
                }

            }
        }
    }
    /// <summary>
    /// Randomizes colors of all grids in game
    /// </summary>
    void RandomizeColors()
    {
        foreach (var item in hexSlots)
        {
            item.SetColor(colors[Random.Range(0, colors.Length)]);
        }
    }
    /// <summary>
    /// AdjustsCamera for changed grid sizes
    /// </summary>
    void AdjustCamera()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(r.bounds);
        }
        Camera.main.transform.position = bounds.center + Vector3.back;
        if (bounds.size.x > bounds.size.y) Camera.main.orthographicSize = bounds.size.x * 4f / 5f;
        else Camera.main.orthographicSize = bounds.size.y * 4f / 5f;

    }
    void Update()
    {
        if (currentState==State.WaitingInput)
        {
            
            if (Input.touchCount == 1)
            {
                Touch t1 = Input.GetTouch(0);
                Debug.Log("DeltaPosition: " + t1.deltaPosition);
                if (t1.phase == TouchPhase.Moved &&selectedHexGroup!=null)
                {
                    Vector2 hexGroupPos = Camera.main.WorldToScreenPoint(selectedHexGroup.transform.position);
                    
                   // Debug.LogWarning ("HexGroupPos: "+hexGroupPos+"  ||  TouchPos:"+t1.position);

                    if (t1.deltaPosition.y> swipeLength && hexGroupPos.x<t1.position.x)
                    {
                        currentState = State.Calculating;
                        rotateRoutine = Rotate(false);
                        StartCoroutine(rotateRoutine);
                       
                    }
                    else if (t1.deltaPosition.y < -swipeLength && hexGroupPos.x < t1.position.x)
                    {
                        currentState = State.Calculating;
                        rotateRoutine = Rotate(true);
                        StartCoroutine(rotateRoutine);
                    }
                    if (t1.deltaPosition.y < -swipeLength && hexGroupPos.x > t1.position.x)
                    {
                        currentState = State.Calculating;
                        rotateRoutine = Rotate(false);
                        StartCoroutine(rotateRoutine);

                    }
                    else if (t1.deltaPosition.y > swipeLength && hexGroupPos.x > t1.position.x)
                    {
                        currentState = State.Calculating;
                        rotateRoutine = Rotate(true);
                        StartCoroutine(rotateRoutine);
                    }
                    else if (t1.deltaPosition.x > swipeLength && hexGroupPos.y < t1.position.y)
                    {
                        currentState = State.Calculating;
                        rotateRoutine = Rotate(true);
                        StartCoroutine(rotateRoutine);
                    }
                    else if (t1.deltaPosition.x < -swipeLength && hexGroupPos.y < t1.position.y)
                    {
                        currentState = State.Calculating;
                        rotateRoutine = Rotate(false);
                        StartCoroutine(rotateRoutine);
                    }
                    else if (t1.deltaPosition.x < -swipeLength && hexGroupPos.y > t1.position.y)
                    {
                        currentState = State.Calculating;
                        rotateRoutine = Rotate(true);
                        StartCoroutine(rotateRoutine);
                    }
                    else if (t1.deltaPosition.x > swipeLength && hexGroupPos.y > t1.position.y)
                    {
                        currentState = State.Calculating;
                        rotateRoutine = Rotate(false);
                        StartCoroutine(rotateRoutine);
                    }



                }
                else if (t1.phase==TouchPhase.Ended)
                {
                    Ray ray = Camera.main.ScreenPointToRay(t1.position);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        FindClosest(hit.point);
                    }
                }

            }
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    FindClosest(hit.point);
                }
            }
            if (Input.GetMouseButtonUp(1)) //Rotates clockwise for testing purposes
            {
                currentState = State.Calculating;
                rotateRoutine = Rotate(true);
                StartCoroutine(rotateRoutine);
            }
            if (Input.GetMouseButtonUp(2)) //Rotates counterclockwise for testing purposes
            {
                currentState = State.Calculating;
                rotateRoutine = Rotate(false);
                StartCoroutine(rotateRoutine);
            }
        }
        Camera.main.transform.position =new Vector3(bounds.center.x, bounds.center.y * gameAreaOffset , bounds.center.z) + Vector3.back;
    }
    IEnumerator rotateRoutine;
    /// <summary>
    /// Rotates selected hex grid group and checks for colors  
    /// </summary>
    /// <param name="clockwise">set true if wanted rotation is clockwise</param>
    /// <returns></returns>
    IEnumerator Rotate(bool clockwise)
    {
        if (selectedHexGroup!=null)
        {
            for (int i = 0; i < 3; i++)
            {
                if (clockwise) selectedHexGroup.RotateClockwise();
                else selectedHexGroup.RotateCounterClockwise();
                StartCoroutine(CheckColors());
                while(checking) yield return new WaitForSeconds(0.01f);
                yield return new WaitForSeconds(0.2f);
            }
            currentState = State.WaitingInput;
        }
        

    }
    /// <summary>
    /// Deactivates Selected hex group
    /// </summary>
    void DeselectHexGroup()
    {
        if (selectedHexGroup != null)
        {
            selectedHexGroup.Deactivate();
        }
    }
    /// <summary>
    /// Finds bombs if exist and calls countdown methods in them
    /// </summary>
    void FindBombs()
    {
        foreach (var item in hexSlots)
        {
            if (item.isBomb) item.BombCountdown();
        }
    }
    /// <summary>
    /// Finds closest hex grid group by given point in space
    /// </summary>
    /// <param name="point"></param>
    void FindClosest(Vector3 point)
    {
        DeselectHexGroup();
        float minDist = 9999f;
        for (int i = 0; i < hexGroups.Count; i++)
        {
            float dist = Vector3.Distance(hexGroups[i].transform.position, point);
            if (dist<minDist)
            {
                minDist = dist;
                selectedHexGroup = hexGroups[i];
            }
        }
        selectedHexGroup.Activate();
    }
    [HideInInspector]
    public bool matchOccured=false, whiteExist = false, checking = false;
    /// <summary>
    /// Check for color matches
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckColors()
    {
        matchOccured=false;
        checking = true;
        foreach (var item in hexGroups)
        {
            item.CheckThree();
        }
        do
        {
            yield return new WaitForSeconds(0.1f);
            whiteExist = false;
            bool whiteFound = false;
            foreach (var item in hexSlots)
            {
                if (item.myColor == Color.white)
                {
                    item.PullColorDown();
                    whiteExist = true;
                    whiteFound = true;
                    if (rotateRoutine!=null)
                    {
                        StopCoroutine(rotateRoutine);
                        currentState = State.WaitingInput;
                    }
                }

            }
            if (!whiteFound)
            {
                foreach (var item in hexGroups)
                {
                    item.CheckThree();
                }
            }
        } while (whiteExist);
        if (!CheckPossibleMoves())
        {
            LoseGame(false);
        }
       
        checking = false;
        creatinglevel = false;
        if (matchOccured) { FindBombs(); }
    }
    /// <summary>
    /// Check for possible moves
    /// </summary>
    /// <returns></returns>
    bool CheckPossibleMoves()
    {
        foreach (var item in hexSlots)
        {
            if (item.CheckPossibleMoves()) { return true; }
        }
        return false;
    }
    /// <summary>
    /// Increases Score and checks is score enough for another bomb
    /// </summary>
    public void AddScore()
    {
        if (!creatinglevel)
        {
            score += 15;
            moves++;
            if (score/300>bombAmount)
            {
                SpawnBomb();
            }
            if (score>=highScore)
            {
                highScore = score;
            }
            UpdateUI(); 
        }
    }
    /// <summary>
    /// Changes a grid to a bomb
    /// </summary>
    public void SpawnBomb()
    {
        while(true)
        {
            int randomIndex = Random.Range(0, xAmount);
            if (!hexSlots[randomIndex, 0].isBomb) { hexSlots[randomIndex, 0].SetBomb(7); bombAmount++; break; }
        }
    }
    /// <summary>
    /// Updates UI
    /// </summary>
    public void UpdateUI()
    {
        scoreText.text = ""+score;
        moveText.text = ""+moves;
        highScoreText.text = "Highscore: " + highScore;
    }
    /// <summary>
    /// Destroys all hex grid existing
    /// </summary>
    public void DestroyAll()
    {
        foreach (var item in hexSlots)
        {
            item.Destroy();
            item.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    /// <summary>
    /// Saves Highscore
    /// </summary>
    public void Save()
    {
        if (highScore == score)
        {
            PlayerPrefs.SetInt("hScore", highScore);
        }
    }
    /// <summary>
    /// Ends Game
    /// </summary>
    /// <param name="isBomb">Controls if game finished by bomb or no moves left</param>
    public void LoseGame(bool isBomb)
    {
        losePanel.SetActive(true);
        if (isBomb)
        {
            losePanel.GetComponentInChildren<Text>().text = "Bomb Detonated!";
        }
        DestroyAll();
        DeselectHexGroup();
        Save();
    }
}
