namespace FHIR.WebSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddOpenApiDocument(); // 註冊服務加入 OpenAPI 文件

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseOpenApi();    // 啟動 OpenAPI 文件
            app.UseSwaggerUi(); // 啟動 Swagger UI

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
