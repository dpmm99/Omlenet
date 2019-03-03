using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omlenet
{
    partial class AboutBox1 : Form
    {
        public AboutBox1()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription
                + Environment.NewLine + @"This application uses components and/or data provided by the following.
DockPanelSuite by Weifen Luo and others
- http://dockpanelsuite.com
Food and nutrient contents database from USDA (SR28)
- https://ndb.nal.usda.gov/ndb/
Nutrition goals
- http://nationalacademies.org/hmd/~/media/Files/Activity%20Files/Nutrition/DRI-Tables/5Summary%20TableTables%2014.pdf?la=en
- https://efsa.onlinelibrary.wiley.com/doi/pdf/10.2903/j.efsa.2012.2815
- https://en.wikipedia.org/wiki/Recommended_maximum_intake_of_alcoholic_beverages
- https://health.gov/dietaryguidelines/2015-scientific-report/PDFs/Scientific-Report-of-the-2015-Dietary-Guidelines-Advisory-Committee.pdf
- https://www.ncbi.nlm.nih.gov/pmc/articles/PMC3672386/
- https://www.ncbi.nlm.nih.gov/pubmed/8815648
- https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2677959/
- https://www.ncbi.nlm.nih.gov/pmc/articles/PMC4698241/
- https://www.ncbi.nlm.nih.gov/books/NBK234922/table/ttt00008/?report=objectonly
- https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2793103/
- https://www.ncbi.nlm.nih.gov/pubmed/12442909
- https://draxe.com/what-is-betaine/
- https://www.healthline.com/nutrition/dietary-cholesterol-does-not-matter

If a URL no longer works, please refer to the archived copy at https://archive.org/web/ dated in or before February 2019.";
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
