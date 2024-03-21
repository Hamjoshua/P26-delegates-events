using System;
using System.Text.RegularExpressions;

namespace app.Matrix
{
    [Serializable]
    public class Matrix2D : IComparable, IComparable<Matrix2D>
    {


        private double[,] _items;
        public int RowsCount
        {
            get
            {
                return _items.GetLength(0);
            }
        }
        public int ColumnsCount
        {
            get
            {
                return _items.GetLength(1);
            }
        }

        public double[] this[int rowIndex]
        {
            get
            {
                double[] ThisRow = new double[ColumnsCount];

                for (int columnIndex = 0; columnIndex < ColumnsCount; ++columnIndex)
                {
                    ThisRow[columnIndex] = _items[rowIndex, columnIndex];
                }
                return ThisRow;
            }

            set
            {
                if (value.GetLength(0) == ColumnsCount)
                {
                    for (int columnIndex = 0; columnIndex < ColumnsCount; ++columnIndex)
                    {
                        this[rowIndex, columnIndex] = value[columnIndex];
                    };
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public double this[int rowIndex, int columnIndex]
        {
            get
            {
                return _items[rowIndex, columnIndex];
            }

            set
            {
                _items[rowIndex, columnIndex] = value;
            }
        }

        // todo 
        public Matrix2D Clone()
        {
            Matrix2D NewMatrix = new Matrix2D(RowsCount, ColumnsCount);
            for (int rowIndex = 0; rowIndex < RowsCount; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < ColumnsCount; ++columnIndex)
                {
                    NewMatrix[rowIndex, columnIndex] = this[rowIndex, columnIndex];
                }
            }
            return NewMatrix;
        }

        public override string ToString()
        {
            string StringMatrix = "[";
            for (int rowIndex = 0; rowIndex < RowsCount; ++rowIndex)
            {
                double[] Columns = this[rowIndex];

                StringMatrix += "[";
                for (int columnIndex = 0; columnIndex < ColumnsCount; ++columnIndex)
                {
                    StringMatrix += Columns[columnIndex].ToString();
                    if (columnIndex < ColumnsCount - 1)
                    {
                        StringMatrix += ",";
                    }
                }
                StringMatrix += "]";

                if (rowIndex < RowsCount - 1)
                {
                    StringMatrix += ",";
                }
            }
            StringMatrix += "]";

            return StringMatrix;
        }

        public int CompareTo(object obj)
        {
            return 0;
        }

        public int CompareTo(Matrix2D matrix)
        {
            if (matrix is null)
            {
                return 1;
            }

            double ThisMatrixDeterminant = this.GetDeterminant();
            double OtherMatrixDeterminant = matrix.GetDeterminant();

            if (ThisMatrixDeterminant > OtherMatrixDeterminant)
            {
                return 1;
            }
            else if (ThisMatrixDeterminant < OtherMatrixDeterminant)
            {
                return -1;
            }
            return 0;
        }

        public static int Compare(Matrix2D first, Matrix2D second)
        {
            return first.CompareTo(second);
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix2D)
            {
                Matrix2D matrix = obj as Matrix2D;
                for (int rowIndex = 0; rowIndex < matrix.RowsCount; ++rowIndex)
                {
                    if (this[rowIndex] != matrix[rowIndex])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Matrix2D(int rowsCount, int columnsCount)
        {
            _items = new double[rowsCount, columnsCount];
            for (int rowIndex = 0; rowIndex < rowsCount; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < columnsCount; ++columnIndex)
                {
                    _items[rowIndex, columnIndex] = 0;
                }
            }
        }

        static public Matrix2D ParseMatrix(string parseString)
        {
            try
            {
                Matrix2D matrix = new Matrix2D(parseString);
                return matrix;
            }
            catch
            {
                return null;
            }

        }

        private Matrix2D(string parseString)
        {
            var stringRows = Regex.Matches(parseString, @"(\b[^\[]+\b)");
            _items = new double[stringRows.Count, stringRows.Count];

            for (int rowIndex = 0; rowIndex < stringRows.Count; ++rowIndex)
            {
                string stringRow = stringRows[rowIndex].Value;
                string[] splittedNumbers = stringRow.Split(',');
                for (int columnIndex = 0; columnIndex < stringRows.Count; ++columnIndex)
                {
                    double parsedNumber = Convert.ToDouble(splittedNumbers[columnIndex]);
                    _items[rowIndex, columnIndex] = parsedNumber;
                }
            }
        }

        public Matrix2D(int rowsCount, int columnsCount, int maxNumberForRandom)
        {
            Random RandomObject = new Random();
            _items = new double[rowsCount, columnsCount];
            for (int rowIndex = 0; rowIndex < rowsCount; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < columnsCount; ++columnIndex)
                {
                    double RandomNumber = RandomObject.Next(maxNumberForRandom);
                    _items[rowIndex, columnIndex] = RandomNumber;
                }
            }
        }

        public double[] GetColumn(int columnIndex)
        {
            double[] ThisColumn = new double[RowsCount];
            for (int rowIndex = 0; rowIndex < this.RowsCount; ++rowIndex)
            {
                double CurrentColumnElement = this[rowIndex, columnIndex];
                ThisColumn[rowIndex] = CurrentColumnElement;
            }
            return ThisColumn;
        }

        private static Matrix2D UniteMatrixes(Matrix2D firstMatrix, Matrix2D secondMatrix, bool plus)
        {
            if (firstMatrix.ColumnsCount != secondMatrix.ColumnsCount || firstMatrix.RowsCount != secondMatrix.RowsCount)
            {
                throw new DifferentMatrixesException("Матрицы не совпадают по размеру");
            }
            Matrix2D NewMatrix = firstMatrix.Clone();
            for (int rowIndex = 0; rowIndex < firstMatrix.RowsCount; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < firstMatrix.ColumnsCount; ++columnIndex)
                {
                    double NewValue = 0;
                    double FirstElement = firstMatrix[rowIndex, columnIndex];
                    double SecondElement = secondMatrix[rowIndex, columnIndex];

                    if (plus)
                    {
                        NewValue = FirstElement + SecondElement;
                    }
                    else
                    {
                        NewValue = FirstElement - SecondElement;
                    }

                    firstMatrix[rowIndex, columnIndex] = NewValue;
                }
            }

            return firstMatrix;

        }

        public static Matrix2D operator +(Matrix2D firstMatrix, Matrix2D secondMatrix)
        {
            return UniteMatrixes(firstMatrix, secondMatrix, true);
        }

        public static Matrix2D operator -(Matrix2D firstMatrix, Matrix2D secondMatrix)
        {
            return UniteMatrixes(firstMatrix, secondMatrix, false);
        }

        public static Matrix2D operator *(Matrix2D matrix, double number)
        {
            Matrix2D NewMatrix = matrix.Clone();
            for (int rowIndex = 0; rowIndex < matrix.RowsCount; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < matrix.ColumnsCount; ++columnIndex)
                {
                    NewMatrix[rowIndex, columnIndex] *= number;
                }
            }
            return NewMatrix;
        }

        public static Matrix2D operator /(Matrix2D matrix, double number)
        {
            Matrix2D NewMatrix = matrix.Clone();
            for (int rowIndex = 0; rowIndex < matrix.RowsCount; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < matrix.ColumnsCount; ++columnIndex)
                {
                    NewMatrix[rowIndex, columnIndex] /= number;
                }
            }
            return NewMatrix;
        }

        public static explicit operator bool(Matrix2D matrix)
        {
            return matrix.RowsCount == matrix.ColumnsCount;
        }

        public static bool operator true(Matrix2D matrix)
        {
            return matrix.RowsCount == matrix.ColumnsCount;
        }

        public static bool operator false(Matrix2D matrix)
        {
            return !(matrix.RowsCount == matrix.ColumnsCount);
        }

        public static Matrix2D operator *(Matrix2D firstMatrix, Matrix2D secondMatrix)
        {
            if (firstMatrix.ColumnsCount != secondMatrix.RowsCount)
            {
                throw new DifferentMatrixesException("Матрицы не совпадают по размеру");
            }

            Matrix2D NewMatrix = new Matrix2D(firstMatrix.RowsCount, secondMatrix.ColumnsCount);

            for (int FirstrowIndex = 0; FirstrowIndex < firstMatrix.RowsCount; ++FirstrowIndex)
            {
                double[] FirstRow = firstMatrix[FirstrowIndex];

                for (int SecondcolumnIndex = 0; SecondcolumnIndex < secondMatrix.ColumnsCount; ++SecondcolumnIndex)
                {
                    double[] SecondColumn = secondMatrix.GetColumn(SecondcolumnIndex);
                    double SumElement = 0;

                    for (int CurrentElementIndex = 0; CurrentElementIndex < secondMatrix.RowsCount; ++CurrentElementIndex)
                    {
                        double CurrentElement = FirstRow[CurrentElementIndex] * SecondColumn[CurrentElementIndex];
                        SumElement += CurrentElement;
                    }

                    NewMatrix[FirstrowIndex, SecondcolumnIndex] = SumElement;
                }
            }

            return NewMatrix;
        }

        public static bool operator ==(Matrix2D firstMatrix, Matrix2D secondMatrix)
        {
            return Equals(firstMatrix, secondMatrix);
        }

        public static bool operator !=(Matrix2D firstMatrix, Matrix2D secondMatrix)
        {
            return !Equals(firstMatrix, secondMatrix);
        }

        public static bool operator >(Matrix2D firstMatrix, Matrix2D secondMatrix)
        {
            return firstMatrix.CompareTo(secondMatrix) == 1;
        }

        public static bool operator <(Matrix2D firstMatrix, Matrix2D secondMatrix)
        {
            return firstMatrix.CompareTo(secondMatrix) == -1;
        }

        public static bool operator >=(Matrix2D firstMatrix, Matrix2D secondMatrix)
        {
            int Result = firstMatrix.CompareTo(secondMatrix);
            return Result == 1 || Result == 0;
        }

        public static bool operator <=(Matrix2D firstMatrix, Matrix2D secondMatrix)
        {
            int Result = firstMatrix.CompareTo(secondMatrix);
            return Result == -1 || Result == 0;
        }

        public Matrix2D RemoveRowAt(int ForbiddenrowIndex)
        {
            Matrix2D NewMatrix = new Matrix2D(RowsCount - 1, ColumnsCount);
            bool ForbiddenrowIndexDetected = false;
            for (int rowIndex = 0; rowIndex < RowsCount; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < ColumnsCount; ++columnIndex)
                {
                    if (rowIndex != ForbiddenrowIndex)
                    {
                        double ValueFromOldMatrix = this[rowIndex, columnIndex];
                        if (ForbiddenrowIndexDetected)
                        {
                            NewMatrix[rowIndex - 1, columnIndex] = ValueFromOldMatrix;
                        }
                        else
                        {
                            NewMatrix[rowIndex, columnIndex] = ValueFromOldMatrix;
                        }
                    }
                    else
                    {
                        ForbiddenrowIndexDetected = true;
                        break;
                    }
                }
            }
            return NewMatrix;
        }

        public Matrix2D RemoveColumnAt(int ForbiddencolumnIndex)
        {
            Matrix2D NewMatrix = new Matrix2D(RowsCount, ColumnsCount - 1);
            bool ForbiddencolumnIndexDetected = false;
            for (int rowIndex = 0; rowIndex < RowsCount; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < ColumnsCount; ++columnIndex)
                {
                    if (columnIndex != ForbiddencolumnIndex)
                    {
                        double ValueFromOldMatrix = this[rowIndex, columnIndex];
                        if (ForbiddencolumnIndexDetected)
                        {
                            NewMatrix[rowIndex, columnIndex - 1] = ValueFromOldMatrix;
                        }
                        else
                        {
                            NewMatrix[rowIndex, columnIndex] = ValueFromOldMatrix;
                        }
                    }
                    else
                    {
                        ForbiddencolumnIndexDetected = true;
                    }
                }
                ForbiddencolumnIndexDetected = false;
            }
            return NewMatrix;
        }

        private static Matrix2D GetSubMatrix(int columnIndex, Matrix2D matrix)
        {
            Matrix2D NewMatrix = matrix.Clone();
            NewMatrix = NewMatrix.RemoveRowAt(0);
            NewMatrix = NewMatrix.RemoveColumnAt(columnIndex);

            return NewMatrix;
        }

        private static double _GetDeterminant(Matrix2D Matrix)
        {
            double Determinant = 0;

            if (Matrix.RowsCount == 1)
            {
                Determinant = Matrix[0, 0];
            }
            else if (Matrix.RowsCount == 2)
            {
                Determinant = Matrix[0, 0] * Matrix[1, 1] - Matrix[0, 1] * Matrix[1, 0];
            }
            else
            {
                for (int columnIndex = 0; columnIndex < Matrix.ColumnsCount; ++columnIndex)
                {
                    double Minor = Math.Pow(-1, columnIndex);
                    double ColumnNumber = Minor * Matrix[0, columnIndex];
                    Matrix2D SubMatrix = GetSubMatrix(columnIndex, Matrix);

                    Determinant += ColumnNumber * _GetDeterminant(SubMatrix);
                }
            }

            return Determinant;
        }

        public Matrix2D Reverse()
        {
            if (GetDeterminant() > 0)
            {
                Matrix2D OnesMatrix = new Matrix2D(RowsCount, RowsCount);
                for (int RowcolumnIndex = 0; RowcolumnIndex < RowsCount; ++RowcolumnIndex)
                {
                    OnesMatrix[RowcolumnIndex, RowcolumnIndex] = 1;
                }

                int rowIndex = 0;
                while (rowIndex < RowsCount + 1)
                {

                    if (rowIndex != 0)
                    {
                        int PreviouscolumnIndex = rowIndex - 1;
                        for (int NextrowIndex = 0; NextrowIndex < RowsCount; ++NextrowIndex)
                        {
                            if (NextrowIndex == PreviouscolumnIndex)
                            {
                                continue;
                            }
                            double PreviousElement = this[NextrowIndex, PreviouscolumnIndex];
                            for (int columnIndex = 0; columnIndex < ColumnsCount; ++columnIndex)
                            {
                                this[NextrowIndex, columnIndex] -= this[PreviouscolumnIndex, columnIndex] * PreviousElement;
                                OnesMatrix[NextrowIndex, columnIndex] -= OnesMatrix[PreviouscolumnIndex, columnIndex] * PreviousElement;
                            }
                        }
                    }

                    if (rowIndex == RowsCount)
                    {
                        break;
                    }
                    double LeadElement = this[rowIndex, rowIndex];

                    Matrix2D MatrixDividedOnLeadElement = this / LeadElement;
                    this[rowIndex] = MatrixDividedOnLeadElement[rowIndex];

                    Matrix2D OnesDividedOnLeadElement = OnesMatrix / LeadElement;
                    OnesMatrix[rowIndex] = OnesDividedOnLeadElement[rowIndex];

                    rowIndex++;
                }
                return OnesMatrix;
            }

            return this;
        }

        public double GetDeterminant()
        {
            return _GetDeterminant(this);
        }

    }
}
