using NUnit.Framework;
using System.Net.Http;
using System.Text;

namespace WebApiTests
{
    public class Tests
    {
        [Test]
        public void Test1()
        {

            HttpClient client = new HttpClient();
            //Выполняем запрос
            StringContent content = new StringContent("[{\"position\":\"manager\",\"salary\": \"150000\",\"bonus\": \"0\"}," +
                "{\"position\":\"technician\",\"salary\": \"35000\",\"bonus\": \"5000\",\"category\": \"B\"}," +
                "{\"position\":\"driver\",\"salary\": \"250\",\"bonus\": \"10000\",\"timeWorked\": \"80\",\"category\": \"A\"}]", 
                Encoding.UTF8, "application/json");

            var response = client.PostAsync("https://localhost:44371/api/calculate_salary_sum", content).Result;

            var answer = response.Content.ReadAsStringAsync().Result;
            //f10 //answer

            //Провряем результат запроса
            if (response.StatusCode == System.Net.HttpStatusCode.OK && (answer == "230250"))
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}