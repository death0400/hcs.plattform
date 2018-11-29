using System;

namespace Hcs.Serialize.Xlsx
{
    public class CellWriteException<T> : Exception
    {
        public CellWriteException(ICellWriter<T> writer, Exception innerException)
        : base($"error when excute CellWriter {writer.GetType()} Tag:{writer.Tag}", innerException)
        {
        }
    }
}