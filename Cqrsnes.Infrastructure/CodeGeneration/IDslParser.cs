namespace Cqrsnes.Infrastructure.CodeGeneration
{
    public interface IDslParser
    {
        Entity Parse(string line);
    }
}
