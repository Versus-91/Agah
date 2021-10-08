using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Pages.Dtos
{
    public class GetPageForEditOutput
    {
        public CreateOrEditPageDto Page { get; set; }

        public string CategoryName { get; set; }

    }
}