using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using static System.String;
using static System.Environment;

namespace University_Menu
{
    public partial class MainWindow
    {
        public static Languages uiLanguage = Languages.English;
        private static int iconFixedSize = 16;
        public static readonly DateTime defaultDate = DateTime.MinValue;

        public static string xmlFolder = AppDomain.CurrentDomain.BaseDirectory;
        public static string xmlFile = xmlFolder + @"\um.xml";
        public static string xmlFolderUser = GetFolderPath(SpecialFolder.ApplicationData) + @"\HY\University Menu";
        public static string xmlFileUser = xmlFolderUser + @"\um.xml";

        public static bool timerFirstLoop = true;
        public const int defaultIdleTimeMin = 225;
        public const int defaultIdleTimeMax = 255;
        public static bool timerConnectionReady = true;
        public static DateTime timerConnectionLastRun = DateTime.MinValue;

        public static int menuItemCount = 0;
        public static int menuItemSubTag = 0;
        public static int zero = 0;

        public static Brush defaultFore = null;
        public static Brush defaultBack = null;
        public static Brush themeFore = Brushes.Black;
        public static Brush themeForeOpposite = Brushes.White;
        public static Brush themePath = Brushes.DarkRed;
        public static bool darkTheme = false;

        public const string hklm = @"HKEY_LOCAL_MACHINE\";
        public const string lmTietos = hklm + @"SOFTWARE\Tietos";
        public const string hkcu = @"HKEY_CURRENT_USER\";
        public const string regUM = @"SOFTWARE\HY\University Menu";
        public const string regCheckup = regUM + @"\Check-up";
        public const string regNetwork = regUM + @"\NetworkSpace";
        public const string regRoaming = regUM + @"\RoamingProfile";

        // Global settings
        public static int iconStatus = 0;

        public static int[] notifyStatus = new int[] { 0 };
        public static IconType checkupIconState = IconType.Normal;
        public static IconType networkIconState = IconType.Normal;
        public static IconType rebootIconState = IconType.Normal;
        public static IconType roamingIconState = IconType.Normal;
        public static IconType warrantyIconState = IconType.Normal;
        public static IconType osupgradeIconState = IconType.Normal;

        public static int[] moduleStatus = new int[] { 0 };
        public static IconType moduleUserIconState = IconType.Normal;
        public static IconType moduleCompIconState = IconType.Normal;

        public static int forceLanguage = -1;

        public static bool allowWelcome = false;
        public static bool allowCheckup = false;
        public static bool allowNetwork = false;
        public static bool allowUserInfo = false;
        public static bool allowCompInfo = false;
        public static bool allowOWA = false;
        public static bool allowChat = false;
        public static bool allowSupport = false;
        public static bool allowReboot = false;
        public static bool allowRoaming = false;
        public static bool allowConnection = false;
        public static bool allowWarranty = false;
        public static bool allowOSUpgrade = false;

        public static int startTimer = 4;
        public static int idleTimerMin = 225;
        public static int idleTimerMax = 255;
        public static int connectionTimer = 15;

        public static string uiHelpEN = "https://helpdesk.it.helsinki.fi/en";
        public static string uiHelpFI = "https://helpdesk.it.helsinki.fi";
        public static string uiHelpSV = "https://helpdesk.it.helsinki.fi/sv";

        public static OSUpgradeGroups OSGroup = OSUpgradeGroups.None;

        // Chat
        public static int chatWindowWidth = 438;
        public static int chatWindowHeight = 605;
        public static string chatUrlEN = "http://www.helsinki.fi/helpdesk-extra/univmenu_chat/univmenu_chat_index.html";
        public static string chatUrlFI = "http://www.helsinki.fi/helpdesk-extra/univmenu_chat/univmenu_chat_index.html";
        public static string chatUrlSV = "http://www.helsinki.fi/helpdesk-extra/univmenu_chat/univmenu_chat_index.html";
        public static bool chatExternalBrowsing = false;

        // Notifications
        public static int checkupMinDays = 30;
        public static int checkupMaxDays = 180;
        public static int checkupPopupInterval = 8;
        public static int checkupBalloonInterval = 3;
        public static string checkupPopupFilePath = Empty;
        public static string checkupPopupFileParameters = Empty;
        public static int networkLimitMB = 25;
        public static int networkPopupInterval = 10;
        public static int networkBalloonInterval = 4;
        public static string networkPopupFilePath = Empty;
        public static string networkPopupFileParameters = Empty;
        public static int rebootWait = 7;
        public static int rebootWaitWU = 4;
        public static int rebootBalloonShowtime = 7;
        public static int rebootPopupShowtime = -1;
        public static string rebootPopupFilePath = Empty;
        public static string rebootPopupFileParameters = Empty;
        public static string warrantyPopupFilePath = Empty;
        public static string warrantyPopupFileParameters = Empty;
        public static int osupgradePopupInterval = 3;

        public static int welcomeShowCount = 1;
        public static int balloonShowTime = 16000;

        public static string notifyHelpdeskPhone = "+358 2 941 55555";
        public static string notifyHelpdeskEmail = "helpdesk@helsinki.fi";
        public static string notifyVPNExecute = Empty;
        public static string notifyVPNParameters = Empty;
        public static string notifyURLCheckupEN = "https://helpdesk.it.helsinki.fi/en/help/5190";
        public static string notifyURLCheckupFI = "https://helpdesk.it.helsinki.fi/help/5190";
        public static string notifyURLCheckupSV = "https://helpdesk.it.helsinki.fi/sv/help/5190";
        public static string notifyURLNetworkEN = "https://helpdesk.it.helsinki.fi/en/help/3313";
        public static string notifyURLNetworkFI = "https://helpdesk.it.helsinki.fi/help/3313";
        public static string notifyURLNetworkSV = "https://helpdesk.it.helsinki.fi/sv/help/3313";
        public static string notifyURLWarrantyEN = "https://helpdesk.it.helsinki.fi/en/help/3313";
        public static string notifyURLWarrantyFI = "https://helpdesk.it.helsinki.fi/help/3313";
        public static string notifyURLWarrantySV = "https://helpdesk.it.helsinki.fi/sv/help/3313";
        public static string notifyURLOSUpgradeEN = "https://helpdesk.it.helsinki.fi/en/help/10857";
        public static string notifyURLOSUpgradeFI = "https://helpdesk.it.helsinki.fi/help/10857";
        public static string notifyURLOSUpgradeSV = "https://helpdesk.it.helsinki.fi/sv/help/10857";
        public static string notifyURLOSUpgrade2EN = "https://flamma.helsinki.fi/en/group/it-ja-puhelin/how-to-purchase-a-computer";
        public static string notifyURLOSUpgrade2FI = "https://flamma.helsinki.fi/fi/group/it-ja-puhelin/nain-hankit-tietokoneen";
        public static string notifyURLOSUpgrade2SV = "https://flamma.helsinki.fi/sv/group/it-ja-puhelin/sa-har-skaffar-du-en-dator";
        public static string notifyLocalFolder = Empty;

        public static int roamingShowCount = 1;
        public static DateTime roamingStartDate = DateTime.MaxValue;
        public static DateTime roamingEndDate = DateTime.MinValue;

        // Support Request
        public static string requestURLPortalEN = "https://helpdesk.it.helsinki.fi/en/help/3238";
        public static string requestURLPortalFI = "https://helpdesk.it.helsinki.fi/help/3238";
        public static string requestURLPortalSV = "https://helpdesk.it.helsinki.fi/sv/help/3238";
        public static string requestURLFlammaEN = "https://flamma.helsinki.fi/en/workstations/workstationpurchases";
        public static string requestURLFlammaFI = "https://flamma.helsinki.fi/fi/tyoasemat/tyoasemahankinnat";
        public static string requestURLFlammaSV = "https://flamma.helsinki.fi/en/workstations/workstationpurchases";

        public static List<SupportCategory> requestCategories = new List<SupportCategory>();
        public static string requestToAddress = "helpdesk@helsinki.fi";
        public static string requestEmailFooterEN = "Computer Name: %hostname%" + NewLine +
                "Model: %model%" + NewLine +
                "Serial Number: %serialnumber%" + NewLine +
                "Username: %username%" + NewLine +
                "Operating System: %os%" + NewLine +
                "CPU: %cpu%" + NewLine +
                "Memory: %memory% GB";
        public static string requestEmailFooterFI = "Konenimi: %hostname%" + NewLine +
                "Malli: %model%" + NewLine +
                "Sarjanumero: %serialnumber%" + NewLine +
                "Käyttäjätunnus: %username%" + NewLine +
                "Käyttöjärjestelmä: %os%" + NewLine +
                "Suoritin: %cpu%" + NewLine +
                "Muisti: %memory% Gt";
        public static string requestEmailFooterSV = "Datorns namn: %hostname%" + NewLine +
                "Modell: %model%" + NewLine +
                "Modell nummer: %serialnumber%" + NewLine +
                "Användarnamn: %username%" + NewLine +
                "Operativ system: %os%" + NewLine +
                "Processor: %cpu%" + NewLine +
                "Minne: %memory% GB";
        public static string requestHostAddress = "smtp.helsinki.fi";
        public static int requestHostPort = 587;

        // Modules
        public static string mailboxCloud = "http://www.helsinki.fi/office365";
        public static string mailboxLocal = "https://office365.helsinki.fi/";

        public static bool moduleChangeUserIcon = false;
        public static int moduleNotifyDateUser = 14;
        public static bool moduleChangeCompIcon = false;
        public static int moduleNotifyDateComp = 30;

        public static string moduleUIAddHeader = Empty;
        public static string moduleUIAddHeaderFI = Empty;
        public static string moduleUIAddHeaderSV = Empty;
        public static string moduleUIAddTooltip = Empty;
        public static string moduleUIAddTooltipFI = Empty;
        public static string moduleUIAddTooltipSV = Empty;
        public static string moduleUIAddExecute = Empty;
        public static string moduleUIAddExecuteFI = Empty;
        public static string moduleUIAddExecuteSV = Empty;
        public static string moduleUIAddParameters = Empty;
        public static string moduleUIAddParametersFI = Empty;
        public static string moduleUIAddParametersSV = Empty;
        public static string moduleUIAddIcon = Empty;

        public static string moduleCIAddHeader = Empty;
        public static string moduleCIAddHeaderFI = Empty;
        public static string moduleCIAddHeaderSV = Empty;
        public static string moduleCIAddTooltip = Empty;
        public static string moduleCIAddTooltipFI = Empty;
        public static string moduleCIAddTooltipSV = Empty;
        public static string moduleCIAddExecute = Empty;
        public static string moduleCIAddExecuteFI = Empty;
        public static string moduleCIAddExecuteSV = Empty;
        public static string moduleCIAddParameters = Empty;
        public static string moduleCIAddParametersFI = Empty;
        public static string moduleCIAddParametersSV = Empty;
        public static string moduleCIAddIcon = Empty;

        public static string moduleUIPasswordUrlEN = "https://helpdesk.it.helsinki.fi/en/help/5020";
        public static string moduleUIPasswordUrlFI = "https://helpdesk.it.helsinki.fi/help/5020";
        public static string moduleUIPasswordUrlSV = "https://helpdesk.it.helsinki.fi/sv/help/5020";
        public static string moduleUIAccountUrlEN = "https://helpdesk.it.helsinki.fi/en/help/5014";
        public static string moduleUIAccountUrlFI = "https://helpdesk.it.helsinki.fi/help/5014";
        public static string moduleUIAccountUrlSV = "https://helpdesk.it.helsinki.fi/sv/help/5014";
        public static string moduleCIWarrantyUrlEN = "https://helpdesk.it.helsinki.fi/en/help/10565";
        public static string moduleCIWarrantyUrlFI = "https://helpdesk.it.helsinki.fi/help/10565";
        public static string moduleCIWarrantyUrlSV = "https://helpdesk.it.helsinki.fi/sv/help/10565";

        // User settings
        public static int welcomeCount = -1;
        public static DateTime lastShown = DateTime.MinValue;
        public static DateTime lastCheckupPopup = DateTime.MinValue;
        public static DateTime lastCheckupBalloon = DateTime.MinValue;
        public static bool checkupBalloon = false;
        public static bool checkupExclude = false;
        public static DateTime lastNetworkPopup = DateTime.MinValue;
        public static DateTime lastNetworkBalloon = DateTime.MinValue;
        public static bool networkBalloon = false;
        public static bool networkExclude = false;
        public static bool rebootExclude = false;
        public static int roamingCount = 0;
        public static bool roamingExclude = false;
        public static DateTime lastWarrantyPopup = DateTime.MinValue;
        public static bool warrantyExclude = false;
        public static DateTime lastOSUpgradePopup = DateTime.MinValue;
        //public static bool osupgradeExclude = false;

        public static string moduleUIDisplayName = Empty;
        public static string moduleUIEmail = Empty;
        public static double moduleUIMailbox = -1;
        public static DateTime moduleUILastPWSet = DateTime.MinValue;
        public static DateTime moduleUIPWExpires = DateTime.MinValue;
        public static DateTime moduleUIAccountExpires = DateTime.MinValue;
        public static string moduleUIHomeDirectory = Empty;
        public static string moduleUIHomeDrive = Empty;

        public static Uri theme = AppearanceManager.LightThemeSource;
        public static FontSize themeFont = FirstFloor.ModernUI.Presentation.FontSize.Large;
        public static Color themeColor = (Color)ColorConverter.ConvertFromString("#FF1BA1E2");
    }
}