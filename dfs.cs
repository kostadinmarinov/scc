
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

        foreach (TVertex neighbour in getNeighbours(vertex))
        {
            if (!visited.Contains(neighbour))
            {
                yield return neighbour;
            }
        }
    }
}