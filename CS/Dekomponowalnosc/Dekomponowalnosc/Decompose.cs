// See https://aka.ms/new-console-template for more information

using System.Net.Mail;

class Decompose
{
    private int[,] macierzIncydencji;

    public Decompose(int[,] macierzIncydencji)
    {
        this.macierzIncydencji = macierzIncydencji;
    }

    public void wypiszWszystkieDoskonalePonumerowania()
    {
        int n = this.macierzIncydencji.GetLength(0);
        int[] outDecompose = new int[n];
        HashSet<int> leftVertexes = new HashSet<int>(n);
        for (int i = 0; i < n; i++)
        {
            leftVertexes.Add(i);
        }

        while (leftVertexes.Equals(new HashSet<int>(n))) // Dopoki nie jest pusty // TODO Jesli nie jest dekomponowalny
        {
            foreach (int v in leftVertexes) // To nie dizala xd
            {
                if (isSimplicial(v, leftVertexes))
                {
                    outDecompose[leftVertexes.Count - 1] = v;
                    leftVertexes.Remove(v);
                }
            }
        }

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine(outDecompose[i]);
        }
    }

    private bool isSimplicial(int v, HashSet<int> leftVertexes)
    {
        HashSet<int> vNeighbours = new HashSet<int>();
        foreach (int w in leftVertexes)
        {
            if (this.macierzIncydencji[v, w] == 1)
            {
                vNeighbours.Add(w);
            }
        }

        foreach (int w in vNeighbours)
        {
            foreach (int w2 in vNeighbours)
            {
                if (this.macierzIncydencji[w, w2] == 0)
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