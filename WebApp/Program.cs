using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

            //CreateDefaultBuilderִ����������
            //��ʹ��Ӧ�ó�����й������ṩӦ�ó���Kestrel����������ΪWeb��������
            //�����ݸ�����Ϊ�� Directory.GetCurrentDirectory���ص�·����
            //��ͨ�����¶�������������ã�
            //��ǰ׺ΪASPNETCORE_�Ļ������������磬ASPNETCORE_ENVIRONMENT����
            //�������в�����
            //������˳�����Ӧ�ó������ã�
            //��appsettings.json��
            //��appsettings.{ Environment}.json��
            //��Ӧ����ʹ����ڳ��򼯵�Development����������ʱ�Ļ��ܹ�������
            //�𻷾�������
            //�������в�����
            //�����ÿ���̨�͵����������־��¼����־��¼����appsettings.json��appsettings.{ Environment}.json�ļ�����־��¼���ò�����ָ������־ɸѡ����
            //��ʹ��ASP.NET Coreģ����IIS��������ʱ��CreateDefaultBuilder������IIS���ɣ��������Ӧ�ó���Ļ�ַ�Ͷ˿ڡ�IIS���ɻ�����Ӧ�ó����Բ�����������
            //�����Ӧ�û���Ϊ��������Development�������뽫ServiceProviderOptions.ValidateScopes��Ϊtrue��
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(config);
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseSetting(WebHostDefaults.ApplicationKey, "CoreWeb");
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        options.Limits.MaxRequestBodySize = 20000000;
                    });
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
