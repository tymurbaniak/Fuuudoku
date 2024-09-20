// See https://aka.ms/new-console-template for more information


Console.WriteLine("Hello, World!");

var path = args.FirstOrDefault();

if (path == null)
{
    Console.WriteLine("Path argument is missing");
    return;
}


