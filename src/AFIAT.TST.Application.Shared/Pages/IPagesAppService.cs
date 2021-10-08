using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AFIAT.TST.Pages.Dtos;
using AFIAT.TST.Dto;

namespace AFIAT.TST.Pages
{
    public interface IPagesAppService : IApplicationService
    {
        Task<PagedResultDto<GetPageForViewDto>> GetAll(GetAllPagesInput input);

        Task<GetPageForViewDto> GetPageForView(int id);

        Task<GetPageForEditOutput> GetPageForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPageDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<PageCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input);

    }
}