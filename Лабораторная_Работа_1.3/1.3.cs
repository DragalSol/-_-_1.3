using System.Globalization;
using System.Xml.Linq;

/// <summary>
/// Класс, представляющий событие в новом журнале
/// </summary>
public class Event
{
    /// <summary>
    /// Дата новых сегодняшних событий
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Результат новых сегодняшних событий (например, "success")
    /// </summary>
    public string Result { get; set; }

    /// <summary>
    /// IP-адрес отправителя новых событий
    /// </summary>
    public string IpFrom { get; set; }

    /// <summary>
    /// HTTP-метод запроса
    /// </summary>
    public string Method { get; set; }

    /// <summary>
    /// URL, к которому обращен запрос
    /// </summary>
    public string UrlTo { get; set; }

    /// <summary>
    /// Код ответа сервера на запрос
    /// </summary>
    public int Response { get; set; }
}

/// <summary>
/// Класс, представляющий журнал новых сегодняшних событий
/// </summary>
public class Log
{
    /// <summary>
    /// Список новых событий в журнале
    /// </summary>
    public List<Event> Events { get; set; }

    /// <summary>
    /// Конструктор класса Log. Инициализирует список новых событий
    /// </summary>
    public Log()
    {
        Events = new List<Event>();
    }
}

/// <summary>
/// Статический класс для преобразования XML в объект класса Log
/// </summary>
public static class XmlParser
{
    /// <summary>
    /// Метод для парсинга XML-строки и создания объекта Log
    /// </summary>
    /// <param name="xmlString">XML-строка</param>
    /// <returns>Объект класса Log</returns>
    public static Log ParseXml(string xmlString)
    {
        Log log = new Log();

        try
        {
            XDocument xmlDoc = XDocument.Parse(xmlString);

            foreach (var eventElement in xmlDoc.Descendants("event"))
            {
                Event logEvent = new Event
                {
                    Date = DateTime.ParseExact(eventElement.Attribute("date").Value, "dd/MMM/yyyy:HH:mm:ss", CultureInfo.InvariantCulture),
                    Result = eventElement.Attribute("result").Value,
                    IpFrom = eventElement.Element("ip-from").Value,
                    Method = eventElement.Element("method").Value,
                    UrlTo = eventElement.Element("url-to").Value,
                    Response = int.Parse(eventElement.Element("response").Value)
                };

                log.Events.Add(logEvent);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing XML: {ex.Message}");
        }

        return log;
    }
}

/// <summary>
/// Класс для выполнения лабораторной работы 1.3
/// </summary>
class Lab1_3
{
    /// <summary>
    /// Точка входа в программу
    /// </summary>
    static void Main()
    {
        string xmlData = @"<log>
                            <event date=""27/May/1999:02:32:46"" result=""success"">
                                <ip-from>195.151.62.18</ip-from>
                                <method>GET</method>
                                <url-to>/mise/</url-to>
                                <response>200</response>
                            </event>
                            <event date=""27/May/1999:02:41:47"" result=""success"">
                                <ip-from>195.209.248.12</ip-from>
                                <method>GET</method>
                                <url-to>soft.htm</url-to>
                                <response>200</response>
                            </event>
                          </log>";

        Log log = XmlParser.ParseXml(xmlData);

        /// Вывод результатов ///
        Console.WriteLine("Events in Log:");
        foreach (var logEvent in log.Events)
        {
            Console.WriteLine($"Date: {logEvent.Date}, Result: {logEvent.Result}, IP: {logEvent.IpFrom}, Method: {logEvent.Method}, URL: {logEvent.UrlTo}, Response: {logEvent.Response}");
        }
    }
}