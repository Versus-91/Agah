using System;
using Abp.Application.Services.Dto;

namespace AFIAT.TST.Pages.Dtos
{
    public class PageDto : EntityDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public double Views { get; set; }

        public string ImageSrc { get; set; }

        public bool IsActive { get; set; }

        public int CategoryId { get; set; }

    }
}