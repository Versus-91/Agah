using Abp.Application.Services.Dto;
using System;

namespace AFIAT.TST.Pages.Dtos
{
    public class GetAllPagesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TitleFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public double? MaxViewsFilter { get; set; }
        public double? MinViewsFilter { get; set; }

        public string ImageSrcFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public string CategoryNameFilter { get; set; }

    }
}