using Abp.Application.Services.Dto;
using System;

namespace AFIAT.TST.Posts.Dtos
{
    public class GetAllTagsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}