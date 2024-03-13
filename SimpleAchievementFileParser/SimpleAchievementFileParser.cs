using SimpleAchievementFileParser.Model;
using System.Collections.Generic;

namespace SimpleAchievementFileParser
{
    public class SimpleAchievementFileParser
    {
        public ISet<string> DlcNames { get; private set; }
        private readonly string _fileName;

        public SimpleAchievementFileParser(string fileName)
        {
            _fileName = fileName;
            DlcNames = new HashSet<string>();
        }

        public IList<Achievement> ParseFile()
        {
            string line;
            IList<Achievement> achievements = new List<Achievement>();
            var fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream);
            line = reader.ReadLine();
            Achievement currentAchievement = null;
            Queue<KeyValuePair<string, string>> queue = new Queue<KeyValuePair<string, string>>();
            while ((line = reader.ReadLine()) != null)
            {
                if (line.FirstOrDefault() == '#' || line == string.Empty) continue;
                if (line.StartsWith("achievement"))
                {
                    currentAchievement = new Achievement();
                    achievements.Add(currentAchievement);
                }
                else if (line.Contains("id"))
                {
                    currentAchievement.Id = int.Parse(line.Split('=')[1].TrimStart().TrimEnd());
                }
                else if (line.Contains("localization"))
                {
                    currentAchievement.Localization = line.Split('=')[1].TrimStart().TrimEnd();
                }
                //else if (line.Contains("visible"))
                //{
                //    currentAchievement.VisibleRequirements = new VisibleRequirements();
                //    queue.Enqueue("visible");
                //}
                else if (line.Contains("has_dlc"))
                {
                    string dlcName = line.Split('=')[1].Replace("\"", "").TrimStart().TrimEnd();
                    DlcNames.Add(dlcName);
                    //currentAchievement.VisibleRequirements.HasAllDlc.Add(dlcName);
                    queue.Enqueue(new KeyValuePair<string, string>("has_dlc", dlcName));
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
                        //string token = queue.Dequeue();
                    }
                }
                else if (line.Contains("}"))
                {
                    queue.Enqueue(new KeyValuePair<string, string>("}", ""));
                    //string token = queue.Dequeue();
                }
                else if (line.Contains("="))
                {
                    string token = line.Split('=')[0].TrimStart().TrimEnd();
                    string value = line.Split('=')[1].TrimStart().TrimEnd();
                    queue.Enqueue(new KeyValuePair<string, string>(token, value));
                }
            }

            CreateAchievements(currentAchievement, queue);

            return achievements;
        }

        public INodeAddAble CreateAchievements(INodeAddAble currentAchievement, Queue<KeyValuePair<string, string>> queue)
        {
            INodeAddAble achievement = currentAchievement;
            INodeAddAble parentObject = null;
            INodeAddAble currentObject = null;
            INodeAddAble node = null;
            Stack<INodeAddAble> nodes = new Stack<INodeAddAble>();
            Stack<KeyValuePair<string, string>> simpleNodes = new Stack<KeyValuePair<string, string>>();
            while (queue.Count > 0)
            {
                KeyValuePair<string, string> token = queue.Dequeue();

                if (token.Key == "possible")
                {
                    currentObject = new Possible();
                    nodes.Push(currentObject);
                }
                else if (token.Key == "happened")
                {
                    currentObject = new Happened();
                    nodes.Push(currentObject);
                }
                else if (token.Key == "custom_trigger_tooltip")
                {
                    parentObject = currentObject;
                    currentObject = new CustomTriggerTooltip();
                    parentObject.Add(currentObject);
                    nodes.Push(currentObject);
                }
                else if (token.Key == "NOT")
                {
                    parentObject = currentObject;
                    currentObject = new NotModel();
                    parentObject.Add(currentObject);
                    nodes.Push(currentObject);
                }
                else if (token.Key == "OR")
                {
                    parentObject = currentObject;
                    currentObject = new OrModel();
                    parentObject.Add(currentObject);
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
                        node.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                    if (nodes.Count == 0)
                        currentAchievement.Add(node);
                    currentObject = parentObject;
                }
                else if (simpleNodes.Count == 0 || simpleNodes.Count > 0 && token.Key != "{") simpleNodes.Push(token);
            }
            return achievement;
        }
    }
}
