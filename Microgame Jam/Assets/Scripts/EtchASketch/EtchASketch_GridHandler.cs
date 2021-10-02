using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EtchASketch_GridHandler : MonoBehaviour {
    
    [SerializeField] private Tilemap playerTiles;
    [SerializeField] private Tilemap targetTiles;
    [SerializeField] private TileBase black;
    [SerializeField] private TileBase white;

    [SerializeField] private Transform player;
    private Vector3Int playerPos;

    [SerializeField] private AudioSource move;
    [SerializeField] private AudioSource flip;
    [SerializeField] private AudioSource win;

    // Start is called before the first frame update
    void Start() {
        Vector3Int[] vectorArray = {new Vector3Int(1, 3, 0), new Vector3Int(2, 2, 0),
                                    new Vector3Int(3, 2, 0), new Vector3Int(4, 2, 0),
                                    new Vector3Int(5, 2, 0), new Vector3Int(6, 2, 0),
                                    new Vector3Int(7, 3, 0), new Vector3Int(2, 6, 0),
                                    new Vector3Int(6, 6, 0)};
        Vector3Int[] amogusArray = {new Vector3Int(1, 1, 0), new Vector3Int(1, 2, 0),
                                    new Vector3Int(1, 3, 0), new Vector3Int(1, 4, 0),
                                    new Vector3Int(1, 5, 0), new Vector3Int(1, 6, 0),
                                    new Vector3Int(6, 1, 0), new Vector3Int(6, 2, 0),
                                    new Vector3Int(6, 3, 0), new Vector3Int(6, 4, 0),
                                    new Vector3Int(6, 5, 0), new Vector3Int(6, 6, 0),
                                    new Vector3Int(2, 6, 0), new Vector3Int(3, 6, 0),
                                    new Vector3Int(4, 6, 0), new Vector3Int(5, 6, 0),
                                    new Vector3Int(2, 4, 0), new Vector3Int(7, 4, 0),
                                    new Vector3Int(2, 1, 0), new Vector3Int(3, 1, 0),
                                    new Vector3Int(3, 2, 0), new Vector3Int(4, 2, 0),
                                    new Vector3Int(4, 1, 0), new Vector3Int(5, 1, 0)};

        int array = Random.Range(1, 3);
        Vector3Int[] arrayToDraw = { };
        switch (array)
        {
            case 1:
                arrayToDraw = vectorArray;
                break;
            case 2:
                arrayToDraw = amogusArray;
                break;
        }

        InitTilemap(targetTiles, arrayToDraw);
        InitTilemap(playerTiles, arrayToDraw);
        GameController.Instance.SetMaxTimer((4 - GameController.Instance.gameDifficulty) * 5);
        playerPos = new Vector3Int(4, 4, 0);
    }

    // Update is called once per frame
    void Update() {
        if (GameController.Instance.timerOn) {
            // Player Input
            if (Input.GetKeyDown(KeyCode.DownArrow) && playerPos.y > 0) {
                move.Play();
                player.position = new Vector3(player.position.x, player.position.y - 1, 0);
                playerPos.y -= 1;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && playerPos.y < 8) {
                move.Play();
                player.position = new Vector3(player.position.x, player.position.y + 1, 0);
                playerPos.y += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && playerPos.x > 0) {
                move.Play();
                player.position = new Vector3(player.position.x - 1, player.position.y, 0);
                playerPos.x -= 1;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && playerPos.x < 8) {
                move.Play();
                player.position = new Vector3(player.position.x + 1, player.position.y, 0);
                playerPos.x += 1;
            }

            if (Input.GetKeyDown("space")) {
                flip.Play();
                if (playerTiles.GetTile(playerPos) == black)
                    playerTiles.SetTile(playerPos, white);
                else
                    playerTiles.SetTile(playerPos, black);
            }
            if (CompareTilemaps()) {
                win.Play();
                GameController.Instance.WinGame();
            }
        }
        
    }

    void InitTilemap(Tilemap tiles, Vector3Int[] arrayToDraw) {
        tiles.ClearAllTiles();
        tiles.size = new Vector3Int(9, 9, 0);
        tiles.ResizeBounds();
        for (int x = 0; x < 9; ++x) {
            for (int y = 0; y < 9; ++y) {
                tiles.SetTile(new Vector3Int(x, y, 0), white);
            }
        }

        int rand = Random.Range(1, 9);
        int rand2, rand3;
        do {
            rand2 = Random.Range(1, 9);
        } while (rand2 == rand);
        do {
            rand3 = Random.Range(1, 9);
        } while (rand3 == rand2 || rand3 == rand);

        if (GameController.Instance.gameDifficulty < 3)
            rand3 = rand;
        if (GameController.Instance.gameDifficulty < 2)
            rand2 = rand;

        //Draw all tiles for targetTiles, but omit one for playerTiles
        for (int i = 0; i < arrayToDraw.Length; ++i) {
            if ((i != rand && i != rand2 && i != rand3) || tiles == targetTiles)
                tiles.SetTile(arrayToDraw[i], black);
        }
    }

    bool CompareTilemaps() {
        bool result = true;

        for (int x = 0; x < 9; ++x) {
            for (int y = 0; y < 9; ++y) {
                if (playerTiles.GetSprite(new Vector3Int(x, y, 0)) != targetTiles.GetSprite(new Vector3Int(x, y, 0))) {
                    result = false;
                }
            }
        }
        return result;
    }

    void OnGUI() {
        GUILayout.Box($"Finish the Drawing!!!");
    }
}
