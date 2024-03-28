using SimpleAchievementFileParser.Model;

namespace SimpleAchievementFileParser
{
    public class AchievementsStructureFileParser(string fileName)
    {
        public ISet<string> DlcNames { get; private set; } = new HashSet<string>();
        private readonly string _fileName = fileName;

        public IList<Achievement> ParseFile()
        {
            string? line;
            var fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream);
            Queue<KeyValuePair<string, string>> queue = new Queue<KeyValuePair<string, string>>();

            while ((line = reader.ReadLine()) != null)
            {
                var splitedLine = line.Split('=');
                if (line.FirstOrDefault() == '#' || line == string.Empty || line.Trim().StartsWith("province_id")) continue;
                if (line.StartsWith(Constants.TokenAchievement))
                {
                    queue.Enqueue(new KeyValuePair<string, string>(Constants.TokenAchievement, ""));
                }
                else if (splitedLine[0].Trim().Equals(Constants.TokenId))
                {
                    queue.Enqueue(new KeyValuePair<string, string>(Constants.TokenId, line.Split('=')[1].TrimStart().TrimEnd()));
                }
                else if (line.Contains(Constants.TokenLocalization))
                {
                    queue.Enqueue(new KeyValuePair<string, string>(Constants.TokenLocalization, line.Split('=')[1].TrimStart().TrimEnd()));
                }
                else if (line.Contains(Constants.TokenHasDlc))
                {
                    string dlcName = line.Split('=')[1].Replace("\"", "").TrimStart().TrimEnd();
                    DlcNames.Add(dlcName);
                    queue.Enqueue(new KeyValuePair<string, string>(Constants.TokenHasDlc, dlcName));
                }
                else if (line.Contains("{"))
                {
                    string token = line.Split('=')[0].TrimStart().TrimEnd();
                    string value = line.Split('=')[1].TrimStart().TrimEnd();
                    queue.Enqueue(new KeyValuePair<string, string>(token, value));
                    queue.Enqueue(new KeyValuePair<string, string>("{", ""));

                    if (line.Contains("}"))
                    {
                        token = line.Split('=')[1].TrimStart().TrimEnd().Replace("{ ", "");
                        value = line.Split('=')[2].TrimStart().TrimEnd().Replace(" }", "");
                        queue.Enqueue(new KeyValuePair<string, string>(token, value));
                        queue.Enqueue(new KeyValuePair<string, string>("}", ""));
                    }
                }
                else if (line.Contains("}"))
                {
                    queue.Enqueue(new KeyValuePair<string, string>("}", ""));
                }
                else if (line.Contains("="))
                {
                    string token = line.Split('=')[0].TrimStart().TrimEnd();
                    string value = line.Split('=')[1].TrimStart().TrimEnd();
                    queue.Enqueue(new KeyValuePair<string, string>(token, value));
                }
            }

            return CreateAchievements(queue);
        }

        public IList<Achievement> CreateAchievements(Queue<KeyValuePair<string, string>> queue)
        {
            INodeAddAble? parentObject = null;
            INodeAddAble? currentObject = null;
            INodeAddAble? node = null;
            Stack<INodeAddAble> nodes = new();
            Stack<KeyValuePair<string, string>> simpleNodes = new Stack<KeyValuePair<string, string>>();
            IList<Achievement> achievements = new List<Achievement>();

            while (queue.Count > 0)
            {
                KeyValuePair<string, string> token = queue.Dequeue();
                if (token.Key == Constants.TokenAchievement)
                {
                    currentObject = new Achievement(null);
                    parentObject = currentObject;
                    achievements.Add((Achievement)currentObject);
                    nodes.Push(currentObject);
                }
                else if (token.Key == Constants.TokenId)
                {
                    currentObject?.Add(Constants.TokenId, token.Value);
                }
                else if (token.Key == Constants.TokenLocalization)
                {
                    currentObject?.Add(Constants.TokenLocalization, token.Value);
                }
                else if (token.Key == Constants.TokenPossible)
                {
                    parentObject = currentObject?.GetParent() ?? currentObject;
                    currentObject = new Possible(currentObject);
                    parentObject?.Add(currentObject);
                    nodes.Push(currentObject);
                }
                else if (token.Key == Constants.TokenHappened)
                {
                    parentObject = currentObject?.GetParent() ?? currentObject;
                    currentObject = new Happened(currentObject);
                    parentObject?.Add(currentObject);
                    nodes.Push(currentObject);
                }
                else if (token.Key == Constants.TokenVisible)
                {
                    parentObject = currentObject?.GetParent() ?? currentObject;
                    currentObject = new VisibleRequirements(currentObject);
                    parentObject?.Add(currentObject);
                    nodes.Push(currentObject);
                }
                else if (token.Key == Constants.TokenCustomTriggerTooltip)
                {
                    parentObject = currentObject;
                    currentObject = new CustomTriggerTooltip(currentObject);
                    parentObject?.Add(currentObject);
                    nodes.Push(currentObject);
                }
                else if (token.Key == Constants.TokenNot)
                {
                    parentObject = currentObject;
                    currentObject = new NotModel(currentObject);
                    parentObject?.Add(currentObject);
                    nodes.Push(currentObject);
                }
                else if (token.Key == Constants.TokenOr)
                {
                    parentObject = currentObject;
                    currentObject = new OrModel(currentObject);
                    parentObject?.Add(currentObject);
                    nodes.Push(currentObject);
                }
                else if (token.Value == "{")
                {
                    parentObject = currentObject?.GetParent() ?? parentObject;
                    currentObject = new UnspecifiedNode(token.Key, currentObject?.GetParent());
                    parentObject?.Add(currentObject);
                    nodes.Push(currentObject);
                }
                else if (token.Key == "{" && (simpleNodes.Count == 0 || simpleNodes.Count > 0 && simpleNodes.Peek().Key != "{")) simpleNodes.Push(new KeyValuePair<string, string>("{", ""));
                else if (token.Key == "}")
                {
                    if (nodes.Count > 0)
                        node = nodes.Pop();
                    while (simpleNodes.Count > 0)
                    {
                        if (simpleNodes.Peek().Key == "{")
                        {
                            simpleNodes.Pop();
                            break;
                        }
                        var keyValuePair = simpleNodes.Pop();
                        node?.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                    if (nodes.Count == 0)
                        currentObject?.Add(node);
                    currentObject = parentObject;
                }
                else if (simpleNodes.Count == 0 || simpleNodes.Count > 0 && token.Key != "{") simpleNodes.Push(token);
            }
            return achievements;
        }
    }
}
