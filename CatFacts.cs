namespace CS_First_HTTP_Client;

public record struct FactStatus(bool verified, int sentCount);

public record struct CatFact(FactStatus status, string _id, string user,
    string text, string source, DateTime updatedAt, string type, 
    DateTime createdAt, bool deleted, bool used);