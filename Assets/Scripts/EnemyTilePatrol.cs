using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Animator))]
public class EnemyTilePatrol : MonoBehaviour
{
    public string groundTilemapName = "Ground"; // Namnet p� Tilemap i Hierarchy
    public int patrolDistance = 5;              // Hur m�nga tiles monster patrullerar
    public float moveCooldown = 0.5f;           // Tid mellan tile-r�relser

    private Tilemap groundTilemap;
    private Vector3Int startCell;
    private Vector3Int currentCell;
    private int direction = 1;                   // 1 = h�ger, -1 = v�nster
    private float timer;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        // Hitta Tilemap automatiskt i scenen
        GameObject tilemapObj = GameObject.Find(groundTilemapName);
        if (tilemapObj != null)
        {
            groundTilemap = tilemapObj.GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Ground Tilemap hittades inte! Kontrollera namnet: " + groundTilemapName);
            enabled = false;
            return;
        }

        // S�tt startcell baserat p� monsterposition
        startCell = groundTilemap.WorldToCell(transform.position);
        currentCell = startCell;

        // Slumpa start-riktning
        direction = Random.Range(0, 2) == 0 ? 1 : -1;
        timer = moveCooldown;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            MoveOneTile();
            timer = moveCooldown;
        }
    }

    void MoveOneTile()
    {
        Vector3Int nextCell = currentCell + new Vector3Int(direction, 0, 0);

        if (groundTilemap.GetTile(nextCell) != null)
        {
            currentCell = nextCell;
            transform.position = groundTilemap.GetCellCenterWorld(currentCell);
        }
        else
        {
            direction *= -1;
        }

        if (Mathf.Abs(currentCell.x - startCell.x) >= patrolDistance)
        {
            direction *= -1;
        }

        // Flip sprite horisontellt
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction > 0 ? 1 : -1);
        transform.localScale = scale;

        if (anim != null)
            anim.SetBool("isRunning", true);
    }
}
