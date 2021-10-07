using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AFIAT.TST.Posts.Dtos;
using AFIAT.TST.Dto;

namespace AFIAT.TST.Posts
{
    public interface IPostsAppService : IApplicationService
    {
        Task<PagedResultDto<GetPostForViewDto>> GetAll(GetAllPostsInput input);

        Task<GetPostForViewDto> GetPostForView(int id);

        Task<GetPostForEditOutput> GetPostForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPostDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<PostCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input);

    }
}