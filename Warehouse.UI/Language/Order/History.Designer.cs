﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Warehouse.Language.Order {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class History {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal History() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Warehouse.Language.Order.History", typeof(History).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel order.
        /// </summary>
        public static string CancelOrder {
            get {
                return ResourceManager.GetString("CancelOrder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you confirm canceling this order?.
        /// </summary>
        public static string CancelOrderConfirm {
            get {
                return ResourceManager.GetString("CancelOrderConfirm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Order.
        /// </summary>
        public static string Order {
            get {
                return ResourceManager.GetString("Order", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Order date.
        /// </summary>
        public static string OrderDate {
            get {
                return ResourceManager.GetString("OrderDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Order Id.
        /// </summary>
        public static string OrderId {
            get {
                return ResourceManager.GetString("OrderId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Total.
        /// </summary>
        public static string TotalCount {
            get {
                return ResourceManager.GetString("TotalCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Total money.
        /// </summary>
        public static string TotalMoney {
            get {
                return ResourceManager.GetString("TotalMoney", resourceCulture);
            }
        }
    }
}
