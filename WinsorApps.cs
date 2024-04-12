namespace CS_First_HTTP_Client;


    /// <summary>
    /// Object (Login) to be used with Winsor Apps API
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    public readonly record struct Login(string email, string password);
    /// <summary>
    /// Error response from the Winsor Apps API
    /// </summary>
    /// <param name="type"></param>
    /// <param name="error"></param>
    public readonly record struct ErrorResponse(string type, string error);
    
    public readonly record struct AuthResponse(string userId, string jwt, DateTime expires, string refreshToken);

    public readonly record struct UserInfo(string id, string firstName, string lastName, string email, bool hasLogin);

    public readonly record struct Assessment(string id, string type, string summary, string description, DateTime start, DateTime end, bool allDay, List<string> affectedClasses);
