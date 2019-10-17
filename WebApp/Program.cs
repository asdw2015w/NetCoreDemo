using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("hosting.json", optional: true, reloadOnChange: true)
                    .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(config);
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel();
                    webBuilder.UseSerilog((context, configuration) =>
                    {
                        configuration
                            .MinimumLevel.Information()
                            // ��־�����������ռ������ Microsoft ��ͷ��������־�����С����Ϊ Information
                            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                            .Enrich.FromLogContext()
                            // ������־���������̨
                            //.WriteTo.Console()
                            // ������־������ļ����ļ��������ǰ��Ŀ�� logs Ŀ¼��
                            // �ռǵ���������Ϊÿ��
                            .WriteTo.File(Path.Combine("logs", @"log.txt"), rollingInterval: RollingInterval.Day);
                        // ���� logger
                        //.CreateLogger();
                    });
                });
        }
    }
}
