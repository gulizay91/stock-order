namespace Order.Api.Persistence.Entities.Common;

public record AuditInformation
{
  public DateTime CreatedDate { get; init; }
  public DateTime? LastModifiedDate { get; set; }
}