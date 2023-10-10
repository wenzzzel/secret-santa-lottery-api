using Microsoft.Azure.Cosmos;

namespace secret_santa_lottery_api.Persistence;

public interface IParticipantRepository
{
    Task<List<Participant>> GetAllParticipantsAsync();
    Task<ItemResponse<Participant>> RemoveParticipantAsync(Participant participant);
    Task<ItemResponse<Participant>> CreateParticipantAsync(Participant participant);
}

public class ParticipantRepository : IParticipantRepository
{
    private readonly string _connectionString;
    private readonly string _databaseId;
    private readonly string _containerId;

    public ParticipantRepository()
    {
        //TODO: Move secrets to azure keyvault
        _connectionString = Environment.GetEnvironmentVariable("participantCosmosConnectionString");
        _databaseId = Environment.GetEnvironmentVariable("participantCosmosDbId");
        _containerId = Environment.GetEnvironmentVariable("participantCosmosContainerId");
    }

    public async Task<List<Participant>> GetAllParticipantsAsync()
    {
        var client = new CosmosClient(_connectionString);

        var container = client.GetContainer(_databaseId, _containerId);

        var participants = new List<Participant>();

        using FeedIterator<Participant> feed = container.GetItemQueryIterator<Participant>("SELECT * FROM c");

        while (feed.HasMoreResults)
        {
            FeedResponse<Participant> results = await feed.ReadNextAsync();

            foreach (var result in results)
            {
                participants.Add(new Participant()
                {
                    id = result.id,
                    name = result.name
                });
            }
        }

        return participants;
    }

    public async Task<ItemResponse<Participant>> RemoveParticipantAsync(Participant participant)
    {
        var client = new CosmosClient(_connectionString);

        var container = client.GetContainer(_databaseId, _containerId);

        var response = await container.DeleteItemAsync<Participant>(participant.id.ToString(), new PartitionKey(participant.name));

        return response;
    }

    public async Task<ItemResponse<Participant>> CreateParticipantAsync(Participant participant)
    {
        var client = new CosmosClient(_connectionString);

        var container = client.GetContainer(_databaseId, _containerId);

        var response = await container.CreateItemAsync(participant);
        
        return response;
    }
}