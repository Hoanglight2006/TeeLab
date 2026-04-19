using DinkToPdf;
using DinkToPdf.Contracts;

public interface IPdfService
{
    byte[] CreatePdf(string htmlContent);
}

public class PdfService : IPdfService
{
    private readonly IConverter _converter;
    public PdfService(IConverter converter) { _converter = converter; }

    public byte[] CreatePdf(string htmlContent)
    {
        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
            },
            Objects = {
                new ObjectSettings() {
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
        };
        return _converter.Convert(doc);
    }
}