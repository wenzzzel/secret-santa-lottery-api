namespace secret_santa_lottery_api.Models;

public class Participant
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Partner { get; set; }

    public Participant(string id, string name, string partner)
    {
        Id = id;
        Name = name;
        Partner = partner;
    }
}