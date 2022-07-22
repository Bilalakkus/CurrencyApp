using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurrencyApp
{
    public class CurrencyService
    {
        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            string strDoc = await GetFromWebAsync();
            if (strDoc == null)
                return null;
            List<Currency> currencies = new List<Currency>();

            var doc=XDocument.Parse(strDoc);

            foreach (XElement element in doc.Root.Elements())
            {
                Currency c = new Currency
                {
                    Name = element.Element("CurrencyName").Value,
                    Code = element.Attribute("CurrencyCode").Value,
                    Buying = element.Element("ForexBuying").Value,
                    Selling = element.Element("ForexSelling").Value
                };
                c.FlagURL = "https://www.tcmb.gov.tr/kurlar/kurlar_tr_dosyalar/images/" + c.Code + ".gif";
                currencies.Add(c);
            }
            return currencies;



        }
        private async Task<string> GetFromWebAsync()
        {
            const string path = "https://www.tcmb.gov.tr/kurlar/today.xml";

            HttpClient client = new HttpClient();
            var responseMessage= await client.GetAsync(path);
            if (responseMessage.IsSuccessStatusCode)
            {
                var stringDoc = await responseMessage.Content.ReadAsStringAsync();
                return stringDoc;
            }
            return null;

        }
    }
}
