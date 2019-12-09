using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MusicPlaylistAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            string songsPath = args[0];
            string resultsPath = args[1];

            List<string[]> songData = new List<string[]>(); //2d List of song data
            int q1 = 0;
            int q2 = new int();
            int q3 = new int();
            IEnumerable<string[]> q4 = Enumerable.Empty<string[]>();
            IEnumerable<string[]> q5 = Enumerable.Empty<string[]>();
            IEnumerable<string[]> q6 = Enumerable.Empty<string[]>();
            String[] q7 = {};

            int songName = 0; //indeces of columns
            int songArtist = 1;
            int songAlbum = 2;
            int songGenre = 3;
            int songSize = 4;
            int songTime = 5;
            int songYear = 6;
            int songPlays = 7;

            string[] values = { };
            int lineCount = 0;
            const Int32 BufferSize = 128;

            try
            {
                using (var fileStream = File.OpenRead(songsPath)) //open file with song data, tab delimited
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize: BufferSize))

                {
                    String line;


                    while ((line = streamReader.ReadLine()) != null) //Get data from text lines
                    {
                        if (lineCount != 0)
                        {
                            values = line.Split('\t');
                            songData.Add(values);
                            if (values.Length != 8)
                            {
                                return;
                            }
                        }

                        lineCount += 1;

                    }


                }
            }
            catch
            {
                Console.WriteLine(String.Format("Failed to open and read {0}", songsPath));
                return;
            }

            if (values.Length != 8)
                {
                    Console.WriteLine(String.Format("Row {0} contains {1} values. It should contain 8.", lineCount + 1, values.Length));
                    Console.ReadLine();
                    return;
                }


            foreach (var songLine in songData)
            {
                if (Convert.ToInt32(songLine[songPlays]) >= 200)
                {
                    q1 += 1;
                }

                if (songLine[songGenre].ToLower() == "alternative")
                {
                    q2 += 1;
                }

                if (songLine[songGenre].ToLower() == "hip-hop/rap" || songLine[songGenre].ToLower() == "hip hop/rap") //The example report on the final project module is wrong! some genre entries in the txt file are missing the hyphen.
                {
                    q3 += 1;
                }
            }

            q4 = from songLine in songData where songLine[songAlbum].ToLower() == "welcome to the fishbowl" select songLine;
            q5 = from songLine in songData where Convert.ToInt32(songLine[songYear]) < 1970 select songLine;
            q6 = from songLine in songData where songLine[songName].Length > 85 select songLine;
            q7 = songData.OrderByDescending(x => Convert.ToInt32(x[songTime])).First();

            List<string> answers = new List<string>();
            answers.Add(String.Format("{0} songs received 200 or more plays.", q1)); //question 1 answer
            answers.Add("");

            answers.Add(String.Format("{0} songs are alternative genre.", q2)); //question 2 answer
            answers.Add("");

            answers.Add(String.Format("{0} songs are hip hop/rap genre.", q3)); //q3 answer
            answers.Add("");

            answers.Add("Songs in album Welcome to the fishbowl:"); //q4 answers
            foreach (var item in q4)
            {
                answers.Add(String.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", item[songName], item[songArtist], item[songAlbum], item[songGenre], item[songSize], item[songTime], item[songYear], item[songPlays]));
            }
            answers.Add("");

            answers.Add("Songs predating 1970:"); //q5 answers
            foreach (var item in q5)
            {
                answers.Add(String.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", item[songName], item[songArtist], item[songAlbum], item[songGenre], item[songSize], item[songTime], item[songYear], item[songPlays]));
            }
            answers.Add("");

            answers.Add("Song names more than 85 characters long:"); //q6 answers
            foreach (var item in q6)
            {
                answers.Add(String.Format("Name: {0}", item[songName]));
            }
            answers.Add("");

            answers.Add(String.Format("Longest Song: Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", q7[songName], q7[songArtist], q7[songAlbum], q7[songGenre], q7[songSize], q7[songTime], q7[songYear], q7[songPlays])); //q7 answer

            //write results to file
            try
            {
                File.WriteAllLines(resultsPath, answers);
            }
            catch
            {
                Console.WriteLine(String.Format("Failed to write results to {0}", resultsPath));
                return;
            }

            //Console.WriteLine(String.Format("{0} songs received 200 or more plays.", q1)); //question 1 answer
           // Console.WriteLine(String.Format("{0} songs are alternative genre.", q2)); //question 2 answer
            //Console.WriteLine(String.Format("{0} songs are hip hop/rap genre.", q3)); //question 3 answer; 
            
            //Console.WriteLine("Songs in album Welcome to the fishbowl:"); //question 4 answer
            //foreach (var item in q4)
            //{
            //    Console.WriteLine(String.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", item[songName], item[songArtist], item[songAlbum], item[songGenre], item[songSize], item[songTime], item[songYear], item[songPlays]));
            //}

            //Console.WriteLine("Songs predating 1970:"); //question 5 answer
            //foreach (var item in q5)
            //{
            //    Console.WriteLine(String.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", item[songName], item[songArtist], item[songAlbum], item[songGenre], item[songSize], item[songTime], item[songYear], item[songPlays]));
            //}

            //Console.WriteLine("Song names more than 85 characters long:"); //question 6 answer
            //foreach (var item in q6)
            //{
            //    Console.WriteLine(String.Format("Name: {0}", item[songName]));
            //}

            //Console.WriteLine(String.Format("Longest Song: Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", q7[songName], q7[songArtist], q7[songAlbum], q7[songGenre], q7[songSize], q7[songTime], q7[songYear], q7[songPlays])); //question 7 answer







        }
    }
}
