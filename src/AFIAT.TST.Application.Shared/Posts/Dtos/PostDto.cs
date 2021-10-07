using System;
using Abp.Application.Services.Dto;

namespace AFIAT.TST.Posts.Dtos
{
    public class PostDto : EntityDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int Views { get; set; }

        public int CategoryId { get; set; }

    }
}