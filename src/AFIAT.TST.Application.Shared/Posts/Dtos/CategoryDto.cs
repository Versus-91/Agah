using System;
using Abp.Application.Services.Dto;

namespace AFIAT.TST.Posts.Dtos
{
    public class CategoryDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

    }
}