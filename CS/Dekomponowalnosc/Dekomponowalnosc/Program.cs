using Dekomponowalnosc;

int[,] macierzIncydencji = new int[7, 7];
macierzIncydencji[0, 1] = 1;
macierzIncydencji[1, 0] = 1;
macierzIncydencji[0, 3] = 1;
macierzIncydencji[3, 0] = 1;
macierzIncydencji[0, 4] = 1;
macierzIncydencji[4, 0] = 1;
macierzIncydencji[1, 2] = 1;
macierzIncydencji[2, 1] = 1;
macierzIncydencji[3, 4] = 1;
macierzIncydencji[4, 3] = 1;
macierzIncydencji[3, 5] = 1;
macierzIncydencji[5, 3] = 1;
macierzIncydencji[3, 6] = 1;
macierzIncydencji[6, 3] = 1;
macierzIncydencji[4, 5] = 1;
macierzIncydencji[5, 4] = 1;
macierzIncydencji[5, 6] = 1;
macierzIncydencji[6, 5] = 1;


Decompose myDecompose = new Decompose(macierzIncydencji);
myDecompose.WypiszWszystkieDoskonalePonumerowania();

Console.WriteLine("\n\nMCS:");

int[]? outDecompose = myDecompose.MCS();

if (outDecompose == null)
{
    Console.WriteLine("Graf nie jest dekomponowalny");
}
else
{
    for (int i = 0; i < outDecompose.Length - 1; i++)
    {
        Console.Write($"{outDecompose[i]}, ");
    }
    Console.WriteLine($"{outDecompose[^1]}");
}

Console.WriteLine("Done");