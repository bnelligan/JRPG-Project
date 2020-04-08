using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileManager : MonoBehaviour
{
    Grid grid;
    Tilemap FloorMap;
    Tilemap WallMap;
    Tilemap ObjectMap;
    public static readonly int FloorLayer = 25;
    public static readonly int WallLayer = 20;
    public static readonly uint MaxSpread = 4;

    // Start is called before the first frame update
    void Awake()
    {
        FloorMap = GetComponentsInChildren<Tilemap>().Where(t => t.gameObject.layer == FloorLayer).FirstOrDefault();
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
    Vector3 FloorHitscan(Vector3 worldPosition, Vector2Int direction, uint spread)
    {
        // Constrain spread to the max
        spread = (uint)Mathf.Min(spread, MaxSpread);
        direction.Clamp(new Vector2Int(-1, -1), new Vector2Int(1, 1));

        // Find closest tile on floor
        Vector3Int nearestCellPos = FloorMap.WorldToCell(worldPosition);
        if (direction == Vector2.zero)
        {
            // Use current cell if they are not moving anywhere
            worldPosition = FloorMap.CellToWorld(nearestCellPos);
            spread = 0;
        }
        else
        {
            // Check tile directly in the target direction
            worldPosition = nearestCellPos + (Vector3Int)direction;
        }

        Dictionary<Vector2Int, string> moves_L = new Dictionary<Vector2Int, string>()
        {
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
            [new Vector2Int(-1, -1)] = "+X",
            [new Vector2Int(-1, 0)] = "+X",
            [new Vector2Int(-1, 1)] = "+Y",
            [new Vector2Int(0, 1)] = "+Y",
        };
        

        // Traverse left and right around the player until the spread is reached
        Vector2Int leftDirection = direction;
        Vector2Int rightDirection = direction;
        for(int i = 0; i <= spread; i++)
        {
            Vector3Int leftPosition = nearestCellPos + (Vector3Int)leftDirection;
            Vector3Int rightPosition = nearestCellPos + (Vector3Int)rightDirection;
            if(FloorMap.HasTile(leftPosition))
            {
                worldPosition = FloorMap.CellToWorld(leftPosition);
                break;
            }
            else if(FloorMap.HasTile(rightPosition))
            {
                worldPosition = FloorMap.CellToWorld(rightPosition);
                break;
            }
            else
            {
                CodedTransform(ref leftDirection, moves_L[leftDirection]);
                CodedTransform(ref rightDirection, moves_R[rightDirection]);
                
            }
        }


        return worldPosition;
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
