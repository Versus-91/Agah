using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using AFIAT.TST.Posts.Dtos;
using AFIAT.TST.Dto;
using Abp.Application.Services.Dto;
using AFIAT.TST.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using AFIAT.TST.Storage;

namespace AFIAT.TST.Posts
{
    [AbpAuthorize(AppPermissions.Pages_Categories)]
    public class CategoriesAppService : TSTAppServiceBase, ICategoriesAppService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoriesAppService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;

        }

        public async Task<PagedResultDto<GetCategoryForViewDto>> GetAll(GetAllCategoriesInput input)
        {

            var filteredCategories = _categoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredCategories = filteredCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var categories = from o in pagedAndFilteredCategories
                             select new
                             {

                                 o.Name,
                                 o.Description,
                                 Id = o.Id
                             };

            var totalCount = await filteredCategories.CountAsync();

            var dbList = await categories.ToListAsync();
            var results = new List<GetCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCategoryForViewDto()
                {
                    Category = new CategoryDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCategoryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCategoryForViewDto> GetCategoryForView(int id)
        {
            var category = await _categoryRepository.GetAsync(id);

            var output = new GetCategoryForViewDto { Category = ObjectMapper.Map<CategoryDto>(category) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Edit)]
        public async Task<GetCategoryForEditOutput> GetCategoryForEdit(EntityDto input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCategoryForEditOutput { Category = ObjectMapper.Map<CreateOrEditCategoryDto>(category) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCategoryDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Create)]
        protected virtual async Task Create(CreateOrEditCategoryDto input)
        {
            var category = ObjectMapper.Map<Category>(input);

            if (AbpSession.TenantId != null)
            {
                category.TenantId = (int?)AbpSession.TenantId;
            }

            await _categoryRepository.InsertAsync(category);

        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Edit)]
        protected virtual async Task Update(CreateOrEditCategoryDto input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, category);

        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _categoryRepository.DeleteAsync(input.Id);
        }

    }
}