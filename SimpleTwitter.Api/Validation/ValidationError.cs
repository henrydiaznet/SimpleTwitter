namespace SimpleTwitter.Api.Validation;

internal class ValidationError
{
    public IEnumerable<string> MemberNames { get; set; } = [];
    public string? ErrorMessage { get; set; }

    public override string ToString()
    {
        return $"{string.Join(",", MemberNames)} : {ErrorMessage}";
    }
}