namespace Common.Containers
{
    public enum DataType
    {
        Float,
        Vector,
        Matrix
    }
    public interface IData
    {
        DataType Type { get; set; }
    }
}
