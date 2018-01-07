using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System.IO;

namespace Diplom_Parser
{
    class SentimentAnalysis
    {
        public void getSentimentalClass(string text)
        {
            ITextAnalyticsAPI client = new TextAnalyticsAPI();
            
            client.AzureRegion = AzureRegions.Westeurope;
            client.SubscriptionKey = "ba3d6f5b9db948cea3577af71c68baa4"; //уникальний ключ сервіса 

           
            SentimentBatchResult result = client.Sentiment(
                new MultiLanguageBatchInput(
                    new List<MultiLanguageInput>()
                    {
                        //вставити замість цього коменти з бд і збс)
                        new MultiLanguageInput("ru", "0", "Это очень хороший телефон!"),
                        new MultiLanguageInput("ru", "1", "доволен покупкой"),
                        new MultiLanguageInput("ru", "2", "Не покупайте это гавно"),
                    }));

            StreamWriter sw = new StreamWriter(new FileStream("sentiment_test.txt", FileMode.Create, FileAccess.Write), Encoding.GetEncoding(1251));

            foreach (var doc in result.Documents)
            {
                sw.Write("\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\");
                sw.Write("Document ID: {0} , Sentiment Score: {1:0.00}", doc.Id, doc.Score);
            }
            sw.Close();
        }
    }
}
