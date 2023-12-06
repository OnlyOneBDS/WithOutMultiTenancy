using WithOutMultiTenancy.Domain.Common.Events;

namespace WithOutMultiTenancy.Application.Catalog.Products;

public class DeleteProductRequest : IRequest<Guid>
{
  public Guid Id { get; set; }

  public DeleteProductRequest(Guid id)
  {
    Id = id;
  }
}

public class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequest, Guid>
{
  private readonly IRepository<Product> _repository;
  private readonly IStringLocalizer _t;

  public DeleteProductRequestHandler(IRepository<Product> repository, IStringLocalizer<DeleteProductRequestHandler> t)
  {
    _repository = repository;
    _t = t;
  }

  public async Task<Guid> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
  {
    var product = await _repository.GetByIdAsync(request.Id, cancellationToken)
      ?? throw new NotFoundException(_t["Product {0} Not Found."]);

    // Add Domain Events to be raised after the commit
    product.DomainEvents.Add(EntityDeletedEvent.WithEntity(product));
    await _repository.DeleteAsync(product, cancellationToken);

    return request.Id;
  }
}

public class ProductsByBrandSpec : Specification<Product>
{
  public ProductsByBrandSpec(Guid brandId)
  {
    Query.Where(p => p.BrandId == brandId);
  }
}