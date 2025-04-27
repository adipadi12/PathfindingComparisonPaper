using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
public class ExperimentDataCollector : MonoBehaviour
{
    [Header("Map Configurations")]
    public int[] widths = { 10, 20, 50 };
    public int[] heights = { 10, 20, 50 };
    public float[] densities = { 0.1f, 0.2f, 0.3f };
    public int runsPerConfig = 5;

    public GridManager gridManager;               // drag in your GridManager
    public BFSPathfinder bfs;                     // drag in your BFSManager object
    public Djikstras dijkstra;           // drag in your DijkstraManager
    public AStar aStar;                 // drag in your AStarManager

    public int runsPerAlgorithm = 10;             // repeat each test N times
    public string outputFileName = "results.csv"; // will be placed in Assets

    IEnumerator Start()
    {
        yield return null; // wait a frame

        // write header once
        File.WriteAllText(Path.Combine(Application.dataPath, outputFileName),
            "Algo,Run,Width,Height,Density,TimeMs,PathLen\n"
        );

        var stopwatch = new Stopwatch();

        // nested loops for every config
        foreach (int w in widths)
            foreach (int h in heights)
                foreach (float d in densities)
                {
                    // re-configure your grid
                    gridManager.width = w;
                    gridManager.height = h;
                    gridManager.obstacleChance = d;
                    gridManager.GenerateGrid(); // rebuild grid

                    Node start = gridManager.grid[0, 0];
                    Node end = gridManager.grid[w - 1, h - 1];

                    for (int run = 1; run <= runsPerConfig; run++)
                    {
                        // BFS
                        stopwatch.Restart();
                        var pathB = bfs.FindPath(start, end);
                        stopwatch.Stop();
                        Log("BFS", run, w, h, d, stopwatch.ElapsedMilliseconds, pathB.Count);

                        // Dijkstra
                        stopwatch.Restart();
                        var pathD = dijkstra.FindPath(start, end);
                        stopwatch.Stop();
                        Log("Dijkstra", run, w, h, d, stopwatch.ElapsedMilliseconds, pathD.Count);

                        // A*
                        stopwatch.Restart();
                        var pathA = aStar.FindPath(start, end);
                        stopwatch.Stop();
                        Log("AStar", run, w, h, d, stopwatch.ElapsedMilliseconds, pathA.Count);

                        yield return null;
                    }
                }

        UnityEngine.Debug.Log("All configs done—check results.csv");
    }

    void Log(string algo, int run, int w, int h, float d, long ms, int len)
    {
        string line = $"{algo},{run},{w},{h},{d:F2},{ms},{len}\n";
        File.AppendAllText(Path.Combine(Application.dataPath, outputFileName), line);
    }
}
