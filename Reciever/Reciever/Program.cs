using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Reciever
{
    class Program
    {
        private static readonly string serviceUrl = "http://localhost:56498";
        static readonly HttpClient Client = new HttpClient();

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "task_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    //todo call fake api


                    Console.WriteLine(" [x] Received {0}", message);

                    GetData(message);

                    Console.WriteLine(" [x] Done");

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };
                channel.BasicConsume(queue: "task_queue",
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static async void GetData(string message)
        {

            String path = String.Format("{0}/requests/{1}", serviceUrl, message);

            var postContent = new FormUrlEncodedContent(
                new[]{
                new KeyValuePair<string, string>("Content-Type", "application/json")
                }
            );


            try
            {
                HttpResponseMessage response = await Client.PostAsync(path, postContent);
                String content = await response.Content.ReadAsStringAsync();
                //res = JsonConvert.DeserializeObject<LawServiceResponse>(content);
                //res = await response.Content.ReadAsAsync<UserSocialResponse>();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                Console.WriteLine(e.StackTrace);

                // Todo Handle exception
            }
        }
    }
}
