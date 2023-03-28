using System.ComponentModel.DataAnnotations;

namespace QrcodeProject.Models
{
	public class QRCodeModel
	{
        [Display(Name = "Enter QRCode Text")]
        public string QRCodeText { get; set; }
	}
}