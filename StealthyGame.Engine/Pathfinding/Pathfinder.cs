using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Pathfinding
{
	class Pathfinder
	{
		/*
		 * function A*(start, goal)
				// The set of nodes already evaluated
				closedSet := {}

				// The set of currently discovered nodes that are not evaluated yet.
				// Initially, only the start node is known.
				openSet := {start}

				// For each node, which node it can most efficiently be reached from.
				// If a node can be reached from many nodes, cameFrom will eventually contain the
				// most efficient previous step.
				cameFrom := the empty map

				// For each node, the cost of getting from the start node to that node.
				gScore := map with default value of Infinity

				// The cost of going from start to start is zero.
				gScore[start] := 0

				// For each node, the total cost of getting from the start node to the goal
				// by passing by that node. That value is partly known, partly heuristic.
				fScore := map with default value of Infinity

				// For the first node, that value is completely heuristic.
				fScore[start] := heuristic_cost_estimate(start, goal)

				while openSet is not empty
					current := the node in openSet having the lowest fScore[] value
					if current = goal
						return reconstruct_path(cameFrom, current)

					openSet.Remove(current)
					closedSet.Add(current)

					for each neighbor of current
						if neighbor in closedSet
							continue		// Ignore the neighbor which is already evaluated.

						if neighbor not in openSet	// Discover a new node
							openSet.Add(neighbor)

						// The distance from start to a neighbor
						tentative_gScore := gScore[current] + dist_between(current, neighbor)
						if tentative_gScore >= gScore[neighbor]
							continue		// This is not a better path.

						// This path is the best until now. Record it!
						cameFrom[neighbor] := current
						gScore[neighbor] := tentative_gScore
						fScore[neighbor] := gScore[neighbor] + heuristic_cost_estimate(neighbor, goal)

				return failure

			function reconstruct_path(cameFrom, current)
				total_path := [current]
				while current in cameFrom.Keys:
					current := cameFrom[current]
					total_path.append(current)
				return total_path
		 */

		public static Path findPath(Node start, Node goal)
		{
			List<Node> closedSet = new List<Node>();
			List<Node> openSet = new List<Node>();
			openSet.Add(start);
			Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
			Dictionary<Node, float> gScore = new Dictionary<Node, float>();
			gScore.Add(start, 0);
			Dictionary<Node, float> fScore = new Dictionary<Node, float>();
			fScore.Add(start, heuristic_cost_estimate(start, goal));



			Node current = null;
			while(openSet.Count > 0)
			{
				fScore = fScore.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
				current = fScore.First().Key;
				fScore.Remove(current);
				if (current == goal)
					return reconstructPath(cameFrom, current);
				openSet.Remove(current);
				closedSet.Add(current);
				foreach (var neighbour in current.Neighbours)
				{
					if (closedSet.Contains(neighbour))
					{
						continue;
					}
					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
					////NOT WIKI
					if (!gScore.ContainsKey(neighbour))
						gScore.Add(neighbour, float.PositiveInfinity);
					if (!fScore.ContainsKey(neighbour))
						fScore.Add(neighbour, float.PositiveInfinity);
					////END NOT WIKI
					float tentativeGScore = gScore[current] + current.DistanceTo(neighbour);
					if (tentativeGScore >= gScore[neighbour])
						continue;
					cameFrom[neighbour] = current;
					gScore[neighbour] = tentativeGScore;
					fScore[neighbour] = gScore[neighbour] + heuristic_cost_estimate(neighbour, goal);
				}
			}

			return new Path(null);
		}

		static Path reconstructPath(Dictionary<Node, Node> cameFrom, Node current)
		{
			Path totalPath = new Path(current);
			while(cameFrom.ContainsKey(current))
			{
				current = cameFrom[current];
				totalPath.Add(current);
			}
			return totalPath;
		}

		static float heuristic_cost_estimate(Node start, Node goal)
		{
			return 0f;
			//float minusX = (start.Parent.Center.X - goal.Parent.Center.X);
			//minusX *= minusX;
			//float minusY = (start.Parent.Center.Y - goal.Parent.Center.Y);
			//minusY *= minusY;
			//return minusX + minusY;
		}
	}
}
