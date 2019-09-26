using System;
using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace University_Menu
{
    public partial class MainWindow
    {
        public void GatherTranslations()
        {
            TranslationItems.Add(AddTranslation(Properties.Resources.ErrorText,
                "Error", "Virhe", "Fel"));
            TranslationItems.Add(AddTranslation(Properties.Resources.NameErrorText,
                "<error>", "<virhe>", "<fel>"));
            TranslationItems.Add(AddTranslation(Properties.Resources.DialogError,
                "Something went wrong.", "Jotain meni vikaan.", "Något gick fel."));
            TranslationItems.Add(AddTranslation(Properties.Resources.DialogFileError,
                "The following file could not be found: ", "Seuraavaa tiedostoa ei löytynyt: ", "Följande fil hittades inte: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.DialogSorry,
                " We are sorry for the inconvience.", " Pahoittelut.", " Vi beklagar detta."));

            // Menu
            TranslationItems.Add(AddTranslation(Properties.Resources.DefaultCheckup,
                "Open a secure VPN connection!", "Avaa suojattu VPN-yhteys!", "Öppna en skyddad VPN-anslutning!"));
            TranslationItems.Add(AddTranslation(Properties.Resources.DefaultNetwork,
                "Home directory space is running low!", "Kotihakemistolevytila vähissä!", "Ont om diskutrymme i hemkatalogen!"));
            TranslationItems.Add(AddTranslation(Properties.Resources.DefaultReboot,
                "Reboot pending!", "Tarvitaan uudelleenkäynnistys!", "Omstart behövs!"));
            TranslationItems.Add(AddTranslation(Properties.Resources.DefaultRoaming,
                "The settings of roaming profile will change!", "Liikkuvan profiilin asetuksia muutetaan!", "Inställningarna för roamingprofil ändras!"));
            TranslationItems.Add(AddTranslation(Properties.Resources.DefaultWarranty,
                "Your computer warranty has expired!", "Tietokoneen takuu on päättynyt!", "Din datorgaranti har löpt ut!"));
            TranslationItems.Add(AddTranslation(Properties.Resources.DefaultOSUpgrade,
                "Computer is running Windows 7 operating system!", "Tietokoneen käyttöjärjestelmä on Windows 7!", "Computer is running Windows 7 operating system!"));
            TranslationItems.Add(AddTranslation(Properties.Resources.DefaultLang,
                 "Settings", "Asetukset", "Inställningar"));
            TranslationItems.Add(AddTranslation(Properties.Resources.DefaultDemo,
                "Debugging Tools", "Testaustyökalut", "Felsöka verktygen"));

            // Notifications
            TranslationItems.Add(AddTranslation(Properties.Resources.UMIconText,
                "University Menu", "Yliopistovalikko", "Universitetsmeny"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UMIconTextNotify,
                "University Menu - Notifications", "Yliopistovalikko - Ilmoituksia", "Universitetsmeny - Notificeringar"));
            TranslationItems.Add(AddTranslation(Properties.Resources.NotifyWelcomeTitle,
                "Hi!", "Hei!", "Hej!"));
            TranslationItems.Add(AddTranslation(Properties.Resources.NotifyWelcomeText,
                "This is the university menu which provides quick links to the most commonly used services of the University of Helsinki. Click and try!",
                "Tämä on yliopistovalikko, joka tarjoaa pikalinkkejä Helsingin yliopiston yleisimpiin palveluihin. Klikkaa ja kokeile!",
                "Detta är universitetsmenyn som erbjuder snabblänkar till de vanligaste tjänsterna vid Helsingfors universitet. Klicka och prova!"));
            TranslationItems.Add(AddTranslation(Properties.Resources.NotifyBalloonTitle,
                "NOTE", "HUOMIO", "OBS"));
            TranslationItems.Add(AddTranslation(Properties.Resources.NotifyCheckupBalloonText,
                "Open a VPN connection to obtain the required updates for this computer! Click this balloon for more information.",
                "Avaa VPN-yhteys, jotta tämä tietokone saa tarvittavat päivitykset! Saat lisätietoja klikkaamalla tätä puhekuplaa.",
                "Öppna VPN-anslutningen för att denna dator ska få de nödvändiga uppdateringarna! Klicka på denna pratbubbla för mer information."));
            TranslationItems.Add(AddTranslation(Properties.Resources.NotifyNetworkBalloonText,
                "The network disk space of your home directory is running low! Click this balloon for more information.",
                "Kotihakemistosi verkkolevytila on vähissä! Saat lisätietoja klikkaamalla tätä puhekuplaa.",
                "Du har ont om utrymme i din hemkatalog på nätverksdisken! Klicka på denna pratbubbla för mer information."));
            TranslationItems.Add(AddTranslation(Properties.Resources.NotifyRebootBalloonText,
                "The computer must be restarted to finalise the installation of the updates! Click this balloon for more information.",
                "Tietokone täytyy käynnistää uudelleen, jotta asennetut päivitykset saadaan viimeisteltyä! Saat lisätietoja klikkaamalla tätä puhekuplaa.",
                "Datorn måste startas om för att de installerade uppdateringarna ska kunna slutföras! Klicka på denna pratbubbla för mer information."));

            // Modules
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserInfo,
                "User Information", "Käyttäjätiedot", "Användaruppgifter"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserFullName,
                "Full Name: ", "Kokonimi: ", "Fullständigt namn: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserName,
                "Username: ", "Käyttäjätunnus: ", "Användarnamn: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserEmail,
                "Email Address: ", "Sähköpostiosoite: ", "E-postadress: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserMailbox,
                "Email Account Type: ", "Sähköpostitilin tyyppi: ", "Typ av e-postkonto: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserMailboxCloud,
                "Cloud (Online)", "Pilvi (online)", "Moln (online)"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserMailboxLocal,
                "Private Cloud (On-Premises)", "Paikallinen pilvi (on-premises)", "Hybridmoln (on-premises)"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserHomeDir,
                "Home Directory: ", "Kotihakemisto: ", "Hemkatalog: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserHomeDrive,
                "Home Drive: ", "Kotilevyasema: ", "Hemnätverkshårdskiva: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserDiskSpace,
                "Disk Space: ; MB; GB; TB", "Levytila: ; Mt; Gt; Tt", "Skivutrymme: ; MB; GB; TB"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserDiskSpaceFree,
                "free; MB ; GB ; TB ", "vapaana; Mt ; Gt ; Tt ", "ledigt; MB ; GB ; TB "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserPrinter,
                "Default Printer: ", "Oletustulostin: ", "Standardskrivare: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserPWChanged,
                "Password last changed on: ", "Salasana vaihdettu viimeksi: ", "Lösenordet har senast ändrats: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserPWExpires,
                "Password will expire on: ", "Salasana vanhenee: ", "Lösenordet förfaller: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserAccountExpires,
                "Account will expire on: ", "Käyttäjätunnus vanhenee: ", "Användarkontot förfaller: "));

            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCopyClipboard,
                "Copy texts to the clipboard", "Kopioi tekstit leikepöydälle", "Kopiera texterna till skrivbordet"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleTooltip,
                "Click the target to copy the text to the clipboard", "Kopioi teksti leikepöydälle klikkaamalla kohdetta", "Kopiera text till klippbordet genom att klicka på objektet"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserHomeTooltip,
                "Clicking: Open location\nRight-clicking: Copy the text to clipboard",
                "Vasen klikkaus: Avaa sijanti\nOikea klikkaus: Kopioi teksti leikepöydälle",
                "Vänsterklick: Öppna läge\nHögerklick: Kopiera texten till urklipp"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserInstructionsTooltip,
                "Clicking: Read instructions on the internet\nRight-clicking: Copy the text to clipboard",
                "Vasen klikkaus: Lue ohjeita internetistä\nOikea klikkaus: Kopioi teksti leikepöydälle",
                "Vänsterklick: Läs instruktioner på internet\nHögerklick: Kopiera texten till urklipp"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompWarrantyTooltipLess,
                "Your computer warranty has expired\n", "Tietokoneesi takuu on päättynyt\n", "Din datorgaranti har löpt ut\n"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompWarrantyTooltipMore,
                "Support for this device has ended!!!\n", "Tuki tälle laitteelle on päättynyt!!!\n", "Stöd för denna enhet har slutat!!!\n"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserPasswordUrl,
                moduleUIPasswordUrlEN, moduleUIPasswordUrlFI, moduleUIPasswordUrlSV));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserAccountUrl,
                moduleUIAccountUrlEN, moduleUIAccountUrlFI, moduleUIAccountUrlSV));

            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompInfo,
                "Computer Information", "Tietokoneen tiedot", "Datoruppgifter"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompHostname,
                "Computer Name: ", "Konenimi: ", "Datorns namn: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompModel,
                "Model: ", "Malli: ", "Modell: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompSN,
                "Serial Number: ", "Sarjanumero: ", "Serienummer: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompCPU,
                "Processor: ", "Suoritin: ", "Processor: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompMemory,
                "Memory: ; MB; GB; TB", "Muisti: ; Mt; Gt; Tt", "Minne: ; MB; GB; TB"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompWifiMAC,
                "Wireless MAC address: ", "Langaton MAC-osoite: ", "Trådlös MAC-adress: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompOS,
                "Operating System: ", "Käyttöjärjestelmä: ", "Operativ system: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompVersion,
                "Version: ", "Versio: ", "Version: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompAddress,
                "Address: ", "Osoite: ", "Adress: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompBuilding,
                "Building: ", "Rakennus: ", "Byggnad: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompRoom,
                "Room: ", "Huone: ", "Rum: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompProfit,
                "Profit Center: ", "Tulosyksikkö: ", "Resultatenhet: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompWarranty,
                "Warranty Expires: ", "Takuun päättymispäivä: ", "Garantins utgångsdatum: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompWarrantyUrl,
                moduleCIWarrantyUrlEN, moduleCIWarrantyUrlFI, moduleCIWarrantyUrlSV));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompWinUpd,
                "Updates last installed: ", "Päivitykset asennettu viimeksi: ", "Uppdateringar installerade senast: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompIsEncrypt,
                "Drive ; is encrypted", "Asema ; on kryptattu", "Hårdskiva ; är krypterad"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompNotEncrypt,
                "Drive ; has not been encrypted", "Asema ; ei ole kryptattu", "Hårdskiva ; har inte krypterad"));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompCert,
                "Certificate Expires: ", "Sertifikaatti vanhenee: ", "Certifikat utgångsdatum: "));

            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleOWA,
                "Outlook Web App (OWA)", "Outlook Web App (OWA)", "Outlook Web App (OWA)"));

            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleChat,
                "Chat with Helpdesk", "Avaa chat-yhteys Helpdeskiin", "Chatta med helpdesk"));

            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleSupportRequest,
                "Submit Support Request", "Lähetä tukipyyntö", "Skicka supportbegäran"));

            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserAdd,
                moduleUIAddHeader,
                (!IsNullOrWhiteSpace(moduleUIAddHeaderFI) ? moduleUIAddHeaderFI : moduleUIAddHeader),
                (!IsNullOrWhiteSpace(moduleUIAddHeaderSV) ? moduleUIAddHeaderSV : moduleUIAddHeader)));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserAdd + Properties.Resources.TooltipTag,
                moduleUIAddTooltip,
                (!IsNullOrWhiteSpace(moduleUIAddTooltipFI) ? moduleUIAddTooltipFI : moduleUIAddTooltip),
                (!IsNullOrWhiteSpace(moduleUIAddTooltipSV) ? moduleUIAddTooltipSV : moduleUIAddTooltip)));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserAdd + Properties.Resources.ExecuteTag,
                moduleUIAddExecute,
                (!IsNullOrWhiteSpace(moduleUIAddExecuteFI) ? moduleUIAddExecuteFI : moduleUIAddExecute),
                (!IsNullOrWhiteSpace(moduleUIAddExecuteSV) ? moduleUIAddExecuteSV : moduleUIAddExecute)));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleUserAdd + Properties.Resources.ParametersTag,
                moduleUIAddParameters,
                (!IsNullOrWhiteSpace(moduleUIAddParametersFI) ? moduleUIAddParametersFI : moduleUIAddParameters),
                (!IsNullOrWhiteSpace(moduleUIAddParametersSV) ? moduleUIAddParametersSV : moduleUIAddParameters)));

            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompAdd,
                moduleCIAddHeader,
                (!IsNullOrWhiteSpace(moduleCIAddHeaderFI) ? moduleCIAddHeaderFI : moduleCIAddHeader),
                (!IsNullOrWhiteSpace(moduleCIAddHeaderSV) ? moduleCIAddHeaderSV : moduleCIAddHeader)));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompAdd + Properties.Resources.TooltipTag,
                moduleCIAddTooltip,
                (!IsNullOrWhiteSpace(moduleCIAddTooltipFI) ? moduleCIAddTooltipFI : moduleCIAddTooltip),
                (!IsNullOrWhiteSpace(moduleCIAddTooltipSV) ? moduleCIAddTooltipSV : moduleCIAddTooltip)));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompAdd + Properties.Resources.ExecuteTag,
                moduleCIAddExecute,
                (!IsNullOrWhiteSpace(moduleCIAddExecuteFI) ? moduleCIAddExecuteFI : moduleCIAddExecute),
                (!IsNullOrWhiteSpace(moduleCIAddExecuteSV) ? moduleCIAddExecuteSV : moduleCIAddExecute)));
            TranslationItems.Add(AddTranslation(Properties.Resources.ModuleCompAdd + Properties.Resources.ParametersTag,
                moduleCIAddParameters,
                (!IsNullOrWhiteSpace(moduleCIAddParametersFI) ? moduleCIAddParametersFI : moduleCIAddParameters),
                (!IsNullOrWhiteSpace(moduleCIAddParametersSV) ? moduleCIAddParametersSV : moduleCIAddParameters)));

            // UI Notification
            TranslationItems.Add(AddTranslation(Properties.Resources.UISettings,
                "settings", "asetukset", "inställningar"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIHelp,
                "help", "apua", "hjälp"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIHelpUrl,
                uiHelpEN, uiHelpFI, uiHelpSV));

            TranslationItems.Add(AddTranslation(Properties.Resources.UINotifyTitle,
                "Notifications", "Ilmoituksia", "Notificeringar"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UINoNotify,
                "No notifications", "Ei ilmoituksia", "Inga meddelanden"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UINoNotifyText,
                "There are currently no active notifications.", "Tällä hetkellä yhtään ilmoitusta ei ole aktiivisena.", "För tillfället är inga meddelanden aktiv."));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportTitle,
                "Support Request", "Tukipyyntö", "Supportbegäran"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportForm,
                "Form", "Lomake", "Form"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPreview,
                "Preview", "Esikatselu", "Förhandsgranska"));

            TranslationItems.Add(AddTranslation(Properties.Resources.UIHelpdeskTitle,
                "How to contact the University of Helsinki Helpdesk?", "Miten otan yhteyttä Helsingin yliopiston Helpdeskiin?", "Hur ska jag kontakta Helpdesk vid Helsingfors universitet?"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIHelpdeskSupport,
                "A Support Request", "Tukipyyntö", "Supportbegäran"));

            TranslationItems.Add(AddTranslation(Properties.Resources.UICheckupWTDTitle,
                "What should I do now?", "Mitä teen nyt?", "Vad ska jag göra nu?"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UICheckupURL,
                notifyURLCheckupEN, notifyURLCheckupFI, notifyURLCheckupSV));
            TranslationItems.Add(AddTranslation(Properties.Resources.UICheckupWTDConnect,
                "Connect to the Eduroam network" + Environment.NewLine + "(if available)", "Yhdistä kone Eduroam-verkkoon" + Environment.NewLine + "(jos saatavilla)", "Anslut datorn till Eduroam-nätverket" + Environment.NewLine + "(om tillgängligt)"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UICheckupWTDVPN,
                "Open a secure VPN connection", "Avaa suojattu VPN-yhteys", "Öppna en skyddad VPN-anslutning"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UICheckupWTDWeb,
                "Read more instructions on the internet", "Lue lisäohjeita internetistä", "Läs ytterligare instruktioner på internet"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIWarrantyWTDTitle,
                "Contact university on-site services of your unit/facility", "Ota yhteys yksikkösi palvelukoordinaattoriin", "Kontakta din enhetens servicekoordinator"));

            TranslationItems.Add(AddTranslation(Properties.Resources.UINetworkWTDHome,
                "Go through your home directory", "Käy kotihakemistosi läpi", "Gå igenom din hemkatalog"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UINetworkWTDHYData,
                "Move files to the local folder", "Siirrä tiedostoja paikalliseen kansioon", "Flytta filer till en lokal mapp"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UINetworkURL,
                notifyURLNetworkEN, notifyURLNetworkFI, notifyURLNetworkSV));
            TranslationItems.Add(AddTranslation(Properties.Resources.UINetworkDiskSpaceCalc,
                "Calculatig...", "Lasketaan...", "Räknar..."));
            TranslationItems.Add(AddTranslation(Properties.Resources.UINetworkDiskSpaceInfo,
                "xx MB (yy %) of the zz disk quota in your home directory is free", "xx Mt (yy %) kotihakemistosi zz levytilasta on vapaana", "xx MB (yy %) av skivutrymmet i din hemkatalog är zz ledigt"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UINetworkDiskSpaceInfoNA,
                "Quota information of the home drive is not available", "Kotilevytilatietoja ei ole saatavilla", "Uppgifterna om hemkatalogen är inte tillgängliga"));

            TranslationItems.Add(AddTranslation(Properties.Resources.UIOSUpgradeURL,
                notifyURLOSUpgradeEN, notifyURLOSUpgradeFI, notifyURLOSUpgradeSV));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIOSUpgradeURL2,
                notifyURLOSUpgrade2EN, notifyURLOSUpgrade2FI, notifyURLOSUpgrade2SV));

            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportMessageTitle,
                "Submit Support Request", "Lähetä tukipyyntö", "Skicka supportbegäran"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportMessage,
                "Use this form to send a support request to the University of Helsinki Helpdesk. Fill in the fields carefully so that we can process your problem as smoothly as possible. You can also add attachments to your support request. ",
                "Tällä lomakkeella voit lähettää tukipyynnön Helsingin yliopiston Helpdeskiin. Täytä alla olevat kentät huolellisesti, jotta asia saadaan käsiteltyä mahdollisimman sujuvasti. Voit myös lisätä tukipyyntöön liitetiedostoja viestisi tueksi. ",
                "Med detta formulär kan du skicka supportbegäran till Helsingfors universitets Helpdesk. Fyll i fälten nedan noggrant så att vi ditt ärende kan behandlas så smidigt som möjligt. Du kan också lägga till bilagor till din begäran. "));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportMessageUrl,
                "Support requests are processed during helpdesk opening hours.",
                "Tukipyyntöjä käsitellään helpdeskin aukioloaikoina.",
                "Supportförfrågningar behandlas under helpdesk öppettider."));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSubject,
                "Subject", "Aihe", "Ämne"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportInfo,
                "The required information concerning the computer is automatically added to the support request.", "Tarvittavat tietokoneen tiedot lisätään lähtevään tukipyyntöön automaattisesti.", "De nödvändiga uppgifterna om datorn läggs till din supportbegäran automatiskt."));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportButtonPreview,
                "Preview and Send", "Esikatsele ja lähetä", "Förhandsgranska och skicka"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportButtonClear,
                "Clear", "Tyhjennä", "Töm"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportButtonSend,
                "Send", "Lähetä", "Skicka"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportButtonEdit,
                "Edit", "Muokkaa", "Ändra"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIPreviewMessage,
                "Please check that the information below is correct. Click Send to send the message to the the University of Helsinki Helpdesk.",
                "Tarkistathan, että alla olevat tiedot ovat oikein, minkä jälkeen klikkaa lähetä-painiketta lähettääksesi viestin Helsingin yliopiston Helpdeskiin.",
                "Kontrollera att uppgifterna nedan är rätt. Klicka på Skicka för att skicka meddelandet till Helsingfors universitets Helpdesk."));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIPreviewCanNotSend,
                "Required information is missing in the support request form. Please check that you have filled in all the required fields of the support request form.",
                "Tukipyyntölomakkeelta puuttuu tarvittavia tietoja. Tarkistathan, että tukipyyntölomakkeen kaikki vaadittavat kentät ovat täytetty.",
                "Nödvändig information saknas på blanketten. Kontrollera att alla erforderliga fälten är ifyllda."));

            TranslationItems.Add(AddTranslation(Properties.Resources.UIAttachmentAdd,
                "Insert an attachment", "Lisää liite", "Infoga en bilaga"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIAttachmentClear,
                "Remove attachments", "Poista liitteet", "Ta bort bifogade filer"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIAttachmentCount0,
                "Choose attachments or drag files here to add attachments to the request", "Valitse liitteitä tai vedä tähän tiedostoja lisätäksesi tukipyyntöön liitteitä", "Välj bilagor eller dra filer hit för att lägga till bilagor till supportbegäran"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIAttachmentCount1,
                " attachment selected", " liite valittuna", " bilaga vald"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIAttachmentCount2,
                " attachments selected", " liitettä valittuna", " bilagor valda"));

            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportBasicMessage,
                "Message", "Viesti", "Meddelande"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportAttachments,
                "Attachments", "Liitteet", "Bilagorna"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportAttachment,
                "Attachment", "Liite", "Bilaga"));

            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportBulletin,
                "Bulletin", "Tiedote", "Meddelande"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSIBulletinInfo,
                "Please make sure that the application is not already available in the university network services: ",
                "Varmistathan, ettei sovellus ole jo tarjolla yliopiston verkkopalvelussa: ",
                "Kontrollera att applikationen inte redan finns i universitetets nättjänst: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSIBulletinSoftwarePortal,
                "Software Portal", "Ohjelmistoportaali", "Programportalen"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSISoftware,
                "Software", "Ohjelma", "Program"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSILicense,
                "License", "Lisenssi", "Licens"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSILicenseInfo,
                "Please note that any licences acquired by you personally are not installed in university computers",
                "Huomaa, että henkilökohtaisesti hankittuja lisenssejä ei asenneta yliopiston tietokoneille",
                "Observera att personliga licenser inte ska installeras på universitetets datorer"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSILicense1,
                "Commercial", "Kaupallinen", "Kommersiell"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSILicense2,
                "Free or Open Source", "Vapaa tai Open Source", "Fri eller Open Source"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSIInstallation,
                "Installation", "Asennus", "Installation"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSIInstallationInfo,
                "Please note that Helpdesk will confirm the actual installation time",
                "Huomaa, että Helpdesk vahvistaa vielä varsinaisen asennusajan",
                "Observera att Helpdesk ännu kommer att bekräfta den faktiska installationstidpunkten"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSIInstallationOption,
                "Option ", "Vaihtoehto ", "Val "));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSIInstallationPickDate,
                "Pick a date", "Valitse päivä", "Välja datum"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSIInstallationTime,
                "Time Window ", "Aikaikkuna ", "Fönster "));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSIInstallationAnyTime,
                "Any", "Milloin tahansa", "Någon"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSIComments,
                "Comments", "Kommentit", "Uttalanden"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSIInstructions,
                "Please log out of your computer at the time of installation. If you have the installation package and/or licence key, place them in your local folder: ",
                "Kirjauduthan asennusajankohtana ulos koneeltasi. Mikäli sinulla on asennuspaketti ja/tai lisenssiavain, sijoita nämä valmiiksi paikalliseen kansioosi: ",
                "Du loggar väl ut från din dator vid installationstidpunkten. Om du har ett installationspaket och/eller en licensnyckel, ska du placera dem färdigt i din lokala mapp: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportSIPortalURL,
                requestURLPortalEN, requestURLPortalFI, requestURLPortalSV));

            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuBulletinInfo,
                "Check the devices to order (desktop computers, laptops, phones) in Flamma: ", "Tarkista tilattavat laitteet (pöytäkoneet, kannettavat, puhelimet) Flammasta: ", "Kontrollera de enheter som kan beställas (bordsdatorer, bärbara datorer, telefoner) i Flamma: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuRedirectInfo,
                "Follow the purchase instructions from Flamma: ", "Seuraa Flamman hankintaohjeita: ", "Följ inköpsanvisningarna från Flamma: "));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuFlammaURL,
                requestURLFlammaEN, requestURLFlammaFI, requestURLFlammaSV));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuOrders,
                "Orders", "Tilaukset", "Köpps"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuAcceptor,
                "Acceptor", "Hyväksyjä", "Godkännare"));
            // SupportPurchases.xaml.cs -> textAcceptorEN, textAcceptorFI, textAcceptorSV
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuMe,
                "Me", "Minä", "Jag"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuEndUser,
                "End User", "Loppukäyttäjä", "Slutanvändare"));
            // SupportPurchases.xaml.cs -> textEndUserEN, textEndUserFI, textEndUserSV
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuWBS,
                "WBS", "WBS", "WBS"));
            // SupportPurchases.xaml.cs -> textWBSEN, textWBSFI, textWBSSV
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuType,
                "Acquisition type", "Hankintatyyppi", "Typ av anskaffning"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuTypeNew,
                "New", "Uusi", "Ny"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuTypeReplace,
                "Replace", "Korvaava", "Byta"));
            // SupportPurchases.xaml.cs -> textPurchaseTypeEN, textPurchaseTypeFI, textPurchaseTypeSV
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuDevice,
                "Device", "Laite", "Enhet"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuDevice1,
                "Desktop", "Pöytäkone", "Bordsdatorer"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuDevice2,
                "Laptop", "Kannettava", "Bärbara datorer"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportPuDevice3,
                "Mobile", "Mobiili", "Mobil"));


            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportCategory,
                "<Choose category>", "<Valitse kategoria>", "<Välj kategori>"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportOther,
                "Other", "Muu", "Övriga ärenden"));

            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportCategoryPurchases,
                "Purchases", "Hankinnat", "Anskaffning"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportCategorySI,
                "Software Installation", "Ohjelmistoasennus", "Programinstallation"));

            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportWait,
                "Checking preconditions, please wait...", "Odotathan, tarkistetaan edellytyksiä...", "Vänta, vi kontrollerar förutsättningarna..."));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportNotIdentified,
                "Information needed for sending the message was not identified. Connect the computer to the university network or open a VPN connection and try again.",
                "Viestin lähetyksen edellyttämiä tietoja ei tunnistettu. Yhdistä tietokone yliopistoverkkoon tai avaa VPN-yhteys ja yritä uudelleen.",
                "De uppgifter som krävs för att skicka meddelandet kunde inte identifieras. Anslut datorn till universitetsnätet eller öppna VPN-anslutningen och försök igen."));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISupportAgain,
                "Try again", "Yritä uudelleen", "Försök igen"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIPreviewAttachmentError,
                "Some of the attachments could not be added to the message. Do you want to continue sending the support request?",
                "Kaikkien liitteiden lisääminen viestiin ei onnistunut. Jatketaanko silti tukipyynnön lähetystä?",
                "Det gick inte att lägga till alla bilagor till meddelandet. Vill du fortsätta att skicka din supportbegäran?"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIPreviewSendError,
                "The message could not be sent.", "Viestiä ei voitu lähettää.", "Meddelandet kunde inte skickas."));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIPreviewSendSuccess,
                "The message was sent successfully.", "Viesti lähetettiin onnistuneesti.", "Sändningen av meddelandet lyckades."));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIPreviewCancel,
                "Cancel", "Peruuta", "Avbryt"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIMessageInfo,
                requestEmailFooterEN, requestEmailFooterFI, requestEmailFooterSV));

            TranslationItems.Add(AddTranslation(Properties.Resources.UIRestartConfirm,
                "I have saved all my documents and closed all applications", "Olen tallentanut kaikki asiakirjani ja sulkenut kaikki ohjelmani", "Jag har sparat alla mina dokument och stängt alla program"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UIRestartButton,
                "Restart now", "Käynnistä uudelleen nyt", "Starta om datorn"));

            // Chat
            TranslationItems.Add(AddTranslation(Properties.Resources.ChatUrl,
                chatUrlEN, chatUrlFI, chatUrlSV));

            //Settings
            TranslationItems.Add(AddTranslation(Properties.Resources.UISettingsAppearance,
                "Appearance", "Ulkoasu", "Utseende"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISettingsAbout,
                "About", "Tietoja", "Info om"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISettingsTheme,
                "Theme:", "Teema:", "Tema:"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISettingsLight,
                "light", "vaalea", "ljus"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISettingsDark,
                "dark", "tumma", "mörk"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISettingsFontSize,
                "Font Size:", "Kirjasinkoko:", "Font storlek:"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISettingsLarge,
                "large", "suuri", "stor"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISettingsSmall,
                "small", "pieni", "liten"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISettingsLanguage,
                "Language:", "Kieli:", "Språk:"));
            TranslationItems.Add(AddTranslation(Properties.Resources.UISettingsApply,
                "Apply", "Käytä", "Använd"));
        }

        public static CurrentText AddTranslation(string id, string en, string fi, string sv)
        {
            CurrentText ct = new CurrentText
            {
                ID = id,
                TextEN = en,
                TextFI = fi,
                TextSV = sv
            };

            switch (uiLanguage)
            {
                case Languages.Suomi:
                    ct.Text = ct.TextFI;
                    break;
                case Languages.Svenska:
                    ct.Text = ct.TextSV;
                    break;
                default:
                    ct.Text = ct.TextEN;
                    break;
            }

            return ct;
        }

        public static string GetTranslation(string id, int languageHash = -1)
        {
            try
            {
                if (languageHash < 0)
                { return (TranslationItems.First(item => item.ID.ToLower() == id.ToLower()).Text ?? Properties.Resources.NA); }
                else
                {
                    switch (languageHash)
                    {
                        case 1:
                            return (TranslationItems.First(item => item.ID == id).TextFI ?? Properties.Resources.NA);
                        case 2:
                            return (TranslationItems.First(item => item.ID == id).TextSV ?? Properties.Resources.NA);
                        default:
                            return (TranslationItems.First(item => item.ID == id).TextEN ?? Properties.Resources.NA);
                    }
                }
            }
            catch
            { return GetTranslation(Properties.Resources.NameErrorText); }
        }

        public static void UpdateTranslations()
        {
            foreach (var item in TranslationItems)
            {
                switch (uiLanguage)
                {
                    case Languages.Suomi:
                        if (!IsNullOrWhiteSpace(item.TextFI)) { item.Text = item.TextFI; }
                        else { item.Text = item.TextEN; }
                        break;
                    case Languages.Svenska:
                        if (!IsNullOrWhiteSpace(item.TextSV)) { item.Text = item.TextSV; }
                        else { item.Text = item.TextEN; }
                        break;
                    default:
                        item.Text = item.TextEN;
                        break;
                }
            }
        }

        public static Languages ConvertLanguage(int input)
        {
            switch (input)
            {
                case 1: return Languages.Suomi;
                case 2: return Languages.Svenska;
                default: return Languages.English;
            }
        }

        public static List<CurrentText> TranslationItems = new List<CurrentText>();

        public class CurrentText
        {
            public string ID { get; set; }
            public string Text { get; set; }
            public string TextEN { get; set; }
            public string TextFI { get; set; }
            public string TextSV { get; set; }
        }

        public enum Languages
        {
            English = 0,
            Suomi = 1,
            Svenska = 2,
        }
    }
}