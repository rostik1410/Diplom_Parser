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
        private ITextAnalyticsAPI client;
        public SentimentAnalysis()
        {
            client = new TextAnalyticsAPI();

            client.AzureRegion = AzureRegions.Westeurope;
            client.SubscriptionKey = "da48766dfb1849a6867023715a174918"; //уникальний ключ сервіса 

        }
        public void getSentimentalClass(List<string> reviews)
        {
           
            SentimentBatchResult result = client.Sentiment(
                new MultiLanguageBatchInput(
                    new List<MultiLanguageInput>()
                    {
                        //вставити замість цього коменти з бд
                        new MultiLanguageInput("ru", "0", "Это очень хороший телефон!"),
                        new MultiLanguageInput("ru", "1", "доволен покупкой"),
                        new MultiLanguageInput("ru", "2", "Не покупайте этот ужасный телефон"),
                        new MultiLanguageInput("ru", "3", "Хорошее решение за свои деньги"),
                        new MultiLanguageInput("ru", "4", "Не советую это к покупке"),
                        new MultiLanguageInput("ru", "5", "Купил телефон три месяца назад. Обновился к андройд 7.1.2. В игры не играю, поэтому батареи хватает на день. Хорошие качество самого телефона, материал метал. Стекло горила глаз 3, хожу без пленки. Царапин нет. Жду обновлений на Андройд 8. Установил гугл камеру, качество снимков увеличилось! Рекомендую аппарат .Переваги: Отличный телефон! Чистый андройд. Качественная сборка. Презентабельный на вид.Недоліки: Нет."),
                    }));

            StreamWriter sw = new StreamWriter(new FileStream("sentiment_test.txt", FileMode.Create, FileAccess.Write), Encoding.GetEncoding(1251));

            foreach (var doc in result.Documents)
            {
                sw.WriteLine("Document ID: {0} , Sentiment Score: {1:0.00}", doc.Id, doc.Score);
            }
            sw.Close();
        }
    }
}
