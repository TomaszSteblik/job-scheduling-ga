namespace GeneticAlgorithm.Infrastructure;

public static class Extensions
{
    public static T[][] Transpose<T>(this T[][] arr1)
    {
        var arr2 = new T[arr1.First().Length][];
        for (int i = 0; i < arr2.Length; i++)
        {
            arr2[i] = new T[arr1.Length];
        }
        for (var i = 0; i < arr1.Length; i++) {
            for (var j = 0; j < arr1[i].Length; j++) {

                arr2[j][i] = arr1[i][j];
            }
        }

        return arr2;
    }
}