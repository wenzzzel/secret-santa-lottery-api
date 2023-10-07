using Microsoft.AspNetCore.Mvc;

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
    public Response Get()
    {
        var participants = new Response()
        {
            Participants = new List<Participant>()
            {
                new() { Id = 0, Name = "Erik" },
                new() { Id = 1, Name = "Erika" },
                new() { Id = 2, Name = "Jimmy" },
                new() { Id = 3, Name = "Alex" }
            }
        };

        return participants;
    }
}
