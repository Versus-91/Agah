using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Posts.Dtos
{
    public class GetPostForEditOutput
    {
        public CreateOrEditPostDto Post { get; set; }

        public string CategoryName { get; set; }

    }
}