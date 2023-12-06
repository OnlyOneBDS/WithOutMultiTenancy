namespace WithOutMultiTenancy.Application.Catalog.Brands;

public class BrandByNameSpec : SingleResultSpecification<Brand>
{
  public BrandByNameSpec(string name)
  {
    Query.Where(b => b.Name == name);
  }
}