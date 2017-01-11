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
			if (axis == 0) {
				//Axis 0 is considered the X axis in this KD tree
				if (building.X < house.X) {
					left = getNode (left, building, dimension);
				} else if (building.X > house.X) {
					right = getNode (right, building, dimension);
				} else if (building.Y < house.Y) {
					//If the X axises are equal, then the Y axises are compared
					left = getNode (left, building, dimension);
				} else if (building.Y > house.Y) {
					right = getNode (right, building, dimension);
				} 
			} else if (axis == 1) {
				// Axis 1 = Y
				if (building.Y < house.Y) {
					left = getNode (left, building, dimension);
				} else if (building.Y > house.Y) {
					right = getNode (right, building, dimension);
				} else if (building.X < house.X) {
					//If the Y axises are equal, then the X axises are compared
					left = getNode (left, building, dimension);
				} else if (building.X > house.X) {
					right = getNode (right, building, dimension);
				}
			} 
		}

		public Node getNode(Node node, Vector2 building, int dimension){
			if (node == null) {//If the node doesn't exist yet, we're done searching. 
				node = new Node (building, dimension);
			} else {
				node.AddNode (building, dimension);
			}
			return node;
		}

		public IEnumerable<Vector2> RangeSearch(IList<Vector2> foundNodes, Vector2 house, float distance){
			//Call the rangesearch method with the root node
			return RangeSearch (foundNodes, house, distance, this, 0);
		}

		public IEnumerable<Vector2> RangeSearch(IList<Vector2> foundNodes, Vector2 house, float distance, Node root, int dimension){
			if (root == null)  //If this node is null, stop the method
				return foundNodes;
			
			int axis = dimension % 2;

			//If this house is in range, we'll add it
			if(Vector2.Distance(house,root.house) <= distance)
				foundNodes.Add(root.house);

			//The dimension is plussed,so we can keep the knowledge of which axis we have to use
			dimension++;

			//The bounds which the houses in range should be within
			float leftBound = (house.X - distance); 
			float rightBound = (house.X + distance);
			float upperBound = (house.Y + distance);
			float downerBound = (house.Y - distance);

			if (axis == 0) { //X axis is 0
				if (root.house.X > leftBound && root.house.X < rightBound) { 
					//If the houses X axis is between the left and right bound, we need to continue our search to the left and right nodes
					RangeSearch (foundNodes, house, distance, root.left, dimension);
					RangeSearch (foundNodes, house, distance, root.right, dimension);
				} else if (root.house.X < leftBound) {
					//If the houses X axis is smaller than the left bound then we can ignore all the nodes left of it
					RangeSearch (foundNodes, house, distance, root.right, dimension);
				} else if (root.house.X > rightBound) {
					//If the houses X axis is greater than the right bound then we can ignore all the nodes right of it
					RangeSearch (foundNodes, house, distance, root.left, dimension);
				}
			} else {// Y axis is 1
				if (root.house.Y > downerBound && root.house.Y < upperBound) {
					//If the houses Y axis is between the left and right bound, we need to continue our search to the left and right nodes
					RangeSearch (foundNodes, house, distance, root.left, dimension);
					RangeSearch (foundNodes, house, distance, root.right, dimension);
				} else if (root.house.Y < downerBound) {
					//If the houses Y axis is smaller than the left bound then we can ignore all the nodes left of it
					RangeSearch (foundNodes, house, distance, root.right, dimension);
				} else if (root.house.Y > upperBound) {
					//If the houses Y axis is greater than the right bound then we can ignore all the nodes right of it
					RangeSearch (foundNodes, house, distance, root.left, dimension);
				}
			}
			return foundNodes;
		}

	}

	}

















/*
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

		}*/