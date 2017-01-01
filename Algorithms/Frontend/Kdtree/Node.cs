using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Frontend
{
	public class Node
	{
		public Node left;
		public Node right;
		public Vector2 house;

		public Node (Vector2 house)
		{
			this.house = house;
		}

		public Node (Vector2 house, List<Vector2> list, int dimension)
		{
			this.house = house;
			AddNodes (list, dimension);
		}
		public void AddNodes(List<Vector2> buildings, int dimension)
		{
			if (buildings.Count > 0) {
				int count = buildings.Count;
				int axis = dimension % count;
				SortList (buildings, axis);
				int median = count / 2;
				house = buildings [median];
				if(buildings.Count > 2){
				Vector2 Left = buildings [median - 1];
				Vector2 Right = buildings [median + 1];
				dimension++;
				var leftList = buildings.Take (median).ToList();
				var rightList = buildings.GetRange (median + 1, count / 2 - 1).ToList();

				left = new Node (Left, leftList, dimension);
				right = new Node (Right, rightList, dimension);
				}
			}

		}

		public void SortList(List<Vector2> buildings, int axis){
			if (axis == 0) {
				buildings.OrderBy (vector => vector.X);
			} else {
				buildings.OrderBy (vector => vector.Y);
			}
		}

		public IEnumerable<Vector2> GetAllNodesWithinDistance(IList<Vector2> foundNodes, Vector2 startPosition, float distance)
		{
			return GetAllNodesWithinDistance(foundNodes, this, startPosition, distance);
		}

		public IEnumerable<Vector2> GetAllNodesWithinDistance(IList<Vector2> foundNodes, Node root, Vector2 startPosition, float distance)
		{
			if (root == null)
				return foundNodes;

			if (Vector2.Distance(startPosition, root.house) <= distance)
				foundNodes.Add(root.house);

			GetAllNodesWithinDistance(foundNodes, root.left, startPosition, distance);
			GetAllNodesWithinDistance(foundNodes, root.right, startPosition, distance);

			return foundNodes;
		}
	}
}


		

