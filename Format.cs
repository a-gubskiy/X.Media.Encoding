using System;

namespace X.Media.Encoding
{
    public class Format
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Extension
        {
            get { return "." + Name.ToLower(); }
        }

        public Format()
            : this(String.Empty, String.Empty)
        {
        }

        public Format(string name)
            : this(name, String.Empty)
        {
        }

        public Format(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof (Format) && Equals((Format) obj);
        }
        
        public bool Equals(Format other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(other.Name, Name) && Equals(other.Description, Description);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (Description != null ? Description.GetHashCode() : 0);
            }
        }
        
        public static readonly Format Webm = new Format("webm");
        public static readonly Format Ogg = new Format("ogg");
        public static readonly Format Mp4 = new Format("mp4");
    }
}
