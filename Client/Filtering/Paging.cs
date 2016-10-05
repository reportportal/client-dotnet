namespace ReportPortal.Client.Filtering
{
    public class Paging
    {
        public Paging(int number, int size)
        {
            Number = number;
            Size = size;
        }

        public int Number { get; set; }

        public int Size { get; set; }
    }
}
