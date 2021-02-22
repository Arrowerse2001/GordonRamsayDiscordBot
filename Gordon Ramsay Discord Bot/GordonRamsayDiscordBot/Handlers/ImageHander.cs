using System.Collections.Generic;

namespace GordonRamsayDiscordBot.Handlers
{
    static class ImageHander
    {
        // Strings for the files
        static readonly string MasterImage = @"imageMaster.txt";
        static readonly string imageProgress = @"imageProgress.txt";
        static public List<string> ImageMasterList = new List<string>();
        static public List<string> images = new List<string>();

        static ImageHander()
        {
            ReloadImages();
            ReloadImagesMasterList();
        }

        // Reload images so no need to restart
        static public void ReloadImages()
        {
            images.Clear();
            images = new List<string>(System.IO.File.ReadAllLines(imageProgress));
            // No empty spaces
            for (int i = 0; i < images.Count; i++)
                if (images[i].Length < 1) images.RemoveAt(i);
        }

        // Reload master image list
        static public void ReloadImagesMasterList()
        {
            ImageMasterList.Clear();
            ImageMasterList = new List<string>(System.IO.File.ReadAllLines(MasterImage));
            // No blanks
            for (int i = 0; i < ImageMasterList.Count; i++)
                if (ImageMasterList[i].Length < 1) ImageMasterList.RemoveAt(i);
        }

        // Rewrite Images
        static public void RewriteImages()
        {
            System.IO.File.WriteAllLines(imageProgress, images.ToArray());
            ReloadImages();
        }

        // Rewrite master
        // Reload Master List
        static public void RewriteMasterListImages()
        {
            System.IO.File.WriteAllLines(MasterImage, ImageMasterList.ToArray());
            ReloadImagesMasterList();
        }

        // Add from master to progress files
        static public void AddMasterImagesToProgressList()
        {
            images.Clear();
            images = new List<string>(System.IO.File.ReadAllLines(MasterImage));
            System.IO.File.WriteAllLines(imageProgress, images.ToArray());

            for (int i = 0; i < images.Count; i++)
                if (images[i].Length < 1) images.RemoveAt(i);
        }

        // Get Image
        static public string GetGordonImage(int index)
        {
            string i = images[index];
            images.RemoveAt(index);
            if (images.Count == 0)
                AddMasterImagesToProgressList();
            else
                RewriteImages();
            return i;
        }
    }
}
