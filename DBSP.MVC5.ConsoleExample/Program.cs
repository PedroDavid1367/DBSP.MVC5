using Owin;
using System;
using Microsoft.Owin;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace DBSP.MVC5.ConsoleExample
{

  public class Program
  {
    static void Main(string[] args)
    {
      string uri = "http://localhost:7667";

      using (WebApp.Start<Startup>(uri))
      {
        Console.WriteLine("Started!");
        Console.ReadKey();
        Console.WriteLine("Stopping");
      }
    }
  }

  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      app.Use(GreetingMiddleware);
      //app.Run(ctx =>
      //{
      //  return ctx.Response.WriteAsync("Hello.");
      //});
      app.Use((ctx, next) => 
      {
        ctx.Response.WriteAsync("Hello.");
        return Task.Run(next);
      });
    }

    public Task GreetingMiddleware(IOwinContext ctx, Func<Task> next)
    {
      var output = new Dictionary<string, string>();
      foreach (var pair in ctx.Environment)
      {
        output.Add(pair.Key, pair.Value != null ? pair.Value.ToString() : "null");
      }
 
      string json = JsonConvert.SerializeObject(output, Formatting.Indented);
      Task.Run(() => Console.WriteLine(json));
      return Task.Run(next);
    }
  }
}
