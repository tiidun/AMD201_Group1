using System.ComponentModel.DataAnnotations;
using static URLShortenerData.Data.DataConstants;

namespace URLShortenerAPI.Models.URLAddress
{
    public class URLAddressBindingModel
    {
        [Display(Name = "URL")]
        [MaxLength(OriginalUrlMaxLenght)]
        [MinLength(OriginalUrlMinLenght)]
        public string URL { get; init; }


        [Display(Name = "Short Code")]
        [MaxLength(ShortUrlMaxLenght)]
        [MinLength(ShortUrlMinLenght)]
        public string ShortCode { get; init; }
    }
}
