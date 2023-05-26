using Funq;
using ServiceStack;
using MyApp.ServiceInterface;
using ServiceStack.IO;

[assembly: HostingStartup(typeof(MyApp.AppHost))]

namespace MyApp;

public class AppHost : AppHostBase, IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services => {
            // Configure ASP.NET Core IOC Dependencies
        });

    public AppHost() : base("MyApp", typeof(MyServices).Assembly) {}

    public override void Configure(Container container)
    {
        // enable server-side rendering, see: https://sharpscript.net/docs/sharp-pages
        Plugins.Add(new SharpPagesFeature {
            EnableSpaFallback = true
        }); 
        
        Plugins.Add(new RequestLogsFeature {
            RequestLogger = new CsvRequestLogger(
                files: new FileSystemVirtualFiles(HostContext.Config.WebHostPhysicalPath),
                requestLogsPattern: "requestlogs/{year}-{month}/{year}-{month}-{day}.csv",
                errorLogsPattern: "requestlogs/{year}-{month}/{year}-{month}-{day}-errors.csv",
                appendEvery: TimeSpan.FromSeconds(1)
            ),
        });

        SetConfig(new HostConfig {
            AddRedirectParamsToQueryString = true,
        });
    }
}