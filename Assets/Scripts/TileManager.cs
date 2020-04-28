using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileManager : MonoBehaviour
{
    private enum TileLayerKey
    {
        INTERACTABLE,
        FLOOR,
        ENVIRONMENT,
        CHARACTER
    }

    Grid grid;
    Dictionary<TileLayerKey, Tilemap> MapLookup;
    Dictionary<TileLayerKey, int> LayerLookup = new Dictionary<TileLayerKey, int>()
    {
        [TileLayerKey.CHARACTER] = 9,
        [TileLayerKey.ENVIRONMENT] = 15,
        [TileLayerKey.INTERACTABLE] = 18,
        [TileLayerKey.FLOOR] = 25,
    };

    // Layer props
    public int CharacterLayer { get { return LayerLookup[TileLayerKey.CHARACTER]; } }
    public int InteractableLayer { get { return LayerLookup[TileLayerKey.INTERACTABLE]; } }
    public int WallLayer { get { return LayerLookup[TileLayerKey.ENVIRONMENT]; } }
    public int FloorLayer { get { return LayerLookup[TileLayerKey.FLOOR]; } }

    // Tilemap props
    Tilemap CharacterMap { get { return MapLookup[TileLayerKey.CHARACTER]; } }
    Tilemap InteractableMap { get { return MapLookup[TileLayerKey.INTERACTABLE]; } }
    Tilemap WallMap { get { return MapLookup[TileLayerKey.ENVIRONMENT]; } }
    Tilemap FloorMap { get { return MapLookup[TileLayerKey.FLOOR]; } }
    
    // Used for hitscan and rotation
    Dictionary<Vector2Int, string> moves_L = new Dictionary<Vector2Int, string>()
    {
        // Looks like a caterpillar! (:
        [new Vector2Int(1, 1)] = "-X",
        [new Vector2Int(0, 1)] = "-X",
        [new Vector2Int(-1, 1)] = "-Y",
        [new Vector2Int(-1, 0)] = "-Y",
        [new Vector2Int(-1, -1)] = "+X",
        [new Vector2Int(0, -1)] = "+X",
        [new Vector2Int(1, -1)] = "+Y",
        [new Vector2Int(1, 0)] = "+Y",
    };
    Dictionary<Vector2Int, string> moves_R = new Dictionary<Vector2Int, string>()
    {
        [new Vector2Int(1, 1)] = "-Y",
        [new Vector2Int(1, 0)] = "-Y",
        [new Vector2Int(1, -1)] = "-X",
        [new Vector2Int(0, -1)] = "-X",
        [new Vector2Int(-1, -1)] = "+Y",
        [new Vector2Int(-1, 0)] = "+Y",
        [new Vector2Int(-1, 1)] = "+X",
        [new Vector2Int(0, 1)] = "+X",
    };

    // Start is called before the first frame update
    void Awake()
    {
        InitMapLookup();
        //FloorMap = GetComponentsInChildren<Tilemap>().Where(t => t.gameObject.layer == FloorLayer).FirstOrDefault();
    }

    private void InitMapLookup()
    {
        MapLookup = new Dictionary<TileLayerKey, Tilemap>()
        {
            [TileLayerKey.FLOOR] = FindMap(TileLayerKey.FLOOR),
            [TileLayerKey.INTERACTABLE] = FindMap(TileLayerKey.INTERACTABLE),
            [TileLayerKey.ENVIRONMENT] = FindMap(TileLayerKey.ENVIRONMENT),
            [TileLayerKey.CHARACTER] = FindMap(TileLayerKey.CHARACTER),

        };
    }
    private Tilemap FindMap(TileLayerKey mapLayer)
    {
        return GetComponentsInChildren<Tilemap>().Where(t => t.gameObject.layer == LayerLookup[mapLayer]).FirstOrDefault();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Perform a hitscan for available floor tiles, and returns the world position for the closest tile in the target direction.
    /// </summary>
    /// <param name="worldPosition">Position of origin in world position</param>
    /// <param name="direction">Normalized direction of hitscan</param>
    /// <param name="spread">0-4 where 0 is straight ahead and 4 is full circle</param>
    /// <returns>World position for the hit tile</returns>
    List<Vector3> TileHitscan(TileLayerKey layer, Vector3 worldPosition, Vector2Int direction, uint spread)
    {
        uint MaxSpread = 4;
        List<Vector3> scanResults = new List<Vector3>();
        Tilemap map = MapLookup[layer];

        // Constrain spread to the max
        spread = (uint)Mathf.Min(spread, MaxSpread);
        direction.Clamp(new Vector2Int(-1, -1), new Vector2Int(1, 1));

        // Find closest tile on floor
        Vector3Int nearestCellPos = map.WorldToCell(worldPosition);
        if (direction == Vector2.zero)
        {
            // Use current cell if they are not moving anywhere
            scanResults.Add(map.GetCellCenterWorld(nearestCellPos));
            spread = 0;
        }

        // Traverse left and right around the center (0,0) until the spread is reached in both directions
        Vector2Int leftDirection = direction;
        Vector2Int rightDirection = direction;
        for(int i = 0; i <= spread && direction != Vector2.zero; i++)
        {
            Vector3Int leftPosition = nearestCellPos + (Vector3Int)leftDirection;
            Vector3Int rightPosition = nearestCellPos + (Vector3Int)rightDirection;
            if(map.HasTile(leftPosition))
            {
                scanResults.Add(map.GetCellCenterWorld(leftPosition));
            }
            else if(map.HasTile(rightPosition))
            {
                scanResults.Add(map.GetCellCenterWorld(rightPosition));
            }
            CodedTransform(ref leftDirection, moves_L[leftDirection]);
            CodedTransform(ref rightDirection, moves_R[rightDirection]);
            
        }
        if (scanResults.Count == 0)
        {
            scanResults.Add(map.GetCellCenterWorld(nearestCellPos));
        }
        return scanResults;
    }

    public Vector3 FindMove(Vector3 worldPosition, Vector2Int direction, uint spread)
    {
        Vector3 availableMove = worldPosition;
        // Find floor tiles
        List<Vector3> FloorHits = TileHitscan(TileLayerKey.FLOOR, worldPosition, direction, spread);
        availableMove = FloorHits[0];
        Debug.Log($"Floor tiles: {FloorHits}");
        // Find wall tiles 
        List<Vector3> WallHits = TileHitscan(TileLayerKey.ENVIRONMENT, worldPosition, direction, spread);
        Debug.Log($"Wall tiles: {WallHits}");

        // Find floors that are not shared with walls
        foreach (Vector3 FloorMove in FloorHits)
        {
            bool blocked = WallHits.Contains(FloorMove);
            if(blocked)
            {
                Debug.Log("Blocked move: " + FloorMove);
            }
            else
            {
                availableMove = FloorMove;
                break;
            }
        }

        return availableMove;
    }
    
    // +X, +Y, -X, -Y
    void CodedTransform(ref Vector2Int vec, string code)
    {
        if (code[0] == '-' && code[1] == 'X')
        {
            vec.x--;
        }
        else if (code[0] == '-' && code[1] == 'Y')
        {
            vec.y--;
        }
        else if (code[0] == '+' && code[1] == 'X')
        {
            vec.x++;
        }
        else if (code[0] == '+' && code[1] == 'Y')
        {
            vec.y++;
        }
    }
}
