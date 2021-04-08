using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;

namespace Repository.Interfaces
{
    public interface IImage : IGenereicInterface<Image>
    {
        Task<List<Image>> LoadImageByProductID(int productId);
        Task<Image> FindAddedImage(DateTime dateTime);

        Task<Image> GetThumbnailImageByProductId(int productId);

        Task<Image> AddImages(List<Image> imgs);
    }
}
