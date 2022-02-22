using JoinLeftExample.Models;
using Newtonsoft.Json;


var authors = LoadData<Author>(@"Data\Authors.json");
var books = LoadData<Book>(@"Data\Books.json");

var joinedData = authors
    .Join(books, a => a.Id, b => b.AuthorId, (a, b) => new { a.FirstName, a.LastName, b.Title, b.Year });
var joinedData2 = authors
    .JoinLeft(books, a => a.Id, b => b.AuthorId, (a, b) => new { a.FirstName, a.LastName, b.Title, b.Year });

Console.WriteLine("Join Example");
joinedData
    .ToList()
    .ForEach(x => Console.WriteLine($"{x.FirstName} {x.LastName} wrote {x.Title} in {x.Year}."));
Console.WriteLine("Join Left Example");
joinedData2
    .ToList()
    .ForEach(x => Console.WriteLine($"{x.FirstName} {x.LastName} wrote {x.Title} in {x.Year}."));

List<T> LoadData<T>(string path) => JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));
public static class Example
{
    public static IEnumerable<T> JoinLeft<S, A, T>(
        this IEnumerable<S> source,
        IEnumerable<A> joined,
        Func<S, int> p,
        Func<A, int> a,
        Func<S, A, T> r)
        where A : new()
    {
        var list = new List<T>();
        foreach (var item in source)
        {
            var result = joined.Where(x => a(x) == p(item));
            if (result.Any())
                foreach (var j in result)
                    list.Add(r(item, j));
            else
                list.Add(r(item, new A()));
        }
        return list;
    }
}