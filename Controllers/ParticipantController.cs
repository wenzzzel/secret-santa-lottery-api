using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using secret_santa_lottery_api.Persistence;

namespace secret_santa_lottery_api.Controllers;

[ApiController]
[Route("[controller]")]
public class ParticipantController : ControllerBase
{
    private readonly ILogger<ParticipantController> _logger;
    private readonly IParticipantRepository _participantRepo;

    public ParticipantController(ILogger<ParticipantController> logger, IParticipantRepository participantRepo)
    {
        _logger = logger;
        _participantRepo = participantRepo;
    }

    [HttpGet(Name = "GetParticipant")]
    public async Task<Response> Get()
    {        
        var participants = await _participantRepo.GetAllParticipantsAsync();

        var retval = new Response() { Participants = participants };

        return retval;
    }

    [HttpDelete(Name = "DeleteParticipant")]
    public async Task<ItemResponse<Participant>> Delete(string id, string name)
    {        
        var participant = new Participant()
        {
            id = id,
            name = name
        };

        return await _participantRepo.RemoveParticipantAsync(participant);
    }

    [HttpPost(Name = "CreateParticipant")]
    public async Task<ItemResponse<Participant>> Post(string id, string name)
    {        
        var participant = new Participant()
        {
            id = id,
            name = name
        };

        return await _participantRepo.CreateParticipantAsync(participant);
    }
}
