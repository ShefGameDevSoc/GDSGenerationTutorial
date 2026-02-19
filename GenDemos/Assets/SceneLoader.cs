using DefaultNamespace;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public GameObject tilePrefab;
    public Sprite AllInside;
    public Sprite AllOutside;
    
    public Sprite N;
    public Sprite E;
    public Sprite S;
    public Sprite W;

    public Sprite NE;
    public Sprite ES;
    public Sprite SW;
    public Sprite WN;
    
    public Sprite Unknown;
    
    public static SceneLoader instance;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    public void RenderTiles(Occupancy[,] tiles)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                var tile = Instantiate(tilePrefab, this.transform);
                tile.transform.position = new Vector3(i, j, 0);
                tile.GetComponent<SpriteRenderer>().sprite = OccupancyToSprite(tiles[i,j]);
            }    
        }
    }

    public Sprite OccupancyToSprite(Occupancy occupancy)
    {
        if (occupancy.isOutside)
        {
            return AllOutside;
        }
        switch (occupancy.north, occupancy.east, occupancy.south, occupancy.west)
        {
            case (false, false, false, false):
                return AllInside;
            
            case (true, false, false,false):
                return N;
            case (false, true, false,false):
                return E;
            case (false, false, true,false):
                return S;
            case (false, false, false,true):
                return W;
            
            case (true, true, false,false):
                return NE;
            case (false, true, true,false):
                return ES;
            case (false, false, true,true):
                return SW;
            case (true, false, false,true):
                return WN;
            
            default:
                return Unknown;
        }
    }
}
