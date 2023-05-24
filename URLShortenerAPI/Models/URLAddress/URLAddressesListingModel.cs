namespace URLShortenerAPI.Models.URLAddress
{
    public class URLAddressesListingModel
    {
        public int Id { get; init; }

        public string OriginalUrl { get; set; }

        public string ShortUrl { get; set; }

        public DateTime DateCreated { get; set; }

        public int Visits { get; set; }
    }
}
