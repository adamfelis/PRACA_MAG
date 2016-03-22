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
        float[][] Array { get; set; }
        float [,] Get2DimArray();
        void Set2DimArray(float [,] value);
        int N { get; }
        int M { get; }
    }
}
