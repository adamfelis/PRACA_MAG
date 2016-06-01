using System.Collections;
using TerrainGenerator;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private const int Radius = 4;

    private Vector2i PreviousPlayerChunkPosition;

    private Transform player;

    private TerrainChunkGenerator generator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Player_ID>().GetLocalPlayer().transform;
        generator = GetComponent<TerrainChunkGenerator>();
        StartCoroutine(InitializeCoroutine());
    }

    public void StartAll()
    {
        StartCoroutine(InitializeCoroutine());
    }

    private IEnumerator InitializeCoroutine()
    {
        var canActivateCharacter = false;
        generator.UpdateTerrain(player.position, Radius);

        do
        {
            var exists = generator.IsTerrainAvailable(player.position);
            if (exists)
                canActivateCharacter = true;
            yield return null;
        } while (!canActivateCharacter);

        PreviousPlayerChunkPosition = generator.GetChunkPosition(player.position);
        player.position = new Vector3(player.position.x, generator.GetTerrainHeight(player.position) + 0.5f, player.position.z);
        player.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (player.gameObject.activeSelf)
        {
            var playerChunkPosition = generator.GetChunkPosition(player.position);
            if (!playerChunkPosition.Equals(PreviousPlayerChunkPosition))
            {
                generator.UpdateTerrain(player.position, Radius);
                PreviousPlayerChunkPosition = playerChunkPosition;
            }
        }
    }
}