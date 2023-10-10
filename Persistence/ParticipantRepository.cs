using Microsoft.Azure.Cosmos;

namespace secret_santa_lottery_api.Persistence;

public interface IParticipantRepository
{
    Task<List<Participant>> GetAllParticipantsAsync();
}

public class ParticipantRepository : IParticipantRepository
{
    public async Task<List<Participant>> GetAllParticipantsAsync()
    {
        //TODO: Move secrets to azure keyvault
        var connectionString = Environment.GetEnvironmentVariable("participantCosmosConnectionString");
        var databaseId = Environment.GetEnvironmentVariable("participantCosmosDbId");
        var containerId = Environment.GetEnvironmentVariable("participantCosmosContainerId");

        var client = new CosmosClient(connectionString);

        var container = client.GetContainer(databaseId, containerId);

        var participants = new List<Participant>();

        using FeedIterator<Participant> feed = container.GetItemQueryIterator<Participant>("SELECT * FROM c");

        while (feed.HasMoreResults)
        {
            FeedResponse<Participant> results = await feed.ReadNextAsync();

            foreach (var result in results)
            {
                participants.Add(new Participant()
                {
                    Id = result.Id,
                    Name = result.Name
                });
            }
        }

        return participants;
    }
}