using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Demo.Stats;
using Newtonsoft.Json;
using System.IO;

namespace TwitterStats
{
    class Program
    {
        static TwitterContext Context;
        static StatsProcessor processor;
        static JsonSerializer serializer;

        static TextCompressionStratagy compressor = new TextCompressionStratagy();

        const int twitterCount = 5;

        static void Main(string[] args)
        {
            Init();

            while (true)
            {
                Console.WriteLine("Введите твиттер-аккаунт:");

                var userName = Console.ReadLine() ?? string.Empty;

                if (userName == string.Empty)
                {
                    break;
                }
                
                try
                {
                    if (!Validate(userName))
                    {
                        throw new Exception("Неверный твиттер-аккаунт");
                    }
                    
                    Store(userName, Collect(userName));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("Работа завершена...");
            Console.Read();
        }

        private static bool Validate(string userName)
        {
            return userName.StartsWith("@") || !userName.Contains("@");
        }

        private static void Store(string userName, Dictionary<char, double> stats)
        {
            string statsJson = ToJson(stats);

            // todo: extract to culrure dependent resources
            string text = $"{userName}, статистика для последних {twitterCount} твитов: {statsJson}";

            Console.WriteLine(text);

            SendTweet(text);

            Console.WriteLine("Статистика сохранена.");
        }

        private static Dictionary<char, double> Collect(string userName)
        {
            processor.Clear();

            var tweets = GetTweets(userName, twitterCount).ToArray();

            foreach (var tweet in tweets)
            {
                processor.Add(tweet);
            }

            return processor.GetFrequence(3);
        }

        private static string ToJson(Dictionary<char, double> stats)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            using (JsonTextWriter writer = new JsonTextWriter(sw))
            {
                writer.QuoteChar = '\'';
                writer.QuoteName = false;

                JsonSerializer ser = new JsonSerializer();
                ser.Serialize(writer, stats);
            }

            var statsJson = sb.ToString();
            return statsJson;
        }

        private static void Init()
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = ConfigurationManager.AppSettings["consumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"],
                    AccessToken = ConfigurationManager.AppSettings["accessToken"],
                    AccessTokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"]
                }
            };

            Context = new TwitterContext(auth);

            serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            var counter = new LettersCounterFactory().GetCounter();

            processor = new StatsProcessor(counter);
        }

        // todo: extract twitter exchange methods to TextProvider and ResultStorage

        static List<string> GetTweets(string accountName, int count)
        {
            var tweets = Context.Status.Where(t => t.Type == StatusType.User && t.ScreenName == accountName && t.Count == count);

            return tweets.Select(t => t.Text).ToList();
        }

        static async Task<List<string>> GetTweetsAsync(string accountName, int count)
        {
            return await Task.Run(() => GetTweets(accountName, count));
        }

        static void SendTweet(string text)
        {
            Task task = Context.TweetAsync(compressor.Compress(text));
            task.Wait();

            if (task.Exception != null)
            {
                throw task.Exception;
            }
        }
    }

    internal class TextCompressionStratagy
    {
        const int maxTweetLength = 279;
        public string Compress(string text)
        {
            var textLength = text.Length;
            var needTruncate = textLength > maxTweetLength;
            return text.Substring(0, needTruncate ? maxTweetLength - 1 : textLength) + (needTruncate ? "…" : "");
        }
    }
}
