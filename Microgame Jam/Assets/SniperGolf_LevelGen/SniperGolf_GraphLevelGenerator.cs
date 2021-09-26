// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// class SniperGolf_GraphLevelGenerator
// {
//     // Adjacency List representation of the level
//     private List<SniperGolf_Node> adj = new List<SniperGolf_Node>();

//     private SniperGolf_Node[,] grid;

//     [SerializeField] private int width = 4;
//     [SerializeField] private int height = 3;

//     public void Generate()
//     {
//         // Generate random grid
//         grid = new SniperGolf_Node[width, height];
//         for (int i = 0; i < height; i++)
//             for (int j = 0; j < width; j++)
//             {
//                 SniperGolf_Node newNode = new SniperGolf_Node(GetRandomTerrainType());
//                 grid[i, j] = newNode;
//                 adj.Add(newNode);
//             }

//         // Add edges to adjacency list
//         int listIndex = 0;
//         for (int i = 0; i < height; i++)
//             for (int j = 0; j < width; j++)
//                 grid[i, j].edges = GetNeighbors(i, j);

//         PrintGrid();
//     }

//     private SniperGolf_TerrainType GetRandomTerrainType()
//     {
//         return (SniperGolf_TerrainType) Random.Range((int)0, SniperGolf_TerrainType.COUNT);
//     }

//     private List<SniperGolf_Node> GetNeighbors(int i, int j)
//     {
//         List<SniperGolf_Node> neighbors = new List<SniperGolf_Node>();

//         // Walls have no neighbors
//         if (grid[i, j].terrain == SniperGolf_TerrainType.Wall)
//             return neighbors;

//         for (int y = i - 1; y <= i + 1; y++)
//             if (y >= 0 && y < height) // check y bounds
//                 for (int x = j - 1; x <= j + 1; x++)
//                     if (x >= 0 && x < width) // check x bounds
//                         if (!(y == i && x == j)) // check that it's not looking at itself
//                         {
//                             SniperGolf_Node neighbor = grid[x, y];
//                             if (neighbor.terrain != SniperGolf_TerrainType.Wall) // no wall neighbors
//                                 neighbors.add(neighbor);
//                         }

//         return neighbors;
//     }

//     private void PrintGrid()
//     {
//         for (int i = 0; i < height; i++)
//             for (int j = 0; j < width; j++)
//                 Console.WriteLine("X");
//             Console.WriteLine();
//     }
// }