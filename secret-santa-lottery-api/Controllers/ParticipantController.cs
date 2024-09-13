using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using secret_santa_lottery_api.Persistence;
using secret_santa_lottery_api.Models;

namespace secret_santa_lottery_api.Controllers;

[ApiController]
[Route("[controller]")]
public class ParticipantController(IParticipantRepository _participantRepo) : ControllerBase
{
    [HttpGet(Name = "GetParticipant")]
    public async Task<Response> Get()
    {        
        var participants = await _participantRepo.GetAllParticipants();

        return new Response(participants);
    }

    [HttpDelete(Name = "DeleteParticipant")]
    public async Task<ItemResponse<Participant>> Delete(string id, string name, string partner)
    {
        return await _participantRepo.DeleteParticipant(new(id, name, partner));
    }

    [HttpPost(Name = "CreateParticipant")]
    public async Task<ItemResponse<Participant>> Post(string id, string name, string partner)
    {
        return await _participantRepo.CreateParticipant(new(id, name, partner));
    }

    [HttpPut(Name = "UpdateParticipant")]
    public async Task<ItemResponse<Participant>> Put(string id, string name, string partner)
    {
        return await _participantRepo.UpdateParticipant(new(id, name, partner));
    }
}
