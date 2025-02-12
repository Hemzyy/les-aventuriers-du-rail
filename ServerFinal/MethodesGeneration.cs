using System.Text;

namespace serverPackage;

public class MethodesGeneration
{
    public string GenerateRandomCode()
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder codeBuilder = new StringBuilder();
        for (int i = 0; i < 6; i++)
        {
            codeBuilder.Append(chars[random.Next(chars.Length)]);
        }
        return codeBuilder.ToString();
    }

}