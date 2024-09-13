namespace secret_santa_lottery_api.Models;

public class Response
{
    public List<Participant> Participants { get; }

    public Response(List<Participant> participants)
    {
        Participants = participants;
    }
}
