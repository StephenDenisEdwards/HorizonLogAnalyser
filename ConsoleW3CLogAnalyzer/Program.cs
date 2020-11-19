using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Horizon.Utility.LogAnylyser;

namespace ConsoleW3CLogAnalyzer
{
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    class Program
    {
        static async Task Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine("TestReportSync_Url - cs-uri-stem");
            await TestReportSync_Url();
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
            Console.WriteLine();

            sw.Restart();
            Console.WriteLine("TestReportAsyncLinq_Method - cs-method");
            await TestReportAsyncLinq_Method();
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
            Console.WriteLine();

            sw.Restart();
            Console.WriteLine("TestReportAsync_Method - cs-method");
            await TestReportAsync_Method();
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
            Console.WriteLine();

            sw.Restart();
            Console.WriteLine("TestReportSyncLinq_Method - cs-method");
            await TestReportSyncLinq_Method();
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
            Console.WriteLine();

            sw.Restart();
            Console.WriteLine("TestReportSync_Method - cs-method"); 
            await TestReportSync_Method();
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
            Console.WriteLine();

            sw.Restart();
            Console.WriteLine("TestSyncQuery_Where_POST - Where(a => a['cs - method'] == 'POST')");
            TestSyncQuery_Where_POST();
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
            Console.WriteLine();

            sw.Restart();
            Console.WriteLine("TestSyncQuery_Where_DELETEorPOST() - Where(a => a['cs - method'] == 'DELETE' || a['cs - method'] == 'POST')");
            TestSyncQuery_Where_DELETEorPOST();
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
            Console.WriteLine();
        }

        static List<LogFactory> GetLogFactories()
        {
            List<LogFactory> factories = new List<LogFactory>();
            factories.Add(new W3cLogFileFactory("w3c.test.short.log"));
            factories.Add(new W3cLogFileFactory("w3c.test.log"));
            factories.Add(new CsvLogFileFactory("csv.test.short.log"));

            if (File.Exists("w3c.test.verylong.log"))
            {
                factories.Add(new W3cLogFileFactory("w3c.test.verylong.log"));
            }

            return factories;
        }

        static async Task TestReportAsyncLinq_Method()
        {
            using (LogAnalyserAsync analyser = new LogAnalyserAsync(GetLogFactories()))
            {
                var distinctMethodCounts = await analyser
                    .GroupBy(d => d["cs-method"])
                    .Select(t => new
                    {
                        Category = t.Key,
                        Count = t.CountAsync()
                    }).ToListAsync();

                distinctMethodCounts.ForEach(d => Console.WriteLine($"{d}"));
            }
        }

        static async Task TestReportAsync_Method()
        {
            using (LogAnalyserAsync analyser = new LogAnalyserAsync(GetLogFactories()))
            {
                var report = new Dictionary<string, ReportType>();

                // Using the async iterator.
                await foreach (Dictionary<string, string> a in analyser)
                {
                    string fieldValue = a["cs-method"];

                    ReportType reportType;

                    if (!report.TryGetValue(fieldValue, out reportType))
                    {
                        reportType = new ReportType { Value = fieldValue, Hits = 0, Rank = 0 };
                        report.Add(fieldValue, reportType);
                    }

                    reportType.Hits++;
                }

                foreach (var reportItem in report)
                {
                    Console.WriteLine($"{reportItem.Value}");
                }
            }
        }

        static async Task TestReportSyncLinq_Method()
        {
            using (var analyser = new LogAnalyser(GetLogFactories()))
            {

                var distinctMethodCounts = analyser
                    .GroupBy(d => d["cs-method"])
                    .Select(t => new
                    {
                        Category = t.Key,
                        Count = t.Count(),
                    }).ToList();

                distinctMethodCounts.ForEach(d => Console.WriteLine($"{d}"));
            }
        }

        static async Task TestReportSync_Method()
        {
            using (var analyser = new LogAnalyser(GetLogFactories()))
            {
                var report = new Dictionary<string, ReportType>();

                // Using the async iterator.
                foreach (Dictionary<string, string> a in analyser)
                {
                    string fieldValue = a["cs-method"];

                    ReportType reportType;

                    if (!report.TryGetValue(fieldValue, out reportType))
                    {
                        reportType = new ReportType { Value = fieldValue, Hits = 0, Rank = 0 };
                        report.Add(fieldValue, reportType);
                    }

                    reportType.Hits++;
                }

                foreach (var reportItem in report)
                {
                    Console.WriteLine($"{reportItem.Value}");
                }
            }
        }

        static async Task TestReportSync_Url()
        {
            using (var analyser = new LogAnalyser(GetLogFactories()))
            {
                var report = new Dictionary<string, ReportType>();

                // Using the async iterator.
                foreach (Dictionary<string, string> a in analyser)
                {
                    string fieldValue = a["cs-uri-stem"];

                    ReportType reportType;

                    if (!report.TryGetValue(fieldValue, out reportType))
                    {
                        reportType = new ReportType { Value = fieldValue, Hits = 0, Rank = 0 };
                        report.Add(fieldValue, reportType);
                    }

                    reportType.Hits++;
                }

                foreach (var reportItem in report)
                {
                    Console.WriteLine($"{reportItem.Value}");
                }
            }
        }

        static void TestSyncQuery_Where_POST()
        {
            using (var analyser = new LogAnalyser(GetLogFactories()))
            {
                IEnumerable<Dictionary<string, string>> query = analyser
                    .Where(a => a["cs-method"] == "POST");

                foreach (Dictionary<string, string> item in query)
                {
                    foreach (var key in item.Keys)
                    {
                        Console.Write($"{key}: {item[key]},  ");
                    }

                    Console.WriteLine();
                }
            }
        }

        static void TestSyncQuery_Where_DELETEorPOST()
        {
            using (var analyser = new LogAnalyser(GetLogFactories()))
            {
                IEnumerable<Dictionary<string, string>> query = analyser
                    .Where(a => a["cs-method"] == "DELETE" || a["cs-method"] == "POST");

                foreach (Dictionary<string, string> item in query)
                {
                    foreach (var key in item.Keys)
                    {
                        Console.Write($"{key}: {item[key]},  ");
                    }

                    Console.WriteLine();
                }
            }
        }
        static async Task TestReportAsync_1()
        {
            var analyser = new LogAnalyserAsync();

            using (ILogStreamReader logReader_1 = new TextLogStreamReader(new StreamReader("w3c.test.log")),
                logReader_2 = new TextLogStreamReader(new StreamReader("w3c.test.short.log")))
            {
                ILogFieldList fieldList_1 = new W3cLogFieldListReader(logReader_1);
                analyser.Add(fieldList_1, logReader_1);

                ILogFieldList fieldList_2 = new W3cLogFieldListReader(logReader_2);
                analyser.Add(fieldList_2, logReader_2);

                var report = new Dictionary<string, ReportType>();

                // Using the async iterator.
                await foreach (var a in analyser)
                {
                    string fieldValue = a["cs-method"];

                    ReportType reportType;

                    if (!report.TryGetValue(fieldValue, out reportType))
                    {
                        reportType = new ReportType { Value = fieldValue, Hits = 0, Rank = 0 };
                        report.Add(fieldValue, reportType);
                    }

                    reportType.Hits++;
                }

                foreach (var reportItem in report)
                {
                    Console.WriteLine($"{reportItem.Key}: {reportItem.Value},  ");
                }
            }
        }

        static async Task TestReportSync_1()
        {
            var fileStream1 = new FileStream("w3c.test.log", FileMode.Open);
            var fileStream2 = new FileStream("w3c.test.short.log", FileMode.Open);

            var analyser = new LogAnalyser();

            using (ILogStreamReader logReader1 = new TextLogStreamReader(new StreamReader(fileStream1)))
            {
                ILogFieldList fieldList1 = new W3cLogFieldListReader(logReader1);
                analyser.Add(fieldList1, logReader1);

                using (ILogStreamReader logReader2 = new TextLogStreamReader(new StreamReader(fileStream2)))
                {
                    ILogFieldList fieldList2 = new W3cLogFieldListReader(logReader2);
                    analyser.Add(fieldList2, logReader2);

                    var report = new Dictionary<string, ReportType>();

                    analyser.ToList().ForEach(a =>
                    {
                        string fieldValue = a["cs-method"];

                        ReportType reportType;

                        if (!report.TryGetValue(fieldValue, out reportType))
                        {
                            reportType = new ReportType { Value = fieldValue, Hits = 0, Rank = 0 };
                            report.Add(fieldValue, reportType);
                        }

                        reportType.Hits++;
                    });

                    foreach (var reportItem in report)
                    {
                        Console.WriteLine($"{reportItem.Key}: {reportItem.Value},  ");
                    }
                }
            }
        }
    }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
}
