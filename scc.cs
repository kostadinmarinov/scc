
using System;
using System.Collections.Generic;
using System.Linq;

public class Scc<TVertex>
{
    IDictionary<TVertex, int> preorderNums = new Dictionary<TVertex, int>();
    Stack<IEnumerator<TVertex>> stack = new Stack<IEnumerator<TVertex>>();
    Stack<TVertex> stack1 = new Stack<TVertex>();
    Stack<int> stack2 = new Stack<int>();
    IDictionary<TVertex, int> verticiesToSccs = new Dictionary<TVertex, int>();
    Func<TVertex, IEnumerable<TVertex>> getNeighbours;
    int sccKey;
    Action<int, IList<TVertex>, IDictionary<TVertex, int>> createScc;

    public void Find(IEnumerable<TVertex> verticies, Func<TVertex, IEnumerable<TVertex>> getNeighbours, Action<int, IList<TVertex>, IDictionary<TVertex, int>> createScc)
    {
        this.getNeighbours = getNeighbours;
        this.createScc = createScc;

        // FindRecursive(verticies);
        FindIterative(verticies);
    }

    private void FindRecursive(IEnumerable<TVertex> verticies)
    {
        foreach (TVertex vertex in verticies)
        {
            if (!preorderNums.ContainsKey(vertex))
            {
                FindRecursive(vertex);
            }
        }
    }

    private void FindRecursive(TVertex vertex)
    {
        stack1.Push(vertex);
        int preorderNum = stack1.Count;

        preorderNums.Add(vertex, preorderNum);
        stack2.Push(preorderNum);

        foreach (TVertex neighbour in getNeighbours(vertex))
        {
            int neighbourPreorderNum;

            if (!preorderNums.TryGetValue(neighbour, out neighbourPreorderNum))
            {
                FindRecursive(neighbour);
            }
            else if (!verticiesToSccs.ContainsKey(neighbour))
            {
                while (neighbourPreorderNum < stack2.Peek())
                {
                    stack2.Pop();
                }
            }
        }

        if (preorderNum == stack2.Peek())
        {
            stack2.Pop();
            sccKey++;
            IList<TVertex> scc = new List<TVertex>();

            while (preorderNum <= stack1.Count)
            {
                TVertex v = stack1.Pop();
                scc.Add(v);
                verticiesToSccs.Add(v, sccKey);
            }

            createScc(sccKey, scc, verticiesToSccs);
        }
    }

    private void FindIterative(IEnumerable<TVertex> verticies)
    {
        verticies
            .Where(vertex => !preorderNums.ContainsKey(vertex))
            .RecursiveSelect(FindNext)
            .Count();
    }

    private IEnumerable<TVertex> FindNext(TVertex vertex)
    {
        stack1.Push(vertex);
        int preorderNum = stack1.Count;

        preorderNums.Add(vertex, preorderNum);
        stack2.Push(preorderNum);

        foreach (TVertex neighbour in getNeighbours(vertex))
        {
            int neighbourPreorderNum;

            if (!preorderNums.TryGetValue(neighbour, out neighbourPreorderNum))
            {
                yield return neighbour;
            }
            else if (!verticiesToSccs.ContainsKey(neighbour))
            {
                while (neighbourPreorderNum < stack2.Peek())
                {
                    stack2.Pop();
                }
            }
        }

        if (preorderNum == stack2.Peek())
        {
            stack2.Pop();
            sccKey++;
            IList<TVertex> scc = new List<TVertex>();

            while (preorderNum <= stack1.Count)
            {
                TVertex v = stack1.Pop();
                scc.Add(v);
                verticiesToSccs.Add(v, sccKey);
            }

            createScc(sccKey, scc, verticiesToSccs);
        }
    }
}