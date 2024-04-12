// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using CS_First_HTTP_Client;

await ApiService.Current.AuthenticateAsync(new("lara.dorosario@winsor.edu", "@*$VKCqrb359"));

var assessments = await ApiService.Current.SendAsync<Assessment[]>(HttpMethod.Get, "api/assessment-calendar/my-calendar");

foreach (var eachAssessment in assessments)
{
    Console.WriteLine($"{eachAssessment.start:dddd dd MMMM} at {eachAssessment.start:hh:mm tt} is {eachAssessment}"); 
}

