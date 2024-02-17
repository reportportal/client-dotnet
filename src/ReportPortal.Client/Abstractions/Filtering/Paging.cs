namespace ReportPortal.Client.Abstractions.Filtering
{
    /// <summary>
    /// Represents the paging information for a collection of items.
    /// </summary>
    public class Paging
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Paging"/> class with the specified number and size.
        /// </summary>
        /// <param name="number">The page number.</param>
        /// <param name="size">The number of items per page.</param>
        public Paging(int number, int size)
        {
            Number = number;
            Size = size;
        }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int Size { get; set; }
    }
}
