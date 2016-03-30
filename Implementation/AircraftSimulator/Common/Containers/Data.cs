using System;

namespace Common.Containers
{
    public class InapropriateDataException : Exception
    {
        public override string Message => "Matrix cannot be empty";
    }

    [Serializable]
    public class Data : IData
    {
        public string Sender
        {
            get; set;
        }

        public MessageType MessageType
        {
            get; set;
        }

        public ActionType Response
        {
            get; set;
        }

        public DataType InputType
        {
            get; set;
        }

        public DataType OutputType
        {
            get; set;
        }


        private float[][] array;
        public float[][] Array
        {
            get { return array; }
            set
            {
                int n = value.GetLength(0);
                int m = value[0].GetLength(0);
                if (n == 0 || m == 0)
                    throw new InapropriateDataException();
                array = value;
            }
        }

        public float[,] Get2DimArray()
        {
            int n = array.GetLength(0);
            int m = array[0].GetLength(0);
            float[,] toRet = new float[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                    toRet[i, j] = array[i][j];
            }
            return toRet;
        }

        public void Set2DimArray(float[,] value)
        {
            int n = value.GetLength(0);
            int m = value.GetLength(1);
            if (n == 0 || m == 0)
                throw new InapropriateDataException();
            array = new float[n][];
            for (int i = 0; i < n; i++)
            {
                array[i] = new float[m];
                for (int j = 0; j < m; j++)
                    array[i][j] = value[i, j];
            }
        }

        public int N => Array.GetLength(0);
        public int M => Array[0].GetLength(0);
    }
}
