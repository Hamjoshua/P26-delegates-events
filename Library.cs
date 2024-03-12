using System;

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

        public double[] this[int RowIndex]
        {
            get
            {
                double[] ThisRow = new double[ColumnsCount];

                for(int ColumnIndex = 0; ColumnIndex < ColumnsCount; ++ColumnIndex)
                {
                    ThisRow[ColumnIndex] = _items[RowIndex, ColumnIndex];
                }
                return ThisRow;
            }

            set
            {
                if (value.GetLength(0) == ColumnsCount)
                {
                    for (int ColumnIndex = 0; ColumnIndex < ColumnsCount; ++ColumnIndex) 
                    {
                        this[RowIndex, ColumnIndex] = value[ColumnIndex];
                    };
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public double this[int RowIndex, int ColumnIndex]
        {
            get
            {
                return _items[RowIndex, ColumnIndex];
            }

            set
            {
                _items[RowIndex, ColumnIndex] = value;
            }
        }

        // todo 
        public Matrix2D Clone()
        {
            Matrix2D NewMatrix = new Matrix2D(RowsCount, ColumnsCount);
            for(int RowIndex = 0; RowIndex < RowsCount; ++RowIndex)
            {
                for(int ColumnIndex = 0; ColumnIndex < ColumnsCount; ++ColumnIndex)
                {
                    NewMatrix[RowIndex, ColumnIndex] = this[RowIndex, ColumnIndex];
                }
            }
            return NewMatrix;
        }

        public override string ToString()
        {
            string StringMatrix = "[";
            for (int RowIndex = 0; RowIndex < RowsCount; ++RowIndex)
            {
                double[] Columns = this[RowIndex];

                StringMatrix += "[";
                for (int ColumnIndex = 0; ColumnIndex < ColumnsCount; ++ColumnIndex)
                {
                    StringMatrix += Columns[ColumnIndex].ToString();
                    if (ColumnIndex < ColumnsCount - 1)
                    {
                        StringMatrix += ", ";
                    }                    
                }
                StringMatrix += "]";

                if (RowIndex < RowsCount - 1)
                {
                    StringMatrix += ",\n";
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
                for (int RowIndex = 0; RowIndex < matrix.RowsCount; ++RowIndex)
                {
                    if (this[RowIndex] != matrix[RowIndex])
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
            for (int RowIndex = 0; RowIndex < rowsCount; ++RowIndex)
            {
                for (int ColumnIndex = 0; ColumnIndex < columnsCount; ++ColumnIndex)
                {
                    _items[RowIndex, ColumnIndex] = 0;
                }
            }
        }

        public Matrix2D(int rowsCount, int columnsCount, int maxNumberForRandom)
        {
            Random RandomObject = new Random();
            _items = new double[rowsCount, columnsCount];
            for (int RowIndex = 0; RowIndex < rowsCount; ++RowIndex)
            {
                for (int ColumnIndex = 0; ColumnIndex < columnsCount; ++ColumnIndex)
                {
                    double RandomNumber = RandomObject.Next(maxNumberForRandom);
                    _items[RowIndex, ColumnIndex] = RandomNumber;
                }
            }
        }

        public double[] GetColumn(int ColumnIndex)
        {
            double[] ThisColumn = new double[RowsCount];
            for (int RowIndex = 0; RowIndex < this.RowsCount; ++RowIndex)
            {
                double CurrentColumnElement = this[RowIndex, ColumnIndex];
                ThisColumn[RowIndex] = CurrentColumnElement;
            }
            return ThisColumn;
        }

        private static Matrix2D UniteMatrixes(Matrix2D firstMatrix, Matrix2D secondMatrix, bool plus)
        {
            if (firstMatrix.ColumnsCount != secondMatrix.ColumnsCount || firstMatrix.RowsCount != secondMatrix.RowsCount)
            {
                throw new DifferentMatrixesException("Матрицы не совпадают по размеру");
            }

            for (int RowIndex = 0; RowIndex < firstMatrix.RowsCount; ++RowIndex)
            {
                for (int ColumnIndex = 0; ColumnIndex < firstMatrix.ColumnsCount; ++ColumnIndex)
                {
                    double NewValue = 0;
                    double FirstElement = firstMatrix[RowIndex, ColumnIndex];
                    double SecondElement = secondMatrix[RowIndex, ColumnIndex];

                    if (plus)
                    {
                        NewValue = FirstElement + SecondElement;
                    }
                    else
                    {
                        NewValue = FirstElement - SecondElement;
                    }

                    firstMatrix[RowIndex, ColumnIndex] = NewValue;
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
            for (int RowIndex = 0; RowIndex < matrix.RowsCount; ++RowIndex)
            {
                for (int ColumnIndex = 0; ColumnIndex < matrix.ColumnsCount; ++ColumnIndex)
                {
                    NewMatrix[RowIndex, ColumnIndex] *= number;
                }
            }
            return NewMatrix;
        }

        public static Matrix2D operator /(Matrix2D matrix, double number)
        {
            Matrix2D NewMatrix = matrix.Clone();
            for (int RowIndex = 0; RowIndex < matrix.RowsCount; ++RowIndex)
            {
                for (int ColumnIndex = 0; ColumnIndex < matrix.ColumnsCount; ++ColumnIndex)
                {
                    NewMatrix[RowIndex, ColumnIndex] /= number;
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

            for (int FirstRowIndex = 0; FirstRowIndex < firstMatrix.RowsCount; ++FirstRowIndex)
            {
                double[] FirstRow = firstMatrix[FirstRowIndex];

                for (int SecondColumnIndex = 0; SecondColumnIndex < secondMatrix.ColumnsCount; ++SecondColumnIndex)
                {
                    double[] SecondColumn = secondMatrix.GetColumn(SecondColumnIndex);
                    double SumElement = 0;

                    for (int CurrentElementIndex = 0; CurrentElementIndex < secondMatrix.RowsCount; ++CurrentElementIndex)
                    {
                        double CurrentElement = FirstRow[CurrentElementIndex] * SecondColumn[CurrentElementIndex];
                        SumElement += CurrentElement;
                    }

                    NewMatrix[FirstRowIndex, SecondColumnIndex] = SumElement;
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

        public Matrix2D RemoveRowAt(int ForbiddenRowIndex)
        {
            Matrix2D NewMatrix = new Matrix2D(RowsCount - 1, ColumnsCount);
            bool ForbiddenRowIndexDetected = false;
            for(int RowIndex = 0; RowIndex < RowsCount; ++RowIndex)
            {
                for(int ColumnIndex = 0; ColumnIndex < ColumnsCount; ++ColumnIndex)
                {
                    if(RowIndex != ForbiddenRowIndex)
                    {
                        double ValueFromOldMatrix = this[RowIndex, ColumnIndex];
                        if (ForbiddenRowIndexDetected)
                        {
                            NewMatrix[RowIndex - 1, ColumnIndex] = ValueFromOldMatrix;
                        }
                        else
                        {
                            NewMatrix[RowIndex, ColumnIndex] = ValueFromOldMatrix;
                        }
                    }
                    else
                    {
                        ForbiddenRowIndexDetected = true;
                        break;
                    }
                }
            }
            return NewMatrix;
        }

        public Matrix2D RemoveColumnAt(int ForbiddenColumnIndex)
        {
            Matrix2D NewMatrix = new Matrix2D(RowsCount, ColumnsCount - 1);
            bool ForbiddenColumnIndexDetected = false;
            for (int RowIndex = 0; RowIndex < RowsCount; ++RowIndex)
            {
                for (int ColumnIndex = 0; ColumnIndex < ColumnsCount; ++ColumnIndex)
                {
                    if (ColumnIndex != ForbiddenColumnIndex)
                    {
                        double ValueFromOldMatrix = this[RowIndex, ColumnIndex];
                        if (ForbiddenColumnIndexDetected)
                        {
                            NewMatrix[RowIndex, ColumnIndex - 1] = ValueFromOldMatrix;
                        }
                        else
                        {
                            NewMatrix[RowIndex, ColumnIndex] = ValueFromOldMatrix;
                        }
                    }
                    else
                    {
                        ForbiddenColumnIndexDetected = true;
                    }
                }
                ForbiddenColumnIndexDetected = false;
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
                for (int ColumnIndex = 0; ColumnIndex < Matrix.ColumnsCount; ++ColumnIndex)
                {
                    double Minor = Math.Pow(-1, ColumnIndex);
                    double ColumnNumber = Minor * Matrix[0, ColumnIndex];
                    Matrix2D SubMatrix = GetSubMatrix(ColumnIndex, Matrix);

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
                for (int RowColumnIndex = 0; RowColumnIndex < RowsCount; ++RowColumnIndex)
                {
                    OnesMatrix[RowColumnIndex, RowColumnIndex] = 1;
                }

                int RowIndex = 0;
                while (RowIndex < RowsCount + 1)
                {

                    if (RowIndex != 0)
                    {
                        int PreviousColumnIndex = RowIndex - 1;
                        for (int NextRowIndex = 0; NextRowIndex < RowsCount; ++NextRowIndex)
                        {
                            if (NextRowIndex == PreviousColumnIndex)
                            {
                                continue;
                            }
                            double PreviousElement = this[NextRowIndex, PreviousColumnIndex];
                            for (int ColumnIndex = 0; ColumnIndex < ColumnsCount; ++ColumnIndex)
                            {
                                this[NextRowIndex, ColumnIndex] -= this[PreviousColumnIndex, ColumnIndex] * PreviousElement;
                                OnesMatrix[NextRowIndex, ColumnIndex] -= OnesMatrix[PreviousColumnIndex, ColumnIndex] * PreviousElement;
                            }
                        }
                    }

                    if (RowIndex == RowsCount)
                    {
                        break;
                    }
                    double LeadElement = this[RowIndex, RowIndex];

                    Matrix2D MatrixDividedOnLeadElement = this / LeadElement;
                    this[RowIndex] = MatrixDividedOnLeadElement[RowIndex];

                    Matrix2D OnesDividedOnLeadElement = OnesMatrix / LeadElement;
                    OnesMatrix[RowIndex] = OnesDividedOnLeadElement[RowIndex];

                    RowIndex++;
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
