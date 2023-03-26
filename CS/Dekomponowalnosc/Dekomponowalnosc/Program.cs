// See https://aka.ms/new-console-template for more information

int[,] macierzIncydencji = new int[3, 3];
macierzIncydencji[0, 1] = 1;
macierzIncydencji[1, 0] = 1;
macierzIncydencji[1, 2] = 1;
macierzIncydencji[2, 1] = 1;


Decompose myDecompose = new Decompose(macierzIncydencji);
myDecompose.wypiszWszystkieDoskonalePonumerowania();