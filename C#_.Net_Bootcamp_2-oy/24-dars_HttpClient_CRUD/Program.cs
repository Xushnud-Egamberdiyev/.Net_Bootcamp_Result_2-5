using System.Net.Http.Headers;
var client = new HttpClient();
var request = new HttpRequestMessage
{
    Method = HttpMethod.Get,
    RequestUri = new Uri("https://tomorrow-io1.p.rapidapi.com/v4/weather/forecast?location=42.15%2C%2082%2C1&timesteps=1h&units=metric"),
    Headers =
    {
        { "X-RapidAPI-Key", "ec5b031c78msh5e68789eb050501p15a42ajsn288b2bc02719" },
        { "X-RapidAPI-Host", "tomorrow-io1.p.rapidapi.com" },
    },
};
using (var response = await client.SendAsync(request))
{
    response.EnsureSuccessStatusCode();
    var body = await response.Content.ReadAsStringAsync();
    Console.WriteLine(body);
}