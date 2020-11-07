using System;

namespace Protocol
{
    /// <summary>
    /// App's User.
    /// </summary>
    [Serializable]
    public class User
    {

        #region Public properties

        /// <summary>
        /// The unique name.
        /// </summary>
        public string Username { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the User instance and initialize it with such <paramref name="username"/>
        /// </summary>
        /// <param name="username">Value for <see cref="Username"/></param>
        public User(string username)
        {
            Username = username;
        }

        #endregion

        #region Overriding comparison operators

        public static bool operator ==(User left, User right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Username == right.Username;
        }
        public static bool operator !=(User left, User right) => !(left == right);

        /// <inheritdoc cref="object.Equals(object?)"/>
        public override bool Equals(object obj) => base.Equals(obj);
        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode() => base.GetHashCode();

        #endregion

        #region ToString override

        ///<inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return Username;
        }

        #endregion

    }
}
