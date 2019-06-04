namespace e_commerce.webapi.Models
{
    public interface IEmailBase
    {
        string ToEmail { get; set; }
        string CCEmail { get; set; }
        string Subject { get; set; }
        string Body { get;  }
        string getHtml();
        string getTemplateName();
         
    }
}