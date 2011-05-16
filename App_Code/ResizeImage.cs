using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

/// <summary>
/// Summary description for ResizeImage
/// </summary>
public class ResizeImage
{
    public ResizeImage()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public void ResizeFromStream(string ImageSavePath, int MaxSideSize, Stream Buffer)
    {
        int intNewWidth;
        int intNewHeight;
        Image imgInput = Image.FromStream(Buffer);

        //Determine image format
        ImageFormat fmtImageFormat = imgInput.RawFormat;

        //get image original width and height
        int intOldWidth = imgInput.Width;
        int intOldHeight = imgInput.Height;

        //determine if landscape or portrait
        int intMaxSide;

        if (intOldWidth >= intOldHeight)
        {
            intMaxSide = intOldWidth;
        }
        else
        {
            intMaxSide = intOldHeight;
        }


        if (intMaxSide > MaxSideSize)
        {
            //set new width and height
            double dblCoef = MaxSideSize / (double)intMaxSide;
            intNewWidth = Convert.ToInt32(dblCoef * intOldWidth);
            intNewHeight = Convert.ToInt32(dblCoef * intOldHeight);
        }
        else
        {
            intNewWidth = intOldWidth;
            intNewHeight = intOldHeight;
        }
        //create new bitmap
        Bitmap bmpResized = new Bitmap(imgInput, intNewWidth, intNewHeight);

        //save bitmap to disk
        bmpResized.Save(ImageSavePath, fmtImageFormat);  
        

        //release used resources
        imgInput.Dispose();
        bmpResized.Dispose();
        Buffer.Close();
    } 

}
