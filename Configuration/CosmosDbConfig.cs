
namespace secret_santa_lottery_api.Configuration;

public class CosmosDbConfig
{
    public string ConnectionString { get; set; }
    public string DatabaseId { get; set; }
    public string ContainerId { get; set; }
}