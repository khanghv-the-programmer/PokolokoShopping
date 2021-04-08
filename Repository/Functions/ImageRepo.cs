using Repository.DataContext;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository.Functions
{
    public class ImageRepo : GenericRepo<Image>, IImage
    {
        public ImageRepo(DatabaseContext context) : base(context)
        {
        }
        public ImageRepo() : base()
        {

        }

        public override Task<List<Image>> GetAll()
        {
            return base.GetAll();
        }

        public async override Task<Image> GetBy(object ID)
        {
            //thumbnail
            return await table.Where(img => img.Idimage == (int)ID ).FirstOrDefaultAsync();
        }

        public async override Task Update(Image obj)
        {
            Image thumb = await table.Where(i => i.Idimage.Equals(obj.Idimage)).FirstOrDefaultAsync();
            thumb.Image1 = obj.Image1;
            thumb.ModifiedDate = DateTime.Now;
            thumb.Name = obj.Name;
            table.Update(thumb);
            await Save();
        }

        public async Task<List<Image>> LoadImageByProductID(int productID)
        {
            return await table.Where(img => img.ProductId == productID).ToListAsync();
        }

        public async Task<Image> FindAddedImage(DateTime dateTime)
        {
            return await table.Where(img => img.CreatedDate.Equals(dateTime)).FirstOrDefaultAsync();
        }

        public async Task<Image> GetThumbnailImageByProductId(int productId)
        {
            return await table.Where(img => img.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task<Image> AddImages(List<Image> imgs)
        {
            foreach (Image image in imgs)
            {
                table.Add(image);
                await Save();
            }
            Image thumb = await GetBy(imgs.ElementAt(0).ParentKey);
            return thumb;


        }
    }
}
