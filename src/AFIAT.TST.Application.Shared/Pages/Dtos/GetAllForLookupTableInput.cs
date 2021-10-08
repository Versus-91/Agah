using Abp.Application.Services.Dto;

namespace AFIAT.TST.Pages.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}