using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SaaS.Domain.PIPL;

namespace SaaS.DataAccess.Services
{
    public class PictureService
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public PictureService(IWebHostEnvironment webHostEnvironment)
        {
            this.hostingEnvironment = webHostEnvironment;
        }

        public bool AddPictureToSaaS(IFormFile picture, Company company)
        {
            string wwwRootPath = this.hostingEnvironment.WebRootPath;
            if (picture != null)
            {
                try
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);
                    string companyPath = @"images\companies\company-" + company.Id;
                    string finalPath = Path.Combine(wwwRootPath, companyPath);

                    if (!Directory.Exists(finalPath))
                        Directory.CreateDirectory(finalPath);
                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        picture.CopyTo(fileStream);
                    }

                    /*CompanyPicture companyPicture = new CompanyPicture()
                    {
                        ImageUrl = @"\" + companyPath + @"\" + fileName,
                        CompanyId = company.Id,
                    };

                    if (company.Picture == null)
                        company.Picture = new CompanyPicture();

                    company.Picture = companyPicture;*/
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
