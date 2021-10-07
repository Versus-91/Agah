using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Posts.Dtos
{
    public class GetTagForEditOutput
    {
        public CreateOrEditTagDto Tag { get; set; }

    }
}