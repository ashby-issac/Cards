using System.Linq;

public static class CardUtility
{
    public static T[,] ConvertTo2D<T>(T[][] jagged)
    {
        if (jagged == null || jagged.Length == 0)
            return new T[0, 0];

        int rows = jagged.Length;
        int cols = jagged.Max(r => r?.Length ?? 0);

        T[,] result = new T[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            if (jagged[i] == null) continue;

            for (int j = 0; j < jagged[i].Length; j++)
            {
                result[i, j] = jagged[i][j];
            }
        }

        return result;
    }

    public static void ConvertToJagged<T>(T[,] array, out T[][] jagged)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        jagged = new T[rows][];

        for (int i = 0; i < rows; i++)
        {
            jagged[i] = new T[cols];
            for (int j = 0; j < cols; j++)
            {
                jagged[i][j] = array[i, j];
            }
        }

    }

}
