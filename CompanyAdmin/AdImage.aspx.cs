using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AlarmasABC.BLL.ProcessCompany;
using AlarmasABC.Core.Admin;
using System.IO;
using System.Text;
using Telerik.Web.UI;
using System.Drawing;

public partial class CompanyAdmin_AdImage : System.Web.UI.Page
{
    bool _isDelete = false;
    bool _isEdit = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            try
            {
                DataSet _ds = new DataSet();
                _ds = FormPermission.LoadPermission();
                if (_ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _ds.Tables[0].Rows)
                    {
                        if (row["FormName"].ToString() == "Ad Image")
                        {
                            if (!((bool)row["delete"]))
                            {
                                _isDelete = true;
                            }
                            if (!((bool)row["insert"]))
                            {
                                _btnUpload.Enabled = false;

                            }

                            if (!((bool)row["Edit"]))
                            {
                                _isEdit = true;

                            }



                        }
                    }
                    
                }
            }
            catch(Exception ex)
            {
                
            }
           

            
        }
    }

    private string _iName;

    public string IName
    {
        get { return _iName; }
        set { _iName = value; }
    }

    private void loadImageGrid()
    {

        ProcessImageUrl _imageInfo = new ProcessImageUrl();
        try
        {
            int _comID = int.Parse(Session["trkCompany"].ToString());
            _imageInfo.GetImageInfo(_comID);
            _rgrdAdImage.DataSource = _imageInfo.Ds;
            //_rgrdAdImage.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            _rgrdAdImage = null;
        }
    }

    private bool isValidData()
    {
        //if (Session["trkCompany"]==null)
        //{
        //    _lblMessage.Text = "Please Select Company.";
        //    _lblMessage.ForeColor = System.Drawing.Color.Red;
        //    return false;
        //}
        if (FileInput.PostedFile.FileName == "")
        {
            _lblMessage.Text = "Please Select Image.";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }



        return true;
    }

    protected void _btnUpload_Click(object sender, EventArgs e)
    {
        string sSavePath = "";
        string sFilename = "";
        try
        {
        ProcessImageUrl pIu = new ProcessImageUrl();
        ImageURL imgU = new ImageURL();

        
        int intThumbWidth = 200;
        int intThumbHeight = 50;

        // Set constant values

        sSavePath = "..\\AdvertisingImages\\";


       
            // If file field isn’t empty

            if (FileInput.PostedFile != null)
            {
                // Check file size (mustn’t be 0)

                HttpPostedFile myFile = FileInput.PostedFile;

                int nFileLen = myFile.ContentLength; 
                if (nFileLen == 0)
                {
                    _lblMessage.Text = "No file was uploaded.";
                    return;
                }

                // Check file extension (must be JPG)

                if (System.IO.Path.GetExtension(myFile.FileName).ToLower() != ".jpg")
                {
                    _lblMessage.Text = "The file must have an extension of JPG";
                    return;
                }



                // Make sure a duplicate file doesn’t exist. 

                sFilename = Path.GetFileName(myFile.FileName);

                if ((File.Exists(Server.MapPath(sSavePath + sFilename))))
                {
                    _lblMessage.Text = "File with same name already exist";
                    return;
                }

                //System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile();

                //myFile.SaveAs(sSavepath+Path.GetFileName(myFile.FileName));AdvertisingImages
                //string fileName = "";
                //string fullName = myFile.FileName.ToString();
                
                //if(fullName.LastIndexOf('/') > 0 )
                //{
                //    fileName = fullName.Substring(fullName.LastIndexOf('/'),fullName.Length);
                //}
                string filerPath = Server.MapPath("AdvertisingImages");
                string filePath = filerPath.Replace("\\CompanyAdmin", "");
                 myFile.SaveAs(filePath + "\\a.jpg");
                 string fullpath = filePath + "\\" + Path.GetFileName(myFile.FileName);
                 ResizeImage(filePath + "\\a.jpg", fullpath, 200, 60, true);
                 File.Delete(filePath + "\\a.jpg");
                
                //lblGridMessage.ForeColor = System.Drawing.Color.Green;
                //lblGridMessage.Text = "Icon Created Successfully...";
                // Resizing Image has comments on 24.12.2008 due to browser bug.
                
                //bool flag = ResizeImage(Path.GetFullPath(myFile.FileName), Server.MapPath(sSavePath + sFilename), intThumbWidth, intThumbHeight, true);
                //if (!flag)
                //{
                //    _lblMessage.Text = "File upload fail.";
                //    return;
                //}




                // Entry to Database

                int isActive = 0;
                if (_chkIsActive.Checked)
                {
                    isActive = 1;
                }
                else
                {
                    isActive = 0;
                }
                imgU.ImageName = sFilename;
                imgU.ImageUrl = _txtURL.Text.ToString();

                int _cID = int.Parse(Session["trkCompany"].ToString());
                imgU.ComID = _cID;
                imgU.IsActive = isActive;

                pIu.IUrl = imgU;

                pIu.AddImageUrl();


                // Displaying success information

                _lblMessage.Text = "File uploaded successfully!";
                _txtURL.Text = "";
                //loadImageGrid(_cID);
                _rgrdAdImage.Rebind();
               
               // _rgrdAdImage.Rebind();
                // Destroy objects


            }
        }
        catch (Exception errArgument)
        {


            //_lblMessage.Text = "File upload fail. "+ errArgument.Message.ToString();
           // File.Delete(Server.MapPath(sSavePath + sFilename));
        }


    }

    public bool ResizeImage(string OriginalFile, string NewFile, int NewWidth, int MaxHeight, bool OnlyResizeIfWider)
    {
        try
        {
            System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(OriginalFile);

            // Prevent using images internal thumbnail
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            if (OnlyResizeIfWider)
            {
                if (FullsizeImage.Width <= NewWidth)
                {
                    NewWidth = FullsizeImage.Width;
                }
            }

            int NewHeight = FullsizeImage.Height * NewWidth / FullsizeImage.Width;
            if (NewHeight > MaxHeight)
            {
                // Resize with height instead
                NewWidth = FullsizeImage.Width * MaxHeight / FullsizeImage.Height;
                NewHeight = MaxHeight;


            }

            System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(NewWidth, NewHeight, null, IntPtr.Zero);

            // Clear handle to original file so that we can overwrite it if necessary
            FullsizeImage.Dispose();

            // Save resized picture
            NewImage.Save(NewFile);
            return true;
        }

        catch (Exception errArgument)
        {


            _lblMessage.Text = "File upload fail."+ errArgument.Message.ToString();
            return false;
            
        }

    }




    private void DeleteFile(string _imageName)
    {
        try
        {
            FileInfo TheFile = new FileInfo(MapPath("..") + "\\AdvertisingImages\\" + _imageName);
            if (TheFile.Exists)
            {
                File.Delete(MapPath("..") + "\\AdvertisingImages\\" + _imageName);
            }
            else
            {
                throw new FileNotFoundException();
            }

           
        }

        catch (FileNotFoundException ex)
        {
            _lblMessage.Text += ex.Message;
        }
       
    }

    protected void _rgrdAdImage_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridEditableItem editedItem = e.Item as GridEditableItem;
        string _imageID = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString();
        Label name = (Label)e.Item.FindControl("_lblImageName");
        string _imageName = name.Text.ToString();


        ProcessImageUrl _imageInfo = new ProcessImageUrl();
        ImageURL _iObj = new ImageURL();
        try
        {
            _iObj.Id = int.Parse(_imageID.ToString());


            _imageInfo.IUrl = _iObj;
            _imageInfo.DeleteImageInfo();

            DeleteFile(_imageName);
            _lblMessage.Text = "Image Deleted successfully.";

        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            _imageInfo = null;
        }
    }
    protected void _rgrdAdImage_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            loadImageGrid();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
    protected void _rgrdAdImage_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridCommandItem )
        {
           
                GridCommandItem commandItem = (GridCommandItem)e.Item;
                LinkButton addNewRecord = (LinkButton)(commandItem.Controls[0].Controls[0].Controls[0].Controls[0].Controls[0]);
                addNewRecord.Visible = false;
            
        }

        //foreach (GridColumn col in _rgrdAdImage.MasterTableView.RenderColumns)
        //{
        //    if (_isEdit)
        //    {
        //        if (col.ColumnType == "GridEditCommandColumn")
        //        {
        //            col.Display = !col.Display;
        //        }
        //    }

        //    if (_isDelete)
        //    {

        //        if (col.ColumnType == "GridButtonColumn")
        //        {
        //            col.Display = !col.Display;
        //        }
        //    }
        //}

    }
    protected void _rgrdAdImage_Edit(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridEditableItem editedItem = e.Item as GridEditableItem;
        string ImageID = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString();

        try
        {
            _rgrdAdImage.MasterTableView.EditMode = GridEditMode.InPlace;
        }
        catch (Exception exp)
        {
            exp.Message.ToString();
        }
        finally
        {

        }
    }


    protected void _rgrdAdImage_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridEditableItem editedItem = e.Item as GridEditableItem;
        string ImageID = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString();
        string ImageURL = (editedItem["ImageURL"].Controls[0] as TextBox).Text;
        //string _imageName = (editedItem["ImageName"].Controls[0] as TextBox).Text;
        int IsActive = 0;
        bool Status = (editedItem["IsActive"].Controls[0] as CheckBox).Checked;

        if (Status)
        {
            IsActive = 1;
        }


        ProcessImageUrl _imageInfo = new ProcessImageUrl();
        ImageURL _iObj = new ImageURL();
        try
        {
            _iObj.Id = int.Parse(ImageID.ToString());
            _iObj.ImageUrl = ImageURL;
            _iObj.ComID = int.Parse(Session["trkCompany"].ToString());
            _iObj.IsActive = IsActive;

            _imageInfo.IUrl = _iObj;
            _imageInfo.UpdateImageInfo();
            _rgrdAdImage.Rebind();
            _lblMessage.Text = "Image Updated successfully.";
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            _imageInfo = null;
        }



    }
}
