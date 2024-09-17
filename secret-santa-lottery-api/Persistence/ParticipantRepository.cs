using System.Net;
using Microsoft.Azure.Cosmos;
using secret_santa_lottery_api.Configuration;
using secret_santa_lottery_api.Models;

namespace secret_santa_lottery_api.Persistence;

public interface IParticipantRepository
{
    Task<List<Participant>> GetAllParticipants();
    Task<HttpStatusCode> DeleteParticipant(string Id, string PartitionKey);
    Task<Participant> CreateParticipant(Participant participant);
    Task<Participant> UpdateParticipant(Participant participant);
}

public class ParticipantRepository : IParticipantRepository
{
    private CosmosClient _client;
    private Container _container;

    public ParticipantRepository(IOptions<CosmosDbConfig> options)
    {
        _client = new CosmosClient(options.Value.ConnectionString);
        _container = _client.GetContainer(options.Value.DatabaseId, options.Value.ContainerId);
    }

    public async Task<List<Participant>> GetAllParticipants()
    {
        var participants = new List<Participant>();

        using FeedIterator<Participant> feed = _container.GetItemQueryIterator<Participant>("SELECT * FROM c");

        while (feed.HasMoreResults)
        {
            FeedResponse<Participant> results = await feed.ReadNextAsync();

            foreach (var result in results)
                participants.Add(new(result.id, result.name, result.partner, result.santaForId));
        }

        return participants;
    }

    public async Task<HttpStatusCode> DeleteParticipant(string Id, string PartitionKey)
    {
        var cosmosResponse = await _container.DeleteItemAsync<Participant>(Id, new PartitionKey(PartitionKey));

        return cosmosResponse.StatusCode;
    }

    public async Task<Participant> CreateParticipant(Participant participant)
    {
        var cosmosResponse = await _container.CreateItemAsync(participant);

        return cosmosResponse.Resource;
    }

    public async Task<Participant> UpdateParticipant(Participant participant)
    {
        var cosmosResponse = await _container.UpsertItemAsync(participant, new(participant.name));

        return cosmosResponse.Resource;
    }
}