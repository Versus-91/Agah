using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Posts.Dtos
{
    public class CreateOrEditTagDto : EntityDto<int?>
    {

        [Required]
        public string Name { get; set; }

    }
}