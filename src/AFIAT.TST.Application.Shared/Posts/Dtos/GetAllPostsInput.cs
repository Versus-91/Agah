using Abp.Application.Services.Dto;
using System;

namespace AFIAT.TST.Posts.Dtos
{
    public class GetAllPostsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TitleFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public int? MaxViewsFilter { get; set; }
        public int? MinViewsFilter { get; set; }

        public string CategoryNameFilter { get; set; }

    }
}