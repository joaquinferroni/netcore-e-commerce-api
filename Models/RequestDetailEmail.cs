using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;


namespace e_commerce.webapi.Models
{
    public class RequestDetailEmail : IEmailBase
    {
        public Order Order { get; set; }
        public string ToEmail { get ; set; }
        public string CCEmail { get ; set; }
        public string Subject { get ; set; }
        public string Body { get =>getHtml() ; }

        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment environment;

        public RequestDetailEmail(IConfiguration conf, IHostingEnvironment environment, Order order){
            this.configuration = conf;
            this.environment = environment;
            this.Order = order;
        }

        public string getHtml()
        {

            string directoryTemplate = configuration.GetValue<string>("DirectoryTemplate");
            bool isProd = configuration.GetValue<bool>("IS_PROD");
            string url_web = configuration.GetValue<string>("URL_DES");
            if(isProd)
            {
                url_web = configuration.GetValue<string>("URL_PROD");
            }
            var emailDirectory =  Path.Combine(environment.ContentRootPath, directoryTemplate);
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>");
            foreach (var item in this.Order.Products)
            {
                sb.AppendLine($"<li>{item.Detail}  X {item.TotalCart} ");
            }
            sb.Append("</ul></br>");
            //sb.Append($"<p><b>Forma de pago: {this.Order.PaymentMethodDescription}</b></p>");
            var body = File.ReadAllText($"{emailDirectory}{getTemplateName()}");
                body = body.Replace("#Items#", sb.ToString());
                body = body.Replace("#URL#",url_web);
            return body;
        }

        public string getTemplateName()
        {
            return "RequestDetail.html";
        }
    }
}