using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EasyUpdater.Web
{

    // Root object
    public class RootObject
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public DataWrapper Data { get; set; }
    }

    // Wrapper for the main data and metadata
    public class DataWrapper
    {
        [JsonProperty("data")]
        public List<WebPlugin> Data { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }

    // Represents a single plugin
    public class WebPlugin
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("repository")]
        public string Repository { get; set; }

        [JsonProperty("repositoryId")]
        public long RepositoryId { get; set; }

        [JsonProperty("authorId")]
        public int AuthorId { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("stars")]
        public int Stars { get; set; }

        [JsonProperty("downloads")]
        public int Downloads { get; set; }

        [JsonProperty("readme")]
        public string Readme { get; set; }

        [JsonProperty("releases")]
        public List<Release> Releases { get; set; }

        [JsonProperty("repoCreatedAt")]
        public DateTime RepoCreatedAt { get; set; }

        [JsonProperty("repoUpdatedAt")]
        public DateTime RepoUpdatedAt { get; set; }

        [JsonProperty("pinned")]
        public bool Pinned { get; set; }

        [JsonProperty("recommended")]
        public bool Recommended { get; set; }

        [JsonProperty("lastCheckedAt")]
        public DateTime LastCheckedAt { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("organizationId")]
        public int? OrganizationId { get; set; }

        [JsonProperty("author")]
        public PluginAuthor Author { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }

        [JsonProperty("organization")]
        public Organization Organization { get; set; }

        [JsonProperty("dependencies")]
        public List<DependencyInfo> Dependencies { get; set; }
    }

    // Release information for a plugin
    public class Release
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("tagName")]
        public string TagName { get; set; }

        [JsonProperty("htmlUrl")]
        public string HtmlUrl { get; set; }

        [JsonProperty("publishedAt")]
        public DateTime PublishedAt { get; set; }

        [JsonProperty("author")]
        public ReleaseAuthor Author { get; set; }

        [JsonProperty("assets")]
        public List<Asset> Assets { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    // Author of a release
    public class ReleaseAuthor
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("avatarUrl")]
        public string AvatarUrl { get; set; }
    }

    // Downloadable asset for a release
    public class Asset
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("downloadUrl")]
        public string DownloadUrl { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("downloadCount")]
        public int DownloadCount { get; set; }
    }

    // Author of a plugin
    public class PluginAuthor
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("avatarUrl")]
        public string AvatarUrl { get; set; }

        [JsonProperty("_count")]
        public Count Count { get; set; }
    }

    // Count of plugins by an author
    public class Count
    {
        [JsonProperty("plugins")]
        public int Plugins { get; set; }
    }

    // Tag associated with a plugin
    public class Tag
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    // Organization associated with a plugin
    public class Organization
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("avatarUrl")]
        public string AvatarUrl { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }
    }

    // Dependency information
    public class DependencyInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("pluginId")]
        public string PluginId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("dependsOn")]
        public string DependsOn { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("dependency")]
        public DependencyDetails Dependency { get; set; }
    }

    // Details of a dependency
    public class DependencyDetails
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("repository")]
        public string Repository { get; set; }

        [JsonProperty("author")]
        public DependencyAuthor Author { get; set; }
    }

    // Author of a dependency
    public class DependencyAuthor
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }



        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("avatarUrl")]
        public string AvatarUrl { get; set; }
    }

    // Metadata for pagination
    public class Meta
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
    }
}