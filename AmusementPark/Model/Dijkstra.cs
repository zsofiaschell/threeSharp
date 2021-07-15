using System;
using System.Collections.Generic;

namespace AmusementPark.Model
{
	public class Dijkstra //Dijkstra algorithm for path searching
	{
		private readonly int NO_PARENT = -1;

		//Dijkstra algorithm for adjancy matrix
		public void DijkstraAlgorithm(int[,] adjacencyMatrix, int startVertex, int destination, List<(int, int)> list, List<(int, int)> path)
		{

			int nVertices = adjacencyMatrix.GetLength(0);

			int[] shortestDistances = new int[nVertices];

			bool[] added = new bool[nVertices];

			for (int vertexIndex = 0; vertexIndex < nVertices; vertexIndex++)
			{
				shortestDistances[vertexIndex] = int.MaxValue;
				added[vertexIndex] = false;
			}

			shortestDistances[startVertex] = 0;

			int[] parents = new int[nVertices];

			parents[startVertex] = NO_PARENT;

			for (int i = 1; i < nVertices; i++)
			{
				int nearestVertex = -1;
				int shortestDistance = int.MaxValue;

				for (int vertexIndex = 0; vertexIndex < nVertices; vertexIndex++)
				{
					if (!added[vertexIndex] && shortestDistances[vertexIndex] < shortestDistance)
					{
						nearestVertex = vertexIndex;
						shortestDistance = shortestDistances[vertexIndex];
					}
				}

                if (nearestVertex != -1)
                {
					added[nearestVertex] = true;
                }

				for (int vertexIndex = 0; vertexIndex < nVertices; vertexIndex++)
				{
					int edgeDistance = 0;

                    if (nearestVertex != -1)
                    {
						edgeDistance = adjacencyMatrix[nearestVertex, vertexIndex];
                    }


					if (edgeDistance > 0 && ((shortestDistance + edgeDistance) < shortestDistances[vertexIndex]))
					{
						parents[vertexIndex] = nearestVertex;
						shortestDistances[vertexIndex] = shortestDistance + edgeDistance;
					}
				}
			}

			GetSolution(startVertex, shortestDistances, parents, destination, list, path);
		}

		// Gets the shortest path to the destination and writes it to the console
		private void GetSolution(int startVertex, int[] distances, int[] parents, int destination, List<(int, int)> list, List<(int, int)> path)
		{
			Console.Write("Vertex\t Distance\tPath");
			Console.Write("\n" + startVertex + " -> ");
			Console.Write(destination + " \t\t ");
			Console.Write(distances[destination] + "\t\t");
			PathPrinting(destination, parents, list, path);
		}

		//Prints the path recursively by the selected vertex
		private void PathPrinting(int currentVertex, int[] parents, List<(int, int)> list, List<(int, int)> path)
		{
			if (currentVertex == NO_PARENT)
			{
				return;
			}
			PathPrinting(parents[currentVertex], parents, list, path);
			Console.Write(currentVertex + " ");
			Console.WriteLine(list[currentVertex] + " ");
			path.Add(list[currentVertex]);
		}

		//Sets the matrix's connection between vertices
		public void SetMatrix(List<(int, int)> list, int[,] matrix)
		{
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = i + 1; j < list.Count; j++)
				{
					if (CheckIfCoordinatesAreNextToEachOther(list[i], list[j]))
					{
						matrix[i, j] = 1;
						matrix[j, i] = 1;
					}
				}
			}
		}

		//Checks if two coordinates are next to each other
		public bool CheckIfCoordinatesAreNextToEachOther((int, int) first, (int, int) secod)
		{
			if (first.Item1 + 1 == secod.Item1 && first.Item2 == secod.Item2)
				return true;
			if (first.Item1 - 1 == secod.Item1 && first.Item2 == secod.Item2)
				return true;
			if (first.Item1 == secod.Item1 && first.Item2 + 1 == secod.Item2)
				return true;
			if (first.Item1 == secod.Item1 && first.Item2 - 1 == secod.Item2)
				return true;

			return false;
		}
	}
}
