namespace FHIR.WebSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services
                .AddControllers(o =>
                {
                    o.OutputFormatters.Add(new Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerOutputFormatter()); // 自動使用默認的 XML 序列化設置
                    o.RespectBrowserAcceptHeader = true; // 啟用對 Accept header 的支持
                })
                .AddXmlSerializerFormatters();  // 加入 XML 格式的支援(輸出 & 輸入都行)

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // 啟用 Swagger 中介軟體
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                c.RoutePrefix = string.Empty; // 將 Swagger UI 設為應用根目錄
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
