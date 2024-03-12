namespace app.Matrix.Extensions
{
    public static class Matrix2DExtensions
    {
        public static void Transpose(this Matrix2D matrix)
        {
            for (int rowIndex = 0; rowIndex < matrix.RowsCount; ++rowIndex)
            {
                for (int columnIndex = rowIndex + 1; columnIndex < matrix.RowsCount; ++columnIndex)
                {
                    double tempNumber = matrix[rowIndex, columnIndex];
                    matrix[rowIndex, columnIndex] = matrix[columnIndex, rowIndex];
                    matrix[columnIndex, rowIndex] = tempNumber;
                }
            }
        }

        public static double Trace(this Matrix2D matrix, int size)
        {
            double sum = 0;

            for (int rowIndex = 0; rowIndex < matrix.RowsCount; ++rowIndex)
            {
                sum += matrix[rowIndex, rowIndex];
            }

            return sum;
        }
    }
}
