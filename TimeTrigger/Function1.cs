using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System.Xml.Linq;

namespace TimeTrigger
{
    public static class Function1
    {
        [FunctionName("FunctionTimeTrigger")]
        public static void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, TraceWriter log)
        {
            var gosocketXML = XDocument.Load(@"Resources/File.xml");            

            var areas = gosocketXML.Descendants("area");
            var areasFilter = gosocketXML.Descendants("area")
                .Where(x => x.Element("employees")
                             .Elements("employee").Count() > 2);

            log.Info("--------------------");
            log.Info("Nodos de tipo “<area>”: " + areas.Count());
            log.Info("--------------------");
            log.Info("Nodos de tipo “<area>” que tienen más de 2 empleados: " + areasFilter.Count());

            foreach (var area in areasFilter)
                log.Info(string.Format("“<area>”: {0}", area.Element("name").Value));

            log.Info("--------------------");
            foreach (var area in areas)
            {
                var totalSalaryByArea = area.Descendants("employees")
                    .Descendants("employee")
                    .Sum(x => double.Parse(x.Attribute("salary").Value) / 100);

                log.Info(string.Format("“<area>” {0}|{1}", area.Element("name").Value, totalSalaryByArea));
            }
            log.Info("--------------------");
        }
    }
}
