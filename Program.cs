using System;
using System.Collections.Generic;

namespace scc
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, IEnumerable<int>> g = new Dictionary<int, IEnumerable<int>>
            {
                { 1, new [] { 4, 2, 3,  } },
                { 2, new [] { 3 } },
                { 3, new [] { 1 } },
                { 4, new [] { 5 } },
                { 5, new [] { 4, 6 } },
                { 6, new [] { 4, } },
            };

            Scc<int> scc = new Scc<int>();
            scc.Find(g.Keys, v => g[v], (sccKey, sccVerticies, vs) => Console.WriteLine($"{sccKey}: {string.Join(", ", sccVerticies)}"));

            Dfs<int> dfs = new Dfs<int>();
            IEnumerable<int> traversed = dfs.Traverse(g.Keys, v => g[v]);
            Console.WriteLine($"DFS: {string.Join(", ", traversed)}");

            Console.Read();
        }
    }
}
