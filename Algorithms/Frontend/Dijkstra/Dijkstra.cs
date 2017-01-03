using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Frontend
{
	public class Dijkstra
	{
		private Graph graph;
		public Dijkstra (List<Tuple<Vector2, Vector2>> roads)
		{
			graph = new Graph();
			foreach(var road in roads){
				graph.addRoad(road);
			}

		}
		public List<Tuple<Vector2,Vector2>> getShortestPath(Vector2 startingBuilding, Vector2 destinationBuilding){
			return graph.shortest_path (startingBuilding, destinationBuilding);
		}
	}

	class Graph
	{
		Dictionary<Vector2, Dictionary<Vector2, int>> vertices = new Dictionary<Vector2, Dictionary<Vector2, int>>();

		public void add_vertex(Vector2 name, Dictionary<Vector2, int> edges)
		{
			vertices[name] = edges;
		}
		public void addRoad(Tuple<Vector2, Vector2> road){
			insertNode (road.Item1);
			insertNode (road.Item2);
			insertRoad (road);
		}

		public void insertNode(Vector2 Node){
			if (!vertices.ContainsKey(Node)) 

			{
				Dictionary<Vector2, int> emptyRoadsList = new Dictionary<Vector2, int>();
				vertices[Node] = emptyRoadsList; 
			}
		}

		public void insertRoad(Tuple<Vector2, Vector2> road){
			Vector2 point1 = road.Item1;
			Vector2 point2 = road.Item2;
			int length = (int) Vector2.Distance (point1, point2);
			if (!vertices[point1].ContainsKey(point2)) 
			{
				vertices[point1].Add(point2, length); 
			}
			if (!vertices[point2].ContainsKey(point1)) 
			{
				vertices[point2].Add(point1, length); 
			}
		}

		public List<Tuple<Vector2, Vector2>> shortest_path(Vector2 start, Vector2 finish)
		{
			var previous = new Dictionary<Vector2, Vector2>();
			var distances = new Dictionary<Vector2, int>();
			var nodes = new List<Vector2>();

			List<Tuple<Vector2, Vector2>> path = null;

			foreach (var vertex in vertices)
			{
				if (vertex.Key == start)
				{
					distances[vertex.Key] = 0;
				}
				else
				{
					distances[vertex.Key] = int.MaxValue;
				}

				nodes.Add(vertex.Key);
			}

			while (nodes.Count != 0)
			{
				nodes.Sort((x, y) => distances[x] - distances[y]);

				var smallest = nodes[0];
				nodes.Remove(smallest);

				if (smallest == finish)
				{

					path = new List<Tuple<Vector2, Vector2>>();
					while (previous.ContainsKey(smallest))
					{
						Tuple<Vector2, Vector2> road = new Tuple<Vector2, Vector2> (smallest, previous [smallest]);
						path.Add(road);
						smallest = previous[smallest];
					}

					break;
				}

				if (distances[smallest] == int.MaxValue)
				{
					break;
				}

				foreach (var neighbor in vertices[smallest])
				{
					var alt = distances[smallest] + neighbor.Value;
					if (alt < distances[neighbor.Key])
					{
						distances[neighbor.Key] = alt;
						previous[neighbor.Key] = smallest;
					}
				}
			}

			return path;
		}
	}}