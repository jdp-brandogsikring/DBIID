// simple C# console application to receive messages from a server
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string url = "http://localhost:5000/";
        using var listener = new HttpListener();
        listener.Prefixes.Add(url);
        listener.Start();
        Console.WriteLine($"Listening for HTTP requests on {url}");

        while (true)
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;

            // Log request info
            Console.WriteLine($"Received {request.HttpMethod} request for {request.Url}");

            // If there is a body, read and log it
            if (request.HasEntityBody)
            {
                using var body = request.InputStream;
                using var reader = new System.IO.StreamReader(body, request.ContentEncoding);
                string requestBody = await reader.ReadToEndAsync();
                Console.WriteLine("Body:");
                Console.WriteLine(requestBody);
            }

            // Respond to the client
            string responseString = "Request received";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            context.Response.Close();
        }
    }
}

