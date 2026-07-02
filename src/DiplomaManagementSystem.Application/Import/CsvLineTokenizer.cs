using System.Text;

namespace DiplomaManagementSystem.Application.Import;

internal static class CsvLineTokenizer
{
    public static string[] Split(string line)
    {
        List<string> fields = [];
        StringBuilder current = new();
        bool inQuotes = false;

        for (int index = 0; index < line.Length; index++)
        {
            char character = line[index];

            if (inQuotes)
            {
                if (character == '"')
                {
                    if (index + 1 < line.Length && line[index + 1] == '"')
                    {
                        current.Append('"');
                        index++;
                    }
                    else
                    {
                        inQuotes = false;
                    }
                }
                else
                {
                    current.Append(character);
                }

                continue;
            }

            if (character == '"')
            {
                inQuotes = true;
            }
            else if (character == ',')
            {
                fields.Add(current.ToString().Trim());
                current.Clear();
            }
            else
            {
                current.Append(character);
            }
        }

        fields.Add(current.ToString().Trim());
        return [.. fields];
    }
}
