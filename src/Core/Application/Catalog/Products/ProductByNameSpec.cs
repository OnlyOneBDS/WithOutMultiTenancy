namespace WithOutMultiTenancy.Application.Catalog.Products;

public class ProductByNameSpec : SingleResultSpecification<Product>
{
  public ProductByNameSpec(string name)
  {
    Query.Where(p => p.Name == name);
  }
}