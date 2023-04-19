using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWritersCompanion
{
    /// <summary>
    /// Class that hold the fields necessary to represent a Category for a
    /// TheWritersCompanion project
    /// Author: Tim Neighbor
    /// </summary>
    public class Category : IComparable<Category>
    {
        #region Fields

        private string name;

        private string folderPath;

        public List<Note> notesInCategory;

        #endregion

        #region Constructors

        public Category()
        {
            name = "";
            folderPath = "";
            notesInCategory = new List<Note>();
        }

        public Category(string inName)
        {
            name = inName;
            folderPath = "";
            notesInCategory = new List<Note>();
        }

        public Category(string inName, string inCurrentDirectory)
        {
            name = inName;
            folderPath = inCurrentDirectory + "\\" + inName;
            notesInCategory = new List<Note>();
        }

        #endregion

        #region Properties

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

        public string FolderPath
        {
            get
            {
                return folderPath;
            }
            set
            {
                folderPath = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tests whether this object is equal to otherObject.
        /// If otherObject is a Category, tests if their Name properties are
        /// equal, otherwise, compares memory addresses
        /// </summary>
        /// <param name="otherObject">The object to compare to as 
        ///                           an object</param>
        /// <returns>True if this is equal to otherObject,
        ///          otherwise false</returns>
        public override bool Equals(object otherObject)
        {
            bool isEqual = false;
            if (otherObject is Category)
            {
                isEqual = Name.Equals(((Category)otherObject).Name) && 
                    FolderPath.Equals(((Category)otherObject).FolderPath);
            }
            return isEqual;
        }


        /// <summary>
        /// Returns the unique name of the Category.
        /// </summary>
        /// <returns>Name of the Category</returns>
        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// Compares this Category object to other using their Name properties
        /// </summary>
        /// <param name="other">The other Category object to compare to</param>
        /// <returns>Positive if this should be placed after other,
        /// negative if this should be place before other,
        /// otherwise, 0 as an int</returns>
        public int CompareTo(Category other)
        {
            int result = 0;

            Name.CompareTo(other.Name);

            return result;
        }

        /// <summary>
        /// Gets this object's hash code
        /// </summary>
        /// <returns>This object's hash code as an int</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
