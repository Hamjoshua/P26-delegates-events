using System;

namespace app.Matrix
{
    public class DifferentMatrixesException : Exception
    {
        public DifferentMatrixesException()
        {
        }

        public DifferentMatrixesException(string message)
            : base(message)
        {
        }

        public DifferentMatrixesException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
