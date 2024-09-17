namespace secret_santa_lottery_api.Models;

public class Participant
{
    public string id { get; set; }

    public string name { get; set; }

    public string partner { get; set; }
    public string santaForId { get; set; }

    public Participant(string id, string name, string partner, string santaForId)
    {
        this.id = id;
        this.name = name;
        this.partner = partner;
        this.santaForId = santaForId;
    }
}