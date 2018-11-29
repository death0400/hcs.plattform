namespace Hcs.Serialize.Xlsx.Static
{
    public class StaticCell<T>
    {
        public int SheetIndex { get; set; }
        public string Address { get; set; }
        public ICellWriter<T> CellWriter { get; set; }

        public StaticCell(int sheetIndex, string address, ICellWriter<T> writer)
        {
            Address = address;
            CellWriter = writer;
            SheetIndex = sheetIndex;
        }
    }
}