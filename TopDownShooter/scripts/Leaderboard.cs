using Godot;
using System.Collections.Generic;

public class Leaderboard : Control
{

    private string savePath = "user://leaderboard.txt";

    private TextEdit leaderboardText;

    public override void _EnterTree()
    {
        base._EnterTree();

        leaderboardText = GetNode<TextEdit>("LeaderBoard");

        File leaderboard = new File();

        // First time creating the leaderboard
        if (!leaderboard.FileExists(savePath))
        {
            leaderboard.Open(savePath, File.ModeFlags.Write);

            Dictionary<string, int> data = new Dictionary<string, int> { { Global.username, Global.enemiesKilled } };

            leaderboard.StoreString(DictionaryToString(data));

            leaderboard.Close();
        }
        else
        {

            leaderboard.Open(savePath, File.ModeFlags.Read);

            Dictionary<string, int> leaderboardData = new Dictionary<string, int>(leaderboard.GetAsText().Count(":"));

            for (int i = 0; i < leaderboard.GetAsText().Count(":"); i++)
            {
                var line = leaderboard.GetLine();

                var username = line.Split(":")[0];
                var enemiesKilled = line.Split(":")[1].ToInt();

                leaderboardData[username] = enemiesKilled;
            }

            if(leaderboardData.ContainsKey(Global.username)){
                if(leaderboardData[Global.username] > Global.enemiesKilled){
                    Global.enemiesKilled = leaderboardData[Global.username];
                }
            }

            if (leaderboardData.Count < 6)
            {
                leaderboardData[Global.username] = Global.enemiesKilled;
            }
            else
            {

                Dictionary<string, int> leaderboardDataCopy = leaderboardData;

                string lowestUser = Global.username;

                foreach (string user in leaderboardData.Keys)
                {
                    if (Global.enemiesKilled > leaderboardData[user])
                    {
                        leaderboardDataCopy.Add(Global.username, Global.enemiesKilled);

                        foreach (string username in leaderboardData.Keys)
                        {
                            if (leaderboardDataCopy[username] < leaderboardDataCopy[lowestUser])
                            {
                                lowestUser = username;
                            }
                        }

                        leaderboardDataCopy.Remove(lowestUser);

                        leaderboardData = leaderboardDataCopy;

                        break;
                    }
                }

            }


            leaderboard.Close();

            leaderboard.Open(savePath, File.ModeFlags.Write);


            foreach (string user in leaderboardData.Keys)
            {
                leaderboard.StoreLine(user + ":" + leaderboardData[user].ToString());
            }

            leaderboard.Close();

        }

        setLeaderboard();
    }

    public void setLeaderboard()
    {

        string leaderboardStr = "";

        string highestuser = "";

        File leaderboard = new File();


        leaderboard.Open(savePath, File.ModeFlags.Read);

        Dictionary<string, int> leaderboardData = new Dictionary<string, int>(leaderboard.GetAsText().Count(":"));

        for (int i = 0; i < leaderboard.GetAsText().Count(":"); i++)
        {
            var line = leaderboard.GetLine();

            var username = line.Split(":")[0];
            var enemiesKilled = line.Split(":")[1].ToInt();

            leaderboardData[username] = enemiesKilled;
        }

        for (int i = 0; i < leaderboard.GetAsText().Count(":"); i++)
        {

            foreach (string user in leaderboardData.Keys)
            {
                highestuser = user;

                foreach (string username in leaderboardData.Keys)
                {
                    if (leaderboardData[username] > leaderboardData[highestuser])
                    {
                        highestuser = username;
                    }
                }

            }

            leaderboardStr += $"{highestuser}\n\tEnemies Killed = {leaderboardData[highestuser]}\n\n";

            leaderboardData.Remove(highestuser);
        }

        leaderboardText.Text = leaderboardStr;

    }


    public void _on_PlayAgain_pressed()
    {
        GetTree().ChangeScene("res://scenes/Main.tscn");
    }

    public void _on_Quit_pressed()
    {
        GetTree().Quit();
    }

    public string DictionaryToString(Dictionary<string, int> dictionary)
    {
        string dictionaryString = "";
        foreach (KeyValuePair<string, int> keyValues in dictionary)
        {
            dictionaryString += keyValues.Key + ":" + keyValues.Value;
        }
        return dictionaryString;
    }

}
