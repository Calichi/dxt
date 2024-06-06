namespace dxt.Service;

public class Names(Database.Context db)
{
    public ulong Add(string pName)
    {
        var name = Get(pName) ?? new() { Name = pName };
        name.Records++;
        db.SaveChanges();
        return name.Records;
    }

    public Model.Person? Get(string name) =>
        db.Persons.FirstOrDefault( p => p.Name == name );
}
