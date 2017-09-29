﻿using System;
using System.Collections.Generic;

#if ASYNC

using System.Threading.Tasks;

#endif

using ZendeskApi_v2.Extensions;
using ZendeskApi_v2.Models.Articles;

namespace ZendeskApi_v2.Requests.HelpCenter
{
    [Flags]
    public enum ArticleSideLoadOptionsEnum
    {
        None = 1,
        Users = 2,
        Sections = 4,
        Categories = 8,
        Translations = 16
    }

    public interface IArticles : ICore
    {
#if SYNC

        IndividualArticleResponse GetArticle(long articleId, ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None);

        GroupArticleResponse GetArticles(ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None, ArticleSortingOptions options = null, int? perPage = null, int? page = null);

        GroupArticleResponse GetArticlesByCategoryId(long categoryId, ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None, ArticleSortingOptions options = null);

        GroupArticleResponse GetArticlesBySectionId(long sectionId, ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None, ArticleSortingOptions options = null);

        GroupArticleResponse GetArticlesByUserId(long userId);

        GroupArticleResponse GetArticlesSinceDateTime(DateTime startTime);

        ArticleSearchResults SearchArticlesFor(string query, string category = "", string section = "", string labels = "", string locale = "", DateTime? createdBefore = null, DateTime? createdAfter = null, DateTime? createdAt = null, DateTime? updatedBefore = null, DateTime? updatedAfter = null, DateTime? updatedAt = null);

        IndividualArticleResponse CreateArticle(long sectionId, Article article);

        IndividualArticleResponse UpdateArticle(Article article);

        bool DeleteArticle(long id);

#endif
#if ASYNC

        Task<IndividualArticleResponse> GetArticleAsync(long articleId, ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None);

        Task<GroupArticleResponse> GetArticlesAsync(ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None, ArticleSortingOptions options = null, int? perPage = null, int? page = null);

        Task<GroupArticleResponse> GetArticlesByCategoryIdAsync(long categoryId, ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None, ArticleSortingOptions options = null);

        Task<GroupArticleResponse> GetArticlesBySectionIdAsync(long sectionId, ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None, ArticleSortingOptions options = null);

        Task<GroupArticleResponse> GetArticlesByUserIdAsync(long userId);

        Task<GroupArticleResponse> GetArticlesSinceDateTimeAsync(DateTime startTime);

        Task<ArticleSearchResults> SearchArticlesForAsync(string query, string category = "", string section = "", string labels = "", string locale = "", DateTime? createdBefore = null, DateTime? createdAfter = null, DateTime? createdAt = null, DateTime? updatedBefore = null, DateTime? updatedAfter = null, DateTime? updatedAt = null);

        Task<IndividualArticleResponse> CreateArticleAsync(long sectionId, Article article);

        Task<IndividualArticleResponse> UpdateArticleAsync(Article article);

        Task<bool> DeleteArticleAsync(long id);

#endif
    }

    public class Articles : Core, IArticles
    {
        private readonly string urlPrefix = "help_center";

        public Articles(string yourZendeskUrl, string user, string password, string apiToken, string p_OAuthToken, string locale)
            : base(yourZendeskUrl, user, password, apiToken, p_OAuthToken)
        {
            if (!locale.IsNullOrWhiteSpace())
            {
                urlPrefix = $"{urlPrefix}/{locale}";
            }
        }

#if SYNC

        public IndividualArticleResponse GetArticle(long articleId, ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None)
        {
            var resourceUrl = this.GetFormattedArticleUri($"{urlPrefix}/articles/{articleId}.json", sideloadOptions);

            return GenericGet<IndividualArticleResponse>(resourceUrl);
        }

        public GroupArticleResponse GetArticles(ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None, ArticleSortingOptions options = null, int? perPage = null, int? page = null)
        {
            var resourceUrl = this.GetFormattedArticlesUri($"{urlPrefix}/articles.json", options, sideloadOptions);

            return GenericPagedGet<GroupArticleResponse>(resourceUrl, perPage, page);
        }

        public GroupArticleResponse GetArticlesByCategoryId(long categoryId, ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None, ArticleSortingOptions options = null)
        {
            var uri = $"{urlPrefix}/categories/{categoryId}/articles.json";
            var resourceUrl = this.GetFormattedArticlesUri(uri, options, sideloadOptions);

            return GenericGet<GroupArticleResponse>(resourceUrl);
        }

        public GroupArticleResponse GetArticlesBySectionId(long sectionId, ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None, ArticleSortingOptions options = null)
        {
            var uri = $"{urlPrefix}/sections/{sectionId}/articles.json";
            var resourceUrl = this.GetFormattedArticlesUri(uri, options, sideloadOptions);

            return GenericGet<GroupArticleResponse>(resourceUrl);
        }

        public GroupArticleResponse GetArticlesByUserId(long userId)
        {
            return GenericGet<GroupArticleResponse>($"help_center/users/{userId}/articles.json");
        }

        public GroupArticleResponse GetArticlesSinceDateTime(DateTime startTime)
        {
            return GenericGet<GroupArticleResponse>($"help_center/incremental/articles.json?start_time={startTime.GetEpoch()}");
        }

        public ArticleSearchResults SearchArticlesFor(string query, string category = "", string section = "", string labels = "", string locale = "", DateTime? createdBefore = null, DateTime? createdAfter = null, DateTime? createdAt = null, DateTime? updatedBefore = null, DateTime? updatedAfter = null, DateTime? updatedAt = null)
        {
            var querystringParams = new Dictionary<string, string> { { "category", category }, { "section", section }, { "label_names", labels },
                { "locale", locale },{ "created_before", $"{createdBefore:yyyy-MM-dd}" }, {"created_after" , $"{createdAfter:yyyy-MM-dd}" },
                { "created_at", $"{createdAt:yyyy-MM-dd}"}, { "updated_before", $"{updatedBefore:yyyy-MM-dd}"}, {"updated_after" , $"{updatedAfter:yyyy-MM-dd}" },{ "updated_at", $"{updatedAt:yyyy-MM-dd}"} };

            return GenericGet<ArticleSearchResults>($"help_center/articles/search.json?query={query}&{querystringParams.GetQueryString()}");
        }

        public IndividualArticleResponse CreateArticle(long sectionId, Article article)
        {
            return GenericPost<IndividualArticleResponse>($"{urlPrefix}/sections/{sectionId}/articles.json", new { article });
        }

        public IndividualArticleResponse UpdateArticle(Article article)
        {
            return GenericPut<IndividualArticleResponse>($"{urlPrefix}/articles/{article.Id}.json", new { article });
        }

        public bool DeleteArticle(long id)
        {
            return GenericDelete($"help_center/articles/{id}.json");
        }

#endif
#if ASYNC

        public async Task<IndividualArticleResponse> GetArticleAsync(long articleId, ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None)
        {
            var resourceUrl = this.GetFormattedArticleUri($"{urlPrefix}/articles/{articleId}.json", sideloadOptions);

            return await GenericGetAsync<IndividualArticleResponse>(resourceUrl);
        }

        public async Task<GroupArticleResponse> GetArticlesAsync(ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None, ArticleSortingOptions options = null, int? perPage = null, int? page = null)
        {
            var resourceUrl = this.GetFormattedArticlesUri($"{urlPrefix}/articles.json", options, sideloadOptions);

            return await GenericPagedGetAsync<GroupArticleResponse>(resourceUrl, perPage, page);
        }

        public async Task<GroupArticleResponse> GetArticlesByCategoryIdAsync(long categoryId, ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None, ArticleSortingOptions options = null)
        {
            var uri = $"{urlPrefix}/categories/{categoryId}/articles.json";
            var resourceUrl = this.GetFormattedArticlesUri(uri, options, sideloadOptions);

            return await GenericGetAsync<GroupArticleResponse>(resourceUrl);
        }

        public async Task<GroupArticleResponse> GetArticlesBySectionIdAsync(long sectionId, ArticleSideLoadOptionsEnum sideloadOptions = ArticleSideLoadOptionsEnum.None, ArticleSortingOptions options = null)
        {
            var uri = $"{urlPrefix}/sections/{sectionId}/articles.json";
            var resourceUrl = this.GetFormattedArticlesUri(uri, options, sideloadOptions);

            return await GenericGetAsync<GroupArticleResponse>(resourceUrl);
        }

        public async Task<GroupArticleResponse> GetArticlesByUserIdAsync(long userId)
        {
            return await GenericGetAsync<GroupArticleResponse>($"help_center/users/{userId}/articles.json");
        }

        public async Task<GroupArticleResponse> GetArticlesSinceDateTimeAsync(DateTime startTime)
        {
            return await GenericGetAsync<GroupArticleResponse>($"help_center/incremental/articles.json?start_time={startTime.GetEpoch()}");
        }

        public async Task<ArticleSearchResults> SearchArticlesForAsync(string query, string category = "", string section = "", string labels = "", string locale = "", DateTime? createdBefore = null, DateTime? createdAfter = null, DateTime? createdAt = null, DateTime? updatedBefore = null, DateTime? updatedAfter = null, DateTime? updatedAt = null)
        {
            var querystringParams = new Dictionary<string, string> { { "category", category }, { "section", section }, { "label_names", labels },
                { "locale", locale },{ "created_before", $"{createdBefore:yyyy-MM-dd}" }, {"created_after" , $"{createdAfter:yyyy-MM-dd}" },
                { "created_at", $"{createdAt:yyyy-MM-dd}"}, { "updated_before", $"{updatedBefore:yyyy-MM-dd}"}, {"updated_after" , $"{updatedAfter:yyyy-MM-dd}" },
                { "updated_at", $"{updatedAt:yyyy-MM-dd}"} };

            return await GenericGetAsync<ArticleSearchResults>($"help_center/articles/search.json?query={query}&{querystringParams.GetQueryString()}");
        }

        public async Task<IndividualArticleResponse> CreateArticleAsync(long sectionId, Article article)
        {
            return await GenericPostAsync<IndividualArticleResponse>($"{urlPrefix}/sections/{sectionId}/articles.json", new { article });
        }

        public async Task<IndividualArticleResponse> UpdateArticleAsync(Article article)
        {
            return await GenericPutAsync<IndividualArticleResponse>($"{urlPrefix}/articles/{article.Id}.json", new { article });
        }

        public async Task<bool> DeleteArticleAsync(long id)
        {
            return await GenericDeleteAsync($"help_center/articles/{id}.json");
        }

#endif

        private string GetFormattedArticlesUri(string resourceUrl, ArticleSortingOptions options, ArticleSideLoadOptionsEnum sideloadOptions)
        {
            if (options != null)
            {
                if (string.IsNullOrEmpty(options.Locale))
                {
                    throw new ArgumentException("Locale is required to sort");
                }

                resourceUrl = options.GetSortingString(resourceUrl, urlPrefix);
            }

            var sideLoads = sideloadOptions.ToString().ToLower().Replace(" ", "");
            if (sideloadOptions != ArticleSideLoadOptionsEnum.None)
            {
                resourceUrl += resourceUrl.Contains("?") ? "&include=" : "?include=";

                //Categories flag REQUIRES sections to be added as well, or nothing will be returned
                if (sideloadOptions.HasFlag(ArticleSideLoadOptionsEnum.Categories) && !sideloadOptions.HasFlag(ArticleSideLoadOptionsEnum.Sections))
                {
                    sideLoads += $",{ArticleSideLoadOptionsEnum.Sections.ToString().ToLower()}";
                }

                resourceUrl += sideLoads;
            }
            return resourceUrl;
        }

        private string GetFormattedArticleUri(string resourceUrl, ArticleSideLoadOptionsEnum sideloadOptions)
        {
            var sideLoads = sideloadOptions.ToString().ToLower().Replace(" ", "");
            if (sideloadOptions != ArticleSideLoadOptionsEnum.None)
            {
                resourceUrl += resourceUrl.Contains("?") ? "&include=" : "?include=";

                //Categories flag REQUIRES sections to be added as well, or nothing will be returned
                if (sideloadOptions.HasFlag(ArticleSideLoadOptionsEnum.Categories) && !sideloadOptions.HasFlag(ArticleSideLoadOptionsEnum.Sections))
                {
                    sideLoads += $",{ArticleSideLoadOptionsEnum.Sections.ToString().ToLower()}";
                }

                resourceUrl += sideLoads;
            }
            return resourceUrl;
        }
    }
}
