using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_123123 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.Text = "请输入文件路径并点击读取内容。";
        }
    }

    protected void btnReadFile_Click(object sender, EventArgs e)
    {
        string filePath = txtFilePath.Text.Trim();

        if (string.IsNullOrEmpty(filePath))
        {
            lblMessage.Text = "文件路径不能为空！";
            return;
        }

        if (!File.Exists(filePath))
        {
            lblMessage.Text = "文件不存在，请检查路径是否正确。";
            return;
        }

        try
        {
            List<string> fileContent = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    fileContent.Add(sr.ReadLine());
                }
            }

            // 绑定内容到 Repeater
            RepeaterFileContent.DataSource = fileContent;
            RepeaterFileContent.DataBind();

            lblMessage.Text = "文件读取成功！";
        }
        catch (Exception ex)
        {
            lblMessage.Text = $"读取文件失败：{ex.Message}";
        }
    }

}