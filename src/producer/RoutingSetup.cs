namespace Producer;

/**
 * <summary>
 * Routing as a dotnet web application
 * </summary>
 */
public static class RoutingSetup
{
    /**
     * <summary>
     * set up a path-base (base of path that can be ignored) and that we serve defaults and use routing
     * </summary>
     * <param name="app"></param>
     * <param name="environment"></param>
     */
    public static void SetupRouting(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UsePathBase("/_/producer/");

        app.UseDefaultFiles();
        app.UseStaticFiles();

        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
    }
}