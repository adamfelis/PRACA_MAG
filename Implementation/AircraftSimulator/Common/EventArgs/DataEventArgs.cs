using Common.Containers;

namespace Common.EventArgs
{
    public class DataEventArgs : System.EventArgs
    {
        public int Id { get; set; }
        public  IDataList DataList { get; set; }
    }
}
