using AFIAT.TST.Posts;

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
    [AbpAuthorize(AppPermissions.Pages_Posts)]
    public class PostsAppService : TSTAppServiceBase, IPostsAppService
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<Category, int> _lookup_categoryRepository;

        public PostsAppService(IRepository<Post> postRepository, IRepository<Category, int> lookup_categoryRepository)
        {
            _postRepository = postRepository;
            _lookup_categoryRepository = lookup_categoryRepository;

        }

        public async Task<PagedResultDto<GetPostForViewDto>> GetAll(GetAllPostsInput input)
        {

            var filteredPosts = _postRepository.GetAll()
                        .Include(e => e.CategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(input.MinViewsFilter != null, e => e.Views >= input.MinViewsFilter)
                        .WhereIf(input.MaxViewsFilter != null, e => e.Views <= input.MaxViewsFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter);

            var pagedAndFilteredPosts = filteredPosts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var posts = from o in pagedAndFilteredPosts
                        join o1 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        select new
                        {

                            o.Title,
                            o.Description,
                            o.IsActive,
                            o.Views,
                            Id = o.Id,
                            CategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                        };

            var totalCount = await filteredPosts.CountAsync();

            var dbList = await posts.ToListAsync();
            var results = new List<GetPostForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPostForViewDto()
                {
                    Post = new PostDto
                    {

                        Title = o.Title,
                        Description = o.Description,
                        IsActive = o.IsActive,
                        Views = o.Views,
                        Id = o.Id,
                    },
                    CategoryName = o.CategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPostForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPostForViewDto> GetPostForView(int id)
        {
            var post = await _postRepository.GetAsync(id);

            var output = new GetPostForViewDto { Post = ObjectMapper.Map<PostDto>(post) };

            if (output.Post.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.Post.CategoryId);
                output.CategoryName = _lookupCategory?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Posts_Edit)]
        public async Task<GetPostForEditOutput> GetPostForEdit(EntityDto input)
        {
            var post = await _postRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPostForEditOutput { Post = ObjectMapper.Map<CreateOrEditPostDto>(post) };

            if (output.Post.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.Post.CategoryId);
                output.CategoryName = _lookupCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPostDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Posts_Create)]
        protected virtual async Task Create(CreateOrEditPostDto input)
        {
            var post = ObjectMapper.Map<Post>(input);

            if (AbpSession.TenantId != null)
            {
                post.TenantId = (int?)AbpSession.TenantId;
            }

            await _postRepository.InsertAsync(post);

        }

        [AbpAuthorize(AppPermissions.Pages_Posts_Edit)]
        protected virtual async Task Update(CreateOrEditPostDto input)
        {
            var post = await _postRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, post);

        }

        [AbpAuthorize(AppPermissions.Pages_Posts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _postRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Posts)]
        public async Task<PagedResultDto<PostCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_categoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var categoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PostCategoryLookupTableDto>();
            foreach (var category in categoryList)
            {
                lookupTableDtoList.Add(new PostCategoryLookupTableDto
                {
                    Id = category.Id,
                    DisplayName = category.Name?.ToString()
                });
            }

            return new PagedResultDto<PostCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}