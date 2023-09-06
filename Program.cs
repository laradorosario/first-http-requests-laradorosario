// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using CS_First_HTTP_Client;

string baseAddress = "https://cat-fact.herokuapp.com";

using var client = new HttpClient() { BaseAddress = new(baseAddress) };

var request = new HttpRequestMessage(HttpMethod.Get, "/facts");

var response = client.Send(request);

if (response.IsSuccessStatusCode)
{
    var content = (List<CatFact>?)JsonSerializer.Deserialize(
        response.Content.ReadAsStream(), typeof(List<CatFact>));

    if (content is null)
    {
        Console.WriteLine("Something went wrong...");
        return;
    }

    foreach (var fact in content)
    {
        Console.WriteLine(fact.text);
    }
}
else
{
    Console.WriteLine($"Request failed with status code {response.StatusCode}");
}