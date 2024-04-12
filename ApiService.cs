using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json;

namespace CS_First_HTTP_Client;
/// <summary>
/// This is a service (a class that mediates btwn your application and some other application
/// Responsible for handling interactions w/ Winsor Apps api
/// All other services I write that need to interact w/ Winsor Apps will depend on this service
/// </summary>
public class ApiService 
{
    /// This is the part of the object that will handle speaking HTTP
    /// and is dedicated to communicating with this particular Host
    private readonly HttpClient _client = new()
    {
        BaseAddress = new("https://forms-dev.winsor.edu")

    };

    /// After logging in, store your token here
    /// so that it can be used later
    private AuthResponse _auth { get; set; }
    
    /// this part is one way to handle Service initialization
    /// Private Constructor so it (the service)
    /// can be initialized in this file
    private ApiService () {}

    /// Singleton - (only one instance of this service can ever exist)
    /// Singleton API Service declaration.
    /// All Services that depend on this API will reference 'ApiService.Current'
    public static readonly ApiService Current = new();

    /// This method must be called sucessfully
    /// b4 you can call any endpoints that require Authentication
    /// <param name="login">Your username + password</param>
    /// <returns> true, if (and only if) the Authentication was successfull</returns>
    public async Task<bool> AuthenticateAsync(Login login)
    {
        // Start a request
     HttpRequestMessage request = new(HttpMethod.Post, "api/auth");
     
     // Convert the login info to JSON text
     string jsonContent = JsonSerializer.Serialize(login);
     
     // Add the JSON text to the Body of the request. You must 
     // indicate that the Content-Type is application/json
     request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
     
     // the await keyword here - tells that this statement 
     // might take a while to process. It allows the rest of the  
     // program to keep going while this is happening.

     var response = await _client.SendAsync(request);
     
     // Get the text from the response body. There is some assumption here..
     var responseContent = await response.Content.ReadAsStringAsync();

     if (!response.IsSuccessStatusCode)
     {
         // This means that something went wrong (bad status code) Prob a failed login.
         // print the response to tje Debug Output tab. 
         Debug.WriteLine((responseContent));
         //login was not successful 
         return false;
         
     }

     _auth = JsonSerializer.Deserialize<AuthResponse>(responseContent);
     return true; 
    }
     
     /// Generic API call to Winsor Apps API that does not have Body Content
     /// </param name="method">GET, POST, PUT, etc.</param>
     /// </param name="authorize">Does this require authorization?</param>
     /// <typeparam name="TOut">Type of the expected response to this API call</typeparam>
     /// <returns> The content of the API response</returns>
     public async Task<TOut?> SendAsync <TOut>(HttpMethod method, string endpoint, bool authorize = true)
     {
         HttpRequestMessage request = new(method, endpoint);

         if (authorize)
         {
             request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _auth.jwt);

         }

         var response = await _client.SendAsync(request);
         // Get the text from the response body. There is some assumption here
         var responseContent = await response.Content.ReadAsStringAsync();

         if (response.IsSuccessStatusCode)
             return JsonSerializer
                 .Deserialize<TOut>(responseContent);
         // This means something went wrong
         Debug.WriteLine(responseContent);
         // the call failed, so return a non-answer
         return default;
     }

     // Send a request that has no content and does not expect a response
     // </param name="method">GET, POST, PUT, etc</param>
     /// </param name="endpoint">Endpoint and QueryString if applicable</param>
     /// </param name="authorize">Does this require authorization?</param>
     /// <returns> True if and only if the call is successful</returns>
     public async Task<bool> SendAsync(HttpMethod method, string endpoint, bool authorize)
     {
         HttpRequestMessage request = new(method, endpoint);
         if (authorize)
         {
             request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _auth.jwt);

         }

         var response = await _client.SendAsync(request);

         if (response.IsSuccessStatusCode)
             return true;

         // Get the test from the response body. There is some assumption
         var responseContent = await response.Content.ReadAsStringAsync();
         // This means that something went wrong
         Debug.WriteLine(responseContent);
         // the call failed, so return a non-answer
         return false;
     }
         
     // Generic API call to Winsor Apps API that does not have Body Content
     // </param name="method">POST, PUT, etc (Cant have content in a GET request)</param>
     /// </param name="endpoint">Endpoint and QueryString if applicable</param>
     /// </param name="authorize">Does this require authorization?</param>
     /// <typeparam name="TOut">Type of the expected response to this API call</typeparam>
     /// <typeparam name="TIn">Type of the content being sent</typeparam>
     /// <returns> The content of the API response</returns>
     public async Task<TOut?> SendAsync<TOut, TIn>(HttpMethod method, string endpoint, TIn content, bool authorize = true)
     {
         HttpRequestMessage request = new(method, endpoint);

         if (authorize)
         {
             request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _auth.jwt);
         }

         var jsonContent = JsonSerializer.Serialize(content);
         request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

         var response = await _client.SendAsync(request);
         //Get the text from the response body. There is assumption
         var responseContent = await response.Content.ReadAsStringAsync();

         if (response.IsSuccessStatusCode)
             return JsonSerializer
                 .Deserialize<TOut>(responseContent);
         
         // This means somethn went wrong
         Debug.WriteLine(responseContent);
         //the call failed so return a non-answer
         return default;
     }
     // Generic API call to Winsor Apps API that does not have Body Content
     // </param name="method">POST, PUT, etc (Cant have content in a GET request)</param>
     /// </param name="endpoint">Endpoint and QueryString if applicable</param>
     /// </param name="authorize">Does this require authorization?</param>
     /// </param name="content">The content of this API call</param>
     /// <typeparam name="TIn">Type of the content being sent</typeparam>
     /// <returns> True if and only if the request is successful</returns>
     public async Task<bool> SendAsync<TIn>(HttpMethod method, string endpoint, TIn content, bool authorize = true)
     {
         HttpRequestMessage request = new(method, endpoint);

         if (authorize)
         {
             request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _auth.jwt);

         }

         var jsonContent = JsonSerializer.Serialize(content);
         request.Content = new StringContent(jsonContent, Encoding.UTF8, "applcation/json");

         var response = await _client.SendAsync(request);
         // Get the text from the response body. There is assumption
         var responseContent = await response.Content.ReadAsStringAsync();

         if (response.IsSuccessStatusCode)
             return true;

         // This means that somethn went wrong
         Debug.WriteLine(responseContent);
         // The call failed.. return non-answer
         return false;
     }

}