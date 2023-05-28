namespace Contracts.Common;

public interface IContract
{
  public Guid CorrelationId { get; set; }
}