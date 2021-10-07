using Abp.Application.Services.Dto;

namespace AFIAT.TST.Posts.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}