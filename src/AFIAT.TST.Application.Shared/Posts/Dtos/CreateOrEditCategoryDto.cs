using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Posts.Dtos
{
    public class CreateOrEditCategoryDto : EntityDto<int?>
    {

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}