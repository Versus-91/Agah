using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AFIAT.TST.Posts.Dtos;
using AFIAT.TST.Dto;

namespace AFIAT.TST.Posts
{
    public interface ITagsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTagForViewDto>> GetAll(GetAllTagsInput input);

        Task<GetTagForViewDto> GetTagForView(int id);

        Task<GetTagForEditOutput> GetTagForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTagDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetTagsToExcel(GetAllTagsForExcelInput input);

    }
}