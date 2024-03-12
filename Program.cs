using System;
using app.Matrix;

namespace program
{
    public class Program
    {
        public delegate void Diagonalize(Matrix2D matrix);
        public static void Main(string[] args)
        {
            Diagonalize diagonalize = delegate (Matrix2D matrix)
            {
                for (int rowIndex = 1; rowIndex < matrix.RowsCount; ++rowIndex)
                {
                    for (int columnIndex = 0; columnIndex < rowIndex; ++columnIndex)
                    {
                        matrix[rowIndex, columnIndex] = 0;
                    }
                }
            };

            // todo menu here
        }
    }

}
