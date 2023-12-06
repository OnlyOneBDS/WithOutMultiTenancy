namespace WithOutMultiTenancy.Application.Catalog.Products;

public class SearchProductsRequest : PaginationFilter, IRequest<PaginationResponse<ProductDto>>
{
  public Guid? BrandId { get; set; }
  public decimal? MinimumRate { get; set; }
  public decimal? MaximumRate { get; set; }
}

public class SearchProductsRequestHandler : IRequestHandler<SearchProductsRequest, PaginationResponse<ProductDto>>
{
  private readonly IReadRepository<Product> _repository;

  public SearchProductsRequestHandler(IReadRepository<Product> repository) => _repository = repository;

  public async Task<PaginationResponse<ProductDto>> Handle(SearchProductsRequest request, CancellationToken cancellationToken)
  {
    var spec = new ProductsBySearchRequestWithBrandsSpec(request);
    return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
  }
}

public class ProductsBySearchRequestWithBrandsSpec : EntitiesByPaginationFilterSpec<Product, ProductDto>
{
  public ProductsBySearchRequestWithBrandsSpec(SearchProductsRequest request) : base(request)
  {
    Query
      .Include(p => p.Brand)
      .OrderBy(c => c.Name, !request.HasOrderBy())
      .Where(p => p.BrandId.Equals(request.BrandId!.Value), request.BrandId.HasValue)
      .Where(p => p.Rate >= request.MinimumRate!.Value, request.MinimumRate.HasValue)
      .Where(p => p.Rate <= request.MaximumRate!.Value, request.MaximumRate.HasValue);
  }
}