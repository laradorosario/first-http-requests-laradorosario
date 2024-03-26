namespace CS_First_HTTP_Client;

public readonly record struct FactStatus(bool verified, int sentCount);

public readonly record struct CatFact(FactStatus status, string _id, string user,
    string text, string source, DateTime updatedAt, string type, 
    DateTime createdAt, bool deleted, bool used);
    
public readonly record struct CatFactShort(string text, string source, string type, DateTime createdAt);