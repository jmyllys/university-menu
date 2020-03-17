using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.String;
using static System.Environment;
using System.Windows;
using FirstFloor.ModernUI.Presentation;
using System.DirectoryServices;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace University_Menu
{
    public partial class MainWindow
    {
        public static void ReadXML(string file, string node, XML xml)
        {
            XmlDocument xmlMenu = new XmlDocument();
            XmlNode nodeMenu;
            int retry = 0, wait = 10;

            Retry:
            Thread.Sleep(wait);
            try
            {
                xmlMenu.Load(file);
                nodeMenu = xmlMenu.DocumentElement.SelectSingleNode(node);
            }
            catch (FileNotFoundException)
            {
                switch (xml)
                {
                    case XML.User:
                        ReadOldUserSettings();
                        WriteXML();
                        return;
                    default:
                        return;
                }
            }
            catch
            {
                if (retry > 5) { return; }

                retry++;
                wait += 100;
                goto Retry;
            }

            switch (xml)
            {
                case XML.Local:
                    GatherMenuReadXml(nodeMenu, ref zero);
                    return;
                case XML.User:
                    ReadUserSettings(nodeMenu);
                    return;
                case XML.Settings:
                    GatherSettings(nodeMenu);
                    return;
                default:
                    return;
            }
        }

        public static void WriteXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDec);

            XmlElement root = xmlDoc.CreateElement("user");
            xmlDoc.AppendChild(root);

            XmlElement settings = xmlDoc.CreateElement("settings");

            AppendAttribute(ref settings, Properties.Resources.UserLanguage, uiLanguage.GetHashCode().ToString());
            AppendAttribute(ref settings, Properties.Resources.UserWelcome, welcomeCount.ToString());
            AppendAttribute(ref settings, Properties.Resources.UserLastShown, Format("{0:g}", lastShown));
            AppendAttribute(ref settings, Properties.Resources.UserCheckupPopup, Format("{0:g}", lastCheckupPopup));
            AppendAttribute(ref settings, Properties.Resources.UserLastCheckupBalloon, Format("{0:g}", lastCheckupBalloon));
            AppendAttribute(ref settings, Properties.Resources.UserCheckupBalloon, checkupBalloon.GetHashCode().ToString());
            AppendAttribute(ref settings, Properties.Resources.UserCheckupExclude, checkupExclude.GetHashCode().ToString());
            AppendAttribute(ref settings, Properties.Resources.UserNetworkPopup, Format("{0:g}", lastNetworkPopup));
            AppendAttribute(ref settings, Properties.Resources.UserLastNetworkBalloon, Format("{0:g}", lastNetworkBalloon));
            AppendAttribute(ref settings, Properties.Resources.UserNetworkBalloon, networkBalloon.GetHashCode().ToString());
            AppendAttribute(ref settings, Properties.Resources.UserNetworkExclude, networkExclude.GetHashCode().ToString());
            AppendAttribute(ref settings, Properties.Resources.UserRebootExclude, rebootExclude.GetHashCode().ToString());
            AppendAttribute(ref settings, Properties.Resources.UserRoaming, roamingCount.ToString());
            AppendAttribute(ref settings, Properties.Resources.UserRoamingExclude, roamingExclude.GetHashCode().ToString());
            AppendAttribute(ref settings, Properties.Resources.UserWarrantyPopup, Format("{0:g}", lastWarrantyPopup));
            AppendAttribute(ref settings, Properties.Resources.UserWarrantyExclude, warrantyExclude.GetHashCode().ToString());
            AppendAttribute(ref settings, Properties.Resources.UserOSUpgradePopup, Format("{0:g}", lastOSUpgradePopup));
            //AppendAttribute(ref settings, Properties.Resources.UserOSUpgradeExclude, osupgradeExclude.GetHashCode().ToString());
            AppendAttribute(ref settings, Properties.Resources.UserTheme, (AppearanceManager.Current.ThemeSource == AppearanceManager.DarkThemeSource ? "1" : "0"));
            AppendAttribute(ref settings, Properties.Resources.UserFont, themeFont.GetHashCode().ToString());
            AppendAttribute(ref settings, Properties.Resources.UserColor, themeColor.ToString());

            XmlElement modules = xmlDoc.CreateElement("modules");

            AppendAttribute(ref modules, Properties.Resources.UserFullName, moduleUIDisplayName);
            AppendAttribute(ref modules, Properties.Resources.UserEmail, moduleUIEmail);
            AppendAttribute(ref modules, Properties.Resources.UserEmailService, moduleUIMailbox.ToString());
            AppendAttribute(ref modules, Properties.Resources.UserPasswordSet, moduleUILastPWSet.ToShortDateString());
            AppendAttribute(ref modules, Properties.Resources.UserPasswordExpire, moduleUIPWExpires.ToShortDateString());
            AppendAttribute(ref modules, Properties.Resources.UserAccountExpire, moduleUIAccountExpires.ToShortDateString());
            AppendAttribute(ref modules, Properties.Resources.UserHomeDir, moduleUIHomeDirectory);
            AppendAttribute(ref modules, Properties.Resources.UserHomeDrive, moduleUIHomeDrive);

            settings.AppendChild(modules);

            root.AppendChild(settings);

            try
            {
                if (Directory.Exists(xmlFolderUser)) { Directory.CreateDirectory(xmlFolderUser); }
                xmlDoc.Save(xmlFileUser);
            }
            catch { return; }
        }

        private static void AppendAttribute(ref XmlElement element, string name, string value)
        {
            try { element.SetAttribute(name, value); }
            catch { return; }
        }

        private static int GetSettingValue(XmlNode node, string attribute, int defaultValue)
        {
            try
            {
                if (int.TryParse(node.Attributes[attribute].InnerText, out int output)) { return output; }
                else { return defaultValue; }
            }
            catch { return defaultValue; }
        }

        private static DateTime GetSettingValue(XmlNode node, string attribute, DateTime defaultValue)
        {
            try
            {
                if (DateTime.TryParse(node.Attributes[attribute].InnerText, out DateTime output)) { return output; }
                else { return defaultValue; }
            }
            catch { return defaultValue; }
        }

        private static void ReadUserSettings(XmlNode node)
        {
            uiLanguage = ConvertLanguage(GetSettingValue(node, Properties.Resources.UserLanguage, uiLanguage.GetHashCode()));
            welcomeCount = GetSettingValue(node, Properties.Resources.UserWelcome, -1);
            lastShown = GetSettingValue(node, Properties.Resources.UserLastShown, defaultDate);
            lastCheckupPopup = GetSettingValue(node, Properties.Resources.UserCheckupPopup, defaultDate);
            lastCheckupBalloon = GetSettingValue(node, Properties.Resources.UserLastCheckupBalloon, defaultDate);
            checkupBalloon = BoolValue(node.Attributes[Properties.Resources.UserCheckupBalloon]?.InnerText ?? false.ToString());
            checkupExclude = BoolValue(node.Attributes[Properties.Resources.UserCheckupExclude]?.InnerText ?? false.ToString());
            lastNetworkPopup = GetSettingValue(node, Properties.Resources.UserNetworkPopup, defaultDate);
            lastNetworkBalloon = GetSettingValue(node, Properties.Resources.UserLastNetworkBalloon, defaultDate);
            networkBalloon = BoolValue(node.Attributes[Properties.Resources.UserNetworkBalloon]?.InnerText ?? false.ToString());
            networkExclude = BoolValue(node.Attributes[Properties.Resources.UserNetworkExclude]?.InnerText ?? false.ToString());
            rebootExclude = BoolValue(node.Attributes[Properties.Resources.UserRebootExclude]?.InnerText ?? false.ToString());
            roamingCount = GetSettingValue(node, Properties.Resources.UserRoaming, 0);
            roamingExclude = BoolValue(node.Attributes[Properties.Resources.UserRoamingExclude]?.InnerText ?? false.ToString());
            lastWarrantyPopup = GetSettingValue(node, Properties.Resources.UserWarrantyPopup, defaultDate);
            warrantyExclude = BoolValue(node.Attributes[Properties.Resources.UserWarrantyExclude]?.InnerText ?? false.ToString());
            lastOSUpgradePopup = GetSettingValue(node, Properties.Resources.UserOSUpgradePopup, defaultDate);
            //osupgradeExclude = BoolValue(node.Attributes[Properties.Resources.UserOSUpgradeExclude]?.InnerText ?? false.ToString());

            int themeCheck = GetSettingValue(node, Properties.Resources.UserTheme, 0);
            if (themeCheck == 1) { theme = AppearanceManager.DarkThemeSource; } else { theme = AppearanceManager.LightThemeSource; }

            int fontCheck = GetSettingValue(node, Properties.Resources.UserFont, 0);
            if (fontCheck == 1) { themeFont = FirstFloor.ModernUI.Presentation.FontSize.Small; } else { themeFont = FirstFloor.ModernUI.Presentation.FontSize.Large; }

            try { themeColor = (Color)ColorConverter.ConvertFromString(node.Attributes[Properties.Resources.UserColor].InnerText); }
            catch { themeColor = (Color)ColorConverter.ConvertFromString("#FF1BA1E2"); }

            string singleNode = "//modules";
            moduleUIDisplayName = node.SelectSingleNode(singleNode)?.Attributes[Properties.Resources.UserFullName]?.InnerText ?? Empty;
            moduleUIEmail = node.SelectSingleNode(singleNode)?.Attributes[Properties.Resources.UserEmail]?.InnerText ?? Empty;
            moduleUIMailbox = GetSettingValue(node.SelectSingleNode(singleNode), Properties.Resources.UserEmailService, -1);
            moduleUILastPWSet = GetSettingValue(node.SelectSingleNode(singleNode), Properties.Resources.UserPasswordSet, defaultDate);
            moduleUIPWExpires = GetSettingValue(node.SelectSingleNode(singleNode), Properties.Resources.UserPasswordExpire, defaultDate);
            moduleUIAccountExpires = GetSettingValue(node.SelectSingleNode(singleNode), Properties.Resources.UserAccountExpire, defaultDate);
            moduleUIHomeDirectory = node.SelectSingleNode(singleNode)?.Attributes[Properties.Resources.UserHomeDir]?.InnerText ?? Empty;
            moduleUIHomeDrive = node.SelectSingleNode(singleNode)?.Attributes[Properties.Resources.UserHomeDrive]?.InnerText ?? Empty;
        }

        private static void ReadOldUserSettings()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(regUM))
            {
                if (key != null)
                {
                    uiLanguage = ConvertLanguage(GetRegValue(hkcu + regUM, Properties.Resources.UserLanguage, uiLanguage.GetHashCode()));
                    welcomeCount = GetRegValue(hkcu + regUM, "StatusWelcome", welcomeCount);
                    lastShown = GetRegValue(hkcu + regUM, Properties.Resources.UserLastShown, lastShown);
                    lastCheckupPopup = GetRegValue(hkcu + regCheckup, "LastPopup", lastCheckupPopup);
                    checkupExclude = BoolValue(GetRegValue(hkcu + regCheckup, "Exclude", checkupExclude.ToString()));
                    lastNetworkPopup = GetRegValue(hkcu + regNetwork, "LastPopup", lastCheckupPopup);
                    networkExclude = BoolValue(GetRegValue(hkcu + regNetwork, "Exclude", networkExclude.ToString()));
                    roamingCount = GetRegValue(hkcu + regRoaming, "CountPopup", roamingCount);
                    roamingExclude = BoolValue(GetRegValue(hkcu + regRoaming, "Exclude", roamingExclude.ToString()));

                    //Registry.CurrentUser.DeleteSubKeyTree(RegRoot);
                }
            }
        }

        private static void GatherSettings(XmlNode nodes)
        {
            forceLanguage = GetSettingValue(nodes, "ForceLanguage", -1);
            if (forceLanguage >= 0)
            {
                switch (forceLanguage)
                {
                    case 0:
                        uiLanguage = Languages.English;
                        break;
                    case 1:
                        uiLanguage = Languages.Suomi;
                        break;
                    case 2:
                        uiLanguage = Languages.Svenska;
                        break;
                    default:
                        forceLanguage = -1;
                        break;
                }
            }

            allowWelcome = BoolValue(nodes.Attributes["AllowWelcome"]?.InnerText ?? false.ToString());
            allowCheckup = BoolValue(nodes.Attributes["AllowCheckup"]?.InnerText ?? false.ToString());
            allowNetwork = BoolValue(nodes.Attributes["AllowNetwork"]?.InnerText ?? false.ToString());
            allowUserInfo = BoolValue(nodes.Attributes["AllowUserInfo"]?.InnerText ?? false.ToString());
            allowCompInfo = BoolValue(nodes.Attributes["AllowComputerInfo"]?.InnerText ?? false.ToString());
            allowOWA = BoolValue(nodes.Attributes["AllowOWA"]?.InnerText ?? false.ToString());
            allowChat = BoolValue(nodes.Attributes["AllowChat"]?.InnerText ?? false.ToString());
            allowSupport = BoolValue(nodes.Attributes["AllowSupportRequest"]?.InnerText ?? false.ToString());
            allowReboot = BoolValue(nodes.Attributes["AllowRebootPending"]?.InnerText ?? false.ToString());
            allowRoaming = BoolValue(nodes.Attributes["AllowRoamingProfile"]?.InnerText ?? false.ToString());
            allowConnection = BoolValue(nodes.Attributes["AllowConnectionMonitor"]?.InnerText ?? false.ToString());
            allowOSUpgrade = BoolValue(nodes.Attributes["AllowOSUpgrade"]?.InnerText ?? false.ToString());

            checkupIconState = IconType.Normal;
            networkIconState = IconType.Normal;
            rebootIconState = IconType.Normal;
            roamingIconState = IconType.Normal;
            moduleUserIconState = IconType.Normal;
            moduleCompIconState = IconType.Normal;
            osupgradeIconState = IconType.Normal;

            iconStatus = 0;
            notifyStatus = new int[] { 0 };
            moduleStatus = new int[] { 0 };

            startTimer = GetSettingValue(nodes, "TimerStart", 4);
            idleTimerMin = GetSettingValue(nodes, "TimerIdleMin", 225);
            idleTimerMax = GetSettingValue(nodes, "TimerIdleMax", 255);
            connectionTimer = GetSettingValue(nodes, "TimerConnectionWait", 15);

            uiHelpEN = nodes.Attributes["HelpUrlEN"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/en";
            uiHelpFI = nodes.Attributes["HelpUrlFI"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/";
            uiHelpSV = nodes.Attributes["HelpUrlSV"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/sv";

            // Support Request
            string singleNode = "//supportrequest";
            _ = new Variable();
            requestCategories.Clear();

            requestToAddress = nodes.SelectSingleNode(singleNode)?.Attributes["ToAddress"]?.InnerText ?? "helpdesk@helsinki.fi";
            requestHostAddress = nodes.SelectSingleNode(singleNode)?.Attributes["HostAddress"]?.InnerText ?? "smtp.helsinki.fi";
            requestHostPort = GetSettingValue(nodes.SelectSingleNode(singleNode), "HostPort", 587);

            requestEmailFooterEN = nodes.SelectSingleNode(singleNode)?.Attributes["MessageInfoEN"]?.InnerText ??
                "Computer Name: %hostname%" + NewLine +
                "Model: %model%" + NewLine +
                "Serial Number: %serialnumber%" + NewLine +
                "Username: %username%" + NewLine +
                "Operating System: %os%" + NewLine +
                "CPU: %cpu%" + NewLine +
                "Memory: %memorygb% GB";
            requestEmailFooterFI = nodes.SelectSingleNode(singleNode)?.Attributes["MessageInfoFI"]?.InnerText ??
                "Konenimi: %hostname%" + NewLine +
                "Malli: %model%" + NewLine +
                "Sarjanumero: %serialnumber%" + NewLine +
                "Käyttäjätunnus: %username%" + NewLine +
                "Käyttöjärjestelmä: %os%" + NewLine +
                "Suoritin: %cpu%" + NewLine +
                "Muisti: %memorygb% Gt";
            requestEmailFooterSV = nodes.SelectSingleNode(singleNode)?.Attributes["MessageInfoSV"]?.InnerText ??
                "Datorns namn: %hostname%" + NewLine +
                "Modell: %model%" + NewLine +
                "Modell nummer: %serialnumber%" + NewLine +
                "Användarnamn: %username%" + NewLine +
                "Operativ system: %os%" + NewLine +
                "Processor: %cpu%" + NewLine +
                "Minne: %memorygb% GB";

            requestURLPortalEN = nodes.SelectSingleNode(singleNode)?.Attributes["PortalUrlEN"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/en/help/3238";
            requestURLPortalFI = nodes.SelectSingleNode(singleNode)?.Attributes["PortalUrlFI"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/help/3238";
            requestURLPortalSV = nodes.SelectSingleNode(singleNode)?.Attributes["PortalUrlSV"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/sv/help/3238";
            requestURLFlammaEN = nodes.SelectSingleNode(singleNode)?.Attributes["FlammaUrlEN"]?.InnerText ?? "https://flamma.helsinki.fi/en/workstations/workstationpurchases";
            requestURLFlammaFI = nodes.SelectSingleNode(singleNode)?.Attributes["FlammaUrlFI"]?.InnerText ?? "https://flamma.helsinki.fi/fi/tyoasemat/tyoasemahankinnat";
            requestURLFlammaSV = nodes.SelectSingleNode(singleNode)?.Attributes["FlammaUrlSV"]?.InnerText ?? "https://flamma.helsinki.fi/en/workstations/workstationpurchases";

            XmlNode sr = nodes.SelectSingleNode(singleNode);
            if (sr != null && sr.HasChildNodes)
            {
                foreach (XmlNode child in sr.ChildNodes)
                {
                    switch (child.Name.ToLower())
                    {
                        case "category":
                            requestCategories.Add(new SupportCategory
                            {
                                CaptionEN = child.Attributes["Caption"]?.InnerText ?? Empty,
                                CaptionFI = child.Attributes["CaptionFI"]?.InnerText ?? Empty,
                                CaptionSV = child.Attributes["CaptionSV"]?.InnerText ?? Empty,
                                Entra1EN = child.Attributes["ExtraField1"]?.InnerText ?? Empty,
                                Entra1FI = child.Attributes["ExtraField1FI"]?.InnerText ?? Empty,
                                Entra1SV = child.Attributes["ExtraField1SV"]?.InnerText ?? Empty,
                                Entra2EN = child.Attributes["ExtraField2"]?.InnerText ?? Empty,
                                Entra2FI = child.Attributes["ExtraField2FI"]?.InnerText ?? Empty,
                                Entra2SV = child.Attributes["ExtraField2SV"]?.InnerText ?? Empty,
                                TitleEN = child.Attributes["Title"]?.InnerText ?? Empty,
                                TitleFI = child.Attributes["TitleFI"]?.InnerText ?? Empty,
                                TitleSV = child.Attributes["TitleSV"]?.InnerText ?? Empty,
                                RequestType = child.Attributes["RequestType"]?.InnerText ?? Empty,
                                Category1 = child.Attributes["Category1"]?.InnerText ?? Empty,
                                Category2 = child.Attributes["Category2"]?.InnerText ?? Empty,
                                Category3 = child.Attributes["Category3"]?.InnerText ?? Empty,
                                SupportGroup = child.Attributes["SupportGroup"]?.InnerText ?? Empty
                            });
                            break;
                        default:
                            break;
                    }
                }
            }

            // Notifications
            singleNode = "//notifications";

            balloonShowTime = GetSettingValue(nodes.SelectSingleNode(singleNode), "BalloonShowTime", 16000);

            checkupMinDays = GetSettingValue(nodes.SelectSingleNode(singleNode), "CheckupStartShow", 30);
            checkupMaxDays = GetSettingValue(nodes.SelectSingleNode(singleNode), "CheckupEndShow", 180);
            checkupBalloonInterval = GetSettingValue(nodes.SelectSingleNode(singleNode), "CheckupBalloonInterval", 3);
            checkupPopupInterval = GetSettingValue(nodes.SelectSingleNode(singleNode), "CheckupPopupInterval", 8);
            checkupPopupFilePath = nodes.SelectSingleNode(singleNode)?.Attributes["CheckupPopupFilePath"]?.InnerText ?? Empty;
            checkupPopupFileParameters = nodes.SelectSingleNode(singleNode)?.Attributes["CheckupPopupFileParameters"]?.InnerText ?? Empty;
            notifyVPNExecute = nodes.SelectSingleNode(singleNode)?.Attributes["CheckupVPNPath"]?.InnerText ?? Empty;
            notifyVPNParameters = nodes.SelectSingleNode(singleNode)?.Attributes["CheckupVPNParameters"]?.InnerText ?? Empty;
            notifyURLCheckupEN = nodes.SelectSingleNode(singleNode)?.Attributes["CheckupUrlEN"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/en/help/5190";
            notifyURLCheckupFI = nodes.SelectSingleNode(singleNode)?.Attributes["CheckupUrlFI"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/help/5190";
            notifyURLCheckupSV = nodes.SelectSingleNode(singleNode)?.Attributes["CheckupUrlSV"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/sv/help/5190";

            networkLimitMB = GetSettingValue(nodes.SelectSingleNode(singleNode), "NetworkShowLimit", 25); // MB
            networkBalloonInterval = GetSettingValue(nodes.SelectSingleNode(singleNode), "NetworkBalloonInterval", 4);
            networkPopupInterval = GetSettingValue(nodes.SelectSingleNode(singleNode), "NetworkPopupInterval", 10);
            networkPopupFilePath = nodes.SelectSingleNode(singleNode)?.Attributes["NetworkPopupFilePath"]?.InnerText ?? Empty;
            networkPopupFileParameters = nodes.SelectSingleNode(singleNode)?.Attributes["NetworkPopupFileParameters"]?.InnerText ?? Empty;
            notifyURLNetworkEN = nodes.SelectSingleNode(singleNode)?.Attributes["NetworkUrlEN"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/en/help/3313";
            notifyURLNetworkFI = nodes.SelectSingleNode(singleNode)?.Attributes["NetworkUrlFI"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/help/3313";
            notifyURLNetworkSV = nodes.SelectSingleNode(singleNode)?.Attributes["NetworkUrlSV"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/sv/help/3313";

            //notifyURLOSUpgradeEN = nodes.SelectSingleNode(singleNode)?.Attributes["OSUpgradeUrlEN"]?.InnerText ?? Empty;
            //notifyURLOSUpgradeFI = nodes.SelectSingleNode(singleNode)?.Attributes["OSUpgradeUrlFI"]?.InnerText ?? Empty;
            //notifyURLOSUpgradeSV = nodes.SelectSingleNode(singleNode)?.Attributes["OSUpgradeUrlSV"]?.InnerText ?? Empty;

            rebootWait = GetSettingValue(nodes.SelectSingleNode(singleNode), "RebootWaitingTime", 7);
            rebootWaitWU = GetSettingValue(nodes.SelectSingleNode(singleNode), "RebootWaitingTimeWU", 4);
            rebootBalloonShowtime = GetSettingValue(nodes.SelectSingleNode(singleNode), "RebootBalloonShowtime", 7);
            rebootPopupShowtime = GetSettingValue(nodes.SelectSingleNode(singleNode), "RebootPopupShowtime", -1);
            rebootPopupFilePath = nodes.SelectSingleNode(singleNode)?.Attributes["RebootPopupFilePath"]?.InnerText ?? Empty;
            rebootPopupFileParameters = nodes.SelectSingleNode(singleNode)?.Attributes["RebootPopupFileParameters"]?.InnerText ?? Empty;

            welcomeShowCount = GetSettingValue(nodes.SelectSingleNode(singleNode), "WelcomeShowCount", 1);

            roamingShowCount = GetSettingValue(nodes.SelectSingleNode(singleNode), "RoamingShowCount", 1);
            roamingStartDate = GetSettingValue(nodes.SelectSingleNode(singleNode), "RoamingStartDate", DateTime.MaxValue);
            roamingEndDate = GetSettingValue(nodes.SelectSingleNode(singleNode), "RoamingEndDate", DateTime.MinValue);

            warrantyPopupFilePath = nodes.SelectSingleNode(singleNode)?.Attributes["WarrantyPopupFilePath"]?.InnerText ?? Empty;
            warrantyPopupFileParameters = nodes.SelectSingleNode(singleNode)?.Attributes["WarrantyPopupFileParameters"]?.InnerText ?? Empty;

            osupgradePopupInterval = GetSettingValue(nodes.SelectSingleNode(singleNode), "OSUpgradePopupInterval", 3);

            notifyHelpdeskPhone = nodes.SelectSingleNode(singleNode)?.Attributes["HelpdeskPhone"]?.InnerText ?? "+358 2 941 55555";
            notifyHelpdeskEmail = nodes.SelectSingleNode(singleNode)?.Attributes["HelpdeskEmail"]?.InnerText ?? "helpdesk@helsinki.fi";
            notifyLocalFolder = nodes.SelectSingleNode(singleNode)?.Attributes["LocalFolder"]?.InnerText ?? Empty;

            // Chat
            singleNode = "//chat";
            string chatURL = "http://www.helsinki.fi/helpdesk-extra/univmenu_chat/univmenu_chat_index.html";

            chatWindowWidth = GetSettingValue(nodes.SelectSingleNode(singleNode), "WindowWidth", 438);
            chatWindowHeight = GetSettingValue(nodes.SelectSingleNode(singleNode), "WindowHeight", 605);
            chatExternalBrowsing = BoolValue(nodes.SelectSingleNode(singleNode)?.Attributes["ExternalBrowsing"]?.InnerText ?? false.ToString());
            chatUrlEN = nodes.SelectSingleNode(singleNode)?.Attributes["UrlEN"]?.InnerText ?? chatURL;
            chatUrlFI = nodes.SelectSingleNode(singleNode)?.Attributes["UrlFI"]?.InnerText ?? chatURL;
            chatUrlSV = nodes.SelectSingleNode(singleNode)?.Attributes["UrlSV"]?.InnerText ?? chatURL;

            // Modules
            singleNode = "//modules";

            moduleChangeUserIcon = BoolValue(nodes.SelectSingleNode(singleNode)?.Attributes["UIShowNotification"]?.InnerText ?? false.ToString());
            moduleNotifyDateUser = GetSettingValue(nodes.SelectSingleNode(singleNode), "UINotifyWarningLimit", 14);
            moduleChangeCompIcon = BoolValue(nodes.SelectSingleNode(singleNode)?.Attributes["CIShowNotification"]?.InnerText ?? false.ToString());
            moduleNotifyDateComp = GetSettingValue(nodes.SelectSingleNode(singleNode), "CINotifyWarningLimit", 60);

            mailboxCloud = nodes.SelectSingleNode(singleNode)?.Attributes["OWACloudUrl"]?.InnerText ?? "http://www.helsinki.fi/office365";
            mailboxLocal = nodes.SelectSingleNode(singleNode)?.Attributes["OWALocalUrl"]?.InnerText ?? "https://office365.helsinki.fi/";

            moduleUIAddHeader = nodes.SelectSingleNode(singleNode)?.Attributes["UIHeader"]?.InnerText ?? Empty;
            moduleUIAddHeaderFI = nodes.SelectSingleNode(singleNode)?.Attributes["UIHeaderFI"]?.InnerText ?? Empty;
            moduleUIAddHeaderSV = nodes.SelectSingleNode(singleNode)?.Attributes["UIHeaderSV"]?.InnerText ?? Empty;
            moduleUIAddTooltip = nodes.SelectSingleNode(singleNode)?.Attributes["UIToolTip"]?.InnerText ?? Empty;
            moduleUIAddTooltipFI = nodes.SelectSingleNode(singleNode)?.Attributes["UIToolTipFI"]?.InnerText ?? Empty;
            moduleUIAddTooltipSV = nodes.SelectSingleNode(singleNode)?.Attributes["UIToolTipSV"]?.InnerText ?? Empty;
            moduleUIAddExecute = nodes.SelectSingleNode(singleNode)?.Attributes["UIExecute"]?.InnerText ?? Empty;
            moduleUIAddExecuteFI = nodes.SelectSingleNode(singleNode)?.Attributes["UIExecuteFI"]?.InnerText ?? Empty;
            moduleUIAddExecuteSV = nodes.SelectSingleNode(singleNode)?.Attributes["UIExecuteSV"]?.InnerText ?? Empty;
            moduleUIAddParameters = nodes.SelectSingleNode(singleNode)?.Attributes["UIParameters"]?.InnerText ?? Empty;
            moduleUIAddParametersFI = nodes.SelectSingleNode(singleNode)?.Attributes["UIParametersFI"]?.InnerText ?? Empty;
            moduleUIAddParametersSV = nodes.SelectSingleNode(singleNode)?.Attributes["UIParametersSV"]?.InnerText ?? Empty;
            moduleUIAddIcon = nodes.SelectSingleNode(singleNode)?.Attributes["UIIcon"]?.InnerText ?? Empty;

            moduleUIPasswordUrlEN = nodes.SelectSingleNode(singleNode)?.Attributes["UIPasswordUrlEN"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/en/help/5020";
            moduleUIPasswordUrlFI = nodes.SelectSingleNode(singleNode)?.Attributes["UIPasswordUrlFI"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/help/5020";
            moduleUIPasswordUrlSV = nodes.SelectSingleNode(singleNode)?.Attributes["UIPasswordUrlSV"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/sv/help/5020";
            moduleUIAccountUrlEN = nodes.SelectSingleNode(singleNode)?.Attributes["UIAccountUrlEN"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/en/help/5014";
            moduleUIAccountUrlFI = nodes.SelectSingleNode(singleNode)?.Attributes["UIAccountUrlFI"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/help/5014";
            moduleUIAccountUrlSV = nodes.SelectSingleNode(singleNode)?.Attributes["UIAccountUrlSV"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/sv/help/5014";

            moduleCIAddHeader = nodes.SelectSingleNode(singleNode)?.Attributes["CIHeader"]?.InnerText ?? Empty;
            moduleCIAddHeaderFI = nodes.SelectSingleNode(singleNode)?.Attributes["CIHeaderFI"]?.InnerText ?? Empty;
            moduleCIAddHeaderSV = nodes.SelectSingleNode(singleNode)?.Attributes["CIHeaderSV"]?.InnerText ?? Empty;
            moduleCIAddTooltip = nodes.SelectSingleNode(singleNode)?.Attributes["CIToolTip"]?.InnerText ?? Empty;
            moduleCIAddTooltipFI = nodes.SelectSingleNode(singleNode)?.Attributes["CIToolTipFI"]?.InnerText ?? Empty;
            moduleCIAddTooltipSV = nodes.SelectSingleNode(singleNode)?.Attributes["CIToolTipSV"]?.InnerText ?? Empty;
            moduleCIAddExecute = nodes.SelectSingleNode(singleNode)?.Attributes["CIExecute"]?.InnerText ?? Empty;
            moduleCIAddExecuteFI = nodes.SelectSingleNode(singleNode)?.Attributes["CIExecuteFI"]?.InnerText ?? Empty;
            moduleCIAddExecuteSV = nodes.SelectSingleNode(singleNode)?.Attributes["CIExecuteSV"]?.InnerText ?? Empty;
            moduleCIAddParameters = nodes.SelectSingleNode(singleNode)?.Attributes["CIParameters"]?.InnerText ?? Empty;
            moduleCIAddParametersFI = nodes.SelectSingleNode(singleNode)?.Attributes["CIParametersFI"]?.InnerText ?? Empty;
            moduleCIAddParametersSV = nodes.SelectSingleNode(singleNode)?.Attributes["CIParametersSV"]?.InnerText ?? Empty;
            moduleCIAddIcon = nodes.SelectSingleNode(singleNode)?.Attributes["CIIcon"]?.InnerText ?? Empty;

            moduleCIWarrantyUrlEN = nodes.SelectSingleNode(singleNode)?.Attributes["CIWarrantyUrlEN"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/en/help/10565";
            moduleCIWarrantyUrlFI = nodes.SelectSingleNode(singleNode)?.Attributes["CIWarrantyUrlFI"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/help/10565";
            moduleCIWarrantyUrlSV = nodes.SelectSingleNode(singleNode)?.Attributes["CIWarrantyUrlSV"]?.InnerText ?? "https://helpdesk.it.helsinki.fi/sv/help/10565";
        }

        private static bool BoolValue(string value)
        {
            if (IsNullOrWhiteSpace(value)) { return false; }

            switch (value.ToLower())
            {
                case "true":
                case "1":
                    return true;
                default:
                    return false;
            }
        }

        public static int GetRegValue(string regPath, string regValue, int defaultValue, bool returnError = false)
        {
            try
            { return (int)Registry.GetValue(regPath, regValue, defaultValue); }
            catch
            {
                if (returnError) { throw; }
                else { return defaultValue; }
            }
        }

        public static string GetRegValue(string regPath, string regValue, string defaultValue, bool returnError = false)
        {
            try
            {
                string input = Registry.GetValue(regPath, regValue, defaultValue).ToString();
                return (IsNullOrWhiteSpace(input) ? defaultValue : input);
            }
            catch
            {
                if (returnError) { throw; }
                else { return defaultValue; }
            }
        }

        public static DateTime GetRegValue(string regPath, string regValue, DateTime defaultValue, bool returnError = false)
        {
            try
            {
                if (DateTime.TryParse(Registry.GetValue(regPath, regValue, defaultValue).ToString(), out DateTime output))
                { return output; }
                else
                { return defaultValue; }
            }
            catch
            {
                if (returnError) { throw; }
                else { return defaultValue; }
            }
        }

        public static void Execute(string execute, string parameters)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = execute,
                Arguments = parameters
            };

            Execute(psi);
        }

        public static void Execute(ProcessStartInfo startInfo)
        {
            using (Process process = new Process())
            {
                try
                {
                    process.StartInfo = startInfo;
                    process.Start();
                }
                catch (FileNotFoundException)
                {
                    ExecuteError(GetTranslation(Properties.Resources.ErrorText),
                        GetTranslation(Properties.Resources.DialogFileError) + "'" + startInfo.FileName + "'.\n\n" + GetTranslation(Properties.Resources.DialogSorry).Trim());
                }
                catch
                {
                    ExecuteError(GetTranslation(Properties.Resources.ErrorText),
                        GetTranslation(Properties.Resources.DialogError) + GetTranslation(Properties.Resources.DialogSorry));
                }
            }
        }

        public static void ExecuteError(string title, string content, MessageBoxImage image = MessageBoxImage.Error)
        {
            try
            {
                ModernDialog md = new ModernDialog
                {
                    Title = title,
                    Content = content,
                    ShowInTaskbar = true,
                    ResizeMode = ResizeMode.CanMinimize,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ShowActivated = true,
                    Owner = null
                };

                md.Buttons = new Button[] { md.CloseButton };
                md.ShowDialog();
            }
            catch
            { MessageBox.Show(content, title, MessageBoxButton.OK, image); }
        }

        public static string VariableConvert(string text, SpaceUnit unit = SpaceUnit.B)
        {
            if (IsNullOrWhiteSpace(text) || text.IndexOf("%") < 0) { return text; }

            Variable variable = new Variable();
            bool exit = false;

            if (exit) { return text; } else { VariableConvert(ref text, "%username%", variable.Username, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%hostname%", variable.Hostname, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%model%", variable.Model, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%serialnumber%", variable.SerialNumber, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%cpu%", variable.Processor, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%memory%", variable.Memory.ToString(), ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%memorykb%", ConvertSpaceUnit(variable.Memory, SpaceUnit.KB).ToString(), ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%memorymb%", ConvertSpaceUnit(variable.Memory, SpaceUnit.MB).ToString(), ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%memorygb%", ConvertSpaceUnit(variable.Memory, SpaceUnit.GB).ToString(), ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%memorytb%", ConvertSpaceUnit(variable.Memory, SpaceUnit.TB).ToString(), ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%os%", variable.OperatingSystem, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%osversion%", variable.OSVersion, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%winupdatetime%", variable.WinUpdateTime, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%winupdatecheck%", variable.WinUpdateSearch, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%sysboottime%", (variable.SystemBootTime != DateTime.MinValue ? 
                variable.SystemBootTime.ToString() : GetTranslation(Properties.Resources.NA)), ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%defaultprinter%", variable.DefaultPrinter, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%building%", variable.Building, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%address%", variable.Address, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%room%", variable.Room, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%warranty%", variable.Warranty, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%profitcenter%", variable.ProfitCenter, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%diskcryptstatus%", variable.DiskCryptStatus, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%ipaddress%", variable.IPAddress, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%macaddress%", variable.MACAddress, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%ethernetmacaddress%", variable.MACAddressEthernet, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%wirelessmacaddress%", variable.MACAddressWireless, ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%compcertificate%", variable.CompCertValid.ToShortDateString(), ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%aovpncertificate%", variable.AoVPNCertValid.ToShortDateString(), ref exit); }
            if (exit) { return text; } else { VariableConvert(ref text, "%domain%", variable.Domain, ref exit); }

            return text;
        }

        private static void VariableConvert(ref string text, string search, string replace, ref bool exit)
        {
            if (text?.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                string from = search, to = replace;

                if (text?.IndexOf("[base64]", StringComparison.OrdinalIgnoreCase) >= 0 && replace != Properties.Resources.NA)
                {
                    from = @"\[base64\]" + search;
                    to = (!IsNullOrWhiteSpace(to) ? Convert.ToBase64String(Encoding.UTF8.GetBytes(to)) : Empty);
                }

                text = Regex.Replace(text, from, to, RegexOptions.IgnoreCase);
                if (text?.IndexOf("%") < 0) { exit = true; }
            }
        }

        public class Variable
        {
            public string Username { get { return UserName; } }
            public string Domain { get { return UserDomainName; } }
            public string Hostname { get { return MachineName; } }
            public string Model { get { return WMIQuery("Win32_ComputerSystem", "Model").Trim(); } }
            public string SerialNumber { get { return WMIQuery("Win32_BIOS", "SerialNumber").Trim(); } }
            public string Processor { get { return WMIQuery("Win32_Processor", "Name").Trim(); } }
            public double Memory
            {
                get
                {
                    try
                    {
                        double.TryParse(WMIQuery("Win32_ComputerSystem", "TotalPhysicalMemory").Trim(), out double input);

                        return input;
                    }
                    catch { return -1; }
                }
            }
            public string OperatingSystem { get { return WMIQuery("Win32_OperatingSystem", "Caption").Trim(); } }
            public string OSVersion { get { return Environment.OSVersion.Version.ToString(); } }
            public string IPAddress
            {
                get
                {
                    try
                    {
                        StringBuilder input = new StringBuilder();
                        IPHostEntry target = Dns.GetHostEntry(Dns.GetHostName());

                        foreach (IPAddress address in target.AddressList)
                        { if (address.AddressFamily == AddressFamily.InterNetwork) { AddText(ref input, address.ToString()); } }

                        return (!IsNullOrWhiteSpace(input.ToString()) ? input.ToString().Trim() : Properties.Resources.NA);
                    }
                    catch { return Properties.Resources.NA; }
                }
            }
            public string MACAddress
            {
                get
                {
                    try
                    {
                        StringBuilder input = new StringBuilder();
                        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

                        foreach (NetworkInterface nic in nics)
                        { if (!IsNullOrWhiteSpace(nic.GetPhysicalAddress().ToString())) { AddText(ref input, nic.GetPhysicalAddress().ToString()); } }

                        return (!IsNullOrWhiteSpace(input.ToString()) ? input.ToString().Trim() : Properties.Resources.NA);
                    }
                    catch { return Properties.Resources.NA; }
                }
            }
            public string MACAddressWireless
            {
                get
                {
                    try
                    {
                        StringBuilder input = new StringBuilder();
                        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

                        foreach (NetworkInterface nic in nics)
                        {
                            if (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && !IsNullOrWhiteSpace(nic.GetPhysicalAddress().ToString()) &&
                                !nic.Description.Contains("Direct Virtual Adapter"))
                            { AddText(ref input, nic.GetPhysicalAddress().ToString()); }
                        }

                        return (!IsNullOrWhiteSpace(input.ToString()) ? input.ToString().Trim() : Properties.Resources.NA);
                    }
                    catch { return Properties.Resources.NA; }
                }
            }
            public string MACAddressEthernet
            {
                get
                {
                    try
                    {
                        StringBuilder input = new StringBuilder();
                        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

                        foreach (NetworkInterface nic in nics)
                        {
                            if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && !IsNullOrWhiteSpace(nic.GetPhysicalAddress().ToString()))
                            { AddText(ref input, nic.GetPhysicalAddress().ToString()); }
                        }

                        return (!IsNullOrWhiteSpace(input.ToString()) ? input.ToString().Trim() : Properties.Resources.NA);
                    }
                    catch { return Properties.Resources.NA; }
                }
            }
            public string WinUpdateTime { get { return WMIQuery("hycustomclass", "WSUSInstall"); } }
            public string WinUpdateSearch { get { return WMIQuery("hycustomclass", "WSUSSearch"); } }
            public DateTime SystemBootTime
            {
                get
                {
                    try { return DateTime.Now.AddMilliseconds(TickCount - (TickCount * 2)); }
                    catch { return DateTime.MinValue; }
                }
            }
            public string DefaultPrinter { get { return WMIQuery("Win32_Printer", "Name", true).Trim(); } }
            public string Building { get { return GetRegValue(lmTietos, "Building", Properties.Resources.NA); } }
            public string Address { get { return GetRegValue(lmTietos, "Address", Properties.Resources.NA); } }
            public string Room { get { return GetRegValue(lmTietos, "Room", Properties.Resources.NA); } }
            public string Warranty { get { return GetRegValue(lmTietos, "Warranty", Properties.Resources.NA); } }
            public string ProfitCenter { get { return GetRegValue(lmTietos, "ProfitCenter", Properties.Resources.NA); } }
            public long HomeDriveFreeSpace
            {
                get
                {
                    try
                    {
                        using (DirectoryEntry entry = new DirectoryEntry("LDAP://ad.helsinki.fi:636")
                        { AuthenticationType = AuthenticationTypes.Secure | AuthenticationTypes.SecureSocketsLayer | AuthenticationTypes.ServerBind })
                        using (DirectorySearcher search = new DirectorySearcher(entry)
                        { Filter = "(&(objectClass=user)(objectCategory=person)(samaccountname=" + UserName + "))" })
                        {
                            search.PropertiesToLoad.Add("homedrive");

                            SearchResult user = search.FindOne();
                            string drive = user?.Properties["homedrive"][0].ToString() ?? Empty;

                            if (!IsNullOrWhiteSpace(drive))
                            {
                                DriveInfo info = new DriveInfo(drive);
                                return info.AvailableFreeSpace;
                            }
                            else { return -1; }
                        }
                    }
                    catch { return -1; }
                }
            }
            public long HomeDriveTotalSize
            {
                get
                {
                    try
                    {
                        using (DirectoryEntry entry = new DirectoryEntry("LDAP://ad.helsinki.fi:636")
                        { AuthenticationType = AuthenticationTypes.Secure | AuthenticationTypes.SecureSocketsLayer | AuthenticationTypes.ServerBind })
                        using (DirectorySearcher search = new DirectorySearcher(entry)
                        { Filter = "(&(objectClass=user)(objectCategory=person)(samaccountname=" + UserName + "))" })
                        {
                            search.PropertiesToLoad.Add("homedrive");

                            SearchResult user = search.FindOne();
                            string drive = user?.Properties["homedrive"][0].ToString() ?? Empty;

                            if (!IsNullOrWhiteSpace(drive))
                            {
                                DriveInfo info = new DriveInfo(drive);
                                return info.TotalSize;
                            }
                            else { return -1; }
                        }
                    }
                    catch { return -1; }
                }
            }
            public string DiskCryptStatus
            {
                get
                {
                    try
                    {
                        StringBuilder input = new StringBuilder();
                        string[] textIsCrypted = GetTranslation(Properties.Resources.ModuleCompIsEncrypt).Split(';');
                        string[] textNotCrypted = GetTranslation(Properties.Resources.ModuleCompNotEncrypt).Split(';');

                        foreach (var disk in GetCryptedDisks())
                        {
                            if (disk.Crypted) { AddText(ref input, textIsCrypted[0] + disk.Name + textIsCrypted[1]); }
                            else { AddText(ref input, textNotCrypted[0] + disk.Name + textNotCrypted[1]); }
                        }

                        return (!IsNullOrWhiteSpace(input.ToString()) ? input.ToString().Trim() : Properties.Resources.NA);
                    }
                    catch { return Properties.Resources.NA; }
                }
            }

            public DateTime CompCertValid
            {
                get
                {
                    try
                    {
                        X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                        List<DateTime> certs = new List<DateTime>();

                        store.Open(OpenFlags.ReadOnly);

                        foreach (X509Certificate2 certificate in store.Certificates)
                        {
                            foreach (X509Extension extension in certificate.Extensions)
                            {
                                if (extension.Oid.FriendlyName == "Certificate Template Information")
                                {
                                    if (extension.Format(true).Contains("Template=ConfigMgr Windows Client Certificate"))
                                    { if (certificate.Subject.Contains(MachineName)) { certs.Add(Convert.ToDateTime(certificate.NotAfter)); } }
                                }
                            }
                        }

                        certs.Sort((a, b) => b.CompareTo(a));
                        return certs.FirstOrDefault();

                    }
                    catch { return DateTime.MinValue; }
                }
            }
            public DateTime AoVPNCertValid
            {
                get
                {
                    try
                    {
                        X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                        List<DateTime> certs = new List<DateTime>();

                        store.Open(OpenFlags.ReadOnly);

                        foreach (X509Certificate2 certificate in store.Certificates)
                        {
                            foreach (X509Extension extension in certificate.Extensions)
                            {
                                if (extension.Oid.FriendlyName == "Certificate Template Information")
                                {
                                    if (extension.Format(true).Contains("Template=hyad-aovpn-comp"))
                                    { if (certificate.Subject.Contains(MachineName)) { certs.Add(Convert.ToDateTime(certificate.NotAfter)); } }
                                }
                            }
                        }

                        certs.Sort((a, b) => b.CompareTo(a));
                        return certs.FirstOrDefault();

                    }
                    catch { return DateTime.MinValue; }
                }
            }
        }

        public static string WMIQuery(string wmiClass, string instance, bool printer = false)
        {
            WqlObjectQuery wqlQuery = new WqlObjectQuery("SELECT * FROM " + wmiClass);

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(wqlQuery))
            {
                string input = Properties.Resources.NA;

                try
                {
                    foreach (ManagementObject search in searcher.Get())
                    {
                        if (printer)
                        { if ((bool)search["Default"] == true) { input = search[instance].ToString(); } }
                        else
                        { input = search[instance].ToString(); }
                    }
                }
                catch { input = Properties.Resources.NA; }

                return input;
            }
        }

        public static void AddText(ref StringBuilder input, string text)
        {
            if (!IsNullOrWhiteSpace(input.ToString())) { input.Append(", "); }

            input.Append(text);
        }

        public static System.Windows.Shapes.Path MenuIcon(string data, byte r = 0, byte g = 0, byte b = 0, int resize = 0)
        {
            if (r == 0 && g == 0 && b == 0)
            {
                if (AppearanceManager.Current.ThemeSource == AppearanceManager.DarkThemeSource)
                {
                    r = 193;
                    g = 193;
                    b = 193;
                }
            }

            try
            {
                return new System.Windows.Shapes.Path
                {
                    MaxHeight = 16 + resize,
                    MaxWidth = 16 + resize,
                    Stretch = Stretch.Uniform,
                    Fill = new SolidColorBrush(Color.FromRgb(r, g, b)),
                    Data = Geometry.Parse(data)
                };
            }
            catch { return null; }
        }

        public static Image MenuIconFromFile(string path)
        {
            try
            {
                Image img = new Image { Source = new BitmapImage(new Uri(path, UriKind.Absolute)) };
                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.NearestNeighbor);
                return img;
            }
            catch { return null; }
        }

        public static void OpenWindow(Popup source, bool restart = false)
        {
            switch (source)
            {
                case Popup.Chat:
                    if (System.Windows.Forms.Application.OpenForms.OfType<Chat>().Any() == false)
                    {
                        var window = new Chat();
                        window.Show();
                    }
                    else { System.Windows.Forms.Application.OpenForms["Chat"].BringToFront(); }

                    return;
                default:
                    if (restart) { uiWindow.Close(); }

                    if (uiWindow == null)
                    {
                        uiWindow = new UI(source);
                        uiWindow.Closed += UiWindow_Closed;
                        uiWindow.ShowActivated = true;
                        uiWindow.Show();
                    }
                    else { uiWindow.Activate(); }

                    return;
            }
        }

        public static UI uiWindow = null;

        private static void UiWindow_Closed(object sender, EventArgs e)
        {
            uiWindow = null;
        }

        public static string DiskSpaceUnit(string header, double value, bool reverse = false)
        {
            string[] inputTexts = header.Split(';');

            try
            {
                if (value < 0) return Properties.Resources.NA;

                int i = 1;
                double inputValue = value / 1024 / 1024;

                while (inputValue >= 1024)
                {
                    inputValue /= 1024;
                    i++;
                    if (i == 3) { break; }
                }

                if (!reverse) { return inputTexts[0] + Math.Round(inputValue, 2) + inputTexts[i]; }
                else { return Math.Round(inputValue, 2) + inputTexts[i] + inputTexts[0]; }
            }
            catch
            {
                if (!reverse) { return inputTexts[0].Trim() + value + " ??"; }
                else { return value + " ?? " + inputTexts[0].Trim(); }
            }
        }

        public static double ConvertSpaceUnit(double space, SpaceUnit unit)
        {
            switch (unit)
            {
                case SpaceUnit.KB:
                    return Math.Round((space / 1024), 2);
                case SpaceUnit.MB:
                    return Math.Round((space / 1024 / 1024), 2);
                case SpaceUnit.GB:
                    return Math.Round((space / 1024 / 1024 / 1024), 2);
                case SpaceUnit.TB:
                    return Math.Round((space / 1024 / 1024 / 1024 / 1024), 2);
                default:
                    return Math.Round(space, 2);
            }
        }

        public enum XML
        {
            Local,
            User,
            Settings
        }

        public class SupportCategory
        {
            public string CaptionEN { get; set; }
            public string CaptionFI { get; set; }
            public string CaptionSV { get; set; }
            public string Entra1EN { get; set; }
            public string Entra1FI { get; set; }
            public string Entra1SV { get; set; }
            public string Entra2EN { get; set; }
            public string Entra2FI { get; set; }
            public string Entra2SV { get; set; }
            public string TitleEN { get; set; }
            public string TitleFI { get; set; }
            public string TitleSV { get; set; }
            public string RequestType { get; set; }
            public string Category1 { get; set; }
            public string Category2 { get; set; }
            public string Category3 { get; set; }
            public string SupportGroup { get; set; }
        }

        public enum SpaceUnit
        {
            B,
            KB,
            MB,
            GB,
            TB
        }

        public static SendSettings sendSettings = new SendSettings();

        public class SendSettings
        {
            public bool Verified = false;
            public string Name { get; set; }
            public string Email { get; set; }
            public string Message { get; set; }
            public string Signature { get; set; }
        }
    }
}