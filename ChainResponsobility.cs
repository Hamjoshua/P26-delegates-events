using app.Matrix;
using app.Matrix.Extensions;
using System;

namespace app.Chain
{
    public delegate void Diagonalize(Matrix2D matrix);

    public abstract class IEvent
    {
        public object[] Args;
    }

    public class AppEvent : IEvent
    {
        public AppEvent(string request)
        {
            string[] args = request.Split();
            Args = args;
        }
    }

    public abstract class BaseHandler
    {
        public BaseHandler() { Next = null; }
        public abstract object Execute(IEvent @event);

        public virtual void Handle(IEvent @event)
        {
            if (@event.Args.Length == ArgsCount)
            {
                object resultObject = Execute(@event);
                if (resultObject != null)
                {
                    Console.WriteLine($"Результат операции: {resultObject}");
                    return;
                }

            }

            Console.WriteLine("Sending event to next Handler...");
            if (Next != null)
            {
                Next.Handle(@event);
            }
            else
            {
                Console.WriteLine("Unknown event. Can't handle.");
            }

        }
        protected void SetNextHandler(BaseHandler newHandler)
        {
            Next = newHandler;
        }

        protected BaseHandler Next { get; set; }

        protected int ArgsCount;
    }

    public class OneArgHandler : BaseHandler
    {
        public OneArgHandler()
        {
            ArgsCount = 1;
            Next = new TwoArgsHandler();
        }
        public override object Execute(IEvent @event)
        {
            string operation = (string)@event.Args[0];
            if ("create".Contains(operation))
            {
                return new Matrix2D(3, 3, 10);
            }

            return null;
        }
    }

    public class TwoArgsHandler : BaseHandler
    {
        Diagonalize diagonalize;

        public TwoArgsHandler()
        {
            ArgsCount = 2;
            Next = new ThreeArgsHandler();

            diagonalize = delegate (Matrix2D matrix)
            {
                for (int rowIndex = 1; rowIndex < matrix.RowsCount; ++rowIndex)
                {
                    for (int columnIndex = 0; columnIndex < rowIndex; ++columnIndex)
                    {
                        matrix[rowIndex, columnIndex] = 0;
                    }
                }
            };
        }
        public override object Execute(IEvent @event)
        {
            string operation = (string)@event.Args[0];
            string matrixString = (string)@event.Args[1];

            Matrix2D matrix = Matrix2D.ParseMatrix(matrixString);
            if (matrix == null)
            {
                return null;
            }

            if ("transpose".Contains(operation))
            {
                matrix.Transpose();
                return matrix;
            }

            if ("trace".Contains(operation))
            {
                return matrix.GetTrace();
            }

            if ("discriminant".Contains(operation))
            {
                return matrix.GetDeterminant();
            }

            if ("reverse".Contains(operation))
            {
                return matrix.Reverse();
            }

            if ("diagonalize".Contains(operation))
            {
                diagonalize(matrix);
                return matrix;
            }

            return null;
        }
    }

    public class ThreeArgsHandler : BaseHandler
    {
        public ThreeArgsHandler()
        {
            ArgsCount = 3;
        }
        public override object Execute(IEvent @event)
        {
            string firstMatrixString = (string)@event.Args[0];
            string secondMatrixString = (string)@event.Args[2];

            Matrix2D firstMatrix = Matrix2D.ParseMatrix(firstMatrixString);
            if (firstMatrix == null)
            {
                return null;
            }

            Matrix2D secondMatrix = Matrix2D.ParseMatrix(secondMatrixString);
            double number = 0;
            if (secondMatrix == null || secondMatrix.ColumnsCount == 1)
            {
                try
                {
                    number = Convert.ToDouble(@event.Args[2]);
                }
                catch
                {
                    return null;
                }
            }

            string operation = (string)@event.Args[1];

            Matrix2D newMatrix = null;
            if (secondMatrix == null || secondMatrix.ColumnsCount == 1)
            {
                switch (operation)
                {
                    case "/":
                        newMatrix = firstMatrix / number;
                        break;
                    case "*":
                        newMatrix = firstMatrix * number;
                        break;
                }
            }
            else
            {
                switch (operation)
                {
                    case "*":
                        newMatrix = firstMatrix * secondMatrix;
                        break;
                    case "+":
                        newMatrix = firstMatrix + secondMatrix;
                        break;
                    case "-":
                        newMatrix = firstMatrix - secondMatrix;
                        break;
                }
            }

            return newMatrix;
        }
    }

    public class ChainApplication
    {
        private BaseHandler _eventHandler;
        public ChainApplication()
        {
            _eventHandler = new OneArgHandler();
        }
        public void Run(string request)
        {
            AppEvent appEvent = new AppEvent(request);
            HandleEvent(appEvent);
        }

        private void HandleEvent(IEvent @event)
        {
            _eventHandler.Handle(@event);
        }
    }
}
