﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PtpChat.Utility.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("37.139.19.21")]
        public string DefaultServer_Host {
            get {
                return ((string)(this["DefaultServer_Host"]));
            }
            set {
                this["DefaultServer_Host"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9001")]
        public int DefaultServer_Port {
            get {
                return ((int)(this["DefaultServer_Port"]));
            }
            set {
                this["DefaultServer_Port"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ptpchat\\logs\\main.log")]
        public string DefaultLoggingFile {
            get {
                return ((string)(this["DefaultLoggingFile"]));
            }
            set {
                this["DefaultLoggingFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ptpchat")]
        public string DefaultApplicationFolder {
            get {
                return ((string)(this["DefaultApplicationFolder"]));
            }
            set {
                this["DefaultApplicationFolder"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool IsLoggingEnabled {
            get {
                return ((bool)(this["IsLoggingEnabled"]));
            }
            set {
                this["IsLoggingEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5f715c17-4a41-482a-ab1f-45fa2cdd702b")]
        public string DefaultServer_Guid {
            get {
                return ((string)(this["DefaultServer_Guid"]));
            }
            set {
                this["DefaultServer_Guid"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LocalNodeId {
            get {
                return ((string)(this["LocalNodeId"]));
            }
            set {
                this["LocalNodeId"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ptpchat-client; 0.0")]
        public string LocalNodeVersion {
            get {
                return ((string)(this["LocalNodeVersion"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("120")]
        public int NodeCutoff {
            get {
                return ((int)(this["NodeCutoff"]));
            }
            set {
                this["NodeCutoff"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("180")]
        public int ChannelCutoff {
            get {
                return ((int)(this["ChannelCutoff"]));
            }
            set {
                this["ChannelCutoff"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int MessageCutoff {
            get {
                return ((int)(this["MessageCutoff"]));
            }
            set {
                this["MessageCutoff"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int MaxMessageResendAttempts {
            get {
                return ((int)(this["MaxMessageResendAttempts"]));
            }
            set {
                this["MaxMessageResendAttempts"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("32")]
        public int DefaultTTL {
            get {
                return ((int)(this["DefaultTTL"]));
            }
            set {
                this["DefaultTTL"] = value;
            }
        }
    }
}
