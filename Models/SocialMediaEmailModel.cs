
public class SocialMediaEmailModel
{
    public int Id { get; set; }
    public string? Emails { get; set; }
    public int UserId { get; set; }
    public int SocialType { get; set; }

    public static implicit operator string(SocialMediaEmailModel v)
    {
        throw new NotImplementedException();
    }
}