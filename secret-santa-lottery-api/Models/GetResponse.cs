namespace secret_santa_lottery_api.Models;

public class GetResponse
{
    public List<Participant> Participants { get; }

    public GetResponse(List<Participant> participants)
    {
        Participants = participants;
    }
}
