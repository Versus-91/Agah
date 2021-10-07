using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AFIAT.TST.Posts.Dtos;
using AFIAT.TST.Dto;

namespace AFIAT.TST.Posts
{
    public interface ICategoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCategoryForViewDto>> GetAll(GetAllCategoriesInput input);

        Task<GetCategoryForViewDto> GetCategoryForView(int id);

        Task<GetCategoryForEditOutput> GetCategoryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCategoryDto input);

        Task Delete(EntityDto input);

    }
}