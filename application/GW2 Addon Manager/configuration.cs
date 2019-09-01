﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Windows;

namespace GW2_Addon_Manager
{
    /// <summary>
    /// The <c>configuration</c> class contains various functions accessing and modifying the configuration file and the configuration file template.
    /// </summary>
    class configuration
    {
        static string config_file_path = "config.ini";
        static string config_template_path = "resources\\config_template.ini";

        static string applicationRepoUrl = "https://api.github.com/repos/fmmmlee/GW2-Addon-Manager/releases/latest";

        /// <summary>
        /// <c>getConfig</c> accesses a configuration file found at <c>config_file_path</c>, which should adhere to proper Json syntax.
        /// </summary>
        /// <returns> a dynamic object representing the configuration file as converted from Json using Json.NET </returns>
        public static dynamic getConfig()
        {
            string config_file = File.ReadAllText(config_file_path);
            dynamic config_obj = JsonConvert.DeserializeObject(config_file);
            return config_obj;
        }

        /// <summary>
        /// <c>getConfigAsJObject</c> accesses a configuration file found at <c>config_file_path</c>, which should adhere to proper Json syntax.
        /// </summary>
        /// <returns> a <c>JObject</c> representing the configuration file as converted from Json using JObject.Parse() </returns>
        public static JObject getConfigAsJObject()
        {
            string config_file = File.ReadAllText(config_file_path);
            JObject config_obj = JObject.Parse(config_file);
            return config_obj;
        }

        /// <summary>
        /// <c>getTemplateConfig</c> accesses the configuration file template found at <c>config_template_path</c>, which should adhere to proper Json syntax.
        /// </summary>
        /// <returns> a dynamic object representing the default configuration file template as converted from Json using Json.NET </returns>
        public static dynamic getTemplateConfig()
        {
            string config_file = File.ReadAllText(config_template_path);
            dynamic config_obj = JsonConvert.DeserializeObject(config_file);
            return config_obj;
        }

        /// <summary>
        /// <c>getTemplateConfigAsJObject</c> accesses the configuration file template found at <c>config_template_path</c>, which should adhere to proper Json syntax.
        /// </summary>
        /// <returns> an instance of <c>JObject</c> representing the default configuration file template as converted from Json using Json.NET </returns>
        public static JObject getTemplateConfigAsJObject()
        {
            string config_file = File.ReadAllText(config_template_path);
            JObject config_obj = JObject.Parse(config_file);
            return config_obj;
        }

        /// <summary>
        /// <c>setConfig</c> overwrites the configuration file found at <c>config_file_path</c>
        /// with a Json string created by serializing<paramref name="config_obj"/>.
        /// </summary>
        /// <param name="config_obj"> the dynamic object to be serialized and written to the file </param>
        public static void setConfig(dynamic config_obj)
        {
            string edited_config_file = JsonConvert.SerializeObject(config_obj, Formatting.Indented);
            File.WriteAllText(config_file_path, edited_config_file);
        }

        /// <summary>
        /// Overwrites the configuration file template in the application's
        /// /resources/ folder with a string created by serializing <paramref name="config_obj"/>.
        /// </summary>
        /// <param name="config_obj"></param>
        public static void setConfigTemplate(dynamic config_obj)
        {
            string edited_config_file = JsonConvert.SerializeObject(config_obj, Formatting.Indented);
            File.WriteAllText(config_template_path, edited_config_file);
        }

        /// <summary>
        /// <c>ApplyDefaultConfig</c> reads the configuration file found at <c>config_file_path</c>
        /// and uses the information within to set properties across <paramref name="viewModel"/>.
        /// </summary>
        /// <param name="viewModel">An instance of the <typeparamref>OpeningViewModel</typeparamref> class serving as the DataContext for the application UI.</param>
        public static void ApplyDefaultConfig(OpeningViewModel viewModel)
        {
            dynamic config_obj = getConfig();

            if ((bool)config_obj.default_configuration.arcdps)
                viewModel.ArcDPS_CheckBox = true;
            if ((bool)config_obj.default_configuration.gw2radial)
                viewModel.GW2Radial_CheckBox = true;
            if ((bool)config_obj.default_configuration.d912pxy)
                viewModel.d912pxy_CheckBox = true;
            if ((bool)config_obj.default_configuration.arcdps_bhud)
                viewModel.arcdps_bhud_CheckBox = true;
            if ((bool)config_obj.default_configuration.arcdps_mechanics)
                viewModel.arcdps_mechanics_CheckBox = true;

            DisplayAddonStatus(viewModel);
        }

        /// <summary>
        /// Displays the latest status of the plugins on the opening screen (disabled, enabled, version, installed).
        /// </summary>
        /// <param name="viewModel">An instance of the <typeparamref>OpeningViewModel</typeparamref> class serving as the DataContext for the application UI.</param>
        public static void DisplayAddonStatus(OpeningViewModel viewModel)
        {
            dynamic config_obj = getConfig();

            if (config_obj.version.arcdps != null)
                viewModel.ArcDPS_Content = "ArcDPS (installed)";
            if (config_obj.version.gw2radial != null)
                viewModel.GW2Radial_Content = "GW2 Radial (" + config_obj.version.gw2radial + " installed)";
            if (config_obj.version.d912pxy != null)
                viewModel.d912pxy_Content = "d912pxy (" + config_obj.version.d912pxy + " installed)";
            if (config_obj.version.arcdps_bhud != null)
                viewModel.arcdps_bhud_Content = "ArcDPS for BlishHUD (" + config_obj.version.arcdps_bhud + " installed)";
            if (config_obj.version.arcdps_mechanics != null)
                viewModel.arcdps_mechanics_Content = "ArcDPS Mechanics plugin (installed)";

            if ((bool)config_obj.disabled.arcdps)
                viewModel.ArcDPS_Content = "Disabled - ArcDPS" + (config_obj.version.arcdps != null ? " (downloaded)" : "");
            if ((bool)config_obj.disabled.gw2radial)
                viewModel.GW2Radial_Content = "Disabled - GW2 Radial " + config_obj.version.gw2radial;
            if ((bool)config_obj.disabled.d912pxy)
                viewModel.d912pxy_Content = "Disabled - d912pxy " + config_obj.version.d912pxy;
            if ((bool)config_obj.disabled.arcdps_bhud)
                viewModel.arcdps_bhud_Content = "Disabled - ArcDPS for BlishHUD " + config_obj.version.arcdps_bhud;
            if ((bool)config_obj.disabled.arcdps_mechanics)
                viewModel.arcdps_mechanics_Content = "Disabled - ArcDPS Mechanics plugin" + (config_obj.version.arcdps_mechanics != null ? "\n(downloaded)" : "");

            if (!(bool)config_obj.disabled.arcdps && config_obj.version.arcdps == null)
                viewModel.ArcDPS_Content = "ArcDPS";
            if (!(bool)config_obj.disabled.gw2radial && config_obj.version.gw2radial == null)
                viewModel.GW2Radial_Content = "GW2 Radial";
            if (!(bool)config_obj.disabled.d912pxy && config_obj.version.d912pxy == null)
                viewModel.d912pxy_Content = "d912pxy";
            if (!(bool)config_obj.disabled.arcdps_bhud && config_obj.version.arcdps_bhud == null)
                viewModel.arcdps_bhud_Content = "ArcDPS for BlishHUD";
            if (!(bool)config_obj.disabled.arcdps_mechanics && config_obj.version.arcdps_mechanics == null)
                viewModel.arcdps_mechanics_Content = "ArcDPS Mechanics plugin";
        }

        /// <summary>
        /// <c>ChangeAddonConfig</c> writes the default add-ons section of the configuration file found at <c>config_file_path</c> using
        /// values found in <paramref name="viewModel"/>, which can be set by the user.
        /// </summary>
        /// <param name="viewModel">An instance of the <typeparamref>OpeningViewModel</typeparamref> class serving as the DataContext for the application UI.</param>
        public static void ChangeAddonConfig(OpeningViewModel viewModel)
        {
            dynamic config_obj = getConfig();

            if (viewModel.ArcDPS_CheckBox)
                config_obj.default_configuration.arcdps = true;
            else
                config_obj.default_configuration.arcdps = false;

            if (viewModel.GW2Radial_CheckBox)
                config_obj.default_configuration.gw2radial = true;
            else
                config_obj.default_configuration.gw2radial = false;

            if (viewModel.d912pxy_CheckBox)
                config_obj.default_configuration.d912pxy = true;
            else
                config_obj.default_configuration.d912pxy = false;

            if (viewModel.arcdps_bhud_CheckBox)
                config_obj.default_configuration.arcdps_bhud = true;
            else
                config_obj.default_configuration.arcdps_bhud = false;

            if (viewModel.arcdps_mechanics_CheckBox)
                config_obj.default_configuration.arcdps_mechanics = true;
            else
                config_obj.default_configuration.arcdps_mechanics = false;

            setConfig(config_obj);
        }


        /// <summary>
        /// <c>SetGamePath</c> both sets the game path for the current application session to <paramref name="path"/> and records it in the configuration file.
        /// </summary>
        /// <param name="path">The game path.</param>
        public static void SetGamePath(string path)
        {
            Application.Current.Properties["game_path"] = path.Replace("\\", "\\\\");
            dynamic config_obj = configuration.getConfig();
            config_obj.game_path = Application.Current.Properties["game_path"].ToString().Replace("\\\\", "\\");
            configuration.setConfig(config_obj);
            DetermineSystemType();
        }


        /// <summary>
        /// <c>SelfVersionStatus</c> either creates config.ini according to the template in /resources/, copies data from a previous installation to
        /// an updated configuration file template and then overwrites config.ini, or does nothing, depending on
        /// the user's installation.
        /// </summary>
        public static void ConfigFileStatus()
        {
            if (File.Exists(config_file_path))
            {
                JObject template_config = getTemplateConfigAsJObject();
                JObject current_config = getConfigAsJObject();

                /* if update flag set */
                if ((bool)template_config.GetValue("isupdate"))
                {
                    /* copy info from old config to template */
                    foreach (var property in current_config)
                    {
                        if (template_config.ContainsKey(property.Key) && property.Key != "application_version")
                        {
                            if (property.Value.HasValues)
                            {
                                JObject child = (JObject)property.Value;

                                foreach (var child_property in child)
                                {
                                    template_config[property.Key][child_property.Key] = child_property.Value;
                                }
                            }
                            else
                            {
                                template_config[property.Key] = property.Value;
                            }
                            
                        }


                    }
                    /* convert the updated JObject to a dynamic and overwrite config.ini */
                    setConfig(template_config.ToObject<dynamic>());

                    dynamic template_disable_update = getTemplateConfig();  //get the unedited config template
                    template_disable_update.isupdate = false;               //set isupdate to false, so program knows not to re-perform config file swapping
                    setConfigTemplate(template_disable_update);             //save to the config file template path
                }
            }
            else
            {
                File.Copy(config_template_path, config_file_path);

                dynamic template_disable_update = getTemplateConfig();  //get the unedited config template
                template_disable_update.isupdate = false;               //set isupdate to false, so program knows not to re-perform config file swapping
                setConfigTemplate(template_disable_update);             //save to the config file template path

                Application.Current.Properties["newinstall"] = true;
                return;
            }
        }

        /// <summary>
        /// Checks if there is a new version of the application available.
        /// </summary>
        /// <param name="viewModel">An instance of the <typeparamref>OpeningViewModel</typeparamref> class serving as the DataContext for the application UI.</param>
        public static void CheckSelfUpdates(OpeningViewModel viewModel)
        {
            string thisVersion = getConfig().application_version;
            string latestVersion;

            dynamic release_info = UpdateHelpers.GitReleaseInfo(applicationRepoUrl);
            latestVersion = release_info.tag_name;

            if (latestVersion != thisVersion)
            {
                viewModel.UpdateAvailable = latestVersion + " available!";
                viewModel.UpdateLinkVisibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Attempts to read the game folder and determine whether the game is running on a 64 or 32-bit system.
        /// Based on that, sets the 'bin_folder' property in the configuration file.
        /// </summary>
        public static void DetermineSystemType()
        {
            dynamic config = getConfig();
            if (Directory.Exists(config.game_path.ToString()))
            {
                if (Directory.Exists(config.game_path.ToString() + "\\bin64"))
                {
                    config.bin_folder = "bin64";
                }
                else if (Directory.Exists(config.game_path.ToString() + "\\bin"))
                {
                    config.bin_folder = "bin";
                }
                setConfig(config);
            }
        }
    }
}
