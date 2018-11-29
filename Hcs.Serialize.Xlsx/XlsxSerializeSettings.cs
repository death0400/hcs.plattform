namespace Hcs.Serialize.Xlsx
{
    public class XlsxSerializeSettings<T>
    {
        public IXlsxTemplate Template { get; set; }
        public IXlsxProcess<T>[] Processes { get; set; }
        public XlsxSerializeSettings()
        {

        }
        public XlsxSerializeSettings(params IXlsxProcess<T>[] processes)
        {
            Processes = processes;
        }
        public XlsxSerializeSettings(IXlsxTemplate tempalte, params IXlsxProcess<T>[] processes)
        {
            Processes = processes;
        }
    }
}