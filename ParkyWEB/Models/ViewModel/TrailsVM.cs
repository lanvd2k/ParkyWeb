using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParkyWEB.Models.ViewModel
{
    public class TrailsVM
    {
        public Trail Trail { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> NationalParkList { get; set; }
    }
}
