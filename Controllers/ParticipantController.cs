using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace secret_santa_lottery_api.Controllers;

[ApiController]
[Route("[controller]")]
public class ParticipantController : ControllerBase
{
    private readonly ILogger<ParticipantController> _logger;

    public ParticipantController(ILogger<ParticipantController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetParticipant")]
    public async Task<Response> Get()
    {        
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

        var retval = new Response() { Participants = participants };

        return retval;
    }
}
