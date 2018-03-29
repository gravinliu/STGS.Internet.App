using System;
using System.Reflection;

namespace STGS.Internet.Tool
{
    public class EnumTextMetaAttribute : Attribute
    {

        public EnumTextMetaAttribute(string text = "", string id = null)
        {
            this.Text = text;
            this.Id = id;
        }

        public string Id { get; set; }
        public string Text { get; set; }
    }
}
