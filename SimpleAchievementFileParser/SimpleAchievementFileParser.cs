﻿using SimpleAchievementFileParser.Model;

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
            Queue<string> queue = new Queue<string>();
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
                else if (line.Contains("visible"))
                {
                    currentAchievement.VisibleRequirements = new VisibleRequirements();
                    queue.Enqueue("visible");
                }
                else if (line.Contains("has_dlc"))
                {
                    string dlcName = line.Split('=')[1].Replace("\"", "").TrimStart().TrimEnd();
                    DlcNames.Add(dlcName);
                    //currentAchievement.VisibleRequirements.HasAllDlc.Add(dlcName);
                    queue.Enqueue("has_dlc");
                }
                else if (line.Contains("{"))
                {
                    string token = line.Split('=')[0].TrimStart().TrimEnd();
                    queue.Enqueue(token);
                    queue.Enqueue("{");

                    if (line.Contains("}"))
                    {
                        token = line.Split('=')[1].TrimStart().TrimEnd().Replace("{ ", "");
                        queue.Enqueue(token);
                        queue.Enqueue("}");
                        //string token = queue.Dequeue();
                    }
                }
                else if (line.Contains("}"))
                {
                    queue.Enqueue("}");
                    //string token = queue.Dequeue();
                }
                else if (line.Contains("="))
                {
                    string token = line.Split('=')[0].TrimStart().TrimEnd();
                    queue.Enqueue(token);
                }
            }

            CreateAchievements(currentAchievement, queue);

            return achievements;
        }

        public INodeAddAble CreateAchievements(INodeAddAble currentAchievement, Queue<string> queue)
        {
            INodeAddAble achievement = currentAchievement;
            INodeAddAble parentObject = null;
            INodeAddAble currentObject = null;
            INodeAddAble node = null;
            Stack<INodeAddAble> nodes = new Stack<INodeAddAble>();
            Stack<string> simpleNodes = new Stack<string>();
            while (queue.Count > 0)
            {
                string token = queue.Dequeue();

                if (token == "possible")
                {
                    currentObject = new Possible();
                    nodes.Push(currentObject);
                }
                else if (token == "happened")
                {
                    currentObject = new Happened();
                    nodes.Push(currentObject);
                }
                else if (token == "custom_trigger_tooltip")
                {
                    parentObject = currentObject;
                    currentObject = new CustomTriggerTooltip();
                    parentObject.Add(currentObject);
                    nodes.Push(currentObject);
                }
                else if (token == "NOT")
                {
                    parentObject = currentObject;
                    currentObject = new NotModel();
                    parentObject.Add(currentObject);
                    nodes.Push(currentObject);
                }
                else if (token == "OR")
                {
                    parentObject = currentObject;
                    currentObject = new OrModel();
                    parentObject.Add(currentObject);
                    nodes.Push(currentObject);
                }
                else if (token == "{" && (simpleNodes.Count == 0 || simpleNodes.Count > 0 && simpleNodes.Peek() != "{")) simpleNodes.Push("{");
                else if (token == "}")
                {
                    if (nodes.Count > 0)
                        node = nodes.Pop();
                    while (simpleNodes.Count > 0)
                    {
                        if (simpleNodes.Peek() == "{")
                        {
                            simpleNodes.Pop();
                            break;
                        }
                        node.Add(simpleNodes.Pop(), "pop");
                    }
                    if (nodes.Count == 0)
                        currentAchievement.Add(node);
                    currentObject = parentObject;
                }
                else if (simpleNodes.Count == 0 || simpleNodes.Count > 0 && token != "{") simpleNodes.Push(token);
            }
            return achievement;
        }
    }
}