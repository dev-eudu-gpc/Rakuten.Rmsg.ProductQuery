﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rakuten {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class UriTemplateErrorMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal UriTemplateErrorMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Rakuten.UriTemplates.UriTemplateErrorMessages", typeof(UriTemplateErrorMessages).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Adjacent variable specifiers are not supported..
        /// </summary>
        internal static string AdjacentVarspecs {
            get {
                return ResourceManager.GetString("AdjacentVarspecs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot create a new URI template from an expression of type &apos;?&apos; without a value for the first variable..
        /// </summary>
        internal static string FirstValueMissing {
            get {
                return ResourceManager.GetString("FirstValueMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The variable name &apos;{0}&apos; is invalid..
        /// </summary>
        internal static string InvalidVariableName {
            get {
                return ResourceManager.GetString("InvalidVariableName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot create a new URI template from an expression without an operator..
        /// </summary>
        internal static string MissingOperator {
            get {
                return ResourceManager.GetString("MissingOperator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A path segment or query component cannot include a mix of literal and variable components..
        /// </summary>
        internal static string MixedLiteralAndVariableComponents {
            get {
                return ResourceManager.GetString("MixedLiteralAndVariableComponents", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Multiple operators may not be applied to an expression..
        /// </summary>
        internal static string MultipleOperators {
            get {
                return ResourceManager.GetString("MultipleOperators", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Multiple question marks are not supported within a URI template..
        /// </summary>
        internal static string MultipleQuestionMarks {
            get {
                return ResourceManager.GetString("MultipleQuestionMarks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Multiple variables per expression are only supported for expressions with an associated operator..
        /// </summary>
        internal static string MultipleVariablesWithoutOperator {
            get {
                return ResourceManager.GetString("MultipleVariablesWithoutOperator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operators are only supported for query components..
        /// </summary>
        internal static string OperatorInNonQueryExpression {
            get {
                return ResourceManager.GetString("OperatorInNonQueryExpression", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The variable &apos;{0}&apos; appears multiple times in the URI template. Each variable may appear only once..
        /// </summary>
        internal static string RepeatedVariableName {
            get {
                return ResourceManager.GetString("RepeatedVariableName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The variable &apos;{0}&apos; must appear within the URI template..
        /// </summary>
        internal static string UndefinedVariable {
            get {
                return ResourceManager.GetString("UndefinedVariable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Encountered unexpected character(s) &apos;{0}&apos;..
        /// </summary>
        internal static string UnexpectedCharacters {
            get {
                return ResourceManager.GetString("UnexpectedCharacters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The operator &apos;{0}&apos; is not supported..
        /// </summary>
        internal static string UnsupportedOperator {
            get {
                return ResourceManager.GetString("UnsupportedOperator", resourceCulture);
            }
        }
    }
}
