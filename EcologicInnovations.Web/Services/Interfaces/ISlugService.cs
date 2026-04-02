Task<string> GenerateUniqueBlogCategorySlugAsync(string source, int? ignoreId = null, CancellationToken cancellationToken = default);
Task<string> GenerateUniqueBlogPostSlugAsync(string source, int? ignoreId = null, CancellationToken cancellationToken = default);
