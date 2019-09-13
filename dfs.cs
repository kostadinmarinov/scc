
using System;
using System.Collections.Generic;
using System.Linq;

public class Dfs<TVertex>
{
    ISet<TVertex> visited = new HashSet<TVertex>();
    Func<TVertex, IEnumerable<TVertex>> getNeighbours;

    public IEnumerable<TVertex> Traverse(IEnumerable<TVertex> verticies, Func<TVertex, IEnumerable<TVertex>> getNeighbours)
    {
        this.getNeighbours = getNeighbours;

        return verticies
            .Where(vertex => !visited.Contains(vertex))
            .RecursiveSelect(TraverseNext);
    }

    private IEnumerable<TVertex> TraverseNext(TVertex vertex)
    {
        visited.Add(vertex);
        return getNeighbours(vertex).Where(neighbour => !visited.Contains(neighbour));
    }

    public IEnumerable<TVertex> Traverse2(IEnumerable<TVertex> verticies, Func<TVertex, IList<TVertex>> getNeighbours)
    {
        IList<TVertex> visited = new List<TVertex>();
        Stack<Tuple<TVertex, int>> stack = new Stack<Tuple<TVertex, int>>();

        foreach (TVertex entryVertex in verticies)
        {
            if (!visited.Contains(entryVertex))
            {
                stack.Push(new Tuple<TVertex, int>(entryVertex, -1));

                while (stack.Count > 0)
                {
                    Tuple<TVertex, int> context = stack.Pop();
                    TVertex vertex = context.Item1;
                    int i = context.Item2;

                    if (i == -1)
                    {
                        visited.Add(vertex);
                        i = 0;
                    }

                    IList<TVertex> neighbours = getNeighbours(vertex);

                    for(; i < neighbours.Count; i++)
                    {
                        if (!visited.Contains(neighbours[i]))
                        {
                            stack.Push(new Tuple<TVertex, int>(vertex, i));
                            stack.Push(new Tuple<TVertex, int>(neighbours[i], - 1));
                            break;
                        }
                    }
                }
            }
        }

        return visited;
    }
}