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
		public int dimension;
		public Node ()
		{
		}
		public Node (Vector2 house)
		{
			this.house = house;
		}

		public Node (Vector2 house, int dimension)
		{
			this.dimension = dimension;
			this.house = house;
			AddNode (house, dimension);
		}
	

		public void AddNode(Vector2 building, int dimension){
			int axis = dimension % 2;
			dimension++;
			if (house == new Vector2 (0, 0)) {
				house = building;
			} 

			if (axis == 0) {
				if (building.X < house.X) {
					left = getNode (left, building, dimension);
				} else if (building.X > house.X) {
					right = getNode (right, building, dimension);
				} else if (building.Y < house.Y) {
					left = getNode (left, building, dimension);
				} else if (building.Y > house.Y) {
					right = getNode (right, building, dimension);
				} 
			} else if (axis == 1) {
				if (building.Y < house.Y) {
					left = getNode (left, building, dimension);
				} else if (building.Y > house.Y) {
					right = getNode (right, building, dimension);
				} else if (building.X < house.X) {
					left = getNode (left, building, dimension);
				} else if (building.X > house.X) {
					right = getNode (right, building, dimension);
				}
			} 
		}
		public Node getNode(Node node, Vector2 building, int dimension){
			if (node == null) {
				node = new Node (building, dimension);
			} else {
				node.AddNode (building, dimension);
			}
			return node;
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


		

