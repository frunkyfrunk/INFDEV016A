using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Frontend;

namespace EntryPoint
{
    public static class Program
    {
		static void Main()
		{
		    var fullscreen = false;
		    
		    Console.WriteLine("Enter number of simulation to run - [1 - 4, q]");
		    
		    while(true)
		    {
				switch(Console.ReadLine())
				{
				    case "1":
						using (var game = VirtualCity.RunAssignment1(SortSpecialBuildingsByDistance,
											     fullscreen))
						    game.Run();
						break;
					
				    case "2":
						using (var game = VirtualCity.RunAssignment2(FindSpecialBuildingsWithinDistanceFromHouse,
											     fullscreen))
						    game.Run();
						break;
					
				    case "3":
						using (var game = VirtualCity.RunAssignment3(FindRoute, fullscreen))
						    game.Run();
						break;
					
				    case "4":
						using (var game = VirtualCity.RunAssignment4(FindRoutesToAll, fullscreen))
						    game.Run();
						break;
					
				    case "q":
						return;
					
					default:
						Console.WriteLine ("Invalid input! Try again! - [1 - 4, q]");
						break;
				}
		    }
		}

		private static IEnumerable<Vector2> SortSpecialBuildingsByDistance(Vector2 house, IEnumerable<Vector2> specialBuildings)
		{
			Vector2[] buildings = specialBuildings.ToArray();
			MergeSort(buildings, 0, buildings.Length-1, house);
			return buildings;
		}

		static public void DoMerge(Vector2[] buildings, int left, int mid, int right, Vector2 house)
		{
			Vector2[] temp = new Vector2[buildings.Length];
			int i, left_end, num_elements, tmp_pos;

			left_end = (mid - 1);
			tmp_pos = left;
			num_elements = (right - left + 1);

			while ((left <= left_end) && (mid <= right))
			{
				if (Vector2.Distance(buildings[left], house) <= Vector2.Distance(buildings[mid], house))
					temp[tmp_pos++] = buildings[left++];
				else
					temp[tmp_pos++] = buildings[mid++];
			}

			while (left <= left_end)
				temp[tmp_pos++] = buildings[left++];

			while (mid <= right)
				temp[tmp_pos++] = buildings[mid++];

			for (i = 0; i < num_elements; i++)
			{
				buildings[right] = temp[right];
				right--;
			}
		}
		static public void MergeSort(Vector2[] buildings, int left, int right, Vector2 house)
		{
			int mid;
			if (right > left)
			{
				mid = (right + left) / 2;
				MergeSort(buildings, left, mid, house);
				MergeSort(buildings, (mid + 1), right, house);
				DoMerge(buildings, left, (mid + 1), right, house);
			}
		}

		private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(IEnumerable<Vector2> specialBuildings, 
													     IEnumerable<Tuple<Vector2, float>> housesAndDistances)
		{
			var Kdtree = new Node ();
			foreach (var house in specialBuildings) {
				Kdtree.AddNode (house, 0);
			}
			List<Vector2> withinDistance = new List<Vector2>();
			foreach (var house in housesAndDistances)
			{
				var WithinDistanceFromHouse = Kdtree.GetAllNodesWithinDistance(new List<Vector2>(), house.Item1, house.Item2);
				foreach (var specialbuilding in WithinDistanceFromHouse) {
					withinDistance.Add (specialbuilding);
				}
			}
			 yield return withinDistance;
		}

		private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding, 
									      Vector2 destinationBuilding,
									      IEnumerable<Tuple<Vector2, Vector2>> roads)
		{
			var dijkstra = new Dijkstra (roads.ToList());
			return dijkstra.getShortestPath(startingBuilding, destinationBuilding);
		}

		private static IEnumerable<IEnumerable<Tuple<Vector2, Vector2>>> FindRoutesToAll(Vector2 startingBuilding, 
												 IEnumerable<Vector2> destinationBuildings,
												 IEnumerable<Tuple<Vector2,
												 Vector2>> roads)
		{
		    List<List<Tuple<Vector2, Vector2>>> result = new List<List<Tuple<Vector2, Vector2>>>();
		    foreach (var d in destinationBuildings)
		    {
			var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
			List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
			var prevRoad = startingRoad;
			for (int i = 0; i < 30; i++)
			{
			    prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, d)).First());
			    fakeBestPath.Add(prevRoad);
			}
			result.Add(fakeBestPath);
		    }
		    return result;
		}
    }
}
