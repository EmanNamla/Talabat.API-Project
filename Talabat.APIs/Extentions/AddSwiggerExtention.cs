namespace Talabat.APIs.Extentions
{
    public static class AddSwiggerExtention
    {
        public static WebApplication UseSwaggerExtention(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }

    }
}
