using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Mirror;

[Serializable]
public class AllCard
{
    public List<GameObject> prefab;
}


public class board : NetworkBehaviour
{
    public static board Instance { get; set; }
    public CardsType _type;

    [SerializeField] public Transform spacePos;
    private List<GameObject> objs = new List<GameObject>();
    [Header("Art stuff")]
    [SerializeField] private Material tileMaterial;
    [SerializeField] private Material hoverTiletileMaterial;
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private Vector3 boardCenter = Vector3.zero;
    [SerializeField] private Material _movementGrid;
    [SerializeField] private Material _attackRange;
    [SerializeField] private Material _hoverSpace;
    private Material spaceMaterial;

    [Header("Prefabs")]
    public List<AllCard> cards = new List<AllCard>();
    

    //LOGIC
    private const int TILE_Conut_X = 15;
    private const int TILE_Conut_Y = 7;
    public GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover;
    private Vector3 bounds;
    private List<Vector2Int> availableMoves = new List<Vector2Int>();
    private GameObject pervoiusObj;
    private GameObject objCard;
    private cursorController curCard;
    
    [HideInInspector]public bool spawnMonster;
    public int idCard;
    [HideInInspector] public bool getHighlist;
    public bool isHitTile { get; set; }
    Cards _cards;

    private void Awake()
    {
        GenerateAllTiles(tileSize, TILE_Conut_X, TILE_Conut_Y);
        //SpawnSingleCard(CardsType.Summon);
        Instance = this;
        spawnMonster = false;

        curCard = Camera.main.GetComponent<cursorController>();
    }

    private void Update()
    {
        if (!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);

        #region Spawing Call
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover")))
        {
           
            if (curCard.summon && spawnMonster || curCard._spaceCard != null && spawnMonster)
            {
                
                Transform spwanPoint = info.collider.transform;
                //Debug.DrawLine(Camera.main.transform.position, hit.point, Color.cyan);
                CmdSpawnSingleCard(curCard._cardType, (curCard.summon) ? spwanPoint : spacePos);
            }
           
        }
        #endregion

        #region Materail Chagnge

        if (isHitTile)
        {
            for (int x = 0; x < TILE_Conut_X; x++)
                for (int y = 0; y < TILE_Conut_Y; y++)
                    if (tiles[x, y].layer == LayerMask.NameToLayer("Highlight") || tiles[x, y].layer == LayerMask.NameToLayer("Hover"))
                    {

                        tiles[x, y].layer = LayerMask.NameToLayer("Tile");
                        MeshRenderer mesh = tiles[x, y].GetComponent<MeshRenderer>();
                        mesh.material = (tiles[x, y].tag == "Highlight") ? tileMaterial : (spaceMaterial);
                    }
        }
        
        #endregion
    }

    #region Generate The Board
    private void GenerateAllTiles(float tilesSize, int tileCountX, int tileCountY)
    {
        yOffset += transform.position.y;
        bounds = new Vector3((tileCountX / 2) * tilesSize, 0, (tileCountX / 2) * tilesSize) + boardCenter;

        tiles = new GameObject[tileCountX, tileCountY];

        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                tiles[x, y] = GenerateSignleTile(tilesSize, x, y);
            }
        }
    }

    private GameObject GenerateSignleTile(float tilesSize, int x, int y)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

        Vector3[] vertics = new Vector3[4];
        vertics[0] = new Vector3(x * tilesSize, yOffset, y * tilesSize) - bounds;
        vertics[1] = new Vector3(x * tilesSize, yOffset, (y + 1) * tilesSize) - bounds;
        vertics[2] = new Vector3((x + 1) * tilesSize, yOffset, y * tilesSize) - bounds;
        vertics[3] = new Vector3((x + 1) * tilesSize, yOffset, (y + 1) * tilesSize) - bounds;

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertics;
        mesh.triangles = tris;

        mesh.RecalculateBounds();

        tileObject.tag = "Highlight";
        tileObject.layer = LayerMask.NameToLayer("Tile");   
        tileObject.AddComponent<gridOnBorad>();
        tileObject.AddComponent<BoxCollider>();

        return tileObject;
    }
    #endregion

    #region Operations
    private Vector2Int LookupTileIndex(GameObject hitInfo)
    {
        for (int x = 0; x < TILE_Conut_X; x++)
        {
            for (int y = 0; y < TILE_Conut_Y; y++)
            {
                if (tiles[x, y] == hitInfo)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return -Vector2Int.one; //Invaid 
    }

    #endregion

    #region Spwaning
    
    [Server]
    private void CmdSpawnSingleCard(CardsType type, Transform _transform)
    {
        switch (type)
        {
            case CardsType.Summon:
                if (UI_Script.Instane.currentPoint >= curCard._summonCard.costPoint)
                {
                    Vector3 position = new Vector3(0, 0.3f, 0) + _transform.gameObject.GetComponent<Collider>().bounds.center;
                    GameObject s_fragment = Instantiate(cards[(int)type - 1].prefab[curCard._card.idCard - 1], position, Quaternion.identity);
                    s_fragment.transform.rotation = cards[(int)type - 1].prefab[curCard._summonCard.idCard - 1].transform.rotation;
                    curCard._summonCard.CostToUse(10, "Summon");
                    curCard._summonCard.Used();
                    spawnMonster = false;
                }
                break;
            case CardsType.Space:
                if (UI_Script.Instane.currentPoint >= curCard._spaceCard.costPoint)
                {
                    for (var i = _transform.childCount - 1; i >= 0; i--)
                    {
                        _transform.transform.GetChild(i).gameObject.SetActive(false) ;
                    }

                    GameObject fragment = null;
                    for (int s=0; s < cards[(int)type - 2].prefab.Count; s++)
                    {
                        if (curCard._spaceCard.idCard - 1 == s)
                        {
                            fragment = Instantiate(cards[(int)type - 2].prefab[curCard._spaceCard.idCard - 1], _transform.position, Quaternion.identity);
                            NetworkServer.Spawn(fragment);

                            fragment.transform.rotation = cards[(int)type - 2].prefab[curCard._spaceCard.idCard - 1].transform.rotation;
                            fragment.transform.parent = _transform.transform;
                        }
                        
                    }

                    OnChangeSpace(fragment.GetComponent<TheGuardian>());
                    fragment.GetComponent<TheGuardian>().useAbility = true;

                    spawnMonster = false;
                    curCard._spaceCard.CostToUse(8, "Space");
                    curCard._spaceCard.Used();
                }
                
                break;
            /*case CardsType.Trap:
                
                if (curCard._trap.idCard == 2)
                {
                    Vector3 t_position = new Vector3(0, 0.4f, 0) + _transform.gameObject.GetComponent<Collider>().bounds.center;
                    for (int i = 0; i < cards[(int)type].prefab.Count; i++)
                    {
                        if (cards[(int)type].prefab[i].GetComponent<VizObjScript>() != null)
                        {
                            Instantiate(cards[(int)type].prefab[i], t_position, Quaternion.identity);
                            
                            curCard._card.Used();
                        }

                    }
                }
                spawnMonster = false;
                break;*/
            default:
                break;
        }


    }

    [Command]
    private void CmdUpdatePosition(GameObject card, Transform position)
    {
        card.transform.position = position.position;
        card.transform.rotation = position.rotation;
    }

    private void OnPositionChanged(Vector3 oldValue, Vector3 newValue)
    {
        transform.position = newValue;
    }

    public void UseSelectedCard(CardsType selectedCardType)
    {
        GameObject summonedObject = null;

        switch (selectedCardType)
        {
            case CardsType.Summon:
                summonedObject = cards[(int)CardsType.Summon - 1].prefab[0];
                break;
            default:
                Debug.LogError("Unknown card type!");
                break;
        }
    }


    

    #endregion

    #region  Highlight Tiles

    public void HoverTile(GameObject obj, string tag)
    {
        MeshRenderer mesh = obj.GetComponent<MeshRenderer>();
        mesh.material = (tag== "Highlight")? hoverTiletileMaterial : _hoverSpace;
        objs.Add(obj);
    }

    public void HighlightTilees(GameObject obj)
    {
        MeshRenderer mesh = obj.GetComponent<MeshRenderer>();
        mesh.material =  (obj.tag=="Highlight")? _movementGrid : _hoverSpace;
        objs.Add(obj);

    }
    public void RemoveHighlightTilees()
    {
        for (int j=0;j<objs.Count;j++)
        {
            for (int x = 0; x < TILE_Conut_X; x++)
                for (int y = 0; y < TILE_Conut_Y; y++)
                    if (tiles[x, y].gameObject.name == objs[j].gameObject.name)
                    {
                        MeshRenderer mesh = tiles[x, y].GetComponent<MeshRenderer>();
                        mesh.material = (tiles[x, y].tag == "Highlight")? tileMaterial: spaceMaterial;
                    }
                    
                    

        }
        
    }

    public void InRangeAttack(GameObject obj)
    {
        MeshRenderer mesh = obj.GetComponent<MeshRenderer>();
        mesh.material = (obj.tag=="Highlight")? _attackRange : _hoverSpace;
        objs.Add(obj);
    }

    #endregion

    #region CardAction
    void OnChangeSpace(TheGuardian guardian)
    {
        for (int x = 0; x < TILE_Conut_X; x++)
            for (int y = 0; y < TILE_Conut_Y; y++)
            {
                if (x < (TILE_Conut_X-3)/2)
                {
                    MeshRenderer mesh = tiles[x, y].GetComponent<MeshRenderer>();
                    spaceMaterial = guardian._material;
                    mesh.material = spaceMaterial;
                    tiles[x, y].tag = "Space";
                }
            }
    }

    public void OnChangeToDefualt()
    {
        for (int x = 0; x < TILE_Conut_X; x++)
            for (int y = 0; y < TILE_Conut_Y; y++)
            {
                if (x < (TILE_Conut_X - 3) / 2)
                {
                    MeshRenderer mesh = tiles[x, y].GetComponent<MeshRenderer>();
                    mesh.material = tileMaterial;
                    tiles[x, y].tag = "Highlight";
                }
            }
    }


    #endregion



    
}
//Get the indexs of the tile i've hit
/*Vector2Int hitpostion = LookupTileIndex(info.transform.gameObject);

//If we're hovering a tile after
if (currentHover == -Vector2Int.one)
{

    currentHover = hitpostion;
    tiles[hitpostion.x, hitpostion.y].layer = LayerMask.NameToLayer("Hover");

}

if (currentHover != hitpostion)
{
    tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
    currentHover = hitpostion;
    tiles[hitpostion.x, hitpostion.y].layer = LayerMask.NameToLayer("Hover");
}*/