//Ty Larson
using System;
using System.Configuration;
using System.Windows.Documents;

/// <summary>
/// Ty Larson
/// </summary>
namespace TheWritersCompanion
{
    /// <summary>
    /// Defines a Note Object
    /// </summary>
    public class Note : IComparable<Note>
    {
        private string name;
        private Category category;
        private string content;
        private string filePath;

        /// <summary>
        /// Constructor for a blank Note Object
        /// </summary>
        public Note()
        {
            name = "";
            category = new Category();
            filePath = "";
        }

        /// <summary>
        /// Constructor for a Note object if using Database as storage
        /// </summary>
        /// <param name="inName"></param>
        /// <param name="inCategory"></param>
        public Note(string inName, Category inCategory)
        {
            name = inName;
            category = inCategory;
        }
        
        /// <summary>
        /// Constructor for a Note Object if using File Directory as storage
        /// </summary>
        /// <param name="inName">The name of the Note</param>
        /// <param name="inCategory">The category that the Note resides in</param>
        /// <param name="inContent">The content of the Note</param>
        /// <param name="inFilePath">The File Path to the Note's location</param>
        public Note(string inName, Category inCategory, string inFilePath)
        {
            name = inName;
            category = inCategory;
            filePath = inFilePath + "\\" + inName + ".txt";
        }

        /// <summary>
        /// Constructor for a Note Object if making a full note
        /// </summary>
        /// <param name="inName"></param>
        /// <param name="inCategory"></param>
        /// <param name="inContent"></param>
        /// <param name="inFilePath"></param>
        public Note(string inName, Category inCategory, string inContent, string inFilePath)
        {
            name = inName;
            category = inCategory;
            content = inContent;
            filePath = inFilePath + "\\" + inName + ".txt";
        }

        /// <summary>
        /// Getter and Setter for name
        /// </summary>
        public string Name
        {
            get 
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Getter and Setter for category
        /// </summary>
        public Category Category
        {
            get
            {
                return category;
            }
            set
            {
                category = value;
            }
        }

        /// <summary>
        /// Getter and Setter for content
        /// </summary>
        public String Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
            }
        }

        /// <summary>
        /// Getter and Setter for filePath
        /// </summary>
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
            }
        }

        /// <summary>
        /// Compares this Note object to other using their Name properties
        /// </summary>
        /// <param name="other">The other Note object to compare to</param>
        /// <returns>Positive if this should be placed after other,
        /// negative if this should be place before other,
        /// otherwise, 0 as an int</returns>
        public int CompareTo(Note other)
        {
            int result = 0;

            Name.CompareTo(other.Name);

            return result;
        }

        /// <summary>
        /// Equals for Note Class
        /// </summary>
        /// <param name="obj">The object being compared</param>
        /// <returns>True if the Note objects are equal</returns>
        public override bool Equals(object obj)
        {
            bool isEqual = false;
            if(obj is Note)
            {
                isEqual = Name.Equals(((Note)obj).Name) && 
                          Category.Equals(((Note)obj).Category) && 
                          Content.Equals(((Note)obj).Content) &&
                          FilePath.Equals(((Note)obj).FilePath);
            }
            return isEqual;
        }

        /// <summary>
        /// Gets this object's hash code
        /// </summary>
        /// <returns>This object's hash code as an int</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
