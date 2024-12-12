public static class SoftwareDeveloperResultExtensions
{
    public static IResult ToResult(this SoftwareDeveloper developer)
    {
        return new SoftwareDeveloperResult(developer);
    }
}