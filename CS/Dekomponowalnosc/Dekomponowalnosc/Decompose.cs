
using System.Runtime.InteropServices.JavaScript;

namespace Dekomponowalnosc;

class Decompose
{
    private int[,] _macierzIncydencji;

    public Decompose(int[,] macierzIncydencji)
    {
        _macierzIncydencji = macierzIncydencji;
    }

    public List<int[]> ZnajdzWszystkieDoskonalePonumerowania(int[]? outDecompose = null, HashSet<int>? leftVertexes = null)
    {
        // Zalozmy, ze leftVertexes jest niepusty
        
        // Funkcja przeglada graf w glab. Idzie do wierzcholka wtedy, gdy jest simplicjalny
        // Jak dojdzie do konca i nie ma juz wiecej wierzcholkow, to printuje swoja scieszke
        
        // Jesli dany graf nie bedzie dekomponowany, to po prostu nic sie nie wypisze
        
        // Implementacja nie jest optymalna, duzo niepotrzebnego kopjowania,
        // przegladania wielokrotnie tych sanych scierzek itp.
        
        if (leftVertexes == null) // poczatek rekurencji
        {
            int n = _macierzIncydencji.GetLength(0);
            outDecompose = new int[n];
            leftVertexes = new HashSet<int>(n);
            for (int i = 0; i < n; i++)
            {
                leftVertexes.Add(i);
            }
        }

        List<int[]> outDecompositions = new List<int[]>();

        List<int> vAll = FindAllSiplicial(leftVertexes);
        foreach (int v in vAll)
        {
            // Musze skopjowac, zeby kazda galas miala swoja wersje
            var myCopyOutDecompose = (int[])outDecompose!.Clone();
            var myCopyLeftVertexes = new HashSet<int>(leftVertexes);
            
            myCopyOutDecompose[myCopyLeftVertexes.Count - 1] = v;
            myCopyLeftVertexes.Remove(v);
            if (myCopyLeftVertexes.Count == 0) // Jestem juz na koncu DFS-a
            {
                outDecompositions.Add(myCopyOutDecompose);
            }
            else // Jeszcze z DFS-em musze pojsc glebiej
            {
                List<int[]> deeper = ZnajdzWszystkieDoskonalePonumerowania(myCopyOutDecompose, myCopyLeftVertexes);
                foreach (int[] decompositon in deeper)
                {
                    outDecompositions.Add(decompositon); // Tu jest sporo kopjowania, ale tylko referencji
                }
            }
        }

        return outDecompositions;
    }

    private List<int> FindAllSiplicial(HashSet<int> leftVertexes)
    {
        List<int> outTable = new List<int>();
        foreach (int v in leftVertexes)
        {
            if (IsSimplicial(v, leftVertexes))
            {
                outTable.Add(v);
            }
        }
        return outTable;
    }

    private bool IsSimplicial(int v, HashSet<int> leftVertexes)
    {
        HashSet<int> vNeighbours = new HashSet<int>();
        foreach (int w in leftVertexes)
        {
            if (_macierzIncydencji[v, w] == 1)
            {
                vNeighbours.Add(w);
            }
        }
        
        // vNeighbours to zbior sasiadow v

        foreach (int w in vNeighbours)
        {
            foreach (int w2 in vNeighbours)
            {
                if (_macierzIncydencji[w, w2] == 0 && w != w2)
                {
                    return false; // Sa sasiedzi v, ktorzy nie sa swoimi sasiadami
                }
            }
        }

        return true;
    }

    public int[]? MCS()
    {
        int n = _macierzIncydencji.GetLength(0);
        int[] outDecompose = new int[n];
        
        outDecompose[0] = 0; // Pierwszy jest jakikolwiek
        HashSet<int> found = new HashSet<int>(n){0};
        HashSet<int> notFound = new HashSet<int>(n);
        for (int i = 1; i < n; i++)
        {
            notFound.Add(i);
        }

        for (int wypelnione = 1; wypelnione < n; wypelnione++) // glowna pentla
        {
            // Znajdzmy bestVertex, czyli wierzcholek z najwiekrza liczba sasiadow wsord juz ponumerowanych
            int bestVertex = -1;
            int bigestNumberOfNeighbours = -1;
            int thisNumberOfNeighbours;
            
            foreach (int v in notFound)
            {
                thisNumberOfNeighbours = GetNumberOfFoundNeighbours(v, found);
                if (thisNumberOfNeighbours > bigestNumberOfNeighbours)
                {
                    bestVertex = v;
                    bigestNumberOfNeighbours = thisNumberOfNeighbours;
                }
            }
            // bestVertex znaleziony

            // Sprawdzmy, czy found \cup ne(bestVertex) jest klitka
            // Czyli, czy bestVertex jest simplicjalny
            if (!IsSimplicial(bestVertex, found))
            {
                return null;
            }
            
            outDecompose[wypelnione] = bestVertex;
            found.Add(bestVertex);
            notFound.Remove(bestVertex);
        }

        return outDecompose;
    }

    public int GetNumberOfFoundNeighbours(int v, HashSet<int> found)
    {
        int numberOfFoundNeighbours = 0;
        for (int i = 0; i < _macierzIncydencji.GetLength(0); i++)
        {
            if (_macierzIncydencji[v, i] == 1 && v != i && found.Contains(i))
            {
                numberOfFoundNeighbours++;
            }
        }

        return numberOfFoundNeighbours;
    }
}