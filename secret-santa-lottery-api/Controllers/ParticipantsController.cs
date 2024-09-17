using Microsoft.AspNetCore.Mvc;
using secret_santa_lottery_api.Persistence;
using secret_santa_lottery_api.Models;

namespace secret_santa_lottery_api.Controllers;

[ApiController]
[Route("[controller]")]
public class ParticipantsController(IParticipantRepository _participantRepo) : ControllerBase
{
    [HttpGet(Name = "GetParticipants")]
    public async Task<GetResponse> Get()
    {        
        var participants = await _participantRepo.GetAllParticipants();
        return new GetResponse(participants);
    }

    [HttpDelete(Name = "DeleteParticipant")]
    public async Task<DeleteResponse> Delete(string id, string name)
    {
        var deleteResult = await _participantRepo.DeleteParticipant(id, name);
        return new DeleteResponse(deleteResult);
    }

    [HttpPost(Name = "CreateParticipant")]
    public async Task<Participant> Post([FromBody] Participant participant)
    {
        return await _participantRepo.CreateParticipant(participant);
    }

    [HttpPut(Name = "UpdateParticipant")]
    public async Task<Participant> Put([FromBody] Participant participant)
    {
        return await _participantRepo.UpdateParticipant(participant);
    }
}