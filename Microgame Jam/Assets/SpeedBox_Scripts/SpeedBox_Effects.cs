using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBox_Effects : MonoBehaviour
{
    [SerializeField] float startSize;
    [SerializeField] float fadeRate;
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject particlePrefab;
    [SerializeField] GameObject shatterPrefab;

    public LineRenderer trailEffect;
    private SpeedBox_Game game;
    [SerializeField] int length = 30;
    [SerializeField] float smoothSpeed = 0.02f;
    public float targetDistance = 0.2f;
    Vector3[] segmentPoses;
    Vector3[] segmentVelocity;
    public Transform targetDirection;

    private void OnEnable()
    {
        trailEffect = GetComponent<LineRenderer>();
        game = GameObject.FindGameObjectWithTag("Grid").GetComponent<SpeedBox_Game>();
    }

    private void Start()
    {

        trailEffect.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentVelocity = new Vector3[length];
        for (int i = 0; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.forward * 3;
        }

        trailEffect.startWidth = game.main.gameDifficulty switch
        {
            1 => 8f,
            2 => 9f,
            3 => 10f,
            _ => 0
        };
        trailEffect.endWidth = 0;
    }

    private void Update()
    {
        segmentPoses[0] = targetDirection.transform.position + Vector3.forward * 3;
        for (int i = 1; i < length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDirection.right * targetDistance, ref segmentVelocity[i], smoothSpeed);
        }
        trailEffect.SetPositions(segmentPoses);
    }

    public void AfterImage(Vector2 from, Vector2 to)
    {
        LineRenderer lineRenderer = Instantiate(linePrefab).GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, to);
        lineRenderer.SetPosition(1, from);
        StartCoroutine(FadeLine(lineRenderer));
    }

    public void HitParticle(Vector2 position, Vector2Int direction, float distance)
    {
        if (distance < 1) return;
        ParticleSystem particles = Instantiate(particlePrefab).GetComponent<ParticleSystem>();
        var emission = particles.emission;
        Quaternion rotation = direction switch
        {
            { x: 0, y: 1 }  => Quaternion.Euler(0, 0, 180),
            { x: 0, y: -1 } => Quaternion.Euler(0, 0, 0),
            { x: -1, y: 0 } => Quaternion.Euler(0, 0, 270),
            { x: 1, y: 0 }  => Quaternion.Euler(0, 0, 90),
            _ => Quaternion.Euler(0, 0, 9)
        };

        emission.SetBurst(0, new ParticleSystem.Burst(0, (int)distance + 3));
        particles.transform.position = (Vector3)position + Vector3.back;
        particles.transform.rotation = rotation;
    }

    public void Shatter(Vector2 position, Color color)
    {
        ParticleSystem particles = Instantiate(shatterPrefab).GetComponent<ParticleSystem>();
        particles.transform.position = (Vector3)position + Vector3.back;
        var main = particles.main;
        main.startColor = color;
    }

    IEnumerator FadeLine (LineRenderer lineRenderer)
    {
        float size = startSize;
        while (size > 0)
        {
            lineRenderer.startWidth = size * 2;
            lineRenderer.endWidth = size;
            size -= Time.deltaTime * fadeRate;
            yield return null;
        }
        Destroy(lineRenderer.gameObject);
    }
}
