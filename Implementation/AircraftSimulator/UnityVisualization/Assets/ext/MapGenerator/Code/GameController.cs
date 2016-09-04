using System.Collections;
using TerrainGenerator;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private const int Radius = 4;

    private Vector2i PreviousPlayerChunkPosition;

    public Transform Player;

    public AircraftsController AircraftController;

    public TerrainChunkGenerator Generator;

    public Button StartButton;

    private Transform chunksParent;

    private float distanceToPlayer = 2000;

    private GameObject loadingGameObject;

    public void StartAll()
    {
        StartCoroutine(InitializeCoroutine());
    }

    private void Awake()
    {
        chunksParent = new GameObject("Terrain Container").transform;
        Generator.ChunksParent = chunksParent;
        loadingGameObject = GameObject.FindGameObjectWithTag(Tags.Loading);
    }

    private IEnumerator InitializeCoroutine()
    {
        var canActivateCharacter = false;
        var positionsToGenerate = Generator.UpdateTerrain(Player.position, Radius);
        float initTerrainTime = 0.0f;
        do
        {
            var exists = Generator.IsTerrainAvailable(Player.position);
            initTerrainTime += Time.deltaTime;
            //if (exists)
            if (Generator.ChunksParent.childCount == positionsToGenerate)
                canActivateCharacter = true;
            yield return null;
        } while (!canActivateCharacter);
        Debug.Log("elapsed time for terrain generation: " + initTerrainTime);
        StartCoroutine(removeLoadingScreen());
        PreviousPlayerChunkPosition = Generator.GetChunkPosition(Player.position);
        enabled = true;
        //Player.position = new Vector3(Player.position.x, Generator.GetTerrainHeight(Player.position) + 0.5f, Player.position.z);
        //Player.gameObject.SetActive(true);
    }

    private IEnumerator removeLoadingScreen()
    {
        float animationTime = 1.0f;
        float elapsedTime = 0.0f;
        var rawImage = loadingGameObject.GetComponent<RawImage>();
        var text = loadingGameObject.GetComponentInChildren<Text>();
        while (elapsedTime < animationTime)
        {
            float factor = (1.0f - elapsedTime/animationTime);
            rawImage.color = new Color(0, 0, 0, factor);
            text.color = new Color(1.0f, 1.0f, 1.0f, factor);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rawImage.color = new Color(0, 0, 0, 0);
        text.color = new Color(1.0f, 1.0f, 1.0f, 0);
    }

    private void Update()
    {
        if (Player != null && Player.gameObject.activeSelf)
        {
            var playerChunkPosition = Generator.GetChunkPosition(Player.position);
            if (!playerChunkPosition.Equals(PreviousPlayerChunkPosition))
            {
                Generator.UpdateTerrain(Player.position, Radius);
                PreviousPlayerChunkPosition = playerChunkPosition;
            }


            //var p = chunksParent.position;
            //chunksParent.position = new Vector3(p.x, (Player.position + Vector3.down * distanceToPlayer).y, p.z);

            for (int i = 0; i < chunksParent.childCount; i++)
            {
                var child = chunksParent.GetChild(i);
                var local = child.localPosition;
                child.localPosition = new Vector3(local.x, (Player.position + Vector3.down * distanceToPlayer).y, local.z);
            }

            //Debug.Log("POSITION.Y = " + (Player.position + Vector3.down * distanceToPlayer).y);
        }
    }

    private Vector3 GetInput()
    {

        Vector3 input = new Vector3();
        if (Input.GetKey(KeyCode.W))
            input.x = 1.0f;
        if (Input.GetKey(KeyCode.S))
            input.x = -1.0f;
        if (Input.GetKey(KeyCode.A))
            input.y = -1.0f;
        if (Input.GetKey(KeyCode.D))
            input.y = 1.0f;
        if (Input.GetKey(KeyCode.DownArrow))
            input.z = -1.0f;
        if (Input.GetKey(KeyCode.UpArrow))
            input.z = 1.0f;
        //movementSettings.UpdateDesiredTargetSpeed(input);
        return input;
    }
}