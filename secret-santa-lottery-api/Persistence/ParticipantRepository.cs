using Microsoft.Azure.Cosmos;
using secret_santa_lottery_api.Configuration;
using secret_santa_lottery_api.Models;

namespace secret_santa_lottery_api.Persistence;

public interface IParticipantRepository
{
    Task<List<Participant>> GetAllParticipants();
    Task<ItemResponse<Participant>> DeleteParticipant(Participant participant);
    Task<ItemResponse<Participant>> CreateParticipant(Participant participant);
    Task<ItemResponse<Participant>> UpdateParticipant(Participant participant);

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
                participants.Add(new(result.Id, result.Name, result.Partner));
        }

        return participants;
    }

    public async Task<ItemResponse<Participant>> DeleteParticipant(Participant participant)
    {
        return await _container.DeleteItemAsync<Participant>(participant.Id.ToString(), new PartitionKey(participant.Name));
    }

    public async Task<ItemResponse<Participant>> CreateParticipant(Participant participant)
    {
        return await _container.CreateItemAsync(participant);
    }

    public async Task<ItemResponse<Participant>> UpdateParticipant(Participant participant)
    {
        return await _container.UpsertItemAsync(participant);
    }
}