namespace XmlDataImporter
{
    using System.Configuration;

    /// <summary>
    /// Данные авторизации.
    /// </summary>
    public static class AuthorizationData
    {
        /// <summary>
        /// Получает имя сервера базы данных.
        /// </summary>
        public static string ServerName => ConfigurationManager.AppSettings["ServerName"] ?? throw new ArgumentNullException(nameof(ServerName));

        /// <summary>
        /// Получает имя базы данных.
        /// </summary>
        public static string DbName => ConfigurationManager.AppSettings["DbName"] ?? throw new ArgumentNullException(nameof(DbName));

        /// <summary>
        /// Получает имя пользователя базы данных.
        /// </summary>
        public static string UserName => ConfigurationManager.AppSettings["UserName"] ?? throw new ArgumentNullException(nameof(UserName));

        /// <summary>
        /// Получает пароль пользователя базы данных.
        /// </summary>
        public static string UserPassword => ConfigurationManager.AppSettings["UserPassword"] ?? throw new ArgumentNullException(nameof(UserPassword));
    }
}