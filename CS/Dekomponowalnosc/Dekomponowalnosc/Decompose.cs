
namespace Dekomponowalnosc;

class Decompose
{
    private int[,] _macierzIncydencji;

    public Decompose(int[,] macierzIncydencji)
    {
        this._macierzIncydencji = macierzIncydencji;
    }

    public void WypiszWszystkieDoskonalePonumerowania()
    {
        int n = this._macierzIncydencji.GetLength(0);
        int[] outDecompose = new int[n];
        HashSet<int> leftVertexes = new HashSet<int>(n);
        for (int i = 0; i < n; i++)
        {
            leftVertexes.Add(i);
        }

        while (leftVertexes.Count != 0) // Dopoki nie jest pusty // TODO Jesli nie jest dekomponowalny
        {
            int v = FindSiplicial(leftVertexes);
            outDecompose[leftVertexes.Count - 1] = v;
            leftVertexes.Remove(v);
        }

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine(outDecompose[i]);
        }
    }

    private int FindSiplicial(HashSet<int> leftVertexes)
    {
        // Jesli graf jest dekomponowalny, to jakis simplicialny musi byc
        foreach (int v in leftVertexes)
        {
            if (IsSimplicial(v, leftVertexes))
            {
                return (v);
            }
        }

        throw new Exception("Podany graf nie jest dekomponowalny");
    }

    private bool IsSimplicial(int v, HashSet<int> leftVertexes)
    {
        HashSet<int> vNeighbours = new HashSet<int>();
        foreach (int w in leftVertexes)
        {
            if (this._macierzIncydencji[v, w] == 1)
            {
                vNeighbours.Add(w);
            }
        }

        foreach (int w in vNeighbours)
        {
            foreach (int w2 in vNeighbours)
            {
                if (this._macierzIncydencji[w, w2] == 0 && w != w2)
                {
                    return false;
                }
            }
        }

        return true;
    }

    void MCS()
    {
        Console.WriteLine("MCS!");
    }
}