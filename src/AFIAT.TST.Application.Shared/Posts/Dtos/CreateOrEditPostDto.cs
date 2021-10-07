using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Posts.Dtos
{
    public class CreateOrEditPostDto : EntityDto<int?>
    {

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int Views { get; set; }

        public int CategoryId { get; set; }

    }
}