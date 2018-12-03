﻿namespace Atanet.Services.Comments
{
    using Microsoft.EntityFrameworkCore;
    using Atanet.Model.ApiResponse.HTTP400;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Services.Common;
    using Atanet.Services.Exceptions;
    using Atanet.Services.UoW;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Atanet.Model.Extensions;

    public class CommentFilterService : ICommentFilterService
    {
        private readonly IQueryService queryService;

        private readonly IPagingValidator pagingValidator;

        public CommentFilterService(IQueryService queryService, IPagingValidator pagingValidator)
        {
            this.queryService = queryService;
            this.pagingValidator = pagingValidator;
        }

        public IEnumerable<CommentDto> GetCommentsForPost(long postId, int page, int pageSize)
        {
            this.pagingValidator.ThrowIfPageOutOfRange(pageSize, page);
            this.ThrowIfPostDoesNotExist(postId);
            var comments = this.QueryCommentDto();
            var queried = comments
                .Where(x => x.PostId == postId)
                .OrderByDescending(x => x.Created);
            var filtered = this.Page(queried, page, pageSize);
            return filtered.ToList();
        }

        private void ThrowIfPostDoesNotExist(long postId)
        {
            if (!this.queryService.Query<Post>().Any(x => x.Id == postId))
            {
                throw new ApiException(x => x.BadRequestResult((
                    ErrorCode.Parse(ErrorCodeType.InvalidReferenceId, AtanetEntityName.Comment, PropertyName.Comment.PostId, AtanetEntityName.Post),
                    new ErrorDefinition(postId, "The given post does not exist", PropertyName.Comment.PostId))));
            }
        }

        private IQueryable<CommentDto> QueryCommentDto()
        {
            var fetched =
                from comment in this.queryService.Query<Comment>().Include(x => x.User)
                select comment.MapTo<CommentDto>();
            return fetched;
        }

        private IQueryable<T> Page<T>(IQueryable<T> queryable, int page, int pageSize) =>
            queryable.Skip(page * pageSize).Take(pageSize);
    }
}
