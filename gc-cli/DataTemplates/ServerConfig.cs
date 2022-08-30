using System.Collections.Generic;

namespace gc_cli.DataTemplates
{
    public class ServerConfig
    {
        public class FolderStructure
        {
            /// <summary>
            /// 
            /// </summary>
            public string resources { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string data { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string packets { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string scripts { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string plugins { get; set; }
        }

        public class ServerDB
        {
            /// <summary>
            /// 
            /// </summary>
            public string connectionUri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string collection { get; set; }
        }

        public class GameDB
        {
            /// <summary>
            /// 
            /// </summary>
            public string connectionUri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string collection { get; set; }
        }

        public class DatabaseInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public ServerDB server { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public GameDB game { get; set; }
        }

        public class Language
        {
            /// <summary>
            /// 
            /// </summary>
            public string language { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string fallback { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string document { get; set; }
        }

        public class Account
        {
            /// <summary>
            /// 
            /// </summary>
            public string autoCreate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> defaultPermissions { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int maxPlayer { get; set; }
        }

        public class Encryption
        {
            /// <summary>
            /// 
            /// </summary>
            public string useEncryption { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string useInRouting { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string keystore { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string keystorePassword { get; set; }
        }

        public class Cors
        {
            /// <summary>
            /// 
            /// </summary>
            public string enabled { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> allowedOrigins { get; set; }
        }

        public class Policies
        {
            /// <summary>
            /// 
            /// </summary>
            public Cors cors { get; set; }
        }

        public class Files
        {
            /// <summary>
            /// 
            /// </summary>
            public string indexFile { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string errorFile { get; set; }
        }

        public class Http
        {
            /// <summary>
            /// 
            /// </summary>
            public string bindAddress { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string accessAddress { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int bindPort { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int accessPort { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Encryption encryption { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Policies policies { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Files files { get; set; }
        }

        public class InventoryLimits
        {
            /// <summary>
            /// 
            /// </summary>
            public int weapons { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int relics { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int materials { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int furniture { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int all { get; set; }
        }

        public class AvatarLimits
        {
            /// <summary>
            /// 
            /// </summary>
            public int singlePlayerTeam { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int multiplayerTeam { get; set; }
        }

        public class ResinOptions
        {
            /// <summary>
            /// 
            /// </summary>
            public string resinUsage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int cap { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int rechargeTime { get; set; }
        }

        public class Rates
        {
            /// <summary>
            /// 
            /// </summary>
            public string adventureExp { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string mora { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string leyLines { get; set; }
        }

        public class GameOptions
        {
            /// <summary>
            /// 
            /// </summary>
            public InventoryLimits inventoryLimits { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public AvatarLimits avatarLimits { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int sceneEntityLimit { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string watchGachaConfig { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string enableShopItems { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string staminaUsage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string energyUsage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public ResinOptions resinOptions { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Rates rates { get; set; }
        }

        public class ItemsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public int itemId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int itemCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int itemLevel { get; set; }
        }

        public class WelcomeMail
        {
            /// <summary>
            /// 
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string content { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sender { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<ItemsItem> items { get; set; }
        }

        public class JoinOptions
        {
            /// <summary>
            /// 
            /// </summary>
            public List<int> welcomeEmotes { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string welcomeMessage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public WelcomeMail welcomeMail { get; set; }
        }

        public class ServerAccount
        {
            /// <summary>
            /// 
            /// </summary>
            public int avatarId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int nameCardId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int adventureRank { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int worldLevel { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string nickName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string signature { get; set; }
        }

        public class Game
        {
            /// <summary>
            /// 
            /// </summary>
            public string bindAddress { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string accessAddress { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int bindPort { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int accessPort { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int loadEntitiesForPlayerRange { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string enableScriptInBigWorld { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string enableConsole { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public GameOptions gameOptions { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public JoinOptions joinOptions { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public ServerAccount serverAccount { get; set; }
        }

        public class Dispatch
        {
            /// <summary>
            /// 
            /// </summary>
            public List<string> regions { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string defaultName { get; set; }
        }

        public class Server
        {
            /// <summary>
            /// 
            /// </summary>
            public string debugLevel { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> DebugWhitelist { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> DebugBlacklist { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string runMode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Http http { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Game game { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Dispatch dispatch { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public FolderStructure folderStructure { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public DatabaseInfo databaseInfo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Language language { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Account account { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Server server { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int version { get; set; }
        }

    }
}
