using AFIAT.TST.Posts;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using AFIAT.TST.Pages.Dtos;
using AFIAT.TST.Dto;
using Abp.Application.Services.Dto;
using AFIAT.TST.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using AFIAT.TST.Storage;

namespace AFIAT.TST.Pages
{
    //[AbpAuthorize(AppPermissions.Pages_Pages)]
    public class PagesAppService : TSTAppServiceBase, IPagesAppService
    {
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<Category, int> _lookup_categoryRepository;

        public PagesAppService(IRepository<Page> pageRepository, IRepository<Category, int> lookup_categoryRepository)
        {
            _pageRepository = pageRepository;
            _lookup_categoryRepository = lookup_categoryRepository;

        }

        public async Task<PagedResultDto<GetPageForViewDto>> GetAll(GetAllPagesInput input)
        {

            var filteredPages = _pageRepository.GetAll()
                        .Include(e => e.CategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ImageSrc.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.MinViewsFilter != null, e => e.Views >= input.MinViewsFilter)
                        .WhereIf(input.MaxViewsFilter != null, e => e.Views <= input.MaxViewsFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ImageSrcFilter), e => e.ImageSrc == input.ImageSrcFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter);

            var pagedAndFilteredPages = filteredPages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var pages = from o in pagedAndFilteredPages
                        join o1 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        select new
                        {

                            o.Title,
                            o.Description,
                            o.Views,
                            o.ImageSrc,
                            o.IsActive,
                            Id = o.Id,
                            CategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                        };

            var totalCount = await filteredPages.CountAsync();

            var dbList = await pages.ToListAsync();
            var results = new List<GetPageForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPageForViewDto()
                {
                    Page = new PageDto
                    {

                        Title = o.Title,
                        Description = o.Description,
                        Views = o.Views,
                        ImageSrc = o.ImageSrc,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    },
                    CategoryName = o.CategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPageForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPageForViewDto> GetPageForView(int id)
        {
            var page = await _pageRepository.GetAsync(id);

            var output = new GetPageForViewDto { Page = ObjectMapper.Map<PageDto>(page) };

            if (output.Page.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.Page.CategoryId);
                output.CategoryName = _lookupCategory?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Pages_Edit)]
        public async Task<GetPageForEditOutput> GetPageForEdit(EntityDto input)
        {
            var page = await _pageRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPageForEditOutput { Page = ObjectMapper.Map<CreateOrEditPageDto>(page) };

            if (output.Page.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.Page.CategoryId);
                output.CategoryName = _lookupCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPageDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Pages_Create)]
        protected virtual async Task Create(CreateOrEditPageDto input)
        {
            var page = ObjectMapper.Map<Page>(input);

            if (AbpSession.TenantId != null)
            {
                page.TenantId = (int?)AbpSession.TenantId;
            }

            await _pageRepository.InsertAsync(page);

        }

        [AbpAuthorize(AppPermissions.Pages_Pages_Edit)]
        protected virtual async Task Update(CreateOrEditPageDto input)
        {
            var page = await _pageRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, page);

        }

        [AbpAuthorize(AppPermissions.Pages_Pages_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _pageRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Pages)]
        public async Task<PagedResultDto<PageCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_categoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var categoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PageCategoryLookupTableDto>();
            foreach (var category in categoryList)
            {
                lookupTableDtoList.Add(new PageCategoryLookupTableDto
                {
                    Id = category.Id,
                    DisplayName = category.Name?.ToString()
                });
            }

            return new PagedResultDto<PageCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}