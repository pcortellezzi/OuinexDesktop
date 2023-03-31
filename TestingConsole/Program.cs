// See https://aka.ms/new-console-template for more information
using OuinexDesktop.Exchanges;

Console.WriteLine("Hello, World!");
new Test();

public class Test
{
    public Test()
    {
        Console.WriteLine("nique ta mere");
        var cex =  new POCEx();
        cex.InitAsync();
    }
}